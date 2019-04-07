using Micro.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysEdit {
    using EVT = EnvironmentVariableTarget;

    public partial class FormEdit : Form {
        const EVT NEW_INDEX = EVT.User;
        const string NEW_NAME = "MyVariable";
        const int MAX_LENGTH = 32767;
        public delegate bool CanSaveHandler(FormEdit sender, EVT newTarget, string newName, out string error);
        public delegate void SaveHandler(FormEdit sender, bool changes);
        public event CanSaveHandler CanSave;
        public event SaveHandler Saved;
        static Dictionary<string, Func<FormEdit, string>> suggestions =
            new Dictionary<string, Func<FormEdit, string>>() {
                {"path",    (a) => { a.pathAutocomplete(); return ";"; } },
                {"pathext", (a) => { a.pathAutocomplete(); return ";"; } },
                {"tmp",     (a) => { a.pathAutocomplete(); return ""; } },
                {"temp",    (a) => { a.pathAutocomplete(); return ""; } },
            };

        static readonly Regex allowedChars = new Regex(@"[A-Za-z0-9#$'()*+,\-.?@[\]_`{}~ ]");
        bool nameChecked = false;
        bool nameProblem = false;
        bool lengthProblem = false;
        EnvValue value;
        int oldPage = 0;
        int diffW, diffH;
        string original, expanded;

        /*protected override CreateParams CreateParams {
            get {
                var parms = base.CreateParams;
                //parms.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                //parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }*/

        public FormEdit(EnvValue value = null) {
            InitializeComponent();
            this.value = value;
            name.TextTransformer += validateName;
            text.TextChanged += textChanged;
            listEditorExEdit.InputTextReq += listEditorExEdit_InputTextReq;
            listEditorExEdit.SizeHint += listEditorExEdit_SizeHint;
            listEditorExEdit.SplitterHintReq += listEditorExEdit_SplitterHintReq;
            listEditorExEdit.SaveUrgent += toRawText;
            listEditorExEdit.LengthSignal += showLength;
            diffW = (Width - listEditorExEdit.Width) + 20;
            diffH = (Height - listEditorExEdit.Height) + 100;
            setTitle();
            load();
        }
        void FormEdit_Shown(object sender, EventArgs e)
            => Program.EnsureVisible(this);
        void FormEdit_KeyUp(object sender, KeyEventArgs e) {
            if (this.ContainsFocus && this.Focused && e.KeyCode == Keys.Escape)
                this.Close();
        }
        void FormEdit_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = !save(true);
        }
        void validateName(AutoTextBox from, StringBuilder text, ref int caret) {
            bool inside(char c, char a, char b)
                => c >= a && c <= b;
            int len = text.Length;
            bool changed = false;
            for (int i = 0; i < len; i++) {
                char c = text[i];
                if ((i == 0 && inside(c, '0', '9')) || !allowedChars.IsMatch(c+"")) {
                    if (nameProblem)
                        return;
                    if (nameChecked) {
                        text.Remove(i, 1);
                        i--;
                        len--;
                        caret--;
                        changed = true;
                    } else {
                        nameProblem = true;
                        nameChecked = true;
                        return;
                    }
                }
            }
            if (!changed) {
                nameProblem = false;
                showProblem();
            }
            applyFolderIcons();
            btnOK.Enabled =
            btnApply.Enabled =
            !nameProblem && len > 0;
        }
        void textChanged(object sender, EventArgs e) {
            /*string txt = text.Text;
            bool isList = txt[txt.Length - 1] == ';' || txt.Contains(';');
            if (wasList != isList) {
                if (isList)
                    tabs.Controls.Add(_tabList);
                else
                    tabs.Controls.Remove(_tabList);
            }
            wasList = isList;*/
            showLength();
        }
        void tabChange(object sender, EventArgs e) {
            switch (oldPage) {
                case 0:
                    toList();
                    break;
                case 1:
                    toRawText();
                    break;
            }
            oldPage = tabs.SelectedIndex;
        }
        void category_SelectedIndexChanged(object sender, EventArgs e)
            => chkExpand.Enabled = category.SelectedIndex != 0;
        void chkShowExp_CheckedChanged(object sender, EventArgs e) {
            bool exp = chkShowExp.Checked;
            text.ReadOnly = exp;
            listEditorExEdit.EnableEdit = !exp;

            if (tabs.SelectedIndex == 1)
                toRawText();

            if (exp) {
                original = text.Text;
                expanded = Environment.ExpandEnvironmentVariables(original);
                text.Text = expanded;
                toList();
            } else {
                text.Text = original;
                toList();
            }
        }
        string listEditorExEdit_InputTextReq()
            => chkShowExp.Checked ? expanded : text.Text;
        string listEditorExEdit_SplitterHintReq() {
            var t = name.Text.ToLower();
            var func = suggestions.ContainsKey(t) ? suggestions[t] : null;
            return func?.Invoke(this);
        }
        void listEditorExEdit_SizeHint(Size obj) {
            var r = Screen.GetWorkingArea(DesktopLocation);
            Width = Math.Min(obj.Width + diffW, r.Width - 4);
            Height = Math.Min(obj.Height + diffH, r.Height - 4);
            //small hack
            applyFolderIcons();
        }
        void btnReload_Click(object sender, EventArgs e) {
            load();
        }
        void btnCancel_Click(object sender, EventArgs e) {
            Close();
        }
        void btnOK_Click(object sender, EventArgs e) {
            save();
            Close();
        }
        void btnApply_Click(object sender, EventArgs e) {
            save();
        }

        void setTitle() {
            if (value != null)
                Text = $"{value.Target} - {value.Name}";
            else
                Text = "New value";
        }
        void load() {
            if (value == null) {
                category.SelectedIndex = (int)NEW_INDEX;
                name.Text = NEW_NAME;
            } else {
                category.SelectedIndex = (int)value.Target;
                name.Text = value.Name;
                var e = value.Expandable;
                chkExpand.Checked = e ?? false;
                chkExpand.Enabled = value.Target != EVT.Process;
                text.Text = value.Value;
            }
            showProblem();
            if (toList())
                oldPage = tabs.SelectedIndex = 1;
        }
        bool save(bool askFirst = false) { //returns true if a choice is made
            if (tabs.SelectedIndex == 1)
                toRawText();
            var t = ((EnumItem<EVT>)category.SelectedItem).value;
            var n = name.Text;
            var e = chkExpand.Checked;
            var v = chkShowExp.Checked ? original : text.Text;
            if (value != null
             ? (value.Target == t && value.Name == n && (value.Expandable == null || value.Expandable == e) && value.Value == v)
             : (t == NEW_INDEX && n == NEW_NAME && !e && v.Length == 0)) {
                Saved?.Invoke(this, false);
                return true;
            }
            DialogResult r;
            if (askFirst) {
                r = MessageBox.Show(
                    "This value has been modified. Save the changes?",
                    Program.NAME,
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );
                if (r != DialogResult.Yes)
                    return r == DialogResult.No;
            }
            string err = "";
            bool? canSave = CanSave?.Invoke(this, t, n, out err);
            if (canSave == null) {
                r = MessageBox.Show(
                    "Before-save check not assigned! Save anyways?",
                    Program.NAME,
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Warning);
            } else if (!canSave.Value) {
                r = MessageBox.Show(
                    err + ". Save anyways?",
                    Program.NAME,
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Error);
            } else
                r = DialogResult.OK;
            if (r != DialogResult.OK)
                return false;
            value = value ?? new EnvValue();
            Tag = value;
            value.Target = t;
            value.Name = n;
            value.Expandable = e;
            value.Value = v;
            setTitle();
            showProblem();
            value.Changed = true;
            Saved?.Invoke(this, true);
            return true;
        }
        void showProblem() {
            if (nameProblem) {
                Text += " (Invalid var name, solve now!)";
                name.BackColor = SystemColors.Info;
                return;
            }
            name.BackColor = SystemColors.Window;
            if (lengthProblem) {
                Text += " (Too long content, solve now!)";
                return;
            }
        }
        bool toList() => listEditorExEdit.toList();
        void toRawText() {
            var t = listEditorExEdit.toRawText();
            if (t != null)
                text.Text = t;
        }
        void applyFolderIcons() {
            string t = name.Text;
            bool has(string c)
                => t.FirstOccurrence(c, false) != -1;
            listEditorExEdit.assignFoldersIcons(
                !has("pathext") && !(has("last") && has("update")) && (
                    has("path") || has("dir") || has("tmp") || has("temp") ||
                    has("folder") || has("home") || has("location")
                )
            );
        }
        void showLength(int forced = -1) {            
            int len = forced >= 0 ? forced : text.Text.Length;
            lblLength.Text = $@"Length: {len}/{MAX_LENGTH}";
            lengthProblem = len > MAX_LENGTH;
            lblLength.ForeColor = lengthProblem ? Color.Red : SystemColors.ControlText;
        }
        void pathAutocomplete() {
            text.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            text.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
            listEditorExEdit.pathAutocomplete();
        }
    }
}
