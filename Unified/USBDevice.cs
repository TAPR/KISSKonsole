using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using HPSDR_USB_LIB_V1;
using System.Threading;
using System.Diagnostics;
using System.Drawing;

namespace KISS_Konsole
{
    class USBDevice : HPSDRDevice
    {
        IntPtr hdev = IntPtr.Zero;                  // USB Ozy device handle

        const int toDeviceSize = 2048;

        public USBDevice(Form1 mainForm)
            : base(mainForm, toDeviceSize)
        {
        }

        public override void Start()
        {
            // start the thread that reads the USB port on Ozy
            if (!start_USB())
            {
                MessageBox.Show("No Hardware found  - Check boards are connected and powered");
                MainForm.OnOffButton_Click(this, EventArgs.Empty); // Toggle ON/OFF Button to OFF
                return;
            }

            // Check that FX2 has been loaded with software, if not call initozy11.bat to load it
            MainForm.Ozy_version = getOzyFirmwareString(); // Get ozy firmware version string - 8 bytes,  returns null for error

            if (MainForm.Ozy_version == null)
            {
                string batchFile = "initozy11.bat";
                // check for presence of initozy11.bat.  Warn and fail if not present.
                if (!System.IO.File.Exists(batchFile))
                {
                    // desired file does not exist in the current directory!
                    string msg = String.Format("The batch file, {0},\ndoes not exist in the current directory, {1}.\n",
                        batchFile, System.IO.Directory.GetCurrentDirectory());
                    msg += "\nOzy/Magister cannot be started without this missing file!\n";
                    msg += "The following files might be the correct file, but MISNAMED!\nYou will have to rename the proper file to the correct name to continue.\n";

                    string[] fileList = System.IO.Directory.GetFiles(System.IO.Directory.GetCurrentDirectory(), "init*", System.IO.SearchOption.TopDirectoryOnly);
                    if (fileList == null)
                    {
                        msg += "\nNo likely mis-named files were found.";
                    }
                    else
                    {
                        foreach (string f in fileList)
                        {
                            msg += String.Format("\nPossible mis-named file {0}", f);
                        }
                    }

                    msg += "\n\nThe radio will be turned OFF when you click on the OK button.";
                    MessageBox.Show(msg, batchFile + " cannot be found!", MessageBoxButtons.OK);

                    MainForm.OnOffButton_Click(this, EventArgs.Empty); // Toggle ON/OFF Button to OFF
                    return;
                }

                MainForm.Cursor = Cursors.WaitCursor;  // show hour glass cursor whilst we load Ozy FPGA
                // call a process to run initozy11.bat in current directory
                ProcessStartInfo start_info = new ProcessStartInfo();
                start_info.FileName = batchFile;
                start_info.UseShellExecute = true;
                Process p = new Process();
                p.StartInfo = start_info;
                bool rc = p.Start();
                System.Console.WriteLine("start returned: " + rc);
                p.WaitForExit();
                System.Console.WriteLine("OzyInit completes");
                stop_USB();  // need to close and re-open USB port since it renumerated
                start_USB();
                MainForm.Cursor = Cursors.Default;  // revert to normal cursor

                // get ozy code version now
                MainForm.Ozy_version = getOzyFirmwareString();
            }

            // check that we are using the correct version of FX2 code
            if (MainForm.Ozy_version != "20090524")
            {
                // note that the Ozy_version may be null.  If so, the old code here would have generated an exception
                // and not failed softly as intended.  The current/new code handles that case reasonably...
                MessageBox.Show(" Wrong version of FX2 code found " + (MainForm.Ozy_version == null ? "(Not Found!)" : MainForm.Ozy_version.ToString()) + "\n-should be 20090524");

                MainForm.OnOffButton_Click(this, EventArgs.Empty); // Toggle ON/OFF Button to OFF
                stop_USB();
                return;
            }

            Debug.WriteLine("Starting USB Thread");

            // start thread to read Ozy data from USB.   DataLoop merely calls Process_Data, 
            // which calls usb_bulk_read() and rcvr.Process(),
            // and stuffs the demodulated audio into AudioRing buffer
            loop_count = 0;         // reset count so that data will be re-collected
            Data_thread = new Thread(new ThreadStart(DataLoop));
            Data_thread.Name = "USB Loop";
            Data_thread.Priority = ThreadPriority.Highest; // run USB thread at high priority
            //USB_thread.IsBackground = true;  // do we need this ?
            Data_thread.Start();
            Data_thread_running = true;
            MainForm.timer1.Enabled = true;  // start timer for bandscope update.

            MainForm.timer2.Enabled = true;  // used by Clip LED, fires every 100mS
            MainForm.timer2.Interval = 100;
            MainForm.timer3.Enabled = true;  // used by VOX, fires on user VOX delay setting 
            MainForm.timer3.Interval = 400;
            MainForm.timer4.Enabled = true;  // used by noise gate, fires on Gate delay setting
            MainForm.timer4.Interval = 100;

            if ((MainForm.PenneyPresent || MainForm.PennyLane) && MainForm.Penny_version != 0)
            {
                SetMicGain();   // select 0 or +20dB mic gain or Line in for Penelope or PennyLane TLV320 ADC
            }
        }

        public override void Stop()
        {
            if (Data_thread_running)
            {
                Data_thread.Abort();  // stop USB thread
                Data_thread_running = false;
            }
            MainForm.timer1.Enabled = false;
            stop_USB();          // kill the USB port 
            MainForm.SyncLED.BackColor = SystemColors.Control;  // no sync so set LED to background
        }

        public override void Close()
        {
            if (Data_thread_running)
                Data_thread.Abort();  // stop USB thread

            if (hdev != IntPtr.Zero)  // check we have an active USB port
                stop_USB();
        }

        public override void SetMicGain()
        {
            // This is used to set the MicGain and Line in when Ozy/Magister is used
            // The I2C settings are as follows: 

            //    For mic input and boost on/off
            //    1E 00 - Reset chip
            //    12 01 - set digital interface active
            //    08 15 - D/A on, mic input, mic 20dB boost
            //    08 14 - ditto but no mic boost
            //    0C 00 - All chip power on
            //    0E 02 - Slave, 16 bit, I2S
            //    10 00 - 48k, Normal mode
            //    0A 00 - turn D/A mute off
            //    00 00 - set Line in gain to 0

            //    For line input                           
            //    1E 00 - Reset chip
            //    12 01 - set digital interface active
            //    08 10 - D/A on, line input
            //    0C 00 - All chip power on
            //    0E 02 - Slave, 16 bit, I2S
            //    10 00 - 48k, Normal mode
            //    0A 00 - turn D/A mute off 
            //    00 00 - set Line in gain to 0

            if ((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 0) && (MainForm.KK_on == true))  // update mic gain on Penny or PennyLane TLV320 
            {

                byte[] Penny_TLV320 = new byte[2];
                byte[] Penny_TLV320_data = new byte[16];

                // need to select the config data depending on the Mic Gain (20dB) selected
                if (MainForm.MicGain20dB)
                    Penny_TLV320_data = new byte[] { 0x1e, 0x00, 0x12, 0x01, 0x08, 0x15, 0x0c, 0x00, 0x0e, 0x02, 0x10, 0x00, 0x0a, 0x00, 0x00, 0x00 };
                else if (MainForm.LineIn)
                    Penny_TLV320_data = new byte[] { 0x1e, 0x00, 0x12, 0x01, 0x08, 0x10, 0x0c, 0x00, 0x0e, 0x02, 0x10, 0x00, 0x0a, 0x00, 0x00, 0x00 };

                else
                    Penny_TLV320_data = new byte[] { 0x1e, 0x00, 0x12, 0x01, 0x08, 0x14, 0x0c, 0x00, 0x0e, 0x02, 0x10, 0x00, 0x0a, 0x00, 0x00, 0x00 };

                // set the I2C interface speed to 400kHZ
                if (!(OZY.Set_I2C_Speed(hdev, 1)))
                {
                    MessageBox.Show("Unable to set I2C speed to 400kHz", "System Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // send the configuration data to the TLV320 on Penelope or PennyLane 
                for (int x = 0; x < 16; x += 2)
                {
                    Penny_TLV320[0] = Penny_TLV320_data[x]; Penny_TLV320[1] = Penny_TLV320_data[x + 1];
                    if (!(OZY.Write_I2C(hdev, 0x1b, Penny_TLV320)))
                    {
                        MessageBox.Show("Unable to configure TLV320 on Penelope via I2C", "System Eror!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        // break out of the configuration loop 
                        break;
                    }
                }
            }
        }

        public override void ProcessWideBandData(ref byte[] EP4buf)
        {
            if (MainForm.doWideBand) // display wide band spectrum
            {
                // Read new buffer of full-bandwidth
                int ret = libUSB_Interface.usb_bulk_read(hdev, 0x84, EP4buf, Form1.EP4BufSize, 100);
                if (ret == Form1.EP4BufSize)
                {
                    float scaleIn = (float)(1.0 / Math.Pow(2, 15));
                    float Sample, RealAverage;

                    RealAverage = 0.0f;
                    int i, k;
                    for (i = 0, k = 0; i < Form1.EP4BufSize; i += 2, ++k)
                    {
                        // use this rather than BitConverter.ToInt16()...since its faster and less CPU intensive
                        // without the '(short)' in there, the value doesn't get sign-extended!
                        // so what should be small negative values show up as (almost) positive values?
                        Sample = scaleIn * (float)(short)((EP4buf[i] << 8) | (EP4buf[i + 1]));

                        RealAverage += Sample;
                        SamplesReal[k] = Sample;
                    }

                    RealAverage /= (float)k;
                    for (i = 0; i < k; ++i)
                    {
                        SamplesReal[i] -= RealAverage;      // Subtract average
                        SamplesImag[i] = SamplesReal[i];    // temporary -- soon will do digital down-conversion
                        // with sin & cos, so we'll actually have I & Q data to enter.
                    }

                    FullBandwidthSpectrum.Process(SamplesReal, SamplesImag, Form1.EP4BufSize / 2);
                }
            }
        }

        // This thread runs at program load and reads data from Ozy
        private void DataLoop()
        {
            int ret;
            while (true) // do this forever 
            {
                if (MainForm.KK_on)  // only read ADC data if USB is selected
                {
                    ret = libUSB_Interface.usb_bulk_read(hdev, 0x86, rbuf, Form1.rbufSize, 1000); // rbuf.Length = 2048
                    if (ret == Form1.rbufSize)
                    {
                        // set flag so know we are receiving valid data used in timer to set Sync LED
                        MainForm.received_flag = true;

                        Process_Data(ref rbuf);
                    }
                    else
                    {
                        // we can't read from EP6 for some reason - perhaps Ozy not primed so 
                        // send some C&C frames to prime the pump.
                        bool force = true;
                        Thread.Sleep(10); // so we don't end up in a tight loop 
                        Debug.WriteLine("EP6 Read Error -  returns \t" + ret);
                        Debug.WriteLine("EP6 Read Error -  hdev \t" + hdev);
                        Data_send(force);  // force C&C data to Ozy to get started 
                    }
                }
                else
                {
                    // Nothing selected so sleep for 100mS so we don't hog all the CPU
                    Thread.Sleep(100);
                }
            }
        }

        // Send four frames of 512 bytes to Ozy
        // The left & right audio to be sent comes from AudioRing buffer, which is filled by ProcessData()
        protected override void Data_send(bool force)
        {
            // if force is set then send C&C anyway even if no data available
            if (!force)
            {
                // ringBufferRequiredSize I words + ringBufferRequiredSize Q words plus  2x sync (6 bytes) plus 2x C&C (10 bytes) = 1024 bytes
                if (AudioRing.Count < 252)
                {
                    return;  // need enough data for 2 frames
                }
            }

            Data_send_core(4, 0);

            if (MainForm.KK_on)  // send the frames to Ozy via the USB
            {
                int ret;
                //do
                //{
                ret = libUSB_Interface.usb_bulk_write(hdev, 0x02, to_Device, toDeviceSize, 100);
                //} while (ret != toDeviceSize);
                // TODO: Need to add error routine if write to USB fails
                if (ret != toDeviceSize)
                {
                    Debug.WriteLine("Write to Ozy failed - returned \t" + ret);
                }
            }
        }
        
        private bool start_USB()
        {
            // look for USB connection to Ozy
            hdev = USB.InitFindAndOpenDevice(0xfffe, 0x0007);
            if (hdev == IntPtr.Zero)
                return false;

            int ret;
            ret = libUSB_Interface.usb_set_configuration(hdev, 1);
            ret = libUSB_Interface.usb_claim_interface(hdev, 0);
            ret = libUSB_Interface.usb_set_altinterface(hdev, 0);
            ret = libUSB_Interface.usb_clear_halt(hdev, 0x02);
            ret = libUSB_Interface.usb_clear_halt(hdev, 0x86);
            ret = libUSB_Interface.usb_clear_halt(hdev, 0x84);
            return true;
        }

        // stop USB interface when we exit the program
        private void stop_USB()
        {
            if (hdev != IntPtr.Zero) // check we have an open USB port 
            {
                // K9TRV gets an error here on the usb_close for hdev being invalid, most likely as a result of the
                // call to usb_release_interface...
                // looks like the usb_close should close the interface...
                // the error/exception can happen under many circumstances for me.
                // Best to place in try/catch so the exceptions are caught, but not reported to the user
                try
                {
                    libUSB_Interface.usb_release_interface(hdev, 0);
                    libUSB_Interface.usb_close(hdev);
                }
                catch
                {
                }
            }
        }

        private string getOzyFirmwareString()
        {
            if (hdev == IntPtr.Zero)
            {
                return null;
            }

            // the following set of declarations MUST match the values used in the FX2 code - hpsdr_commands.h
            byte VRQ_SDR1K_CTL = 0x0d;
            byte SDR1KCTRL_READ_VERSION = 0x7;
            byte VRT_VENDOR_IN = 0xC0;
            byte[] buf = new byte[8];
            int rc = libUSB_Interface.usb_control_msg(hdev, VRT_VENDOR_IN, VRQ_SDR1K_CTL, SDR1KCTRL_READ_VERSION, 0, buf, buf.Length, 1000);
            Debug.WriteLine("read version rc: " + rc);
            Debug.WriteLine("read version hdev = \t" + hdev);

            string result = null;

            if (rc == 8)    // got length we expected
            {
                char[] cbuf = new char[8];
                for (int i = 0; i < 8; i++)
                {
                    cbuf[i] = (char)(buf[i]);
                }
                result = new string(cbuf);
                Debug.WriteLine("version: >" + result + "<");
            }
            return result;
        }

        /*
         * NOT to be called if using a Hermes
         * */
        public override bool CheckVersions()
        {
            bool result = true;

            string whatsPresent;
            if (MainForm.PenneyPresent || MainForm.PennyLane)
            {
                whatsPresent = String.Format("You have a Penelope or PennyLane with firmware version {0}.\n", MainForm.Penny_version);
            }
            else
            {
                whatsPresent = String.Format("You have no Penelope or PennyLane.\n");
            }

            whatsPresent += String.Format("You have a Mercury with firmware version {0}.\n", MainForm.Merc_version);
            whatsPresent += String.Format("Your Ozy/Magister has firmware version {0}.\n", MainForm.Ozy_FPGA_version);
            whatsPresent += "\nThis is an incompatible collection of firmware.\n";
            whatsPresent += "\nThe radio will now stop running when you press OK.";

            // in KISS Konsole, it is always presumed that you have a Mercury or Mercury-equivalent (Hermes)
            switch (MainForm.Ozy_FPGA_version)
             {
                 case 18:
                     if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 13)) ||
                          ((MainForm.Merc_version != 29)))
                     {
                         MessageBox.Show(whatsPresent, "You must use Penney Version 13 and Mercury version 29 with Ozy version 18", MessageBoxButtons.OK);
                         result = false;
                     }
                     break;

                 case 19:
                     if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 14)) ||
                          ((MainForm.Merc_version != 29)))
                     {
                         MessageBox.Show(whatsPresent, "You must use Penney Version 14 and Mercury version 29 with Ozy version 19", MessageBoxButtons.OK);
                         result = false;
                     }
                     break;

                 case 20:
                     if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 15)) ||
                        ((MainForm.Merc_version != 30)))
                     {
                         MessageBox.Show(whatsPresent, "You must use Penney Version 15 and Mercury version 30 with Ozy version 20", MessageBoxButtons.OK);
                         result = false;
                     }
                     break;

                 case 21:
                     if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 16)) ||
                         ((MainForm.Merc_version != 31)))
                     {
                         MessageBox.Show(whatsPresent, "You must use Penney Version 16 and Mercury version 31 with Ozy version 21", MessageBoxButtons.OK);
                         result = false;
                     }
                     break;

                 case 22: // K5SO Diversity & non-diversity
                     if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 17)) ||
                         ((MainForm.Merc_version != 32 && MainForm.Merc_version != 72)))
                     {
                         whatsPresent = "This looks like it might be a board set with firmware for K5SO's diversity and non-diversity\n\n" + whatsPresent;
                         MessageBox.Show(whatsPresent, "You must use Penney Version 17 and Mercury version 32 or 72 with Ozy version 22", MessageBoxButtons.OK);
                         result = false;
                     }
                     break;

                 // Ozy v2.3 has a problem with some hardware configs/PC option selections yield incorrect freq assignment for Rx1, incorrect 122.88 MHz clock assignment when Penelope selected as source
                 case 23:
                     if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 17)) ||
                         ((MainForm.Merc_version != 33)))
                     {
                         MessageBox.Show(whatsPresent, "You must use Penney Version 17 and Mercury version 33 with Ozy version 23", MessageBoxButtons.OK);
                         MessageBox.Show(whatsPresent, "Ozy version 23 has problems.  Please upgrade to Ozy version 24", MessageBoxButtons.OK);
                         result = false;
                     }
                     break;

                 case 24:
                     if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 17)) ||
                         ((MainForm.Merc_version != 33)))
                     {
                         MessageBox.Show(whatsPresent, "You must use Penney Version 17 and Mercury version 33 with Ozy version 24", MessageBoxButtons.OK);
                         result = false;
                     }
                     break;

                 default:
                     MessageBox.Show(whatsPresent, "This version of Ozy hasn't been entered into the program.", MessageBoxButtons.OK);
                     whatsPresent = "Please contact K9TRV or the current KISS Konsole maintainer with a screen shot of this message,\n"
                         + "but ONLY after you check for updates to KISS Konsole that might handle this version of Ozy.\n\n" + whatsPresent;
                     result = false;
                     break;
             }

            return result;
        }

        private byte[] dummyByte = new byte[1];
        private byte[] dummyTwoBytes = new byte[2];

        public bool readI2CByte(byte targetAddr)
        {
            bool result;
            result = OZY.Read_I2C(hdev, targetAddr, ref dummyByte);
            return result;
        }

        public bool readI2CTwoBytes(byte targetAddr)
        {
            bool result;
            result = OZY.Read_I2C(hdev, targetAddr, ref dummyTwoBytes);
            return result;
        }

        const byte Mercury1FWVersionAddr = 0x10;
        const byte Mercury2FWVersionAddr = 0x11;
        const byte Mercury3FWVersionAddr = 0x12;
        const byte Mercury4FWVersionAddr = 0x13;
        const byte Mercury1OverloadAddr = 0x14;
        const byte Mercury2OverloadAddr = 0x15;
        const byte Mercury3OverloadAddr = 0x16;
        const byte Mercury4OverloadAddr = 0x17;
        const byte PenneyFWVersionAddr = 0x20;
        const byte PenneyPowerOutAddr = 0x21;
        const byte AlexFwdPowerOutAddr = 0x22;
        const byte AlexRevPowerOutAddr = 0x23;

        public override void SpecialI2CProcessing()
        {
            readI2CByte(PenneyFWVersionAddr);
            readI2CByte(Mercury1FWVersionAddr);
            readI2CByte(Mercury1OverloadAddr);

            readI2CTwoBytes(PenneyPowerOutAddr);
            readI2CTwoBytes(AlexFwdPowerOutAddr);
            readI2CTwoBytes(AlexRevPowerOutAddr);
        }
    }
}
