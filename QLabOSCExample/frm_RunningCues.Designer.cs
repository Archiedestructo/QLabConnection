namespace QLabOSCExample
{
    partial class frm_RunningCues
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
            this.components = new System.ComponentModel.Container();
            this.tmr_Update = new System.Windows.Forms.Timer(this.components);
            this.btn_Connect = new System.Windows.Forms.Button();
            this.txt_Passphrase = new System.Windows.Forms.TextBox();
            this.btn_GO = new System.Windows.Forms.Button();
            this.pnl_RunningCues = new System.Windows.Forms.FlowLayoutPanel();
            this.pnl_PausedCues = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // tmr_Update
            // 
            this.tmr_Update.Tick += new System.EventHandler(this.tmr_Update_Tick);
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(12, 12);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(75, 23);
            this.btn_Connect.TabIndex = 0;
            this.btn_Connect.Text = "Connect";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // txt_Passphrase
            // 
            this.txt_Passphrase.Location = new System.Drawing.Point(94, 14);
            this.txt_Passphrase.Name = "txt_Passphrase";
            this.txt_Passphrase.Size = new System.Drawing.Size(100, 20);
            this.txt_Passphrase.TabIndex = 1;
            // 
            // btn_GO
            // 
            this.btn_GO.Location = new System.Drawing.Point(1108, 11);
            this.btn_GO.Name = "btn_GO";
            this.btn_GO.Size = new System.Drawing.Size(75, 23);
            this.btn_GO.TabIndex = 3;
            this.btn_GO.Text = "GO";
            this.btn_GO.UseVisualStyleBackColor = true;
            this.btn_GO.Click += new System.EventHandler(this.btn_GO_Click);
            // 
            // pnl_RunningCues
            // 
            this.pnl_RunningCues.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnl_RunningCues.Location = new System.Drawing.Point(12, 40);
            this.pnl_RunningCues.Name = "pnl_RunningCues";
            this.pnl_RunningCues.Size = new System.Drawing.Size(551, 614);
            this.pnl_RunningCues.TabIndex = 4;
            // 
            // pnl_PausedCues
            // 
            this.pnl_PausedCues.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnl_PausedCues.Location = new System.Drawing.Point(569, 40);
            this.pnl_PausedCues.Name = "pnl_PausedCues";
            this.pnl_PausedCues.Size = new System.Drawing.Size(614, 614);
            this.pnl_PausedCues.TabIndex = 5;
            // 
            // frm_RunningCues
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1195, 666);
            this.Controls.Add(this.pnl_PausedCues);
            this.Controls.Add(this.pnl_RunningCues);
            this.Controls.Add(this.btn_GO);
            this.Controls.Add(this.txt_Passphrase);
            this.Controls.Add(this.btn_Connect);
            this.Name = "frm_RunningCues";
            this.Text = "frm_RunningCues";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmr_Update;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.TextBox txt_Passphrase;
        private System.Windows.Forms.Button btn_GO;
        private System.Windows.Forms.FlowLayoutPanel pnl_RunningCues;
        private System.Windows.Forms.FlowLayoutPanel pnl_PausedCues;
    }
}