namespace SysEdit {
    partial class FormEdit {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEdit));
            this.tabs = new System.Windows.Forms.TabControl();
            this.tabText = new System.Windows.Forms.TabPage();
            this.text = new System.Windows.Forms.TextBox();
            this.tabList = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.table1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.name = new Micro.WinForms.AutoTextBox();
            this.chkExpand = new System.Windows.Forms.CheckBox();
            this.chkShowExp = new System.Windows.Forms.CheckBox();
            this.lblLength = new System.Windows.Forms.Label();
            this.listEditorExEdit = new SysEdit.ListEditorExEdit();
            this.category = new SysEdit.EnumCombo();
            this.tabs.SuspendLayout();
            this.tabText.SuspendLayout();
            this.tabList.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.table1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.table1.SetColumnSpan(this.tabs, 3);
            this.tabs.Controls.Add(this.tabText);
            this.tabs.Controls.Add(this.tabList);
            this.tabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabs.Location = new System.Drawing.Point(3, 59);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(531, 242);
            this.tabs.TabIndex = 0;
            this.tabs.SelectedIndexChanged += new System.EventHandler(this.tabChange);
            // 
            // tabText
            // 
            this.tabText.Controls.Add(this.text);
            this.tabText.Location = new System.Drawing.Point(4, 24);
            this.tabText.Margin = new System.Windows.Forms.Padding(0);
            this.tabText.Name = "tabText";
            this.tabText.Padding = new System.Windows.Forms.Padding(0, 5, 5, 5);
            this.tabText.Size = new System.Drawing.Size(523, 214);
            this.tabText.TabIndex = 0;
            this.tabText.Text = "Raw text";
            this.tabText.UseVisualStyleBackColor = true;
            // 
            // text
            // 
            this.text.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.text.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList;
            this.text.BackColor = System.Drawing.SystemColors.Window;
            this.text.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.text.Font = new System.Drawing.Font("Consolas", 12F);
            this.text.Location = new System.Drawing.Point(0, 5);
            this.text.Margin = new System.Windows.Forms.Padding(0);
            this.text.Multiline = true;
            this.text.Name = "text";
            this.text.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.text.Size = new System.Drawing.Size(518, 204);
            this.text.TabIndex = 0;
            // 
            // tabList
            // 
            this.tabList.Controls.Add(this.listEditorExEdit);
            this.tabList.Location = new System.Drawing.Point(4, 24);
            this.tabList.Margin = new System.Windows.Forms.Padding(0);
            this.tabList.Name = "tabList";
            this.tabList.Padding = new System.Windows.Forms.Padding(0, 2, 2, 2);
            this.tabList.Size = new System.Drawing.Size(523, 214);
            this.tabList.TabIndex = 2;
            this.tabList.Text = "List";
            this.tabList.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.table1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.btnOK);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Controls.Add(this.btnReload);
            this.flowLayoutPanel1.Controls.Add(this.btnApply);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(213, 304);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(324, 29);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(3, 3);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(84, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnReload
            // 
            this.btnReload.Location = new System.Drawing.Point(165, 3);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(75, 23);
            this.btnReload.TabIndex = 2;
            this.btnReload.Text = "Reload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(246, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // table1
            // 
            this.table1.AutoSize = true;
            this.table1.ColumnCount = 3;
            this.table1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.table1.Controls.Add(this.label1, 0, 0);
            this.table1.Controls.Add(this.label2, 0, 1);
            this.table1.Controls.Add(this.category, 1, 0);
            this.table1.Controls.Add(this.name, 1, 1);
            this.table1.Controls.Add(this.chkExpand, 2, 0);
            this.table1.Controls.Add(this.chkShowExp, 2, 1);
            this.table1.Controls.Add(this.tabs, 0, 2);
            this.table1.Controls.Add(this.flowLayoutPanel1, 1, 3);
            this.table1.Controls.Add(this.lblLength, 0, 3);
            this.table1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.table1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.table1.Location = new System.Drawing.Point(0, 0);
            this.table1.Name = "table1";
            this.table1.RowCount = 4;
            this.table1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.table1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.table1.Size = new System.Drawing.Size(537, 333);
            this.table1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Category:";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name:";
            // 
            // name
            // 
            this.name.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.name.Font = new System.Drawing.Font("Consolas", 9F);
            this.name.Location = new System.Drawing.Point(100, 31);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(225, 22);
            this.name.TabIndex = 3;
            // 
            // chkExpand
            // 
            this.chkExpand.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkExpand.AutoSize = true;
            this.chkExpand.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExpand.Location = new System.Drawing.Point(417, 4);
            this.chkExpand.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.chkExpand.Name = "chkExpand";
            this.chkExpand.Size = new System.Drawing.Size(117, 19);
            this.chkExpand.TabIndex = 5;
            this.chkExpand.Text = "Can be expanded";
            this.chkExpand.UseVisualStyleBackColor = true;
            // 
            // chkShowExp
            // 
            this.chkShowExp.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.chkShowExp.AutoSize = true;
            this.chkShowExp.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkShowExp.Location = new System.Drawing.Point(425, 32);
            this.chkShowExp.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.chkShowExp.Name = "chkShowExp";
            this.chkShowExp.Size = new System.Drawing.Size(109, 19);
            this.chkShowExp.TabIndex = 5;
            this.chkShowExp.Text = "Show expanded";
            this.chkShowExp.UseVisualStyleBackColor = true;
            this.chkShowExp.CheckedChanged += new System.EventHandler(this.chkShowExp_CheckedChanged);
            // 
            // lblLength
            // 
            this.lblLength.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLength.AutoSize = true;
            this.lblLength.Location = new System.Drawing.Point(3, 311);
            this.lblLength.Name = "lblLength";
            this.lblLength.Size = new System.Drawing.Size(91, 15);
            this.lblLength.TabIndex = 6;
            this.lblLength.Text = "Length: 0/32767";
            // 
            // listEditorExEdit
            // 
            this.listEditorExEdit.AllowDuplicate = false;
            this.listEditorExEdit.AutoSize = true;
            this.listEditorExEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listEditorExEdit.EditAfterDuplicate = false;
            this.listEditorExEdit.EnableBackup = false;
            this.listEditorExEdit.EnableEdit = true;
            this.listEditorExEdit.EnableLoad = false;
            this.listEditorExEdit.EnableMove = true;
            this.listEditorExEdit.EnableSettings = false;
            this.listEditorExEdit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listEditorExEdit.Location = new System.Drawing.Point(0, 2);
            this.listEditorExEdit.Margin = new System.Windows.Forms.Padding(0);
            this.listEditorExEdit.Name = "listEditorExEdit";
            this.listEditorExEdit.Size = new System.Drawing.Size(521, 212);
            this.listEditorExEdit.TabIndex = 0;
            this.listEditorExEdit.VisibleBackup = false;
            this.listEditorExEdit.VisibleEdit = true;
            this.listEditorExEdit.VisibleLoad = false;
            this.listEditorExEdit.VisibleMove = true;
            this.listEditorExEdit.VisibleSettings = false;
            // 
            // category
            // 
            this.category.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.category.DropDownHeight = 65;
            this.category.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.category.Enums = SysEdit.EnumType.EVT;
            this.category.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.category.FormattingEnabled = true;
            this.category.IntegralHeight = false;
            this.category.Location = new System.Drawing.Point(100, 3);
            this.category.Name = "category";
            this.category.Size = new System.Drawing.Size(121, 22);
            this.category.TabIndex = 4;
            this.category.SelectedIndexChanged += new System.EventHandler(this.category_SelectedIndexChanged);
            // 
            // FormEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(537, 333);
            this.Controls.Add(this.table1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(545, 360);
            this.Name = "FormEdit";
            this.Text = "Edit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormEdit_FormClosing);
            this.Shown += new System.EventHandler(this.FormEdit_Shown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormEdit_KeyUp);
            this.tabs.ResumeLayout(false);
            this.tabText.ResumeLayout(false);
            this.tabText.PerformLayout();
            this.tabList.ResumeLayout(false);
            this.tabList.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.table1.ResumeLayout(false);
            this.table1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage tabText;
        private System.Windows.Forms.TextBox text;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TableLayoutPanel table1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Micro.WinForms.AutoTextBox name;
        private EnumCombo category;
        private System.Windows.Forms.TabPage tabList;
        private ListEditorExEdit listEditorExEdit;
        private System.Windows.Forms.CheckBox chkExpand;
        private System.Windows.Forms.CheckBox chkShowExp;
        private System.Windows.Forms.Label lblLength;
    }
}