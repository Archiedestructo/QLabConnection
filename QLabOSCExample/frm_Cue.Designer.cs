namespace QLabOSCExample
{
    partial class frm_Cue
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lbl_CueNumber = new System.Windows.Forms.Label();
            this.lbl_CueName = new System.Windows.Forms.Label();
            this.lbl_CueType = new System.Windows.Forms.Label();
            this.slider0 = new System.Windows.Forms.TrackBar();
            this.txt_Slider0 = new System.Windows.Forms.TextBox();
            this.pbar_Action = new System.Windows.Forms.ProgressBar();
            this.tmr_Update = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.slider0)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl_CueNumber
            // 
            this.lbl_CueNumber.AutoSize = true;
            this.lbl_CueNumber.Location = new System.Drawing.Point(3, 0);
            this.lbl_CueNumber.Name = "lbl_CueNumber";
            this.lbl_CueNumber.Size = new System.Drawing.Size(69, 13);
            this.lbl_CueNumber.TabIndex = 0;
            this.lbl_CueNumber.Text = "Cue Number:";
            // 
            // lbl_CueName
            // 
            this.lbl_CueName.AutoSize = true;
            this.lbl_CueName.Location = new System.Drawing.Point(3, 13);
            this.lbl_CueName.Name = "lbl_CueName";
            this.lbl_CueName.Size = new System.Drawing.Size(60, 13);
            this.lbl_CueName.TabIndex = 1;
            this.lbl_CueName.Text = "Cue Name:";
            // 
            // lbl_CueType
            // 
            this.lbl_CueType.AutoSize = true;
            this.lbl_CueType.Location = new System.Drawing.Point(3, 26);
            this.lbl_CueType.Name = "lbl_CueType";
            this.lbl_CueType.Size = new System.Drawing.Size(56, 13);
            this.lbl_CueType.TabIndex = 2;
            this.lbl_CueType.Text = "Cue Type:";
            // 
            // slider0
            // 
            this.slider0.Location = new System.Drawing.Point(6, 42);
            this.slider0.Maximum = 12;
            this.slider0.Minimum = -60;
            this.slider0.Name = "slider0";
            this.slider0.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.slider0.Size = new System.Drawing.Size(45, 104);
            this.slider0.TabIndex = 3;
            this.slider0.TickFrequency = 10;
            this.slider0.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.slider0.Scroll += new System.EventHandler(this.slider0_Scroll);
            // 
            // txt_Slider0
            // 
            this.txt_Slider0.Location = new System.Drawing.Point(12, 141);
            this.txt_Slider0.Name = "txt_Slider0";
            this.txt_Slider0.Size = new System.Drawing.Size(32, 20);
            this.txt_Slider0.TabIndex = 4;
            this.txt_Slider0.TextChanged += new System.EventHandler(this.txt_Slider0_TextChanged);
            // 
            // pbar_Action
            // 
            this.pbar_Action.Location = new System.Drawing.Point(389, 3);
            this.pbar_Action.Name = "pbar_Action";
            this.pbar_Action.Size = new System.Drawing.Size(100, 23);
            this.pbar_Action.TabIndex = 13;
            // 
            // tmr_Update
            // 
            this.tmr_Update.Enabled = true;
            this.tmr_Update.Tick += new System.EventHandler(this.tmr_Update_Tick);
            // 
            // frm_Cue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pbar_Action);
            this.Controls.Add(this.txt_Slider0);
            this.Controls.Add(this.slider0);
            this.Controls.Add(this.lbl_CueType);
            this.Controls.Add(this.lbl_CueName);
            this.Controls.Add(this.lbl_CueNumber);
            this.Name = "frm_Cue";
            this.Size = new System.Drawing.Size(496, 170);
            this.Load += new System.EventHandler(this.frm_Cue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.slider0)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_CueNumber;
        private System.Windows.Forms.Label lbl_CueName;
        private System.Windows.Forms.Label lbl_CueType;
        private System.Windows.Forms.TrackBar slider0;
        private System.Windows.Forms.TextBox txt_Slider0;
        private System.Windows.Forms.ProgressBar pbar_Action;
        private System.Windows.Forms.Timer tmr_Update;
    }
}
