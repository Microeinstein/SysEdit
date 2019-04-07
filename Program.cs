using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Micro.WinForms;

namespace SysEdit {
    static class Program {
        public static string AppDir = AppDomain.CurrentDomain.BaseDirectory;
        internal static readonly ListEditorIcons icons = new ListEditorIcons() {
            Load       = Properties.Resources.arrow_circle_315,
            Save       = Properties.Resources.disk,
            Export     = Properties.Resources.application_export,
            Import     = Properties.Resources.application_import,
            Add        = Properties.Resources.plus_circle,
            Remove     = Properties.Resources.cross_circle,
            Edit       = Properties.Resources.pencil,
            MoveUp     = Properties.Resources.arrow_090,
            MoveDown   = Properties.Resources.arrow_270,
            MoveTop    = Properties.Resources.arrow_stop_090,
            MoveBottom = Properties.Resources.arrow_stop_270,
        };
        public static readonly char[] invalidPathChars = Path.GetInvalidPathChars();
        public const string NAME = nameof(SysEdit);
        const int WM_SETREDRAW = 11;
        const int caseDiff = 'a' - 'A';

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, bool wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, UIntPtr wParam, string lParam, uint fuFlags, uint uTimeout, out UIntPtr lpdwResult);

        public static void SuspendDrawing(this Control parent) {
            SendMessage(parent.Handle, WM_SETREDRAW, false, 0);
        }
        public static void ResumeDrawing(this Control parent) {
            SendMessage(parent.Handle, WM_SETREDRAW, true, 0);
            parent.Refresh();
        }
        public static T[] ToArray<T>(this ICollection items) {
            var ret = new T[items.Count];
            items.CopyTo(ret, 0);
            return ret;
        }
        public static Size GetSize(this string text, Font f)
            => TextRenderer.MeasureText(text, f);
        public static int GetMaxWidth(this ICollection c, Font f)
            => c.ToArray<object>().Concat(new[] { "042" }).Select(i => i.ToString().GetSize(f).Width).Max();
        public static void MoveToEnd<T>(this T[] a, T b) {
            int count = 0;
            int len = a.Length;
            for (int i = 0; i < len; i++) {
                if (count > 0)
                    a[i - count] = a[i];
                if (a[i].Equals(b))
                    count++;
            }
            len = a.Length;
            for (int i = len - count; i < len; i++)
                a[i] = b;
        }
        public static bool IsValidPath(string s)
            => !invalidPathChars.Any(c => s.Contains(c));
        public static IEnumerable<char> AsEnum(this StringBuilder sb) {
            int len = sb.Length;
            for (int i = 0; i < len; i++)
                yield return sb[i];
        }
        public static int FirstOccurrence(this IEnumerable<char> enumerable, string txt, bool caseSensitive = true) {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));
            if (txt == null)
                throw new ArgumentNullException(nameof(txt));
            int len = txt.Length;
            if (len == 0)
                return 0;
            char invertCase(ref char c)
                => (c >= 'a' && c <= 'z') ? (char)(c - caseDiff)
                 : (c >= 'A' && c <= 'Z') ? (char)(c + caseDiff)
                 : c;
            int pos = -1, eq = 0;
            foreach (var c in enumerable) {
                pos++;
                recheck:
                char c2 = txt[eq];
                if (c == c2 || (!caseSensitive && c == invertCase(ref c2)))
                    eq++;
                else if (eq > 0) {
                    eq = 0;
                    goto recheck;
                }
                if (eq == len)
                    return pos - len + 1;
            }
            return -1;
        }
        public static bool IsElevated()
            => WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
        public static void SendEnvironmentMessage()
            => SendMessageTimeout(
                (IntPtr)0xffff, //HWND_BROADCAST,
                0x1a,   //WM_SETTINGCHANGE,
                UIntPtr.Zero,
                "Environment",
                0x2,    //SMTO_ABORTIFHUNG
                5000,   //5 seconds
                out var _
            );
        public static void EnsureVisible(Form f) {
            var r = Screen.GetWorkingArea(f.DesktopLocation);
            var b = f.DesktopBounds;
            if (b.Right > r.Right)
                f.Left = r.Width - 2 - f.Width;
            if (b.Bottom > r.Bottom)
                f.Top = r.Height - 2 - f.Height;
        }
        public static string FromAppDir(string relativePath)
            => Path.IsPathRooted(relativePath) ? relativePath : Path.Combine(AppDir, relativePath);
        public static bool NewTextFile(string path) {
            bool e;
            path = FromAppDir(path);
            if (e = !File.Exists(path)) {
                var _str = File.CreateText(path);
                _str.Flush();
                _str.Close();
            }
            return e;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormBase());
        }
    }
    
    public enum EnumType {
        None,
        EVT
    }

    public class EnumCombo : EnumCombo<EnumType> {
        public static EnumConfig<EnumType> enumConfig = new EnumConfig<EnumType>(null, typeof(EnvironmentVariableTarget));
        public EnumCombo() : base(enumConfig) { }
    }
}
