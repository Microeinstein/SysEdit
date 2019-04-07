namespace SysEdit {
    partial class FormBase {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBase));
            this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
            this.dlgSave = new System.Windows.Forms.SaveFileDialog();
            this.listEditorExBase = new SysEdit.ListEditorExBase();
            this.SuspendLayout();
            // 
            // dlgOpen
            // 
            this.dlgOpen.FileName = "exported.env";
            this.dlgOpen.Filter = "Environment variables|*.env|All files|*.*";
            this.dlgOpen.InitialDirectory = "shell:desktop";
            this.dlgOpen.Multiselect = true;
            this.dlgOpen.Title = "Import environment variables";
            // 
            // dlgSave
            // 
            this.dlgSave.FileName = "exported.env";
            this.dlgSave.Filter = "Environment variables|*.env|All files|*.*";
            this.dlgSave.InitialDirectory = "shell:desktop";
            this.dlgSave.Title = "Export environment variables";
            // 
            // listEditorExBase
            // 
            this.listEditorExBase.AllowDuplicate = false;
            this.listEditorExBase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listEditorExBase.EditAfterDuplicate = false;
            this.listEditorExBase.EnableBackup = true;
            this.listEditorExBase.EnableEdit = true;
            this.listEditorExBase.EnableLoad = true;
            this.listEditorExBase.EnableMove = true;
            this.listEditorExBase.EnableSettings = false;
            this.listEditorExBase.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.listEditorExBase.Location = new System.Drawing.Point(0, 0);
            this.listEditorExBase.Margin = new System.Windows.Forms.Padding(0);
            this.listEditorExBase.Name = "listEditorExBase";
            this.listEditorExBase.Size = new System.Drawing.Size(832, 523);
            this.listEditorExBase.TabIndex = 0;
            this.listEditorExBase.VisibleBackup = true;
            this.listEditorExBase.VisibleEdit = true;
            this.listEditorExBase.VisibleLoad = true;
            this.listEditorExBase.VisibleMove = false;
            this.listEditorExBase.VisibleSettings = false;
            // 
            // FormBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 523);
            this.Controls.Add(this.listEditorExBase);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormBase";
            this.Text = "SysEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBase_FormClosing);
            this.Load += new System.EventHandler(this.FormBase_Load);
            this.Shown += new System.EventHandler(this.FormBase_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private ListEditorExBase listEditorExBase;
        private System.Windows.Forms.OpenFileDialog dlgOpen;
        private System.Windows.Forms.SaveFileDialog dlgSave;
    }
}

