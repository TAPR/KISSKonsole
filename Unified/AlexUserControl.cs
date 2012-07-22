using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace KISS_Konsole
{
    public partial class AlexUserControl : UserControl
    {
        //
        // Summary:
        //     Occurs when the value of the AlexUserControl.Checked property
        //     changes.
        public event EventHandler StateChanged;

        public AlexUserControl()
        {
            InitializeComponent();
        }

        bool supressChangeEvents = false;
        bool tempSupressChangeEvents = false;

        public int Value
        {
            get {
                int retValue = 0;

                // encode receive antenna selection
                if (radioButtonR1.Checked) retValue |= 1;
                if (radioButtonR2.Checked) retValue |= 2;
                if (radioButtonR3.Checked) retValue |= 3;

                // encode receive special selection (these likely should be radio buttons, with a 4th for 'none')
                if (chk4.Checked) retValue |= 1 << 3;   // R1
                if (chk5.Checked) retValue |= 1 << 4;   // R2
                if (chk6.Checked) retValue |= 1 << 5;   // XV

                // encode transmit antenna selection
                if (radioButtonT1.Checked) retValue |= 1 << 6;
                if (radioButtonT2.Checked) retValue |= 2 << 6;
                if (radioButtonT3.Checked) retValue |= 3 << 6;

                // encode receive attenuator
                if (comboBoxRXAtten.SelectedIndex != -1) retValue |= comboBoxRXAtten.SelectedIndex << 8;

                return retValue;
            }

            set
            {
                supressChangeEvents = true;

                // set RX antenna choice
                switch (value & 0x3)
                {
                    case 0: // invalid!
                        radioButtonR1.Checked = true;
                        break;
                    case 1:
                        radioButtonR1.Checked = true;
                        break;
                    case 2:
                        radioButtonR2.Checked = true;
                        break;
                    case 3:
                        radioButtonR3.Checked = true;
                        break;
                }

                chk4.Checked = ((value & (1 << 3)) != 0) ? true : false;
                chk5.Checked = ((value & (1 << 4)) != 0) ? true : false;
                chk6.Checked = ((value & (1 << 5)) != 0) ? true : false;

                // set TX antenna choice
                switch ((value >> 6) & 0x3)
                {
                    case 0: // invalid state!
                        radioButtonT1.Checked = true;
                        break;
                    case 1:
                        radioButtonT1.Checked = true;
                        break;
                    case 2:
                        radioButtonT2.Checked = true;
                        break;
                    case 3:
                        radioButtonT3.Checked = true;
                        break;
                }

                // set RX attenuator
                comboBoxRXAtten.SelectedIndex = (value >> 8) & 0x3;

                supressChangeEvents = false;
            }
        }

        private void chk4_CheckedChanged(object sender, EventArgs e)
        {
            // act somewhat like a group of radio buttons, but allow also for NONE to be checked
            if (chk4.Checked)
            {
                tempSupressChangeEvents = true;
                chk5.Checked = false;
                chk6.Checked = false;
                tempSupressChangeEvents = false;
            }

            if (!supressChangeEvents && !tempSupressChangeEvents && (StateChanged != null))
                StateChanged(sender, e);
        }

        private void chk5_CheckedChanged(object sender, EventArgs e)
        {
            // act somewhat like a group of radio buttons, but allow also for NONE to be checked
            if (chk5.Checked)
            {
                tempSupressChangeEvents = true;
                chk4.Checked = false;
                chk6.Checked = false;
                tempSupressChangeEvents = false;
            }

            if (!supressChangeEvents && !tempSupressChangeEvents && (StateChanged != null))
                StateChanged(sender, e);
        }

        private void chk6_CheckedChanged(object sender, EventArgs e)
        {
            // act somewhat like a group of radio buttons, but allow also for NONE to be checked
            if (chk6.Checked)
            {
                tempSupressChangeEvents = true;
                chk4.Checked = false;
                chk5.Checked = false;
                tempSupressChangeEvents = false;
            }

            if (!supressChangeEvents && !tempSupressChangeEvents && (StateChanged != null))
                StateChanged(sender, e);
        }

        private void radioButtonR1_CheckedChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (StateChanged != null) && radioButtonR1.Checked)
                StateChanged(sender, e);
        }

        private void radioButtonR2_CheckedChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (StateChanged != null) && radioButtonR2.Checked)
                StateChanged(sender, e);
        }

        private void radioButtonR3_CheckedChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (StateChanged != null) && radioButtonR3.Checked)
                StateChanged(sender, e);
        }

        private void radioButtonT1_CheckedChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (StateChanged != null) && radioButtonT1.Checked)
                StateChanged(sender, e);
        }

        private void radioButtonT2_CheckedChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (StateChanged != null) && radioButtonT2.Checked)
                StateChanged(sender, e);
        }

        private void radioButtonT3_CheckedChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (StateChanged != null) && radioButtonT3.Checked)
                StateChanged(sender, e);
        }

        private void comboBoxRXAtten_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (StateChanged != null))
                StateChanged(sender, e);
        }
    }
}
