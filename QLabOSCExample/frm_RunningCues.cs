using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QLabConnection;

namespace QLabOSCExample
{
    public partial class frm_RunningCues : Form
    {
        public frm_RunningCues()
        {
            InitializeComponent();
        }

        QLabServer server = new QLabServer();
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            server.ServerName = "r022849";
            if (txt_Passphrase.Text.Equals(""))
                server.CurrentWorkspace_Connect();
            else
                server.CurrentWorkspace_Connect(Convert.ToInt16(txt_Passphrase.Text));

            if (server.Workspaces.Count > 0)
            {
                pnl_RunningCues.Controls.Clear();
                pnl_PausedCues.Controls.Clear();

                currentWorkspace = server.Workspaces[0];

                tmr_Update.Enabled = true;
            }
        }

        Workspace currentWorkspace = null;
        private void tmr_Update_Tick(object sender, EventArgs e)
        {
            foreach (Cue cue in currentWorkspace.RunningCues)
            {
                if (pnl_RunningCues.Controls.Find(cue.uniqueID, false).Count() == 0)
                {
                    frm_Cue cueHolder = new frm_Cue(cue);
                    cueHolder.Name = cue.uniqueID;
                    pnl_RunningCues.Controls.Add(cueHolder);
                }
            }
            foreach (Cue cue in currentWorkspace.RunningOrPausedCues)
            {
                if (!cue.isRunning)
                {
                    if (pnl_RunningCues.Controls.Find(cue.uniqueID, false).Count() == 0)
                    {
                        frm_Cue cueHolder = new frm_Cue(cue);
                        cueHolder.Name = cue.uniqueID;
                        pnl_RunningCues.Controls.Add(cueHolder);
                    }
                }
            }
        }

        private void btn_GO_Click(object sender, EventArgs e)
        {
            server.CurrentWorkspace_Go();
        }
    }
}
