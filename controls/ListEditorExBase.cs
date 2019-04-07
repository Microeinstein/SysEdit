using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using Micro.WinForms;
using System.Media;

namespace SysEdit {
    using EVT = EnvironmentVariableTarget;

    public partial class ListEditorExBase : ListEditor {
        public event Action<bool> UnsavedChangesUpdated;
        Dictionary<EnvValue, FormEdit> edits = new Dictionary<EnvValue, FormEdit>();
        Dictionary<EVT, ListViewGroup> groups = new Dictionary<EVT, ListViewGroup>();
        Font consolas = new Font("Consolas", 9);

        public FileDialog dlgOpen;
        public SaveFileDialog dlgSave;
        protected ColumnHeader cname, cexp;
        protected bool UnsavedChanges {
            get => _uc;
            set {
                if (value == _uc)
                    return;
                _uc = value;
                UnsavedChangesUpdated?.Invoke(value);
            }
        }
        bool _uc;

        public ListEditorExBase() : base() {
            InitializeComponent();
            InitializeMyComponent();
            ApplyIcons(Program.icons);
            btnLoad.Text = "Reload";
            btnLoad.Click   += btnLoad_Click;
            btnSave.Click   += btnSave_Click;
            btnExport.Click += btnExport_Click;
            btnImport.Click += btnImport_Click;
            addClick         = addVarialbe;
            removeClick      = removeVariables;
            editClick        = editVariable;
            updateGUIEx     += base_updateGUIEx;
        }
        void InitializeMyComponent() {
            this.list.SuspendLayout();
            this.SuspendLayout();
            
            cname = AddColumn(0, "Name", 80);
            cexp = AddColumn(1, "Exp.", 36, HorizontalAlignment.Right);
            // 
            // list
            // 
            this.list.LabelEdit = false;
            this.list.BorderStyle = BorderStyle.Fixed3D;
            this.list.Sorting = SortOrder.Ascending;
            this.list.ListViewItemSorter = new ListViewItemComparer();

            VisibleLoad = VisibleBackup = VisibleEdit = 
            EnableLoad = EnableBackup = EnableEdit = true;
            UpdateGUI();
            UpdateAutomatically = true;
            
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        void btnLoad_Click(object sender, EventArgs e) {
            if (!checkUnsaved())
                return;
            ValuesHelper.ReadValues();
            readVariables();
        }
        void btnSave_Click(object sender, EventArgs e) {
            ValuesHelper.SaveValues();
            UnsavedChanges = false;
        }
        void btnExport_Click(object sender, EventArgs e) {
            if (dlgSave?.ShowDialog() == DialogResult.OK) {
                ValuesHelper.ExportValues(dlgSave.FileName);
            }
        }
        void btnImport_Click(object sender, EventArgs e) {
            if (dlgOpen?.ShowDialog() == DialogResult.OK) {
                int err = ValuesHelper.ImportValues(dlgOpen.FileName);
                if (err > 0) {
                    MessageBox.Show(
                        $@"Unable to import {err} environment variables.",
                        "Warning",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                }
                readVariables();
            }
        }
        void formEdit_FormClosed(object sender, FormClosedEventArgs e) {
            var fe = (FormEdit)sender;
            var v = (EnvValue)fe.Tag;
            if (v != null && edits.ContainsKey(v))
                edits.Remove(v);
            fe.FormClosed -= formEdit_FormClosed;
            fe.Saved -= formEdit_Saved;
            ParentForm.Activate();
        }
        void formEdit_Saved(FormEdit sender, bool changes) {
            if (!changes)
                return;
            var v = (EnvValue)sender.Tag;
            list.BeginUpdate();
            updateVariable(v);
            reapplyVariable(v.Name);
            list.Sort();
            list.EndUpdate();
        }
        bool formEdit_CanSave(FormEdit sender, EVT newTarget, string newName, out string error) {
            error = "";
            var self = (EnvValue)sender.Tag;
            bool changedTarget = self == null || self.Target != newTarget;
            bool changedName = self == null || self.Name != newName;
            if (!(changedTarget || changedName))
                return true;
            string genError()
                => $"There's already a value called \"{newName}\" of type {newTarget}. ";
            foreach (var value in ValuesHelper.Data) {
                if (value.MarkedForRemoval || self == value)
                    continue;
                bool sameTarget = value.Target == newTarget,
                     sameName = value.Name == newName;
                if (changedTarget && changedName && sameTarget && sameName) {
                    error = genError() + "You're probably messing up the system..";
                    return false;
                } else if (!(changedTarget || sameTarget) || !(changedName || sameName))
                    continue;
                else if (!changedName && sameName) {
                    error = genError() + "You probably forgot to change the name";
                    return false;
                }
            }
            return true;
        }
        void base_updateGUIEx()
            => sep1.Visible = EnableEdit;

        public void readVariables() {
            UnsavedChanges = false;
            list.BeginUpdate();
            list.Items.Clear();
            list.Groups.Clear();
            groups.Clear();
            //viewPath.Items.Clear();
            foreach (var g in ValuesHelper.targets) {
                var gr = new ListViewGroup(g == EVT.User ? Environment.UserName : g.ToString());
                groups[g] = gr;
                list.Groups.Add(gr);
            }
            //ListViewGroupsEx.SetGroupCollapse(list, ListViewGroupsEx.GroupState.COLLAPSED);
            foreach (var value in ValuesHelper.Data) {
                var lvi = new ListViewItem();
                value.Tag = lvi;
                lvi.Tag = value;
                updateVariable(value);
                list.Items.Add(lvi);
            }
            list.Sort();
            list.EndUpdate();
            //foreach (var item in ValuesHelper.Path[evt]) {
            //    var lvi = viewPath.Items.Add(item);
            //    viewPath.Groups[i].Items.Add(lvi);
            //    lvi.ImageIndex = Directory.Exists(item) ? 0 : 1;
            //}
            cname.Width = -1;
            cvalue.Width = -1;
        }
        public void updateVariable(EnvValue v) {
            ListViewItem lvi = (ListViewItem)v.Tag;
            var data = ValuesHelper.Data;
            if (!data.Contains(v)) {
                var toBeRemoved = data.FirstOrDefault(e => e.Name == v.Name && e.MarkedForRemoval);
                if (toBeRemoved != null)
                    data.Remove(toBeRemoved);
                ValuesHelper.Data.Add(v);
            }
            if (lvi == null) {
                v.Tag = lvi = new ListViewItem();
                lvi.Tag = v;
                list.Items.Add(lvi);
            }
            if (v.Changed)
                UnsavedChanges = true;
            lvi.Group = null;
            lvi.Group = groups[v.Target];
            lvi.Text = v.Name;
            lvi.Font = consolas;
            lvi.UseItemStyleForSubItems = true;
            string t = v.Value;
            if (t.Length > 260) //listview limit
                t = t.Substring(0, 256) + "...";
            if (lvi.SubItems.Count < 3) {
                lvi.SubItems.Add(v.Expandable == true ? "*" : "");
                lvi.SubItems.Add(t);
            } else {
                lvi.SubItems[1].Text = v.Expandable == true ? "*" : "";
                lvi.SubItems[2].Text = t;
            }
        }
        public bool editVariable() {
            var si = list.SelectedItems;
            if (si.Count == 0)
                return false;
            foreach (ListViewItem lvi in si) {
                var v = (EnvValue)lvi.Tag;
                FormEdit f;
                if (!edits.ContainsKey(v)) {
                    edits[v] = f = new FormEdit(v) { Tag = v };
                    f.FormClosed += formEdit_FormClosed;
                    f.CanSave += formEdit_CanSave;
                    f.Saved += formEdit_Saved;
                    f.Show(this);
                } else
                    edits[v].Activate();
            }
            return true;
        }
        public void addVarialbe() {
            FormEdit f;
            f = new FormEdit();
            f.FormClosed += formEdit_FormClosed;
            f.CanSave += formEdit_CanSave;
            f.Saved += formEdit_Saved;
            f.Show(this);
        }
        public bool removeVariables() {
            var si = list.SelectedItems;
            if (si.Count == 0)
                return false;
            var r = MessageBox.Show(
                $"Warning: this will remove {si.Count} variables. Are you sure?",
                Program.NAME,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );
            if (r != DialogResult.Yes)
                return false;
            var cpy = si.ToArray<ListViewItem>();
            foreach (var item in cpy) {
                list.Items.Remove(item);
                var v = (EnvValue)item.Tag;
                v.MarkedForRemoval = true;
                reapplyVariable(v.Name);
            }
            UnsavedChanges = true;
            return true;
        }
        public bool checkUnsaved() { //returns true if a choice is made
            if (!UnsavedChanges)
                return true;
            var r = MessageBox.Show(
                "There are some unsaved changes. Do you want to save them?",
                Program.NAME,
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning
            );
            if (r != DialogResult.Yes)
                return r == DialogResult.No;
            btnSave.PerformClick();
            return true;
        }
        void reapplyVariable(string name) {
            var sameName = ValuesHelper.Data.Where(
                e => !e.MarkedForRemoval && e.Name == name && e.Target != EVT.Process
            ).ToArray();
            var result =
                sameName.FirstOrDefault(e => e.Target == EVT.User) ??
                sameName.FirstOrDefault(e => e.Target == EVT.Machine);
            Environment.SetEnvironmentVariable(name, result?.Value ?? null, EVT.Process);
        }

        class ListViewItemComparer : IComparer {
            public int Compare(object x, object y)
                => ((ListViewItem)x).Text.CompareTo(((ListViewItem)y).Text);
        }
    }
}
