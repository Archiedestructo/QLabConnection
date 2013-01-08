using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QLabConnection;

namespace QLabOSCExample
{
    public partial class frm_Cue : UserControl
    {
        Cue cue;
        public frm_Cue(Cue holdingCue)
        {
            cue = holdingCue;

            InitializeComponent();

            if (cue != null)
            {
                lbl_CueName.Text = "Cue Name: " + cue.name;
                lbl_CueNumber.Text = "Cue Number: " + cue.number;
                lbl_CueType.Text = "Cue Type: " + cue.type;
            }
        }

        public void Update()
        {
            if (!cue.isRunning)
            {
                this.Dispose();
                return;
            }

            pbar_Action.Value = (int)(cue.percentActionElapsed * 100);

            if (cue.GetType().Equals(typeof(AudioCue)))
            {
                slider0.Value = (int)((AudioCue)cue).GetVolumeLevel(0);
                txt_Slider0.Text = slider0.Value.ToString();
            }
        }

        private void tmr_Update_Tick(object sender, EventArgs e)
        {
            Update();
        }

        private void slider0_Scroll(object sender, EventArgs e)
        {
            if (cue.GetType().Equals(typeof(AudioCue)))
            {
                ((AudioCue)cue).SetVolumeLevel(0, slider0.Value);
                txt_Slider0.Text = slider0.Value.ToString();
            }
        }

        private void txt_Slider0_TextChanged(object sender, EventArgs e)
        {
            int val;
            if (int.TryParse(txt_Slider0.Text, out val))
            {
                if (val >= slider0.Minimum &&
                    val <= slider0.Maximum)
                {
                    slider0.Value = val;
                    slider0_Scroll(sender, e);
                }
            }
        }

        private void frm_Cue_Load(object sender, EventArgs e)
        {

        }
    }
}
