using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SysEdit {
    public class INIFile {
        [DllImport("kernel32.dll")]
        public static extern long WritePrivateProfileString(
            string section,
            string key,
            string val,
            string filePath);

        [DllImport("kernel32.dll")]
        public static extern uint GetPrivateProfileString(
            string section,
            string key,
            string def,
            StringBuilder retVal,
            uint size,
            string filePath);

        public readonly string path;
        public bool hangReadIfMissing = false,
                    hangWriteIfMissing = false;
        StringBuilder temp = new StringBuilder(65535);

        public object this[string section, string key, TypeCode cast = TypeCode.String] {
            get => Read(section, key, cast);
            set => Write(section, key, value);
        }


        public INIFile(string path) {
            this.path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public object Read(string section, string key, TypeCode cast = TypeCode.String) {
            if (!File.Exists(path)) {
                if (hangReadIfMissing)
                    throw new FileNotFoundException();
                else
                    return null;
            }
            temp.Clear();
            GetPrivateProfileString(section, key, "", temp, 65535, path);
            return Convert.ChangeType(temp.Length == 0 ? GetDefault(Type.GetType("System." + cast)) : temp.ToString(), cast);
        }
        public void Write(string section, string key, object value) {
            if (hangWriteIfMissing && !File.Exists(path))
                throw new FileNotFoundException();
            else
                Program.NewTextFile(path);
            WritePrivateProfileString(section, key, (string)Convert.ChangeType(value, TypeCode.String), path);
        }
        public void DeleteSection(string section)
            => this[section ?? throw new ArgumentNullException(nameof(section)), null] = null;
        public void DeleteKey(string section, string key)
            => this[
                section ?? throw new ArgumentNullException(nameof(section)),
                key ?? throw new ArgumentNullException(nameof(key))
            ] = null;

        public static object GetDefault(Type type) {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }
    }
}
