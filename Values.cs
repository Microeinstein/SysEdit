using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SysEdit {
    using EVT = EnvironmentVariableTarget;
    using RVK = RegistryValueKind;

    public static class ValuesHelper {
        const string
            SystemVars = @"SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
            UserVars = @"Environment";
        public static readonly List<EnvValue> Data = new List<EnvValue>();
        public static readonly EVT[] targets = (EVT[])Enum.GetValues(typeof(EVT));
        static readonly Type TYPE_EVT = typeof(EVT);

        static ValuesHelper() {
            targets.MoveToEnd(EVT.Process);
            ReadValues();
        }

        public static IEnumerable<object> GetVariables(EVT t) {
            if (t != EVT.Process) {
                RegistryKey key;
                if (t == EVT.Machine)
                    key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(SystemVars);
                else
                    key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default).OpenSubKey(UserVars);
                var names = key.GetValueNames();
                foreach (var k in names) {
                    yield return k;
                    yield return key.GetValueKind(k) == RVK.ExpandString;
                    yield return key.GetValue(k, null, RegistryValueOptions.DoNotExpandEnvironmentNames);
                }
                yield break;
            }
            var env = Environment.GetEnvironmentVariables(t);
            foreach (DictionaryEntry item in env) {
                yield return item.Key;
                yield return null;
                yield return item.Value;
            }
        }
        
        public static void ReadValues() {
            Data.Clear();
            foreach (var t in targets) {
                var env = GetVariables(t);
                int info_index = 0;
                string name = "",
                       value = "";
                bool? exp = null;
                foreach (object item in env) {
                    switch (info_index) {
                        case 0:
                            name = (string)item;
                            break;
                        case 1:
                            exp = (bool?)item;
                            break;
                        case 2:
                            value = (string)item;
                            Data.Add(new EnvValue() { Target = t, Name = name, Expandable = exp, Value = value });
                            break;
                    }
                    info_index++;
                    if (info_index >= 3)
                        info_index = 0;
                }
            }
        }
        public static void SaveValues() {
            var admin = Program.IsElevated();
            var sysKey = !admin ? null : RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(SystemVars, true);
            var usrKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Default).OpenSubKey(UserVars, true);
            bool anythingChanged = false;
            foreach (var value in Data) {
                bool del = value.MarkedForRemoval;
                if (!(value.Changed || del))
                    continue;
                anythingChanged = true;
                switch (value.Target) {
                    case EVT.Process:
                        Environment.SetEnvironmentVariable(value.Name, del ? null : value.Value, value.Target);
                        break;
                    case EVT.User:
                        if (del) {
                            //Environment.SetEnvironmentVariable(value.Name, null, EVT.Process);
                            usrKey.DeleteValue(value.Name, false);
                        } else
                            usrKey.SetValue(value.Name, value.Value, value.Expandable == true ? RVK.ExpandString : RVK.String);
                        break;
                    case EVT.Machine when admin:
                        if (del) {
                            //Environment.SetEnvironmentVariable(value.Name, null, EVT.Process);
                            sysKey.DeleteValue(value.Name, false);
                        } else
                            sysKey.SetValue(value.Name, value.Value, value.Expandable == true ? RVK.ExpandString : RVK.String);
                        break;
                }
                value.Changed = false;
            }
            var toDel = Data.Where(e => e.MarkedForRemoval).ToArray();
            foreach (var value in toDel)
                Data.Remove(value);
            if (anythingChanged) {
                Program.SendEnvironmentMessage();
                /*//Reload as process-side variables
                var resultView = new Dictionary<string, string>();
                foreach (var value in Data) {
                    if (value.Target == EVT.Process)
                        continue;
                    //if user, or if machine then also if does not exist
                    if (value.Target != EVT.Machine || !resultView.ContainsKey(value.Name))
                        resultView[value.Name] = value.Value;
                }
                foreach (var kv in resultView)
                    Environment.SetEnvironmentVariable(kv.Key, kv.Value, EVT.Process);*/
            }
            SystemSounds.Asterisk.Play();
        }
        public static int ImportValues(string filepath) {
            var stream = new StreamReader(File.Open(filepath, FileMode.Open, FileAccess.Read, FileShare.None));
            int errors = 0;
            while (!stream.EndOfStream) {
                var line = stream.ReadLine();
                var match = Regex.Match(line, @"^(\w+?)(\*?)\\([A-Za-z0-9#$'()*+,\-.?@[\]_`{}~ ]+?)=(.+)$");
                if (!match.Success) {
                    errors++;
                    continue;
                }
                var grps = match.Groups;
                var env = new EnvValue() {
                    Target = (EVT)Enum.Parse(TYPE_EVT, grps[1].Value),
                    Expandable = grps[2].Length > 0,
                    Name = grps[3].Value,
                    Value = Encoding.Unicode.GetString(Convert.FromBase64String(grps[4].Value))
                };
                var existing = Data.FindIndex(e => env > e);
                if (existing != -1)
                    Data[existing] = env;
            }
            stream.Close();
            stream.Dispose();
            if (errors == 0)
                SystemSounds.Asterisk.Play();
            return errors;
        }
        public static void ExportValues(string filepath) {
            var stream = new StreamWriter(File.Open(filepath, FileMode.Create, FileAccess.Write, FileShare.None));
            var sorted = Data.Where(e => e.Target == EVT.User).Concat(Data.Where(e => e.Target == EVT.Machine));
            foreach (var e in sorted) {
                stream.WriteLine(@"{0}{1}\{2}={3}",
                    e.Target.ToString(),
                    e.Expandable == true ? "*" : "",
                    e.Name,
                    Convert.ToBase64String(Encoding.Unicode.GetBytes(e.Value))
                );
            }
            stream.Flush();
            stream.Close();
            stream.Dispose();
            SystemSounds.Asterisk.Play();
        }
    }

    public class EnvValue {
        public EVT Target { get; set; }
        public string Name { get; set; }
        public bool? Expandable { get; set; }
        public virtual string Value { get; set; }
        public bool MarkedForRemoval { get; set; }
        public object Tag;
        public bool Changed = false;

        public EnvValue() { }
        public EnvValue(EnvValue from) {
            Name = from.Name;
            Target = from.Target;
            Value = from.Value;
        }

        public static bool operator >(EnvValue a, EnvValue b)
            => a.Target == b.Target && a.Name == b.Name;
        public static bool operator <(EnvValue a, EnvValue b)
            => !(a > b);
    }

    /*public class EnvListValue : EnvValue {
        public override string Value {
            get {
                sb.Clear();
                int len = List.Count;
                for (int i = 0; i < len; i++) {
                    sb.Append(List[i]);
                    if (i < len - 1)
                        sb.Append(sep);
                }
                return sb.ToString();
            }
            set {
                List.Clear();
                List.AddRange(value.Split(new[] { sep }, StringSplitOptions.RemoveEmptyEntries));
            }
        }
        public string Separator {
            get => sep;
            set {
                string old = Value;
                sep = value;
                Value = old;
            }
        }
        public readonly List<string> List = new List<string>();
        string sep = ";";
        StringBuilder sb = new StringBuilder();
    }*/
}
