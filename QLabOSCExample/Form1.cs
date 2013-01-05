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
    public partial class Form1 : Form
    {
        #region Initialization Methods
        public Form1()
        {
            InitializeComponent();
        }

        QLabServer server;
        private void Form1_Load(object sender, EventArgs e)
        {
            server = new QLabServer();
            server.WorkspaceUpdated += new WorkspaceUpdatedHandler(server_WorkspaceUpdated);
            server.CueUpdated += new CueUpdatedHandler(server_CueUpdated);
        }

        void server_CueUpdated(string WorkspaceID, string CueID)
        {
            MessageBox.Show("CUE UPDATES");
        }

        void server_WorkspaceUpdated(string WorkspaceID)
        {
            PopulateWorkspaces();
        }

        #endregion

        #region Connection Methods
        private void btn_Connect_Click(object sender, EventArgs e)
        {
            if (!txt_ServerName.Text.Equals(""))
                server.ServerName = txt_ServerName.Text;
            if (!txt_ServerIP.Text.Equals(""))
                server.ServerIPAddress = txt_ServerIP.Text;

            if (!txt_ServerName.Text.Equals("") || !txt_ServerIP.Text.Equals(""))
            {
                bool connected;
                if (txt_Passcode.Text.Equals(""))
                    connected = server.CurrentWorkspace_Connect();
                else
                    connected = server.CurrentWorkspace_Connect(Convert.ToInt16(txt_Passcode.Text));

                if (connected)
                {
                    PopulateWorkspaces();

                    group_CurrentWorkspaceCommands.Enabled = true;
                }
            }
        }

        private void PopulateWorkspaces()
        {
            tv_Workspaces.Nodes.Clear();
            foreach (Workspace workspace in server.Workspaces)
            {
                TreeNode workspaceNode = new TreeNode(workspace.displayName + "(" + workspace.uniqueID + ")");
                workspaceNode.ToolTipText = workspace.uniqueID;

                foreach (Cue cueList in workspace.Cues)
                {
                    TreeNode cueListNode = new TreeNode(cueList.name + " (" + cueList.uniqueID + ") (" + cueList.type + ")");
                    cueListNode.Name = cueList.uniqueID;

                    PopulateWorkspaces(cueList, cueListNode);

                    workspaceNode.Nodes.Add(cueListNode);
                    cueListNode.Expand();
                }

                tv_Workspaces.Nodes.Add(workspaceNode);
                workspaceNode.Expand();

                if (workspace.hasPasscode)
                    workspace.Connect(Convert.ToInt16(txt_Passcode.Text));
            }
        }
        private void PopulateWorkspaces(Cue parentCue, TreeNode parentNode)
        {
            foreach (Cue cue in parentCue.cues)
            {
                TreeNode cueNode = new TreeNode(cue.name + " (" + cue.type + ")");
                cueNode.Name = cue.uniqueID;
                parentNode.Nodes.Add(cueNode);

                PopulateWorkspaces(cue, cueNode);
            }
        }

        private void cbox_RequestUpdates_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Workspace workspace in server.Workspaces)
                workspace.Update(cbox_RequestUpdates.Checked);
        }
        #endregion

        #region Current Workspace Commands
        private void btn_CurrentWorkspace_GO_Click(object sender, EventArgs e)
        {
            server.CurrentWorkspace_Go();
        }

        private void btn_CurrentWorkspace_Stop_Click(object sender, EventArgs e)
        {
            server.CurrentWorkspace_Stop();
        }
        #endregion

        #region Custom Commands
        private void btn_CustomCommand1_Click(object sender, EventArgs e)
        {
            OSCMessage.SendOSCMessage(txt_CustomCommand1.Text);
        }

        private void btn_CustomCommand2_Click(object sender, EventArgs e)
        {
            OSCMessage.SendOSCMessage(txt_CustomCommand2.Text);
        }

        private void btn_CustomCommand3_Click(object sender, EventArgs e)
        {
            OSCMessage.SendOSCMessage(txt_CustomCommand3.Text);
        }

        private void btn_CustomCommand4_Click(object sender, EventArgs e)
        {
            OSCMessage.SendOSCMessage(txt_CustomCommand4.Text);
        }

        private void btn_CustomCommand5_Click(object sender, EventArgs e)
        {
            OSCMessage.SendOSCMessage(txt_CustomCommand5.Text);
        }

        private void btn_CustomCommand6_Click(object sender, EventArgs e)
        {
            OSCMessage.SendOSCMessage(txt_CustomCommand6.Text);
        }

        private void btn_CustomCommand7_Click(object sender, EventArgs e)
        {
            OSCMessage.SendOSCMessage(txt_CustomCommand7.Text);
        }

        private void btn_CustomCommand8_Click(object sender, EventArgs e)
        {
            OSCMessage.SendOSCMessage(txt_CustomCommand8.Text);
        }

        private void btn_CustomCommand9_Click(object sender, EventArgs e)
        {

            OSCMessage.SendOSCMessage(txt_CustomCommand9.Text);
        }
        #endregion

        #region Selected Cue Getters
        private void tv_Workspaces_AfterSelect(object sender, TreeViewEventArgs e)
        {
            group_SelectedCueInfo.Enabled = (SelectedCue != null);
            group_SelectedCueCommands.Enabled = (SelectedCue != null);

            Cue selectedCue = SelectedCue;
            if (selectedCue != null)
            {
                txt_SelectedCue_Name.Text = selectedCue.name;
                cbox_SelectedCue_Armed.Checked = selectedCue.armed;
                cbox_SelectedCue_Flagged.Checked = selectedCue.flagged;
                if (selectedCue.GetType().Equals(typeof(AudioCue)))
                    tbar_MasterVolume.Value = Convert.ToInt16(((AudioCue)selectedCue).GetMasterVolume());
            }
        }

        private Cue SelectedCue
        {
            get
            {
                if (tv_Workspaces.SelectedNode != null)
                    return server.GetCue(tv_Workspaces.SelectedNode.Name);
                return null;
            }
        }
        #endregion

        #region Selected Cue Info
        private void tbar_MasterVolume_Scroll(object sender, EventArgs e)
        {
            if (SelectedCue != null && SelectedCue.GetType().Equals(typeof(AudioCue)))
            {
                ((AudioCue)SelectedCue).SetMasterVolume(tbar_MasterVolume.Value);
            }
        }

        private void txt_SelectedCue_Name_TextChanged(object sender, EventArgs e)
        {
            if (SelectedCue != null)
                SelectedCue.name = txt_SelectedCue_Name.Text;
        }
        private void txt_SelectedCue_Number_TextChanged(object sender, EventArgs e)
        {
            if (SelectedCue != null)
                SelectedCue.number = txt_SelectedCue_Number.Text;
        }

        private void cbox_SelectedCue_Flagged_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectedCue != null)
                SelectedCue.flagged = cbox_SelectedCue_Flagged.Checked;
        }

        private void cbox_SelectedCue_Armed_CheckedChanged(object sender, EventArgs e)
        {
            if (SelectedCue != null)
                SelectedCue.armed = cbox_SelectedCue_Armed.Checked;
        }
        #endregion

        #region Selected Cue Commands
        private void btn_SelectedCue_GO_Click(object sender, EventArgs e)
        {
            if (SelectedCue != null)
                SelectedCue.Go();
        }

        private void btn_SelectedCue_Stop_Click(object sender, EventArgs e)
        {
            if (SelectedCue != null)
                SelectedCue.Stop();
        }

        private void btn_SelectedCue_Load_Click(object sender, EventArgs e)
        {
            if (SelectedCue != null)
                SelectedCue.Load();
        }

        private void btn_SelectedCue_Pause_Click(object sender, EventArgs e)
        {
            if (SelectedCue != null)
                SelectedCue.Pause();
        }

        private void btn_SelectedCue_Resume_Click(object sender, EventArgs e)
        {
            if (SelectedCue != null)
                SelectedCue.Resume();
        }
        #endregion

        private void btn_TestButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(SelectedCue.hasCueTargets.ToString());
            MessageBox.Show(SelectedCue.isBroken.ToString());
        }


    }
}
