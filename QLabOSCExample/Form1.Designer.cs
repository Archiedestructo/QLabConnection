namespace QLabOSCExample
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_ServerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_ServerIP = new System.Windows.Forms.TextBox();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_Passcode = new System.Windows.Forms.TextBox();
            this.tv_Workspaces = new System.Windows.Forms.TreeView();
            this.group_CurrentWorkspaceCommands = new System.Windows.Forms.GroupBox();
            this.btn_CurrentWorkspace_Stop = new System.Windows.Forms.Button();
            this.btn_CurrentWorkspace_GO = new System.Windows.Forms.Button();
            this.txt_CustomCommand2 = new System.Windows.Forms.TextBox();
            this.group_CustomCommands = new System.Windows.Forms.GroupBox();
            this.btn_CustomCommand1 = new System.Windows.Forms.Button();
            this.txt_CustomCommand1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_CustomCommand9 = new System.Windows.Forms.Button();
            this.txt_CustomCommand9 = new System.Windows.Forms.TextBox();
            this.btn_CustomCommand8 = new System.Windows.Forms.Button();
            this.txt_CustomCommand8 = new System.Windows.Forms.TextBox();
            this.btn_CustomCommand7 = new System.Windows.Forms.Button();
            this.txt_CustomCommand7 = new System.Windows.Forms.TextBox();
            this.btn_CustomCommand6 = new System.Windows.Forms.Button();
            this.txt_CustomCommand6 = new System.Windows.Forms.TextBox();
            this.btn_CustomCommand5 = new System.Windows.Forms.Button();
            this.txt_CustomCommand5 = new System.Windows.Forms.TextBox();
            this.btn_CustomCommand4 = new System.Windows.Forms.Button();
            this.txt_CustomCommand4 = new System.Windows.Forms.TextBox();
            this.btn_CustomCommand3 = new System.Windows.Forms.Button();
            this.txt_CustomCommand3 = new System.Windows.Forms.TextBox();
            this.btn_CustomCommand2 = new System.Windows.Forms.Button();
            this.group_SelectedCueInfo = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_SelectedCue_Number = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbox_SelectedCue_Armed = new System.Windows.Forms.CheckBox();
            this.cbox_SelectedCue_Flagged = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_SelectedCue_Name = new System.Windows.Forms.TextBox();
            this.tbar_MasterVolume = new System.Windows.Forms.TrackBar();
            this.group_SelectedCueCommands = new System.Windows.Forms.GroupBox();
            this.btn_SelectedCue_Resume = new System.Windows.Forms.Button();
            this.btn_SelectedCue_Pause = new System.Windows.Forms.Button();
            this.btn_SelectedCue_Load = new System.Windows.Forms.Button();
            this.btn_SelectedCue_Stop = new System.Windows.Forms.Button();
            this.btn_SelectedCue_GO = new System.Windows.Forms.Button();
            this.cbox_RequestUpdates = new System.Windows.Forms.CheckBox();
            this.btn_TestButton = new System.Windows.Forms.Button();
            this.group_CurrentWorkspaceCommands.SuspendLayout();
            this.group_CustomCommands.SuspendLayout();
            this.group_SelectedCueInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbar_MasterVolume)).BeginInit();
            this.group_SelectedCueCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // txt_ServerName
            // 
            this.txt_ServerName.Location = new System.Drawing.Point(116, 6);
            this.txt_ServerName.Name = "txt_ServerName";
            this.txt_ServerName.Size = new System.Drawing.Size(195, 20);
            this.txt_ServerName.TabIndex = 0;
            this.txt_ServerName.Text = "r022849";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "QLab Server Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "QLab Server IP";
            // 
            // txt_ServerIP
            // 
            this.txt_ServerIP.Location = new System.Drawing.Point(116, 32);
            this.txt_ServerIP.Name = "txt_ServerIP";
            this.txt_ServerIP.Size = new System.Drawing.Size(195, 20);
            this.txt_ServerIP.TabIndex = 2;
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(12, 61);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(127, 23);
            this.btn_Connect.TabIndex = 4;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Passcode";
            // 
            // txt_Passcode
            // 
            this.txt_Passcode.Location = new System.Drawing.Point(205, 63);
            this.txt_Passcode.Name = "txt_Passcode";
            this.txt_Passcode.Size = new System.Drawing.Size(106, 20);
            this.txt_Passcode.TabIndex = 5;
            this.txt_Passcode.Text = "7900";
            // 
            // tv_Workspaces
            // 
            this.tv_Workspaces.Location = new System.Drawing.Point(12, 110);
            this.tv_Workspaces.Name = "tv_Workspaces";
            this.tv_Workspaces.Size = new System.Drawing.Size(299, 397);
            this.tv_Workspaces.TabIndex = 7;
            this.tv_Workspaces.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tv_Workspaces_AfterSelect);
            // 
            // group_CurrentWorkspaceCommands
            // 
            this.group_CurrentWorkspaceCommands.Controls.Add(this.btn_CurrentWorkspace_Stop);
            this.group_CurrentWorkspaceCommands.Controls.Add(this.btn_CurrentWorkspace_GO);
            this.group_CurrentWorkspaceCommands.Enabled = false;
            this.group_CurrentWorkspaceCommands.Location = new System.Drawing.Point(15, 513);
            this.group_CurrentWorkspaceCommands.Name = "group_CurrentWorkspaceCommands";
            this.group_CurrentWorkspaceCommands.Size = new System.Drawing.Size(296, 191);
            this.group_CurrentWorkspaceCommands.TabIndex = 8;
            this.group_CurrentWorkspaceCommands.TabStop = false;
            this.group_CurrentWorkspaceCommands.Text = "Current Workspace Commands";
            // 
            // btn_CurrentWorkspace_Stop
            // 
            this.btn_CurrentWorkspace_Stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CurrentWorkspace_Stop.Location = new System.Drawing.Point(6, 137);
            this.btn_CurrentWorkspace_Stop.Name = "btn_CurrentWorkspace_Stop";
            this.btn_CurrentWorkspace_Stop.Size = new System.Drawing.Size(284, 48);
            this.btn_CurrentWorkspace_Stop.TabIndex = 6;
            this.btn_CurrentWorkspace_Stop.Text = "Stop";
            this.btn_CurrentWorkspace_Stop.UseVisualStyleBackColor = true;
            this.btn_CurrentWorkspace_Stop.Click += new System.EventHandler(this.btn_CurrentWorkspace_Stop_Click);
            // 
            // btn_CurrentWorkspace_GO
            // 
            this.btn_CurrentWorkspace_GO.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_CurrentWorkspace_GO.Location = new System.Drawing.Point(6, 19);
            this.btn_CurrentWorkspace_GO.Name = "btn_CurrentWorkspace_GO";
            this.btn_CurrentWorkspace_GO.Size = new System.Drawing.Size(284, 112);
            this.btn_CurrentWorkspace_GO.TabIndex = 5;
            this.btn_CurrentWorkspace_GO.Text = "GO";
            this.btn_CurrentWorkspace_GO.UseVisualStyleBackColor = true;
            this.btn_CurrentWorkspace_GO.Click += new System.EventHandler(this.btn_CurrentWorkspace_GO_Click);
            // 
            // txt_CustomCommand2
            // 
            this.txt_CustomCommand2.Location = new System.Drawing.Point(6, 106);
            this.txt_CustomCommand2.Name = "txt_CustomCommand2";
            this.txt_CustomCommand2.Size = new System.Drawing.Size(299, 20);
            this.txt_CustomCommand2.TabIndex = 9;
            // 
            // group_CustomCommands
            // 
            this.group_CustomCommands.Controls.Add(this.btn_CustomCommand1);
            this.group_CustomCommands.Controls.Add(this.txt_CustomCommand1);
            this.group_CustomCommands.Controls.Add(this.label4);
            this.group_CustomCommands.Controls.Add(this.btn_CustomCommand9);
            this.group_CustomCommands.Controls.Add(this.txt_CustomCommand9);
            this.group_CustomCommands.Controls.Add(this.btn_CustomCommand8);
            this.group_CustomCommands.Controls.Add(this.txt_CustomCommand8);
            this.group_CustomCommands.Controls.Add(this.btn_CustomCommand7);
            this.group_CustomCommands.Controls.Add(this.txt_CustomCommand7);
            this.group_CustomCommands.Controls.Add(this.btn_CustomCommand6);
            this.group_CustomCommands.Controls.Add(this.txt_CustomCommand6);
            this.group_CustomCommands.Controls.Add(this.btn_CustomCommand5);
            this.group_CustomCommands.Controls.Add(this.txt_CustomCommand5);
            this.group_CustomCommands.Controls.Add(this.btn_CustomCommand4);
            this.group_CustomCommands.Controls.Add(this.txt_CustomCommand4);
            this.group_CustomCommands.Controls.Add(this.btn_CustomCommand3);
            this.group_CustomCommands.Controls.Add(this.txt_CustomCommand3);
            this.group_CustomCommands.Controls.Add(this.btn_CustomCommand2);
            this.group_CustomCommands.Controls.Add(this.txt_CustomCommand2);
            this.group_CustomCommands.Location = new System.Drawing.Point(921, 6);
            this.group_CustomCommands.Name = "group_CustomCommands";
            this.group_CustomCommands.Size = new System.Drawing.Size(378, 318);
            this.group_CustomCommands.TabIndex = 11;
            this.group_CustomCommands.TabStop = false;
            this.group_CustomCommands.Text = "Custom OSC Commands";
            // 
            // btn_CustomCommand1
            // 
            this.btn_CustomCommand1.Location = new System.Drawing.Point(309, 79);
            this.btn_CustomCommand1.Name = "btn_CustomCommand1";
            this.btn_CustomCommand1.Size = new System.Drawing.Size(63, 23);
            this.btn_CustomCommand1.TabIndex = 28;
            this.btn_CustomCommand1.Text = "Send";
            this.btn_CustomCommand1.UseVisualStyleBackColor = true;
            this.btn_CustomCommand1.Click += new System.EventHandler(this.btn_CustomCommand1_Click);
            // 
            // txt_CustomCommand1
            // 
            this.txt_CustomCommand1.Location = new System.Drawing.Point(6, 81);
            this.txt_CustomCommand1.Name = "txt_CustomCommand1";
            this.txt_CustomCommand1.Size = new System.Drawing.Size(299, 20);
            this.txt_CustomCommand1.TabIndex = 27;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(18, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(354, 51);
            this.label4.TabIndex = 26;
            this.label4.Text = "Use the following areas to enter custom OSC Commands. These can be used in the OS" +
                "C Controls area of Worspace Preferences to define and capture commands.";
            // 
            // btn_CustomCommand9
            // 
            this.btn_CustomCommand9.Location = new System.Drawing.Point(309, 281);
            this.btn_CustomCommand9.Name = "btn_CustomCommand9";
            this.btn_CustomCommand9.Size = new System.Drawing.Size(63, 23);
            this.btn_CustomCommand9.TabIndex = 25;
            this.btn_CustomCommand9.Text = "Send";
            this.btn_CustomCommand9.UseVisualStyleBackColor = true;
            this.btn_CustomCommand9.Click += new System.EventHandler(this.btn_CustomCommand9_Click);
            // 
            // txt_CustomCommand9
            // 
            this.txt_CustomCommand9.Location = new System.Drawing.Point(6, 283);
            this.txt_CustomCommand9.Name = "txt_CustomCommand9";
            this.txt_CustomCommand9.Size = new System.Drawing.Size(299, 20);
            this.txt_CustomCommand9.TabIndex = 24;
            // 
            // btn_CustomCommand8
            // 
            this.btn_CustomCommand8.Location = new System.Drawing.Point(309, 256);
            this.btn_CustomCommand8.Name = "btn_CustomCommand8";
            this.btn_CustomCommand8.Size = new System.Drawing.Size(63, 23);
            this.btn_CustomCommand8.TabIndex = 23;
            this.btn_CustomCommand8.Text = "Send";
            this.btn_CustomCommand8.UseVisualStyleBackColor = true;
            this.btn_CustomCommand8.Click += new System.EventHandler(this.btn_CustomCommand8_Click);
            // 
            // txt_CustomCommand8
            // 
            this.txt_CustomCommand8.Location = new System.Drawing.Point(6, 258);
            this.txt_CustomCommand8.Name = "txt_CustomCommand8";
            this.txt_CustomCommand8.Size = new System.Drawing.Size(299, 20);
            this.txt_CustomCommand8.TabIndex = 22;
            // 
            // btn_CustomCommand7
            // 
            this.btn_CustomCommand7.Location = new System.Drawing.Point(309, 231);
            this.btn_CustomCommand7.Name = "btn_CustomCommand7";
            this.btn_CustomCommand7.Size = new System.Drawing.Size(63, 23);
            this.btn_CustomCommand7.TabIndex = 21;
            this.btn_CustomCommand7.Text = "Send";
            this.btn_CustomCommand7.UseVisualStyleBackColor = true;
            this.btn_CustomCommand7.Click += new System.EventHandler(this.btn_CustomCommand7_Click);
            // 
            // txt_CustomCommand7
            // 
            this.txt_CustomCommand7.Location = new System.Drawing.Point(6, 233);
            this.txt_CustomCommand7.Name = "txt_CustomCommand7";
            this.txt_CustomCommand7.Size = new System.Drawing.Size(299, 20);
            this.txt_CustomCommand7.TabIndex = 20;
            // 
            // btn_CustomCommand6
            // 
            this.btn_CustomCommand6.Location = new System.Drawing.Point(309, 206);
            this.btn_CustomCommand6.Name = "btn_CustomCommand6";
            this.btn_CustomCommand6.Size = new System.Drawing.Size(63, 23);
            this.btn_CustomCommand6.TabIndex = 19;
            this.btn_CustomCommand6.Text = "Send";
            this.btn_CustomCommand6.UseVisualStyleBackColor = true;
            this.btn_CustomCommand6.Click += new System.EventHandler(this.btn_CustomCommand6_Click);
            // 
            // txt_CustomCommand6
            // 
            this.txt_CustomCommand6.Location = new System.Drawing.Point(6, 208);
            this.txt_CustomCommand6.Name = "txt_CustomCommand6";
            this.txt_CustomCommand6.Size = new System.Drawing.Size(299, 20);
            this.txt_CustomCommand6.TabIndex = 18;
            // 
            // btn_CustomCommand5
            // 
            this.btn_CustomCommand5.Location = new System.Drawing.Point(309, 180);
            this.btn_CustomCommand5.Name = "btn_CustomCommand5";
            this.btn_CustomCommand5.Size = new System.Drawing.Size(63, 23);
            this.btn_CustomCommand5.TabIndex = 17;
            this.btn_CustomCommand5.Text = "Send";
            this.btn_CustomCommand5.UseVisualStyleBackColor = true;
            this.btn_CustomCommand5.Click += new System.EventHandler(this.btn_CustomCommand5_Click);
            // 
            // txt_CustomCommand5
            // 
            this.txt_CustomCommand5.Location = new System.Drawing.Point(6, 182);
            this.txt_CustomCommand5.Name = "txt_CustomCommand5";
            this.txt_CustomCommand5.Size = new System.Drawing.Size(299, 20);
            this.txt_CustomCommand5.TabIndex = 16;
            // 
            // btn_CustomCommand4
            // 
            this.btn_CustomCommand4.Location = new System.Drawing.Point(309, 155);
            this.btn_CustomCommand4.Name = "btn_CustomCommand4";
            this.btn_CustomCommand4.Size = new System.Drawing.Size(63, 23);
            this.btn_CustomCommand4.TabIndex = 15;
            this.btn_CustomCommand4.Text = "Send";
            this.btn_CustomCommand4.UseVisualStyleBackColor = true;
            this.btn_CustomCommand4.Click += new System.EventHandler(this.btn_CustomCommand4_Click);
            // 
            // txt_CustomCommand4
            // 
            this.txt_CustomCommand4.Location = new System.Drawing.Point(6, 157);
            this.txt_CustomCommand4.Name = "txt_CustomCommand4";
            this.txt_CustomCommand4.Size = new System.Drawing.Size(299, 20);
            this.txt_CustomCommand4.TabIndex = 14;
            // 
            // btn_CustomCommand3
            // 
            this.btn_CustomCommand3.Location = new System.Drawing.Point(309, 129);
            this.btn_CustomCommand3.Name = "btn_CustomCommand3";
            this.btn_CustomCommand3.Size = new System.Drawing.Size(63, 23);
            this.btn_CustomCommand3.TabIndex = 13;
            this.btn_CustomCommand3.Text = "Send";
            this.btn_CustomCommand3.UseVisualStyleBackColor = true;
            this.btn_CustomCommand3.Click += new System.EventHandler(this.btn_CustomCommand3_Click);
            // 
            // txt_CustomCommand3
            // 
            this.txt_CustomCommand3.Location = new System.Drawing.Point(6, 131);
            this.txt_CustomCommand3.Name = "txt_CustomCommand3";
            this.txt_CustomCommand3.Size = new System.Drawing.Size(299, 20);
            this.txt_CustomCommand3.TabIndex = 12;
            // 
            // btn_CustomCommand2
            // 
            this.btn_CustomCommand2.Location = new System.Drawing.Point(309, 104);
            this.btn_CustomCommand2.Name = "btn_CustomCommand2";
            this.btn_CustomCommand2.Size = new System.Drawing.Size(63, 23);
            this.btn_CustomCommand2.TabIndex = 11;
            this.btn_CustomCommand2.Text = "Send";
            this.btn_CustomCommand2.UseVisualStyleBackColor = true;
            this.btn_CustomCommand2.Click += new System.EventHandler(this.btn_CustomCommand2_Click);
            // 
            // group_SelectedCueInfo
            // 
            this.group_SelectedCueInfo.Controls.Add(this.btn_TestButton);
            this.group_SelectedCueInfo.Controls.Add(this.label7);
            this.group_SelectedCueInfo.Controls.Add(this.txt_SelectedCue_Number);
            this.group_SelectedCueInfo.Controls.Add(this.label6);
            this.group_SelectedCueInfo.Controls.Add(this.cbox_SelectedCue_Armed);
            this.group_SelectedCueInfo.Controls.Add(this.cbox_SelectedCue_Flagged);
            this.group_SelectedCueInfo.Controls.Add(this.label5);
            this.group_SelectedCueInfo.Controls.Add(this.txt_SelectedCue_Name);
            this.group_SelectedCueInfo.Controls.Add(this.tbar_MasterVolume);
            this.group_SelectedCueInfo.Enabled = false;
            this.group_SelectedCueInfo.Location = new System.Drawing.Point(317, 6);
            this.group_SelectedCueInfo.Name = "group_SelectedCueInfo";
            this.group_SelectedCueInfo.Size = new System.Drawing.Size(598, 501);
            this.group_SelectedCueInfo.TabIndex = 12;
            this.group_SelectedCueInfo.TabStop = false;
            this.group_SelectedCueInfo.Text = "Selected Cue Info";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(248, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Number";
            // 
            // txt_SelectedCue_Number
            // 
            this.txt_SelectedCue_Number.Location = new System.Drawing.Point(310, 23);
            this.txt_SelectedCue_Number.Name = "txt_SelectedCue_Number";
            this.txt_SelectedCue_Number.Size = new System.Drawing.Size(195, 20);
            this.txt_SelectedCue_Number.TabIndex = 7;
            this.txt_SelectedCue_Number.TextChanged += new System.EventHandler(this.txt_SelectedCue_Number_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Master Volume";
            // 
            // cbox_SelectedCue_Armed
            // 
            this.cbox_SelectedCue_Armed.AutoSize = true;
            this.cbox_SelectedCue_Armed.Location = new System.Drawing.Point(9, 72);
            this.cbox_SelectedCue_Armed.Name = "cbox_SelectedCue_Armed";
            this.cbox_SelectedCue_Armed.Size = new System.Drawing.Size(56, 17);
            this.cbox_SelectedCue_Armed.TabIndex = 5;
            this.cbox_SelectedCue_Armed.Text = "Armed";
            this.cbox_SelectedCue_Armed.UseVisualStyleBackColor = true;
            this.cbox_SelectedCue_Armed.CheckedChanged += new System.EventHandler(this.cbox_SelectedCue_Armed_CheckedChanged);
            // 
            // cbox_SelectedCue_Flagged
            // 
            this.cbox_SelectedCue_Flagged.AutoSize = true;
            this.cbox_SelectedCue_Flagged.Location = new System.Drawing.Point(9, 49);
            this.cbox_SelectedCue_Flagged.Name = "cbox_SelectedCue_Flagged";
            this.cbox_SelectedCue_Flagged.Size = new System.Drawing.Size(64, 17);
            this.cbox_SelectedCue_Flagged.TabIndex = 4;
            this.cbox_SelectedCue_Flagged.Text = "Flagged";
            this.cbox_SelectedCue_Flagged.UseVisualStyleBackColor = true;
            this.cbox_SelectedCue_Flagged.CheckedChanged += new System.EventHandler(this.cbox_SelectedCue_Flagged_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Name";
            // 
            // txt_SelectedCue_Name
            // 
            this.txt_SelectedCue_Name.Location = new System.Drawing.Point(47, 23);
            this.txt_SelectedCue_Name.Name = "txt_SelectedCue_Name";
            this.txt_SelectedCue_Name.Size = new System.Drawing.Size(195, 20);
            this.txt_SelectedCue_Name.TabIndex = 2;
            this.txt_SelectedCue_Name.TextChanged += new System.EventHandler(this.txt_SelectedCue_Name_TextChanged);
            // 
            // tbar_MasterVolume
            // 
            this.tbar_MasterVolume.Location = new System.Drawing.Point(9, 124);
            this.tbar_MasterVolume.Maximum = 12;
            this.tbar_MasterVolume.Minimum = -60;
            this.tbar_MasterVolume.Name = "tbar_MasterVolume";
            this.tbar_MasterVolume.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbar_MasterVolume.Size = new System.Drawing.Size(45, 104);
            this.tbar_MasterVolume.TabIndex = 0;
            this.tbar_MasterVolume.TickFrequency = 10;
            this.tbar_MasterVolume.Scroll += new System.EventHandler(this.tbar_MasterVolume_Scroll);
            // 
            // group_SelectedCueCommands
            // 
            this.group_SelectedCueCommands.Controls.Add(this.btn_SelectedCue_Resume);
            this.group_SelectedCueCommands.Controls.Add(this.btn_SelectedCue_Pause);
            this.group_SelectedCueCommands.Controls.Add(this.btn_SelectedCue_Load);
            this.group_SelectedCueCommands.Controls.Add(this.btn_SelectedCue_Stop);
            this.group_SelectedCueCommands.Controls.Add(this.btn_SelectedCue_GO);
            this.group_SelectedCueCommands.Enabled = false;
            this.group_SelectedCueCommands.Location = new System.Drawing.Point(317, 513);
            this.group_SelectedCueCommands.Name = "group_SelectedCueCommands";
            this.group_SelectedCueCommands.Size = new System.Drawing.Size(598, 191);
            this.group_SelectedCueCommands.TabIndex = 13;
            this.group_SelectedCueCommands.TabStop = false;
            this.group_SelectedCueCommands.Text = "Selected Cue Commands";
            // 
            // btn_SelectedCue_Resume
            // 
            this.btn_SelectedCue_Resume.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SelectedCue_Resume.Location = new System.Drawing.Point(296, 137);
            this.btn_SelectedCue_Resume.Name = "btn_SelectedCue_Resume";
            this.btn_SelectedCue_Resume.Size = new System.Drawing.Size(122, 48);
            this.btn_SelectedCue_Resume.TabIndex = 9;
            this.btn_SelectedCue_Resume.Text = "Resume";
            this.btn_SelectedCue_Resume.UseVisualStyleBackColor = true;
            this.btn_SelectedCue_Resume.Click += new System.EventHandler(this.btn_SelectedCue_Resume_Click);
            // 
            // btn_SelectedCue_Pause
            // 
            this.btn_SelectedCue_Pause.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SelectedCue_Pause.Location = new System.Drawing.Point(296, 77);
            this.btn_SelectedCue_Pause.Name = "btn_SelectedCue_Pause";
            this.btn_SelectedCue_Pause.Size = new System.Drawing.Size(122, 48);
            this.btn_SelectedCue_Pause.TabIndex = 8;
            this.btn_SelectedCue_Pause.Text = "Pause";
            this.btn_SelectedCue_Pause.UseVisualStyleBackColor = true;
            this.btn_SelectedCue_Pause.Click += new System.EventHandler(this.btn_SelectedCue_Pause_Click);
            // 
            // btn_SelectedCue_Load
            // 
            this.btn_SelectedCue_Load.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SelectedCue_Load.Location = new System.Drawing.Point(296, 19);
            this.btn_SelectedCue_Load.Name = "btn_SelectedCue_Load";
            this.btn_SelectedCue_Load.Size = new System.Drawing.Size(122, 48);
            this.btn_SelectedCue_Load.TabIndex = 7;
            this.btn_SelectedCue_Load.Text = "Load";
            this.btn_SelectedCue_Load.UseVisualStyleBackColor = true;
            this.btn_SelectedCue_Load.Click += new System.EventHandler(this.btn_SelectedCue_Load_Click);
            // 
            // btn_SelectedCue_Stop
            // 
            this.btn_SelectedCue_Stop.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SelectedCue_Stop.Location = new System.Drawing.Point(6, 137);
            this.btn_SelectedCue_Stop.Name = "btn_SelectedCue_Stop";
            this.btn_SelectedCue_Stop.Size = new System.Drawing.Size(284, 48);
            this.btn_SelectedCue_Stop.TabIndex = 6;
            this.btn_SelectedCue_Stop.Text = "Stop";
            this.btn_SelectedCue_Stop.UseVisualStyleBackColor = true;
            this.btn_SelectedCue_Stop.Click += new System.EventHandler(this.btn_SelectedCue_Stop_Click);
            // 
            // btn_SelectedCue_GO
            // 
            this.btn_SelectedCue_GO.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_SelectedCue_GO.Location = new System.Drawing.Point(6, 19);
            this.btn_SelectedCue_GO.Name = "btn_SelectedCue_GO";
            this.btn_SelectedCue_GO.Size = new System.Drawing.Size(284, 112);
            this.btn_SelectedCue_GO.TabIndex = 5;
            this.btn_SelectedCue_GO.Text = "GO";
            this.btn_SelectedCue_GO.UseVisualStyleBackColor = true;
            this.btn_SelectedCue_GO.Click += new System.EventHandler(this.btn_SelectedCue_GO_Click);
            // 
            // cbox_RequestUpdates
            // 
            this.cbox_RequestUpdates.AutoSize = true;
            this.cbox_RequestUpdates.Location = new System.Drawing.Point(12, 87);
            this.cbox_RequestUpdates.Name = "cbox_RequestUpdates";
            this.cbox_RequestUpdates.Size = new System.Drawing.Size(161, 17);
            this.cbox_RequestUpdates.TabIndex = 14;
            this.cbox_RequestUpdates.Text = "Request Updates from QLab";
            this.cbox_RequestUpdates.UseVisualStyleBackColor = true;
            this.cbox_RequestUpdates.CheckedChanged += new System.EventHandler(this.cbox_RequestUpdates_CheckedChanged);
            // 
            // btn_TestButton
            // 
            this.btn_TestButton.Location = new System.Drawing.Point(9, 472);
            this.btn_TestButton.Name = "btn_TestButton";
            this.btn_TestButton.Size = new System.Drawing.Size(127, 23);
            this.btn_TestButton.TabIndex = 9;
            this.btn_TestButton.Text = "Test Button";
            this.btn_TestButton.UseVisualStyleBackColor = true;
            this.btn_TestButton.Click += new System.EventHandler(this.btn_TestButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1311, 716);
            this.Controls.Add(this.cbox_RequestUpdates);
            this.Controls.Add(this.group_SelectedCueCommands);
            this.Controls.Add(this.group_SelectedCueInfo);
            this.Controls.Add(this.group_CustomCommands);
            this.Controls.Add(this.group_CurrentWorkspaceCommands);
            this.Controls.Add(this.tv_Workspaces);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txt_Passcode);
            this.Controls.Add(this.btn_Connect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_ServerIP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_ServerName);
            this.Name = "Form1";
            this.Text = "QLab OSC Example";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.group_CurrentWorkspaceCommands.ResumeLayout(false);
            this.group_CustomCommands.ResumeLayout(false);
            this.group_CustomCommands.PerformLayout();
            this.group_SelectedCueInfo.ResumeLayout(false);
            this.group_SelectedCueInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbar_MasterVolume)).EndInit();
            this.group_SelectedCueCommands.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_ServerName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_ServerIP;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txt_Passcode;
        private System.Windows.Forms.TreeView tv_Workspaces;
        private System.Windows.Forms.GroupBox group_CurrentWorkspaceCommands;
        private System.Windows.Forms.Button btn_CurrentWorkspace_Stop;
        private System.Windows.Forms.Button btn_CurrentWorkspace_GO;
        private System.Windows.Forms.TextBox txt_CustomCommand2;
        private System.Windows.Forms.GroupBox group_CustomCommands;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_CustomCommand9;
        private System.Windows.Forms.TextBox txt_CustomCommand9;
        private System.Windows.Forms.Button btn_CustomCommand8;
        private System.Windows.Forms.TextBox txt_CustomCommand8;
        private System.Windows.Forms.Button btn_CustomCommand7;
        private System.Windows.Forms.TextBox txt_CustomCommand7;
        private System.Windows.Forms.Button btn_CustomCommand6;
        private System.Windows.Forms.TextBox txt_CustomCommand6;
        private System.Windows.Forms.Button btn_CustomCommand5;
        private System.Windows.Forms.TextBox txt_CustomCommand5;
        private System.Windows.Forms.Button btn_CustomCommand4;
        private System.Windows.Forms.TextBox txt_CustomCommand4;
        private System.Windows.Forms.Button btn_CustomCommand3;
        private System.Windows.Forms.TextBox txt_CustomCommand3;
        private System.Windows.Forms.Button btn_CustomCommand2;
        private System.Windows.Forms.Button btn_CustomCommand1;
        private System.Windows.Forms.TextBox txt_CustomCommand1;
        private System.Windows.Forms.GroupBox group_SelectedCueInfo;
        private System.Windows.Forms.GroupBox group_SelectedCueCommands;
        private System.Windows.Forms.Button btn_SelectedCue_Resume;
        private System.Windows.Forms.Button btn_SelectedCue_Pause;
        private System.Windows.Forms.Button btn_SelectedCue_Load;
        private System.Windows.Forms.Button btn_SelectedCue_Stop;
        private System.Windows.Forms.Button btn_SelectedCue_GO;
        private System.Windows.Forms.CheckBox cbox_RequestUpdates;
        private System.Windows.Forms.TrackBar tbar_MasterVolume;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_SelectedCue_Name;
        private System.Windows.Forms.CheckBox cbox_SelectedCue_Armed;
        private System.Windows.Forms.CheckBox cbox_SelectedCue_Flagged;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_SelectedCue_Number;
        private System.Windows.Forms.Button btn_TestButton;
    }
}

