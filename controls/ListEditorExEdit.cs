using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Micro.WinForms;

namespace SysEdit {
    public partial class ListEditorExEdit : ListEditor {
        static ImageList images = new ImageList();
        public event Func<string> InputTextReq, SplitterHintReq;
        public event Action<Size> SizeHint;
        public event Action<int> LengthSignal;
        public event Action SaveUrgent;

        public bool isPathList;
        protected ToolStripLabel label3;
        protected ToolStripTextBox splitchar;
        protected ToolStripButton btnSplit;
        protected ToolStripSeparator sepS;
        //TextBox editor;
        StringBuilder rawBuffer = new StringBuilder();
        int prefWidth;
        bool autoChar = true,
             loading = false,
             unchanged = false;

        static ListEditorExEdit() {
            images.ImageSize = new Size(16, 16);
            images.ColorDepth = ColorDepth.Depth32Bit;
            images.Images.Add("folder", Properties.Resources.folder);
            images.Images.Add("file", Properties.Resources.document);
            images.Images.Add("missing", Properties.Resources.exclamation_red);
        }
        public ListEditorExEdit() {
            InitializeComponent();
            InitializeMyComponent();
            ApplyIcons(Program.icons);
            prefWidth = TextRenderer.MeasureText(cvalue.Text, list.Font).Width + 20;
            //list.AfterLabelEdit += list_AfterLabelEdit;
            updateGUIEx += base_updateGUIEx;
        }
        void InitializeMyComponent() {
            label3 = new ToolStripLabel();
            splitchar = new ToolStripTextBox();
            btnSplit = new ToolStripButton();
            sepS = new ToolStripSeparator();
            //editor = new TextBox();
            toolstrip.SuspendLayout();
            list.SuspendLayout();
            table.SuspendLayout();
            SuspendLayout();

            // 
            // toolstrip
            // 
            int from = 0;
            toolstrip.Items.Insert(from++, label3);
            toolstrip.Items.Insert(from++, splitchar);
            toolstrip.Items.Insert(from++, btnSplit);
            toolstrip.Items.Insert(from++, sepS);
            // 
            // label3
            // 
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(47, 22);
            label3.Text = "Splitter:";
            // 
            // splitchar
            // 
            splitchar.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            splitchar.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList;
            splitchar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            splitchar.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            splitchar.Name = "splitchar";
            splitchar.Size = new System.Drawing.Size(25, 25);
            splitchar.Text = ";";
            splitchar.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            splitchar.TextChanged += splitchar_TextChanged;
            // 
            // btnSplit
            // 
            btnSplit.Image = global::SysEdit.Properties.Resources.scissors;
            btnSplit.ImageTransparentColor = System.Drawing.Color.Magenta;
            btnSplit.Name = "btnSplit";
            btnSplit.Size = new System.Drawing.Size(50, 22);
            btnSplit.Text = "Split";
            btnSplit.Click += btnSplit_Click;
            // 
            // sepS
            // 
            sepS.Name = "sepS";
            sepS.Size = new System.Drawing.Size(6, 25);
            /*
            // editor
            // 
            editor.Visible = false;
            editor.BorderStyle = BorderStyle.FixedSingle;
            editor.Margin = new Padding(0);
            editor.AcceptsReturn = false;*/
            // 
            // list
            // 
            list.Items.AddedItem += list_Changed;
            list.Items.RemovedItem += list_Changed;
            list.Items.MovedItems += list_Changed;
            list.Font = splitchar.Font;
            list.SmallImageList = images;
            list.integralScroll = true;
            list.SubItemEditing += list_SubItemEditing;
            list.SubItemEndEditing += list_SubItemEndEditing;
            listChanged += lengthSignal;
            
            //Controls.Add(editor);

            VisibleEdit = VisibleMove =
            EnableEdit = EnableMove = true;
            UpdateGUI();
            UpdateAutomatically = true;


            toolstrip.ResumeLayout(false);
            list.ResumeLayout(false);
            table.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        void list_Changed(object sender, object whatever) {
            unchanged = false;
            lengthSignal();
        }
        void list_SubItemEditing(object sender, SubItemEditingEA e) {
            unchanged = false;
            //e.Editor = editor;
        }
        void list_SubItemEndEditing(object sender, SubItemEndEditingEA e) {
            unchanged = false;
            updateIcon(e.Item, (string)e.Value);
            lengthSignal();
        }
        /*void list_AfterLabelEdit(object sender, LabelEditEventArgs e) {
            unchanged = false;
            var item = list.Items[e.Item];
            Task.Run(async () => {
                await Task.Delay(10);
                ParentForm.Invoke((Action<ListViewItem>)updateIcon, item);
            });
        }*/
        void splitchar_TextChanged(object sender, EventArgs e) {
            btnSplit.Enabled = splitchar.Text.Length > 0;
            autoChar = loading;
            lengthSignal();
        }
        void btnSplit_Click(object sender, EventArgs e) {
            SaveUrgent?.Invoke();
            toList();
        }
        void base_updateGUIEx()
            => sepS.Visible = EnableEdit;

        public bool toList() {
            ChangingList = true;
            unchanged = true;
            string text = InputTextReq();
            if (autoChar) {
                loading = true;
                var sugg = SplitterHintReq?.Invoke();
                if (sugg == null) {
                    var grps = text.Where(c => Program.invalidPathChars.Contains(c)).ToLookup(c => c).ToArray();
                    char theChar = ';';
                    if (grps.Length > 0) {
                        var counts = grps.ToLookup(g => g.Count());
                        theChar = counts[counts.Max(g => g.Key)].FirstOrDefault()?.Key ?? theChar;
                    }
                    splitchar.Text = theChar + "";
                } else
                    splitchar.Text = sugg;
                loading = false;
            }
            if (unchanged = splitchar.Text.Length == 0)
                return false;
            list.Items.Clear();
            string[] lines = text.Split(new[] { splitchar.Text }, StringSplitOptions.None);
            int? minY = null,
                 maxY = null;
                 //minX = null,
                 //maxX = null;
            foreach (var l in lines) {
                var lvi = list.Items.Add(l);
                var y = lvi.Index * list.Font.Height;
                //minX = minX == null ? p.X : Math.Min(minX.Value, p.X);
                minY = minY == null ? y : Math.Min(minY.Value, y);
                //maxX = maxX == null ? p.X : Math.Max(maxX.Value, p.X);
                maxY = maxY == null ? y : Math.Max(maxY.Value, y);
            }
            //cvalue.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            //var realContentWidth = cvalue.Width - 2;
            cvalue.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
            int width = cvalue.Width = Math.Max(cvalue.Width, prefWidth) + 10;
            SizeHint?.Invoke(new Size(width, maxY.Value - minY.Value));
            ChangingList = false;
            lengthSignal();
            return true;
        }
        public string toRawText() {
            if (unchanged)
                return null;
            rawBuffer.Clear();
            bool notOne = false;
            foreach (ListViewItem lvi in list.Items) {
                if (notOne || !(notOne = true))
                    rawBuffer.Append(splitchar.Text);
                rawBuffer.Append(lvi.Text);
            }
            return rawBuffer.ToString();
        }
        public void assignFoldersIcons(bool isPathList) {
            this.isPathList = isPathList;
            foreach (ListViewItem item in list.Items)
                updateIcon(item);
        }
        public void pathAutocomplete() {
            list.editor.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            list.editor.AutoCompleteSource = AutoCompleteSource.FileSystemDirectories;
        }
        void updateIcon(ListViewItem lvi, string newText = null) {
            int icon = (isPathList ? 1 : 0);
            string exp = Environment.ExpandEnvironmentVariables(newText ?? lvi.Text);
            if (string.IsNullOrWhiteSpace(exp) || !Program.IsValidPath(exp))
                goto showIcon;
            string root = Directory.GetDirectoryRoot(exp);
            if (root != @"C:\" && exp[0] == '\\')
                exp = @"C:" + exp;
            if (Directory.Exists(exp))
                icon = 2;
            else if (File.Exists(exp))
                icon = 3;
            showIcon:
            switch (icon) {
                case 1:
                    lvi.ImageKey = "missing";
                    break;
                case 2:
                    lvi.ImageKey = "folder";
                    break;
                case 3:
                    lvi.ImageKey = "file";
                    break;
                default:
                    lvi.ImageKey = null;
                    break;
            }
        }
        void lengthSignal() {
            if (ChangingList)
                return;
            int len = Math.Max(0, list.Items.Count - 1) * splitchar.Text.Length;
            foreach (ListViewItem item in list.Items)
                len += item.Text.Length;
            LengthSignal?.Invoke(len);
        }
    }
}
