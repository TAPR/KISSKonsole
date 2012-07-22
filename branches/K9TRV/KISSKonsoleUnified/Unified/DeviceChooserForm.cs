using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KISS_Konsole
{
    public partial class DeviceChooserForm : Form
    {
        List<MetisHermesDevice> mhd;

        public DeviceChooserForm(List<MetisHermesDevice> mhd, string LastMACChosen)
        {
            InitializeComponent();

            this.mhd = mhd;

            labelBoardsFound.Text = String.Format("Boards found ({0}):", mhd.Count);
            int index = 0;
            foreach (MetisHermesDevice m in mhd)
            {
                string s;
                s = String.Format("{0} MAC {1}, IP {2}, on host port IP {3}", m.deviceType.ToString(), m.MACAddress, m.IPAddress, m.hostPortIPAddress);
                comboBox1.Items.Add(s);

                if (LastMACChosen.ToUpper().CompareTo(m.MACAddress.ToUpper()) == 0)
                {
                    comboBox1.SelectedIndex = index;
                }
                ++index;
            }

            textBoxLastMAC.Text = LastMACChosen;
        }

        int chosenItem = -1;

        public int GetChosenItem()
        {
            return chosenItem;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            chosenItem = comboBox1.SelectedIndex;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                buttonOK.Enabled = true;
            }
            else
            {
                buttonOK.Enabled = false;
            }
        }
    }
}
