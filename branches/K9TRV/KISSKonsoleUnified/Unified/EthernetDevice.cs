using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace KISS_Konsole
{
    class EthernetDevice : HPSDRDevice
    {
        uint sequence_number = 0;  // 32 bit unsigned integer for the sequence number
        uint last_sequence_number;

        //uint Spectrum_sequ_number = 0;
        uint last_spectrum_sequence_number;

        const int toDeviceSize = 1024 + 8;             // now includes the 8 bytes before the data

        // get the name of this PC and, using it, the IP address of the first adapter
        String strHostName = Dns.GetHostName();
        public IPAddress[] addr = Dns.GetHostEntry(Dns.GetHostName()).AddressList;

        // get a socket to send and receive on
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // set an endpoint
        IPEndPoint iep;

        // receive data buffer for EP6 (normal data)
        byte[] data = new byte[1032];

        int Ethernet_count = 0;

        const int MetisPort = 1024;
        const int LocalPort = 0;
        private IPEndPoint MetisEP = null;

        public EthernetDevice(Form1 mainForm)
            : base(mainForm, toDeviceSize)
        {
        }

        private bool DiscoverMetisOnPort(ref List<MetisHermesDevice> mhdList, IPAddress HostIP, IPAddress targetIP)
        {
            bool result = false;

            // configure a new socket object for each Ethernet port we're scanning
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Listen to data on this PC's IP address. Allow the program to allocate a free port.
            iep = new IPEndPoint(HostIP, LocalPort);  // was iep = new IPEndPoint(ipa, 0);

            try
            {
                // bind to socket and Port
                socket.Bind(iep);
                socket.ReceiveBufferSize = 0xFFFF;   // no lost frame counts at 192kHz with this setting
                socket.Blocking = true;

                IPEndPoint localEndPoint = (IPEndPoint)socket.LocalEndPoint;
                Console.WriteLine("Looking for Metis boards using host adapter IP {0}, port {1}", localEndPoint.Address, localEndPoint.Port);

                if (Metis_Discovery(ref mhdList, iep, targetIP))
                {
                    result = true;
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Caught an exception while binding a socket to endpoint {0}.  Exception was: {1} ", iep.ToString(), ex.ToString());
                result = false;
            }
            finally
            {
                socket.Close();
                socket = null;
            }

            return result;
        }

        public override void Start()
        {
            // adapterSelected ranges from 1 thru number Of Adapters.  However, we need an 'index', which
            // is adapterSelected-1.
            int adapterIndex = MainForm.adapterSelected - 1;

            // get the name of this PC and, using it, the IP address of the first adapter
            IPAddress[] addr = Dns.GetHostEntry(strHostName).AddressList;

            MainForm.GetNetworkInterfaces();

            List<IPAddress> addrList = new List<IPAddress>();

            // make a list of all the adapters that we found in Dns.GetHostEntry(strHostName).AddressList
            foreach (IPAddress a in addr)
            {
                // make sure to get only IPV4 addresses!
                // test added because Erik Anderson noted an issue on Windows 7.  May have been in the socket
                // construction or binding below.
                if (a.AddressFamily == AddressFamily.InterNetwork)
                {
                    addrList.Add(a);
                }
            }

            bool foundMetis = false;
            List<MetisHermesDevice> mhd = new List<MetisHermesDevice>();

            if (MainForm.DoFastEthernetConnect && (MainForm.EthernetHostIPAddress.Length > 0) && (MainForm.Metis_IP_address.Length > 0))
            {
                // if success set foundMetis to true, and fill in ONE mhd entry.
                IPAddress targetIP;
                IPAddress hostIP;
                if (IPAddress.TryParse(MainForm.EthernetHostIPAddress, out hostIP) && IPAddress.TryParse(MainForm.Metis_IP_address, out targetIP))
                {
                    Console.WriteLine(String.Format("Attempting fast re-connect to host adapter {0}, metis IP {1}", MainForm.EthernetHostIPAddress, MainForm.Metis_IP_address));

                    if (DiscoverMetisOnPort(ref mhd, hostIP, targetIP))
                    {
                        foundMetis = true;

                        // make sure that there is only one entry in the list!
                        if (mhd.Count > 0)
                        {
                            // remove the extra ones that don't match!
                            MetisHermesDevice m2 = null;
                            foreach (var m in mhd)
                            {
                                if (m.IPAddress.CompareTo(MainForm.Metis_IP_address) == 0)
                                {
                                    m2 = m;                                  
                                }
                            }

                            // clear the list and put our single element in it, if we found it.
                            mhd.Clear();
                            if (m2 != null)
                            {
                                mhd.Add(m2);
                            }
                            else
                            {
                                foundMetis = false;
                            }
                        }
                    }
                }
            }

            if (!foundMetis)
            {
                foreach (IPAddress ipa in addrList)
                {
                    if (DiscoverMetisOnPort(ref mhd, ipa, null))
                    {
                        foundMetis = true;
                    }
                }
            }

            if (!foundMetis)
            {
                MessageBox.Show("No Metis/Hermes board found  - Check HPSDR is connected and powered");
                MainForm.OnOffButton_Click(this, EventArgs.Empty); // Toggle ON/OFF Button to OFF
                return;
            }
            int chosenDevice = 0;

            if (mhd.Count > 1)
            {
                // show selection dialog.
                DeviceChooserForm dcf = new DeviceChooserForm(mhd, MainForm.MetisMAC);
                DialogResult dr = dcf.ShowDialog();
                if (dr == DialogResult.Cancel)
                {
                    MainForm.OnOffButton_Click(this, EventArgs.Empty); // Toggle ON/OFF Button to OFF
                    return;
                }

                chosenDevice = dcf.GetChosenItem();
            }

            MainForm.Metis_IP_address = mhd[chosenDevice].IPAddress;
            MainForm.MetisMAC = mhd[chosenDevice].MACAddress;
            MainForm.EthernetHostIPAddress = mhd[chosenDevice].hostPortIPAddress.ToString();

            iep = new IPEndPoint(mhd[chosenDevice].hostPortIPAddress, LocalPort);

            // bind (open) the socket so we can use the Metis/Hermes that was found/selected
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(iep);
            socket.ReceiveBufferSize = 0xFFFF;   // no lost frame counts at 192kHz with this setting
            socket.SendBufferSize = 1032;
            socket.Blocking = true;

            // create an endpoint for sending to Metis
            MetisEP = new IPEndPoint(IPAddress.Parse(MainForm.Metis_IP_address), MetisPort);

            IPEndPoint localEndPoint2 = (IPEndPoint)socket.LocalEndPoint;
            Console.WriteLine("Starting Ethernet Thread: host adapter IP {0}, port {1}", localEndPoint2.Address, localEndPoint2.Port);

            //// start thread to read Metis data from Ethernet.   DataLoop merely calls Process_Data, 
            //// which calls usb_bulk_read() and rcvr.Process(),
            //// and stuffs the demodulated audio into AudioRing buffer
            loop_count = 0;         // reset count so that data will be re-collected
            Data_thread = new Thread(new ThreadStart(DataLoop));
            Data_thread.Name = "Ethernet Loop";
            Data_thread.Priority = ThreadPriority.Highest; // run USB thread at high priority
            Data_thread.Start();
            Data_thread_running = true;

            Data_send(true); // send a frame to Ozy to prime the pump
            Thread.Sleep(20);
            Data_send(true); // send a frame to Ozy to prime the pump,with freq this time 

            // reset the sequence_number to match what Metis does following a Discovery
            sequence_number = 0;
            last_sequence_number = 0xFFFFFFFF;
            //Spectrum_sequ_number = 0;
            last_spectrum_sequence_number = 0xFFFFFFFF;

            // start data from Metis
            Metis_start_stop(MainForm.Metis_IP_address, 0x03);   // bit 0 is start, bit 1 is wide spectrum

            MainForm.timer1.Enabled = true;  // start timer for bandscope update etc.

            MainForm.timer2.Enabled = true;  // used by Clip LED, fires every 100mS
            MainForm.timer2.Interval = 100;
            MainForm.timer3.Enabled = true;  // used by VOX, fires on user VOX delay setting 
            MainForm.timer3.Interval = 400;
            MainForm.timer4.Enabled = true;  // used by noise gate, fires on Gate delay setting
            MainForm.timer4.Interval = 100;

            // update the display on the SetupForm if it is being displayed
            if (MainForm.Setup_form != null)
            {
                MainForm.Setup_form.UpdateEthernetInfo();
            }
        }

        public override void Stop()
        {
            if (Data_thread_running)
            {
                Data_thread.Abort();  // stop data thread

                // stop data from Metis
                Metis_start_stop(MainForm.Metis_IP_address, 0x00); // stop data from Metis
                socket.Close();
                socket = null;

                Data_thread_running = false;
            }

            if (MainForm.timer1.Enabled)
                MainForm.timer1.Stop();          // stop timer1

            MainForm.SyncLED.BackColor = SystemColors.Control;  // no sync so set LED to background
        }

        public override void Close()
        {
            if (Data_thread_running)
            {
                Data_thread.Abort();  // stop data thread

                Metis_start_stop(MainForm.Metis_IP_address, 0x00);
                socket.Close();
                socket = null;

                Data_thread_running = false;
            }
        }

        public override void SetMicGain()
        {
        }

        public override void ProcessWideBandData(ref byte[] EP4buf)
        {
            if (MainForm.doWideBand) // display wide band spectrum
            {
                // EP4 contains 4k x 16 bit raw ADC samples
                // Actually it may contain 4 times that! (16k x 16 bit raw adc samples)
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
                        Sample = scaleIn * (float)(short)((EP4buf[i + 1] << 8) | (EP4buf[i]));
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

        // This thread runs at program load and reads data Metis
        private void DataLoop()
        {
            uint Metis_sequ_number;
            uint Spectrum_sequ_number;
            bool start = false;

            while (true) // do this forever 
            {
                bool data_available = false;

                // Poll the port to see if data is available 
                data_available = socket.Poll(100000, SelectMode.SelectRead);  // wait 100msec  for time out *****

                if (data_available)
                {
                    // check received data is for EP4 or EP6, if not return
                    int recv = socket.Receive(data);          // data holds the UDP Payload

                    // check for EP6 data (receiver)
                    if ((data[3] == 0x06) && MainForm.KK_on)
                    {
                        // set flag so know we are receiving valid data used in timer to set Sync LED
                        MainForm.received_flag = true;

                        // get the sequence number
                        Metis_sequ_number = ((uint)(data[4] << 24) | (uint)(data[5] << 16) | (uint)(data[6] << 8) | (uint)data[7]);

                        if (Metis_sequ_number == unchecked(last_sequence_number + 1))
                        {
                            MainForm.IsInSequence = true;  // use this to set the colour of the Sync LED when the timer fires
                        }
                        else
                        {
                            MainForm.IsInSequence = false;
                            Console.WriteLine("EP6 Sequence error! last = " + last_sequence_number + " current = " + Metis_sequ_number);
                        }

                        last_sequence_number = Metis_sequ_number;

                        if (++Ethernet_count == 2) // we have two 1024 byte frames so concatinate and send to Process_Data
                        {
                            Ethernet_count = 0;  // reset Ethernet frame counter
                            // add to previous frame
                            Array.Copy(data, 8, rbuf, 1024, 1024);

                            // we now have 2048 samples so process them
                            Process_Data(ref rbuf);
                        }
                        // only one frame so save this one and wait for the next
                        else
                        {
                            Array.Copy(data, 8, rbuf, 0, 1024);
                        }
                    }

                    // check for data to EP4 i.e. wideband spectrum data
                    if ((data[3] == 0x04) && MainForm.KK_on)
                    {
                        Spectrum_sequ_number = ((uint)(data[4] << 24) | (uint)(data[5] << 16) | (uint)(data[6] << 8) | (uint)data[7]);

                        if (Spectrum_sequ_number != unchecked(last_spectrum_sequence_number + 1))
                        {
                            Console.WriteLine("EP4 (WideBand) Sequence error! last = " + last_sequence_number + " current = " + Spectrum_sequ_number);
                            // if a wideband (EP4) sequence error, discard any accumulated frames so that all the
                            // wideband data is from the same group.
                            start = false;
                        }

                        last_spectrum_sequence_number = Spectrum_sequ_number;

                        // start copying data when last 3 bits of sequence number are 0.  This is ONLY VALID if
                        // EP4BufSize = 8192 (4096 I and 4096 Q samples).
                        // if '16k I and 16k Q samples, then EP4BufSize = 32768, and the bitmask has 2 more bits in it (0x1f)
                        // AND Spectrum_count goes to 31, not 7.
                        if ((sampleMask & data[7]) == 0)
                        {
                            start = true;
                            Spectrum_count = 0;  // reset Spectrum frame counter
                        }

                        if (start)
                        {
                            Array.Copy(data, 8, EP4buf, (Spectrum_count * 1024), 1024);
                        }

                        if (Spectrum_count++ == (numWideBuffers - 1)) // we have numWideBuffers by 1024 byte frames so concatenate and process
                        {
                            start = false;
                            ProcessWideBandData(ref EP4buf);
                        }
                    }
                }
            } 
        }

        // Send two frames of 512 bytes to Metis/Hermes/Griffin
        // The left & right audio to be sent comes from AudioRing buffer, which is filled by ProcessData()
        protected override void Data_send(bool force)
        {
            // if force is set then send C&C anyway even if no data available
            if (!force)
            {
                // ringBufferRequiredSize I words + ringBufferRequiredSize Q words plus  2x sync (6 bytes) plus 2x C&C (10 bytes) = 1024 bytes
                if (AudioRing.Count < 126)
                {
                    return;  // need enough data for 2 frames
                }
            }

            Data_send_core(2, 8);

            if (MainForm.KK_on)  // send the frames to Ozy using UDP/IP
            {
                // put the header info into the start of the to_Device buffer
                to_Device[0] = 0xEF;   // first 2 bytes are ether type
                to_Device[1] = 0xFE;
                to_Device[2] = 0x01;   // frame type = 1
                to_Device[3] = 0x02;   // end point = EP2

                // insert sequence number.  It's in 'big-endian' order (MSByte first, LSByte last)
                to_Device[4] = (byte)(sequence_number >> 24);
                to_Device[5] = (byte)((sequence_number >> 16) & 0xff);
                to_Device[6] = (byte)((sequence_number >> 8) & 0xff);
                to_Device[7] = (byte)(sequence_number & 0xff);

                try
                {
                    //Send the UDP/IP packet out the network device
                    int rc = socket.SendTo(to_Device, MetisEP);
                }
                catch (Exception d)
                {
                    Console.WriteLine("-- " + d.Message);
                }

                // increment the sequence number for next time.  Make sure it's an unchecked operation, so that
                // no ArithmeticException is thrown when it wraps back to 0 (overflow.)
                unchecked
                {
                    sequence_number++;
                }
            }
        }
        
        /* 
          * Send a UDP/IP packet to the IP address and port of the Metis board
          * Payload is 
          *      0xEFFE,0x04,run, 60 nulls
          *    where run = 0x00 to stop data and 0x01 to start
          * 
          */

        private void Metis_start_stop(string Metis_IP_address, byte run)
        {
            byte[] Ether_Type = new byte[] { 0xEF, 0xFE };
            byte[] HPSDR_Frame_type = new byte[] { 0x04 };
            byte run_state = run;

            // create an array to hold the data to send to the Network 
            byte[] data_to_send = new byte[64];

            // concatenate all the data into one array
            Ether_Type.CopyTo(data_to_send, 0);   // concatinate the data

            HPSDR_Frame_type.CopyTo(data_to_send, 2);

            data_to_send[3] = run_state;

            // set IP address and port of the Metis card we wish to communicate 
            IPEndPoint MetisEP = new IPEndPoint(IPAddress.Parse(Metis_IP_address), MetisPort);

            try
            {
                // Send the UDP/IP packet out the network device
                int rc = socket.SendTo(data_to_send, MetisEP);
            }
            catch (Exception d)
            {
                Console.WriteLine("-- " + d.Message);
            }
        }

        /*
          Broadcast a Metis Discovery packet and look for boards.
         
          Code Broadcasts (IP address FF.FF.FF.FF, the 'all broadcast address') a  UDP/IP PC HPSDR discovery packet on port 1024 with the following format:

              0xEFFE, 0x02, <sixty bytes of zero>
          
         It then listens on the PC *from* port for a Metis reply with the following format:
    
              0xEFFE, 0x02, Metis MAC address <41 bytes of zero>
          
         Metis' IP address is obtained from the packet header.
          
        */

        private bool Metis_Discovery(ref List<MetisHermesDevice> mhdList, IPEndPoint iep, IPAddress targetIP)
        {
            string MetisMAC;

            socket.SendBufferSize = 1024;

            // set up HPSDR Metis discovery packet
            byte[] Metis_discovery = new byte[63];
            Array.Clear(Metis_discovery, 0, Metis_discovery.Length);

            byte[] Metis_discovery_preamble = new byte[] { 0xEF, 0xFE, 0x02 };
            Metis_discovery_preamble.CopyTo(Metis_discovery, 0);

            bool have_Metis = false;            // true when we find an Metis
            int time_out = 0;

            // set socket option so that broadcast is allowed.
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

            // need this so we can Broadcast on the socket
            IPEndPoint broadcast = new IPEndPoint(IPAddress.Broadcast, MetisPort);
            string receivedIP = "";   // the IP address Metis obtains; assigned, from DHCP or APIPA (169.254.x.y)

            IPAddress hostPortIPAddress = iep.Address;
            IPAddress hostPortMask = IPAddress.Broadcast;

            // find the subnet mask that goes with this host port
            foreach (NicProperties n in MainForm.nicProperties)
            {
                if (hostPortIPAddress.Equals(n.ipv4Address))
                {
                    hostPortMask = n.ipv4Mask;
                    break;
                }
            }

            // send every second until we either find an Metis board or exceed the number of attempts
            while (!have_Metis)            // #### djm should loop for a while in case there are multiple Metis boards
            {
                // send a broadcast to the port 1024
                socket.SendTo(Metis_discovery, broadcast);

                // now listen on  send port for any Metis cards
                Console.WriteLine("Ready to receive.... ");
                int recv;
                byte[] data = new byte[100];

                bool data_available;

                // await possibly multiple replies, if there are multiple Metis/Hermes on this port,
                // which MIGHT be the 'any' port, 0.0.0.0
                do
                {
                    // Poll the port to see if data is available 
                    data_available = socket.Poll(100000, SelectMode.SelectRead);  // wait 100 msec  for time out    

                    if (data_available)
                    {
                        EndPoint remoteEP = new IPEndPoint(IPAddress.None, 0);
                        recv = socket.ReceiveFrom(data, ref remoteEP);                 // recv has number of bytes we received
                        //string stringData = Encoding.ASCII.GetString(data, 0, recv); // use this to print the received data

                        Console.WriteLine("raw Discovery data = " + BitConverter.ToString(data, 0, recv));

                        // get Metis MAC address from the payload
                        byte[] MAC = { 0, 0, 0, 0, 0, 0 };
                        Array.Copy(data, 3, MAC, 0, 6);
                        MetisMAC = BitConverter.ToString(MAC);
                        byte codeVersion = data[9];
                        byte boardType = data[10];

                        // check for HPSDR frame ID and type 2 (not currently streaming data, which also means 'not yet in use')
                        // changed to find Metis boards, even if alreay in use!  This prevents the need to power-cycle metis.
                        // (G Byrkit, 8 Jan 2012)
                        if ((data[0] == 0xEF) && (data[1] == 0xFE) && ((data[2] & 0x02) != 0))
                        {
                            Console.WriteLine("\nFound a Metis/Hermes/Griffin.  Checking whether it qualifies");

                            // get Metis IP address from the IPEndPoint passed to ReceiveFrom.
                            IPEndPoint ripep = (IPEndPoint)remoteEP;
                            IPAddress receivedIPAddr = ripep.Address;
                            receivedIP = receivedIPAddr.ToString();

                            Console.WriteLine("Metis IP from IP Header = " + receivedIP);
                            Console.WriteLine("Metis MAC address from payload = " + MetisMAC);
                            if (!SameSubnet(receivedIPAddr, hostPortIPAddress, hostPortMask))
                            {
                                // device is NOT on the subnet that this port actually services.  Do NOT add to list!
                                Console.WriteLine("Not on subnet of host adapter! Adapter IP {0}, Adapter mask {1}",
                                    hostPortIPAddress.ToString(), hostPortMask.ToString());
                            }
                            else if (receivedIPAddr.Equals(hostPortIPAddress))
                            {
                                Console.WriteLine("Rejected: contains same IP address as the host adapter; not from a Metis/Hermes/Griffin");
                            }
                            else if (MetisMAC.Equals("00-00-00-00-00-00"))
                            {
                                Console.WriteLine("Rejected: contains bogus MAC address of all-zeroes");
                            }
                            else
                            {
                                MetisHermesDevice mhd = new MetisHermesDevice();
                                mhd.IPAddress = receivedIP;
                                mhd.MACAddress = MetisMAC;
                                mhd.deviceType = (DeviceType)boardType;
                                mhd.codeVersion = codeVersion;
                                mhd.InUse = false;
                                mhd.hostPortIPAddress = hostPortIPAddress;

                                if (targetIP != null)
                                {
                                    if (mhd.IPAddress.CompareTo(targetIP.ToString()) == 0)
                                    {
                                        have_Metis = true;
                                        mhdList.Add(mhd);
                                        return true;
                                    }
                                }
                                else
                                {
                                    have_Metis = true;
                                    mhdList.Add(mhd);
                                }
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("No data  from Port = ");
                        if ((++time_out) > 5)
                        {
                            Console.WriteLine("Time out!");
                            return false;
                        }
                    }
                } while (data_available);
            }

            return have_Metis;
        }

        /// <summary>
        /// Determines whether the board and hostAdapter IPAddresses are on the same subnet,
        /// using subnetMask to make the determination.  All addresses are IPV4 addresses
        /// </summary>
        /// <param name="board">IP address of the remote device</param>
        /// <param name="hostAdapter">IP address of the ethernet adapter</param>
        /// <param name="subnetMask">subnet mask to use to determine if the above 2 IPAddresses are on the same subnet</param>
        /// <returns>true if same subnet, false otherwise</returns>
        public bool SameSubnet(IPAddress board, IPAddress hostAdapter, IPAddress subnetMask)
        {
            byte[] boardBytes = board.GetAddressBytes();
            byte[] hostAdapterBytes = hostAdapter.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (boardBytes.Length != hostAdapterBytes.Length)
            {
                return false;
            }
            if (subnetMaskBytes.Length != hostAdapterBytes.Length)
            {
                return false;
            }

            for (int i = 0; i < boardBytes.Length; ++i)
            {
                byte boardByte = (byte)(boardBytes[i] & subnetMaskBytes[i]);
                byte hostAdapterByte = (byte)(hostAdapterBytes[i] & subnetMaskBytes[i]);
                if (boardByte != hostAdapterByte)
                {
                    return false;
                }
            }
            return true;
        }

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
            whatsPresent += String.Format("Your Metis has firmware version {0}.\n", MainForm.Ozy_FPGA_version);
            whatsPresent += "\nThis is an incompatible collection of firmware.\n";
            whatsPresent += "\nIf you want to continue anyway, open the Settings form and select 'Skip Version Checking'.\n";
            whatsPresent += "\nThe radio will now stop running when you press OK.";

            switch (MainForm.Ozy_FPGA_version)
            {
                case 13:
                    if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 13)) ||
                         ((MainForm.Merc_version != 29)))
                    {
                        MessageBox.Show(whatsPresent, "You must use Penney Version 13 and Mercury version 29 with Metis version 13", MessageBoxButtons.OK);
                        result = false;
                    }
                    break;

                case 14:
                    if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 14)) ||
                         ((MainForm.Merc_version != 29)))
                    {
                        MessageBox.Show(whatsPresent, "You must use Penney Version 14 and Mercury version 29 with Metis version 14", MessageBoxButtons.OK);
                        result = false;
                    }
                    break;

                case 15:
                    if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 15)) ||
                       ((MainForm.Merc_version != 30)))
                    {
                        MessageBox.Show(whatsPresent, "You must use Penney Version 15 and Mercury version 30 with Metis version 15", MessageBoxButtons.OK);
                        result = false;
                    }
                    break;

                case 16:
                    if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 16)) ||
                        ((MainForm.Merc_version != 31)))
                    {
                        MessageBox.Show(whatsPresent, "You must use Penney Version 16 and Mercury version 31 with Metis version 16", MessageBoxButtons.OK);
                        result = false;
                    }
                    break;

                case 17:
                    if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 17)) ||
                        ((MainForm.Merc_version != 32)))
                    {
                        MessageBox.Show(whatsPresent, "You must use Penney Version 17 and Mercury version 32 with Metis version 17", MessageBoxButtons.OK);
                        result = false;
                    }
                    break;

                case 18:
                    if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 17)) ||
                        ((MainForm.Merc_version != 32)))
                    {
                        MessageBox.Show(whatsPresent, "You must use Penney Version 17 and Mercury version 32 with Metis version 18", MessageBoxButtons.OK);
                        result = false;
                    }
                    break;

                    // Metis 19 and Mercury 33 support 16k samples on 'wideband' data.  And other things...
                // Metis 19 has a problem with some hardware configs/PC option selections yield incorrect freq assignment for Rx1, incorrect 122.88 MHz clock assignment when Penelope selected as source
                case 19:
                    if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 17)) ||
                        ((MainForm.Merc_version != 33)))
                    {
                        MessageBox.Show(whatsPresent, "Metis version 19 has problems.  Please upgrade to Metis version 21", MessageBoxButtons.OK);
                        //MessageBox.Show(whatsPresent, "You must use Penney Version 17 and Mercury version 33 with Metis version 19", MessageBoxButtons.OK);
                        result = false;
                    }
                    break;

                // Metis 20 has a problem with some hardware configs/PC option selections yield incorrect freq assignment for Rx1
                case 20:
                        MessageBox.Show(whatsPresent, "Metis version 20 has problems.  Please upgrade to Metis version 21", MessageBoxButtons.OK);
                        result = false;
                    break;

                case 21:
                    if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 17)) ||
                        ((MainForm.Merc_version != 33)))
                    {
                        MessageBox.Show(whatsPresent, "You must use Penney Version 17 and Mercury version 33 with Metis version 21", MessageBoxButtons.OK);
                        result = false;
                    }
                    break;

                case 22:
                    if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 17)) ||
                        ((MainForm.Merc_version != 33)))
                    {
                        MessageBox.Show(whatsPresent, "You must use Penney Version 17 and Mercury version 33 with Metis version 22", MessageBoxButtons.OK);
                        result = false;
                    }
                    break;

                case 23:
                    if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 17)) ||
                        ((MainForm.Merc_version != 33)))
                    {
                        MessageBox.Show(whatsPresent, "You must use Penney Version 17 and Mercury version 33 with Metis version 23", MessageBoxButtons.OK);
                        result = false;
                    }
                    break;

                case 24:
                    if (((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Penny_version != 17)) ||
                        ((MainForm.Merc_version != 33)))
                    {
                        MessageBox.Show(whatsPresent, "You must use Penney Version 17 and Mercury version 33 with Metis version 24", MessageBoxButtons.OK);
                        result = false;
                    }
                    break;

                default:
                    MessageBox.Show(whatsPresent, "This version of Metis hasn't been entered into the program.", MessageBoxButtons.OK);
                    whatsPresent = "Please contact K9TRV or the current KISS Konsole maintainer with a screen shot of this message,\n"
                        + "but ONLY after you check for updates to KISS Konsole that might handle this version of Metis.\n\n" + whatsPresent;
                    result = false;
                    break;
            }

            return result;
        }
    }
}
