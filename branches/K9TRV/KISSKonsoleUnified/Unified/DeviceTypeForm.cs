using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KISS_Konsole
{
    public partial class DeviceTypeForm : Form
    {
        public DeviceTypeForm( KKMethod currentDeviceType )
        {
            InitializeComponent();

            if (currentDeviceType == KKMethod.Ethernet)
            {
                radioButtonEthernet.Checked = true;
            }
            else if (currentDeviceType == KKMethod.USB)
            {
                radioButtonUSB.Checked = true;
            }
        }

        public KKMethod GetChosenDeviceType()
        {
            if (radioButtonEthernet.Checked)
                return KKMethod.Ethernet;
            else
                return KKMethod.USB;
        }
    }
}
