using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SysEdit {
    using EVT = EnvironmentVariableTarget;

    public partial class FormBase : Form {
        Dictionary<EnvValue, FormEdit> edits = new Dictionary<EnvValue, FormEdit>();
        Dictionary<EVT, ListViewGroup> groups = new Dictionary<EVT, ListViewGroup>();
        Font consolas = new Font("Consolas", 9);
        bool uc = false;

        public FormBase() {
            InitializeComponent();
            listEditorExBase.UnsavedChangesUpdated += unsavedChangesUpdated;
            //toolstrip.fixItemMetrics();

            //var sml = viewPath.SmallImageList = new ImageList();
            //sml.ColorDepth = ColorDepth.Depth32Bit;
            //sml.ImageSize = new Size(16, 16);
            //sml.Images.Add(Properties.Resources.folder);
            //sml.Images.Add(Properties.Resources.exclamation_red);
            if (!Program.IsElevated()) {
                var r = MessageBox.Show(
                    "This tool does NOT have Administrator rights and thus it won't be able to write Machine-side variables. What's your choice?",
                    Program.NAME,
                    MessageBoxButtons.AbortRetryIgnore,
                    MessageBoxIcon.Warning
                );
                if (r == DialogResult.Retry) {
                    Process.Start(new ProcessStartInfo(Environment.GetCommandLineArgs()[0]) {
                        Verb = "RunAs",
                        UseShellExecute = true,
                    });
                }
                if (r != DialogResult.Ignore)
                    Environment.Exit(0);
            }
        }
        void FormBase_Load(object sender, EventArgs e) {
            listEditorExBase.readVariables();
            listEditorExBase.dlgOpen = dlgOpen;
            listEditorExBase.dlgSave = dlgSave;
        }
        void FormBase_Shown(object sender, EventArgs e)
            => Program.EnsureVisible(this);
        void FormBase_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = !listEditorExBase.checkUnsaved();
        }
        void unsavedChangesUpdated(bool obj) {
            if (obj == uc)
                return;
            if (uc = obj)
                Text = Program.NAME + '*';
            else
                Text = Program.NAME;
        }
    }
}
