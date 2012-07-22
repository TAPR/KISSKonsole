using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace KISS_Konsole
{
    public partial class CheckBoxesUserControl : UserControl
    {
        //
        // Summary:
        //     Occurs when the value of the CheckBoxesUserControl.Checked property
        //     changes.
        public event EventHandler CheckedChanged;

        public CheckBoxesUserControl()
        {
            InitializeComponent();
        }

        bool supressChangeEvents = false;

        public int Value
        {
            get {
                int retValue = 0;
                if (chk1.Checked) retValue |= 1 << 0;
                if (chk2.Checked) retValue |= 1 << 1;
                if (chk3.Checked) retValue |= 1 << 2;
                if (chk4.Checked) retValue |= 1 << 3;
                if (chk5.Checked) retValue |= 1 << 4;
                if (chk6.Checked) retValue |= 1 << 5;
                if (chk7.Checked) retValue |= 1 << 6;

                return retValue;
            }

            set
            {
                supressChangeEvents = true;
                chk1.Checked = ((value & (1 << 0)) != 0) ? true : false;
                chk2.Checked = ((value & (1 << 1)) != 0) ? true : false;
                chk3.Checked = ((value & (1 << 2)) != 0) ? true : false;
                chk4.Checked = ((value & (1 << 3)) != 0) ? true : false;
                chk5.Checked = ((value & (1 << 4)) != 0) ? true : false;
                chk6.Checked = ((value & (1 << 5)) != 0) ? true : false;
                chk7.Checked = ((value & (1 << 6)) != 0) ? true : false;
                supressChangeEvents = false;
            }
        }

        private void chk1_CheckedChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (CheckedChanged != null))
                CheckedChanged(sender, e);
        }

        private void chk2_CheckedChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (CheckedChanged != null))
                CheckedChanged(sender, e);
        }

        private void chk3_CheckedChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (CheckedChanged != null))
                CheckedChanged(sender, e);
        }

        private void chk4_CheckedChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (CheckedChanged != null))
                CheckedChanged(sender, e);
        }

        private void chk5_CheckedChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (CheckedChanged != null))
                CheckedChanged(sender, e);
        }

        private void chk6_CheckedChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (CheckedChanged != null))
                CheckedChanged(sender, e);
        }

        private void chk7_CheckedChanged(object sender, EventArgs e)
        {
            if (!supressChangeEvents && (CheckedChanged != null))
                CheckedChanged(sender, e);
        }
    }
}
