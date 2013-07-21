using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing;

namespace KISS_Konsole
{
    // this is the base class for USB (Ozy/Magister) and Ethernet (Metis/Hermes/Griffin) devices
    // the derived classes implement the particular function appropriate for that connection method (USB or Ethernet)
    // this base class may implement versions of methods that are common to both
    class HPSDRDevice
    {
        protected bool Data_thread_running = false;
        protected Thread Data_thread;	// runs the loop facility 

        // Ozy/Magister/Metis/Hermes EP6 samples read buffer for real-time receiver
        public byte[] rbuf = new byte[Form1.rbufSize];

        // Ozy/Magister/Metis/Hermes EP4 samples read buffer for full-bandwidth spectrum display
        public byte[] EP4buf = new byte[Form1.EP4BufSize];

        // to allow access to methods and fields and such on Form1
        protected Form1 MainForm = null;

        protected float GainConstant = (float)(1.0 / (Math.Pow(2, 15) - 1));  // convert from 16 bit signed integer to float.
        protected double MicAGC = 1.0;
        protected float scaleIn = (float)(1.0 / Math.Pow(2, 23));     // scale factor for converting 24-bit int from ADC to float
        protected float envelope;

        // these are ALL 12-bit analog levels (0-4095)
        protected UInt16 AIN5 = 0;          // forward power penelope or hermes
        protected UInt16 AIN1 = 0;          // forward power alex or apollo
        protected UInt16 AIN2 = 0;          // reverse power alex or apollo
        protected UInt16 AIN3 = 0;          // AIN3 from Penney or Hermes
        protected UInt16 AIN4 = 0;          // AIN4 from Penney or Hermes
        protected UInt16 AIN6 = 0;          // AIN6 from Penney, 13.8v power supply on Hermes

        public double AlexForwardVolts = 0.0;
        public double AlexReverseVolts = 0.0;
        public double AlexForwardPower = 0.0;
        public double AlexReversePower = 0.0;
        public double PenelopeForwardVolts = 0.0;
        public double PenelopeForwardPower = 0.0;
        public double AIN3Volts = 0.0;
        public double AIN4Volts = 0.0;
        public float SupplyVolts = 0.0f;

        protected bool IO1, IO2, IO3 = false;
        protected float MicAGCGain = 1.0f;  // used in the mic AGC loop
        protected float TempMicAGC = 0;
        protected float TempMicPeak = 0;

        protected byte rc0, rc1, rc2, rc3, rc4;               // Command & Control bytes received from Ozy/Metis

        protected byte C0 = 0x00;
        protected byte C1, C2, C3, C4;

        public const int RingBufferSize = 8192;
        protected SharpDSP2._1.RingBuffer AudioRing = new SharpDSP2._1.RingBuffer(RingBufferSize);          // create a ring buffer for rcvr audio output complex values
        protected SharpDSP2._1.RingBuffer TransmitAudioRing = new SharpDSP2._1.RingBuffer(RingBufferSize);  // create a ring buffer for Tx audio output complex values


        // for full bandwidth power spectrum display--
        protected SharpDSP2._1.DSPState FullBandwidthSpectrumState;
        protected SharpDSP2._1.DSPBuffer FullBandwidthSpectrumBuffer;
        protected SharpDSP2._1.OutbandPowerSpectrumSignal FullBandwidthSpectrum;

        // Provide a filter for SSB transmit
        protected SharpDSP2._1.DSPState TransmitFilterState;
        protected SharpDSP2._1.DSPBuffer TransmitFilterBuffer;
        protected SharpDSP2._1.Filter TransmitFilter;

        // Provide a filter for the Speech Processor
        protected SharpDSP2._1.DSPState ProcessorFilterState;
        protected SharpDSP2._1.DSPBuffer ProcessorFilterBuffer;
        protected SharpDSP2._1.Filter ProcessorFilter;

        protected SharpDSP2._1.CPX[] outbuffer = new SharpDSP2._1.CPX[Form1.outbufferSize];     // allocate a buffer for DSP output

        // FM declarations
        protected double TwoPi = 2.0 * Math.PI;
        protected double oscphase = 0.0;
        protected double cvtmod2freq;

        protected short[] CWnoteSin = new short[240];      // An array of samples for baseband CW note;
        protected short[] CWnoteCos = new short[240];
        protected short[] RCCWnoteSin = new short[560];    // An array of samples with raised cosine leading and training envelope
        protected short[] RCCWnoteCos = new short[560];    // for CW baseband note.     
        protected int CWnoteIndex = 0;

        protected int loop_count = 0;         // delay until we read FPGA code version from Ozy

        protected bool PTTReleased = false;
        protected int CWtailIndex = 0;
        protected bool LastPTT = false;
        protected int PTTDelay = 0;
        protected int RFDelay = 0;
        protected int send_state = 0;
        protected byte Drive_Level = 0; // sets Hermes and PennyLane power output

        protected bool dot = false;           // true when dot key from Ozy is active
        protected bool dash = false;          // true when dash key from Ozy is active

        protected const byte sync = 0x7F;

        protected int MicSampleIn = 0;                        // with input
        protected int MicSample = 0;                          // Counter used to decimate Mic samples when EP6 sample rate is > 48k

        protected int sample_no = 0;
        protected int SampleCounter = 0;      // used to pace calls to Data_send() in ProcessData512 when SampleRate > 48000

        // these are now at class scope, rather than being re-created each time a buffer is processed
        // in ProcessWideBandData.  That should churn less!
        protected float[] SamplesReal = new float[Form1.EP4BufSize / 2];
        protected float[] SamplesImag = new float[Form1.EP4BufSize / 2];

        protected float scaleOut = (float)Math.Pow(2, 15);  // scale factor to convert DSP output to 16-bit short int audio output sample\

        // array to send to Ozy/Magister or Metis/Hermes
        protected byte[] to_Device;

        protected int sampleMask = 0x7;
        protected int numWideBuffers = 8;

        // used currently by EthernetDevice.cs, located here so HPSDRDevice.cs can reset it when setting sampleMask and numWideBuffers
        protected int Spectrum_count = 0;


        [FlagsAttribute]
        protected enum C1Bits
        {
            TenMHzAtlas = 0x00,
            TenMHzPenelope = 0x04,
            TenMHzMercury = 0x08,
            Clock122Mercury = 0x10,
            PenelopePresent = 0x20,
            MercuryPresent = 0x40,
            MicPenelope = 0x80,
        }

        public void SetTxBandwidth(float TxFilterLow, float TxFilterHigh)
        {
            // Force re-calculation of filter coefficients (esp after change in SampleRate)
            TransmitFilter.FilterFrequencyLow = TxFilterLow;
            TransmitFilter.FilterFrequencyHigh = TxFilterHigh;

            // ProcessorFilter was NOT being adjusted to track the changes in the TransmitFilter
            // discovered 17 July 2012 (approx) by G Byrkit, K9TRV and confirmed as a bug
            // by Phil VK6APH
            ProcessorFilter.FilterFrequencyLow = TxFilterLow;
            ProcessorFilter.FilterFrequencyHigh = TxFilterHigh;
        }

        // this is Number of Samples, which is doubled to get number of entries, including real and imaginary.
        // for example, if numSamples is 8192, then there are 4096 samples, 1 I sample and 1 Q sample.
        // You see this in that SamplesReal is numSamples/2 and SamplesImag is numSamples/2 in length
        public void InitEP4Buffer ()
        {
            // if numSamples = 8192, then there are 4096 SamplesReal and 4096 SamplesImag
            EP4buf = new byte[Form1.EP4BufSize];        // holds all the samples, both I and Q

            SamplesReal = new float[Form1.EP4BufSize / 2];
            SamplesImag = new float[Form1.EP4BufSize / 2];

            MainForm.FullBandwidthPowerSpectrumData = new float[Form1.EP4BufSize];
            FullBandwidthSpectrumState = new SharpDSP2._1.DSPState();
            FullBandwidthSpectrumState.DSPBlockSize = Form1.EP4BufSize / 2;
            FullBandwidthSpectrumBuffer = new SharpDSP2._1.DSPBuffer(FullBandwidthSpectrumState);
            FullBandwidthSpectrum = new SharpDSP2._1.OutbandPowerSpectrumSignal(ref FullBandwidthSpectrumBuffer, SharpDSP2._1.WindowType_e.BLACKMANHARRIS_WINDOW);
            
            if (Form1.EP4BufSize == 8192)
            {
                numWideBuffers = 8;
            }
            else if (Form1.EP4BufSize == 32768)
            {
                numWideBuffers = 32;
            }
            sampleMask = numWideBuffers - 1;
            Spectrum_count = 0;
        }

        public HPSDRDevice(Form1 mainForm, int toDeviceSize)
        {
            MainForm = mainForm;

            to_Device = new byte[toDeviceSize];

            TransmitFilterState = new SharpDSP2._1.DSPState();
            TransmitFilterState.DSPBlockSize = Form1.iqsize;
            TransmitFilterState.DSPSampleRate = 48000; // Tx sample rate is always 48kHz

            TransmitFilterBuffer = new SharpDSP2._1.DSPBuffer(TransmitFilterState);

            TransmitFilter = new SharpDSP2._1.Filter(ref TransmitFilterBuffer);
            TransmitFilter.FilterType = SharpDSP2._1.FilterType_e.BandPass;
            TransmitFilter.FilterWindowType = SharpDSP2._1.WindowType_e.BLACKMANHARRIS_WINDOW;
            TransmitFilter.FilterFrequencyLow = MainForm.TxFilterLow;
            TransmitFilter.FilterFrequencyHigh = MainForm.TxFilterHigh;

            // Set up Speech Processor - use same settings as the Transmit Filter
            ProcessorFilterState = new SharpDSP2._1.DSPState();
            ProcessorFilterState.DSPBlockSize = Form1.iqsize;
            ProcessorFilterState.DSPSampleRate = 48000; // Tx sample rate is always 48kHz
            ProcessorFilterBuffer = new SharpDSP2._1.DSPBuffer(ProcessorFilterState);

            ProcessorFilter = new SharpDSP2._1.Filter(ref ProcessorFilterBuffer);
            ProcessorFilter.FilterType = SharpDSP2._1.FilterType_e.BandPass;
            ProcessorFilter.FilterWindowType = SharpDSP2._1.WindowType_e.BLACKMANHARRIS_WINDOW;
            ProcessorFilter.FilterFrequencyLow = MainForm.TxFilterLow;
            ProcessorFilter.FilterFrequencyHigh = MainForm.TxFilterHigh;
            
            // Set up for full bandwidth (wideband) spectrum display
            InitEP4Buffer();

            // Calculate sample values for baseband CW note
            double deltaf = Math.PI / 40.0;     // (2 PI f / 48k) gives an 600 Hz note at 48 ksps
            for (int i = 0; i < 240; ++i)
            {
                CWnoteSin[i] = (short)((Math.Pow(2, 15) - 1) * Math.Sin(deltaf * i)); // was 29000
                CWnoteCos[i] = (short)((Math.Pow(2, 15) - 1) * Math.Cos(deltaf * i));
            }

            // calculate sample values for raised cosine profile for CW note, 240 samples at 48k gives 
            // a rise time of 5mS
            double inc = Math.PI / 240;
            float[] LeadingRaisedCosine = new float[240];  // Raised cosine to apply to leading and 
            float[] TrailingRaisedCosine = new float[240]; // training edges of CW note
            for (int i = 0; i < 240; ++i)
            {
                LeadingRaisedCosine[i] = 0.5f * (1 + (float)(Math.Cos(Math.PI + (inc * i))));
                TrailingRaisedCosine[i] = 0.5f * (1 + (float)(Math.Cos(inc * i)));
            }

            // calculate the sample values for a raised cosine profile CW note.The arrray will be 560 samples long.
            // The first 240 samples will have a raised cosine profile, the middle 80 plain samples and the remaining
            // 240 samples a decaying raised cosine profile.

            for (int i = 0; i < 240; i++) // do the first and last 240 samples
            {
                RCCWnoteSin[i] = (short)((float)LeadingRaisedCosine[i] * CWnoteSin[i]);
                RCCWnoteCos[i] = (short)((float)LeadingRaisedCosine[i] * CWnoteCos[i]);
                RCCWnoteSin[i + 320] = (short)((float)TrailingRaisedCosine[i] * CWnoteSin[i]);
                RCCWnoteCos[i + 320] = (short)((float)TrailingRaisedCosine[i] * CWnoteCos[i]);
            }

            for (int i = 240, j = 0; i < 320; i++, j++)  // middle 80 unshaped samples
            {
                RCCWnoteSin[i] = CWnoteSin[j];
                RCCWnoteCos[i] = CWnoteCos[j];
            }
        }

        protected void Process_Data(ref byte[] rbuf)
        {
            #region how Process_Data works
            /*
             * 
             Process_Data  capitalizes on the fact that the USB data from Ozy is well ordered and
             consistent as it arrives from the USB bulk read with respect to where sync pulses and Cn, I, Q,
             and L,R values are located in the array. A simple initial check for the presence of sync pulses
             at the beginning of rbuf is all that is done to establish that the data within rbuf is likely
             to be valid Rx data.

            C0 bits 7-3 is command and control "address"
            C0 bit 0 is PTT from Penelope
            C0 bit 1 is "dash"
            C0 bit 2 is "dot" 

            when C0 "address" == 00000xxx then
            C1 bit 0 is Mercury ADC overload bit
               bit 1 is Hermes IO1 input
               bit 2 is Hermes IO2 input
               bit 3 is Hermes IO3 input
            C2 is Mercury software serial number
            C3 is Penelope software serial number
            C4 is Ozy software serial number

            when C0 is binary 00001xxx then 
            C1 is bits 15-8 of Penelope or Hermes forward power (only 12 bits used) AIN5
            C2 is bits 7-0 of Penelope or Hermes forward power AIN5
            C3 – Bits 15-8 of Forward Power from Alex or Apollo (AIN1)
            C4 – Bits 7-0  of Forward Power from Alex or Apollo (AIN1)

            when C0 is binary 00010xxx  then 
            C1 – Bits 15-8 of Reverse Power from Alex or Apollo (AIN2)
            C2 - Bits 7-0  of Reverse Power from Alex or Apollo (AIN2)
            C3 – Bits 15-8 of AIN3 from Penny or Hermes
            C4 – Bits 7-0  of AIN3 from Penny or Hermes

            when C0 is binary 00011xxx then
            C1 – Bits 15-8 of AIN4 from Penny or Hermes
            C2 - Bits 7-0  of AIN4 from Penny or Hermes
            C3 – Bits 15-8 of AIN6 from Penny or Hermes (13.8v supply on Hermes)
            C4 – Bits 7-0  of AIN6 from Penny or Hermes (13.8v supply on Hermes)


            For a full description of the USB protocol see the document in \trunk\Documents.
            
            If the force flag is set then we send C&C data to Ozy even if we don't have 
            valid I&Q or Microphone  audio. This is so that Ozy knows what clocks to use
            and will start sending data. 
              
             */

            #endregion

            bool PTT_active = false;
            bool ADC_bit = false;

            // check that sync pulses are present in the front of rbuf...JAM
            if (rbuf[0] == sync && rbuf[1] == sync && rbuf[2] == sync)
            {
                MainForm.IsSynced = true;  // use this to set the colour of the Sync LED when the timer fires
            }
            else
            {
                MainForm.IsSynced = false;
                Debug.WriteLine(String.Format("Process_Data: Sync Failed - rbuf = \t{0}\t{1}\t{2}", rbuf[0], rbuf[1], rbuf[2]));
                return;
            }

            for (int frame = 0; frame < 4; frame++)
            {
                int coarse_pointer = frame * 512; //512 bytes total in each frame
                rc0 = rbuf[coarse_pointer + 3];
                rc1 = rbuf[coarse_pointer + 4];
                rc2 = rbuf[coarse_pointer + 5];
                rc3 = rbuf[coarse_pointer + 6];
                rc4 = rbuf[coarse_pointer + 7];

                PTT_active = ((rc0 & 0x01) != 0) ? true : false;
                dash = ((rc0 & 0x02) != 0) ? true : false;
                dot = ((rc0 & 0x04) != 0) ? true : false;
                ADC_bit = ((rc1 & 1) != 0) ? true : false;

                // temporarily link dot and dash inputs to PTT for testing
                if ((PTT_active || dot || dash) && (MainForm.PenneyPresent || MainForm.PennyLane || MainForm.Hermes))  // only enable Tx if Penny, PennyLane or Hermes selected
                {
                    MainForm.PTT = true;  // PTT active on Atlas bus so set PTT flag
                }
                else
                {
                    MainForm.PTT = false;
                }

                // shift off the PTT, Dash and Dot bits to learn what type of message this is
                byte rc0Shifted = (byte)(rc0 >> 3);

                switch (rc0Shifted)
                {
                    case 0:
                        MainForm.ADCOverload = ADC_bit ? true : false;

                        if (MainForm.Hermes) // get status of user inputs on Hermes
                        {
                            IO1 = ((rc1 & 2) != 0) ? true : false;
                            IO2 = ((rc1 & 4) != 0) ? true : false;
                            IO3 = ((rc1 & 8) != 0) ? true : false;
                        }

                        // get serial # of Mercury and Penelope software, only need to do this once when KK starts
                        if (loop_count == 99)  // wait for 100 reads so Ozy data is stable
                        {
                            MainForm.Merc_version = (int)rc2;        // Version number of Mercury FPGA code
                            MainForm.Penny_version = (int)rc3;       // Version number of Penelope FPGA code
                            MainForm.Ozy_FPGA_version = (int)rc4;    // Version number of Ozy/Metis/Hermes FPGA code

                            if (MainForm.SetupFormValid())
                            {
                                MethodInvoker mi = new MethodInvoker(MainForm.Setup_form.SetHPSDRVersions);
                                MainForm.Invoke(mi);
                            }

                            if (!MainForm.Hermes)
                            {
                                Debug.WriteLine("Mercury version = \t" + MainForm.Merc_version);
                                Debug.WriteLine("Penny version = \t" + MainForm.Penny_version);
                                Debug.WriteLine("Ozy/Magister/Metis FPGA code version = \t" + MainForm.Ozy_FPGA_version);
                            }
                            else
                            {
                                Debug.WriteLine("Hermes version = \t" + MainForm.Ozy_FPGA_version);
                            }

                            if (MainForm.KKDevice == KKMethod.Ethernet)
                            {
                                // presume the 'normal' (original) EP4BufSize value
                                Form1.EP4BufSize = 8192;
                                if (MainForm.Allow16kWidebandSamples)
                                {
                                    if (MainForm.Hermes)
                                    {
                                        // allow 16k wideband spectrum
                                        Form1.EP4BufSize = 32768;
                                    }
                                    else if (MainForm.Merc_version >= 33 && MainForm.Ozy_FPGA_version >= 19)
                                    {
                                        // it's Metis with a sufficient version of mercury code
                                        Form1.EP4BufSize = 32768;
                                    }
                                }

                                InitEP4Buffer();
                            }

                            if (!MainForm.Hermes)
                            {
                                // NOT Hermes
                                if (MainForm.Merc_version < 27)
                                {
                                    MessageBox.Show("Warning - Mercury code version is V" + ((float)(MainForm.Merc_version / 10f)).ToString() +
                                    "\nMust be V2.7 or higher");
                                }
                                if (!MainForm.SkipVersionCheck)
                                {
                                    // check Mercury and Penelope versions, especially for compatibility with Ozy and Metis.
                                    if (!CheckVersions())
                                    {
                                        // CheckVersions failed.  We should NOT run.
                                        // Toggle ON/OFF Button to OFF.
                                        // use MethodInvoker to do this, as otherwise it would fail (throw an exception)
                                        // for being a cross-threaded call.
                                        MethodInvoker mi = new MethodInvoker(MainForm.ToggleOnOffButton);
                                        MainForm.Invoke(mi);
                                    }
                                }
                            }
                            else
                            {
                                // IS Hermes
                                if (MainForm.Ozy_FPGA_version < 18)
                                    MessageBox.Show("Warning - Hermes code version is V" + ((float)(MainForm.Ozy_FPGA_version / 10f)).ToString() +
                                    "\nMust be V1.8 (production release version) or higher");
                            }

                            // check we find a Penny board if selected 
                            if (MainForm.Penny_version == 0 && (MainForm.PenneyPresent || MainForm.PennyLane))
                                MessageBox.Show(" Warning - Penelope/PennyLane selected but board not found");
                        }
                        if (loop_count < 100)
                            loop_count++;
                        break;

                    case 1:  // then C0 = 0000_1xxx
                        // forward power
                        if (MainForm.PennyLane || MainForm.PenneyPresent || MainForm.Hermes)
                        {
                            AIN5 = (UInt16)((UInt16)(rc1 << 8) + (UInt16)rc2);

                            PenelopeForwardVolts = 3.3 * (double)AIN5 / 4095.0;
                            PenelopeForwardPower = PenelopeForwardVolts * PenelopeForwardVolts / 0.09;
                        }
                        if (MainForm.Alex || MainForm.Apollo)
                        {
                            AIN1 = (UInt16)((UInt16)(rc3 << 8) + (UInt16)rc4);

                            AlexForwardVolts = 3.3 * (double)AIN1 / 4095.0;
                            AlexForwardPower = AlexForwardVolts * AlexForwardVolts / 0.09;
                        }
                        break;

                    case 2:  // then C0 = 0001_0xxx
                        if (MainForm.Alex || MainForm.Apollo)
                        {
                            // reverse power
                            AIN2 = (UInt16)((UInt16)(rc1 << 8) + (UInt16)rc2);

                            AlexReverseVolts = 3.3 * (double)AIN2 / 4095.0;
                            AlexReversePower = AlexReverseVolts * AlexReverseVolts / 0.09;
                        }

                        if (MainForm.PennyLane || MainForm.PenneyPresent || MainForm.Hermes)
                        {
                            AIN3 = (UInt16)((UInt16)(rc3 << 8) + (UInt16)rc4);
                            AIN3Volts = 3.3 * (double)AIN3 / 4095.0;
                        }

                        break;

                    case 3:  // then C0 = 0001_1xxx
                        if (MainForm.PenneyPresent || MainForm.PennyLane || MainForm.Hermes)
                        {
                            AIN4 = (UInt16)((UInt16)(rc1 << 8) + (UInt16)rc2);
                            AIN6 = (UInt16)((UInt16)(rc3 << 8) + (UInt16)rc4);

                            AIN4Volts = 3.3 * (double)AIN4 / 4095.0;

                            if (MainForm.Hermes) // read supply volts applied to board
                            {
                                SupplyVolts = (float)((float)AIN6 / 186.0f);
                            }
                        }
                        break;
                }

                // get the I, and Q data from rbuf, convert to float, & put into SignalBuffer.cpx for DSP processing
                for (int i = 8; i < 512; i += 8)
                {
                    int k = coarse_pointer + i;
                    // get the I, Q, and mic bytes and form the integer data...JAM

                    // use the following rather than BitConverter.ToInt32 since uses much less CPU

                    // get an I sample...JAM
                    MainForm.SignalBuffer.cpx[sample_no].real = scaleIn * (float)((rbuf[k + 2] << 8) | (rbuf[k + 1] << 16) | (rbuf[k] << 24));

                    // get a Q sample
                    MainForm.SignalBuffer.cpx[sample_no].imaginary = scaleIn * (float)((rbuf[k + 5] << 8) | (rbuf[k + 4] << 16) | (rbuf[k + 3] << 24));

                    // Get a microphone sample & store it in a simple buffer.
                    // If EP6 sample rate > 48k, the Mic samples received contain duplicates, which we'll ignore.
                    // Thus the Mic samples are decimated before processing.  
                    // The receive audio is fully processed at the higher sampling rate, and then is decimated.
                    // By the time Data_send() is called, both streams are at 48ksps.
                    // When the transmit buffer has reached capacity, all samples are processed, and placed in TransmitAudioRing.
                    // 
                    MicSample += 48000;
                    if (MicSample >= MainForm.SampleRate)
                    {
                        MicSample = 0;
                        // Place sample value in both real & imaginary parts since we do an FFT next.
                        // When Mic AGC and speech processing are added, they should be done here, 
                        // working with a stream of real samples.

                        // TransmitFilter.Process() effects a relative phase shift between the real & imaginary parts.

                        // In the following line, need (short) cast to ensure that the result is sign-extended
                        // before being converted to float.
                        float Sample = (float)(MicAGC * GainConstant * (double)(short)((rbuf[k + 7]) | (rbuf[k + 6] << 8)));

                        // Apply MicGain
                        Sample *= MainForm.MicGain;

                        // check if VOX is on, if so are we over the set threshold?
                        if (MainForm.VOXOn && (Math.Abs(Sample) > MainForm.VOXThreshold))
                        {
                            MainForm.MOX_set = true;  // turn Tx on
                            MainForm.VOXDelay = true; // timer3 looks after the VOX hang time
                        }

                        // if the Noise Gate is on then check the MicPeak is > threshold

                        if (MainForm.NoiseGate && (Math.Abs(Sample) > MainForm.NoiseThreshold))
                        {
                            MainForm.NoiseGateSwitch = true;
                            MainForm.NoiseGateDelay = true;
                        }
                        //else NoiseGateSwitch = false;


                        // Compensate for the loss  in the bass cut filter if on
                        if (MainForm.BassCutOn)
                        {
                            Sample *= 2;  // add 6dB per sample
                            // apply bass cut - subtract the previous sample from the current sample to give 6dB per octave bass cut
                            if (MicSampleIn > 0)
                            {
                                ProcessorFilterBuffer.cpx[MicSampleIn].real = Sample - 0.99f * ProcessorFilterBuffer.cpx[MicSampleIn - 1].real;
                                ProcessorFilterBuffer.cpx[MicSampleIn].imaginary = Sample - 0.99f * ProcessorFilterBuffer.cpx[MicSampleIn - 1].imaginary;
                            }
                            else if (MicSampleIn == 0)
                            {
                                ProcessorFilterBuffer.cpx[MicSampleIn].real = Sample - 0.99f * ProcessorFilterBuffer.cpx[Form1.iqsize - 1].real;
                                ProcessorFilterBuffer.cpx[MicSampleIn].imaginary = Sample - 0.99f * ProcessorFilterBuffer.cpx[Form1.iqsize - 1].imaginary;
                            }
                        }
                        else
                        {
                            ProcessorFilterBuffer.cpx[MicSampleIn].real = Sample;
                            ProcessorFilterBuffer.cpx[MicSampleIn].imaginary = Sample;
                        }

                        float MicPeak = 0;
                        if (++MicSampleIn == Form1.iqsize)         // If TransmitFilterBuffer is full, process the data
                        {
                            MicSampleIn = 0;

                            if (MainForm.chkMicAGC.Checked)          // if Mic AGC is enabled apply 
                            {

                                // Apply fast attack, slow decay AGC to the microphone samples
                                // First find the largest sample in the array 
                                for (int x = 0; x < Form1.iqsize; x++)
                                {
                                    if (MicPeak < Math.Abs(ProcessorFilterBuffer.cpx[x].real))
                                        MicPeak = Math.Abs(ProcessorFilterBuffer.cpx[x].real);
                                }

                                // Normalise the samples to the peak value so this = 1.0
                                // if the MicPeak is bigger than the previous value then immediately reduce the gain (fast attack)
                                if (MicPeak >= 1.0f)
                                {
                                    MicAGCGain = 1.0f / MicPeak;
                                    //ClipOn = true;
                                }

                                // If less then gradually increase the gain (slow decay) but only by 10dB max
                                else if (MicAGCGain < 3)
                                {
                                    MicAGCGain *= 1.01f;
                                }
                            }

                            else MicAGCGain = 1.0f;

                            TempMicAGC = MicAGCGain; //so we can display 

                            // Speech Processor is on at all times in order to catch any mic signals that are > +/- 1.0

                            if (!MainForm.ProcessorOn)  // only apply clipper gain if the Processor is on
                            {
                                MainForm.ProcGain = 1.0f;
                            }

                            // apply microphone AGC, Processor gain and Mute to the samples in the processor filter buffer.
                            // The ProcessorFilter.Process has a gain of 2 so reduce signal level by this amount

                            MicPeak = 0;


                            for (int x = 0; x < Form1.iqsize; x++)
                            {
                                ProcessorFilterBuffer.cpx[x].real *= (MicAGCGain * MainForm.ProcGain); // * Mute);
                                ProcessorFilterBuffer.cpx[x].imaginary = ProcessorFilterBuffer.cpx[x].real;
                                if (MicPeak < Math.Abs(ProcessorFilterBuffer.cpx[x].real))
                                    MicPeak = Math.Abs(ProcessorFilterBuffer.cpx[x].real);
                            }

                            TempMicPeak = MicPeak;

                            ProcessorFilter.Process();    // Apply a transmit bandwidth filter & produce a complex signal
                            // This reads from and writes to the ProcessorFilterBuffer

                            // the ProcessorFilterBuffer now contains the I and Q mic samples, apply the speech processor
                            for (int enve = 0; enve < Form1.iqsize; enve++)
                            {
                                // calculate the envelope of the I and Q signals using Sqrt(I*I + Q*Q)
                                envelope = (float)Math.Sqrt(ProcessorFilterBuffer.cpx[enve].real * ProcessorFilterBuffer.cpx[enve].real +
                                                            ProcessorFilterBuffer.cpx[enve].imaginary * ProcessorFilterBuffer.cpx[enve].imaginary);
                                // apply the clipper
                                if (envelope > 1.0f)
                                {
                                    ProcessorFilterBuffer.cpx[enve].real /= envelope;  // this is our processed microphone signal
                                    MainForm.ClipOn = true;
                                    // don't do this on this thread!  ClipLED.BackColor = Color.Green;
                                }
                            }

                            // copy the processed data to the TransmitFilterBuffer. This now containes our processed mic signal.
                            for (int enve = 0; enve < Form1.iqsize; ++enve)
                            {
                                TransmitFilterBuffer.cpx[enve].real = ProcessorFilterBuffer.cpx[enve].real * 0.49f;  // was 0.59f
                                TransmitFilterBuffer.cpx[enve].imaginary = TransmitFilterBuffer.cpx[enve].real;
                            }

                            TransmitFilter.Process(); // This reads from and writes to the TransmitFilterBuffer

                            if ((MainForm.NoiseGate && MainForm.NoiseGateSwitch) || !MainForm.NoiseGate)  // Enable if NoiseGate selected and above threshold
                            {
                                for (int sample = 0; sample < Form1.iqsize; ++sample)
                                {
                                    TransmitAudioRing.Write(TransmitFilterBuffer.cpx[sample]);  // Place processed data in ring buffer for Data_send()
                                }
                            }
                        }
                    }

                    // This single sample is now complete.
                    // The routine will generate 504 such samples from each rbuf that is processed.
                    // Whenever we've accumulated iqsize samples, process them.
                    sample_no++;            // increment the sample number counter
                    if (sample_no == Form1.iqsize)
                    {
                        sample_no = 0;
                        MainForm.read_ready = false;           // just in case we try to display whilst data is changing
                        MainForm.rcvr.DoDSPProcess(ref MainForm.SignalBuffer.cpx, ref outbuffer);  // Do all the DSP processing for rcvr

                        // If SampleRate > 48000, decimate by skipping samples as we place them into the AudioRing buffer
                        // since sample rate to Ozy is always 48k.
                        int SampleSpacing = MainForm.SampleRate / 48000;

                        for (int sample = 0; sample < Form1.outbufferSize; sample += SampleSpacing)
                        {
                            AudioRing.Write(outbuffer[sample]);
                        }

                        MainForm.read_ready = true;  // flag used to indicate to the graphic display code we have data ready

                    }
                    // If SampleRate is 48000, send a frame to Ozy for every frame read
                    // If SampleRate is 96000, send a frame for every two read (audio is decimated by factor of two)
                    // If SampleRate is 192000, send a frame for every four read (audio is decimated by factor of four)
                    SampleCounter += 48000;
                    if (SampleCounter >= MainForm.SampleRate)
                    {
                        SampleCounter = 0;
                        bool force = false;   // conventional write to Ozy
                        Data_send(force);  // send the data to Ozy over USB
                    }
                }
            }
        }

        /// <summary>
        /// The core that Data_send calls from derived classes.
        /// </summary>
        /// <param name="frames">2 or 4, number of frames to pack into the buffer</param>
        /// <param name="bufferOffset">offset into the buffer where to start.  0 for Ozy/Magister (USB), 8 for Metis/Hermes (Ethernet)</param>
        protected void Data_send_core(int frames, int bufferOffset)
        {
            #region how Data_send works *** add Hermes controls and full duplex
            /*          
             
             Send frames to Ozy -- frames consist of 512 bytes as follows:
             <0x7f><0x7f><0x7f><C0><C1><C2><C3><C4><Left><Left><Right><Right><I><I><Q><Q><Left>..... 

             C0 bit 0 is "PTT"
             
             if CO = 0x0000_000x then
             C1 bits:
             7      Mic source: 0 = Janus, 1 = Penelope
             6,5    boards present:  00 neither, 01 Penelope, 10 Mercury, 11 both
             4      122.88MHz source: 0 Penelope, 1 Mercury
             3,2    10MHz ref source: 00 atlas, 01 Penelope, 10 Mercury
             1,0    sampling rate:    00 48kHz, 01 96kHz, 10 192 kHz
             * 
             C2 bits:
             7-1    Penelope open collector outputs 6-0
             0      xmt mode: 1 class E, 0 all other modes

             C3 bits:
             7    Alex Rx out: 0 off, 1 on
             6,5  Alex Rx antenna: 00 none, 01 Rx1, 10 Rx2, 11 XV
             4    Mercury ADC random: 0 off, 1 on
             3    Mercury ADC dither:  0 off, 1 on
             2    Mercury preamp: 0 off, 1 on
             1,0  Alex attenuator: 00 0dB, 01 10dB, 10 20dB, 11 30dB

             C4 bits
             7-3  unused
             1,0  Alex Tx relay: 00 Tx1, 01 Tx2, 10 Tx3
             2    1 = full duplex, 0 = simplex 
             if C4[2] = 0 and C0 = 0x0000_001x then C1..C4 hold frequency in Hz for Penny and Mercury, C1 = MSB
             if C4[2] = 1 and C0 = 0x0000_001x then C1..C4 hold frequency in Hz for Penny, C1 = MSB
             if C4[2] = 1 and C0 = 0x0000_010x then C1..C4 hold frequency in Hz for Mercury, C1 = MSB
             

             For a full description of the USB protocol see the document in \trunk\Documents
             */

            #endregion

            float SampleReal = 1.0f;
            float SampleImag = 1.0f;
            short I_data = 0;
            short Q_data = 0;
            int frame_number = 0;
            int pntr; int x = 0;

            // set gain levels OUTSIDE the for loops...
            // TxGain is the product of the Drive setting and the Gain per band setting
            float TxGain = MainForm.DriveGain * (float)(MainForm.BandGain / 100.0f);
            // PennyLane and Hermes uses Drive control - 0x00 to 0xFF, whereas Penny varies the amplitude of 
            // I&Q to vary her output level
            if (MainForm.PennyLane || MainForm.Hermes)
            {
                Drive_Level = (byte)((int)(TxGain * 255));
                TxGain = 1.0f; // Drive the DAC to max at all times for best S/N ratio
            }
            else Drive_Level = 0;

            float MicScaleOut = (float)Math.Pow(2, 15) * TxGain;

            // send 2 frames of 512 bytes for a total of 1024 bytes to Ozy via Ethernet
            for (frame_number = 0; frame_number < frames; frame_number++)
            {
                // cycle between C0 = 0x00, 0x02, 0x04 and 0x12 using a state machine so its easy to add other states in the future
                switch (send_state)
                {
                    case 0:
                        C0 = 0x00; C1 = 0x00; C2 = 0x00;

                        byte AlexAntSel;
                        byte AlexRXSel;
                        byte AlexRXAtten;
                        byte AlexRX = 0;
                        int AlexState;
                        switch (MainForm.CurrentBand)
                        {
                            case OpBand.M160:
                                AlexState = MainForm.Alex160mState;
                                break;
                            case OpBand.M80:
                                AlexState = MainForm.Alex80mState;
                                break;
                            case OpBand.M60:
                                AlexState = MainForm.Alex60mState;
                                break;
                            case OpBand.M40:
                                AlexState = MainForm.Alex40mState;
                                break;
                            case OpBand.M30:
                                AlexState = MainForm.Alex30mState;
                                break;
                            case OpBand.M20:
                                AlexState = MainForm.Alex20mState;
                                break;
                            case OpBand.M17:
                                AlexState = MainForm.Alex17mState;
                                break;
                            case OpBand.M15:
                                AlexState = MainForm.Alex15mState;
                                break;
                            case OpBand.M12:
                                AlexState = MainForm.Alex12mState;
                                break;
                            case OpBand.M10:
                                AlexState = MainForm.Alex10mState;
                                break;
                            case OpBand.M6:
                                AlexState = MainForm.Alex6mState;
                                break;
                            case OpBand.GC:
                            default:
                                AlexState = MainForm.AlexGCState;
                                break;
                        }
                        // decode AlexState:
                        // AA.TT.BBB.RR where
                        // AA is RX Attenuator
                        // TT is TX antenna selection (0 is NOT valid!)
                        // BBB is RX special selection
                        // RR is TX antenna to use for RX (0 is NOT valid!) (see AlexUserControl.cs)
                        AlexAntSel = (byte)((0x03 & ((MainForm.PTT || MainForm.MOX_set) ? AlexState >> 6 : AlexState)));
                        if (AlexAntSel != 0)
                            --AlexAntSel;
                        AlexRXAtten = (byte)(0x03 & (AlexState >> 8));
                        MainForm.AlexAtten = 10.0 * AlexRXAtten;
                        AlexRXSel = (byte)(0x07 & (AlexState >> 3));
                        switch (AlexRXSel)
                        {
                            case 1:                 // if AlexRxSel is not 0 then set AlexRx to 1
                                AlexRXSel = 1;
                                AlexRX = 1;
                                break;
                            case 2:
                                AlexRXSel = 2;
                                AlexRX = 1;
                                break;
                            case 4:
                                AlexRXSel = 3;
                                AlexRX = 1;
                                break;
                        }

                        // set C4 bit 2 if Duplex is selected
                        C4 = MainForm.Duplex ? (byte)0x04 : (byte)0x00;     // duplex on/off, 1 receiver

                        if (MainForm.Alex)
                        {
                            C4 |= AlexAntSel;
                        }

                        // determine the settings for the various clock options 
                        if ((MainForm.PenneyPresent || MainForm.PennyLane) && (MainForm.Atlas10MHz || MainForm.Excalibur))
                        {
                            C1 = (byte)(C1Bits.MicPenelope | C1Bits.MercuryPresent | C1Bits.PenelopePresent | C1Bits.Clock122Mercury | C1Bits.TenMHzAtlas);
                        }
                        else if ((MainForm.PenneyPresent || MainForm.PennyLane) && MainForm.Penelope10MHz)
                        {
                            C1 = (byte)(C1Bits.MicPenelope | C1Bits.MercuryPresent | C1Bits.PenelopePresent | C1Bits.Clock122Mercury | C1Bits.TenMHzPenelope);
                        }
                        else if ((MainForm.PenneyPresent || MainForm.PennyLane) && MainForm.Mercury10MHz)
                        {
                            C1 = (byte)(C1Bits.MicPenelope | C1Bits.MercuryPresent | C1Bits.PenelopePresent | C1Bits.Clock122Mercury | C1Bits.TenMHzMercury);
                        }

                        else if (MainForm.Atlas10MHz || MainForm.Excalibur)
                        {
                            C1 = (byte)(C1Bits.MercuryPresent | C1Bits.Clock122Mercury | C1Bits.TenMHzAtlas);
                        }

                        else // must be Mercury and Mercury10MHz
                        {
                            C1 = (byte)(C1Bits.MercuryPresent | C1Bits.Clock122Mercury | C1Bits.TenMHzMercury);
                        }

                        // set sampling rate bits in C1
                        switch (MainForm.SampleRate)
                        {
                                // there was an error here that 48000 was represented as 480000.  It however had no real effect as an error
                                // since the result was still the C1 was set to 0
                            case 48000:
                                // value for lower 2 bits of C1 is 0
                                break;
                            case 96000:
                                C1 = (byte)(C1 | 0x01);
                                break;
                            case 192000:
                                C1 = (byte)(C1 | 0x02);
                                break;

                            case 384000:    // added 26 Jan 2013 initially for Hermes, now also for Metis and Ozy
                                C1 = (byte)(C1 | 0x03);
                                break;
                        }

                        // set Preamp bit if selected
                        C3 = MainForm.PreampOn ? (byte)0x04 : (byte)0x00;

                        if (MainForm.Alex)
                        {
                            C3 |= (byte)(AlexRXAtten | (AlexRXSel << 5) | (AlexRX << 7));
                        }

                        // check if Penelope Open Collector outputs are enabled, if so set outputs as per user selection
                        if (MainForm.PennyOC)
                        {
                            // find what band we are using and if receive or transmit
                            switch (MainForm.CurrentBand)
                            {
                                case OpBand.M160: C2 = (byte)((MainForm.PTT || MainForm.MOX_set) ? MainForm.Penny160mTxOC : MainForm.Penny160mRxOC); break;
                                case OpBand.M80: C2 = (byte)((MainForm.PTT || MainForm.MOX_set) ? MainForm.Penny80mTxOC : MainForm.Penny80mRxOC); break;
                                case OpBand.M60: C2 = (byte)((MainForm.PTT || MainForm.MOX_set) ? MainForm.Penny60mTxOC : MainForm.Penny60mRxOC); break;
                                case OpBand.M40: C2 = (byte)((MainForm.PTT || MainForm.MOX_set) ? MainForm.Penny40mTxOC : MainForm.Penny40mRxOC); break;
                                case OpBand.M30: C2 = (byte)((MainForm.PTT || MainForm.MOX_set) ? MainForm.Penny30mTxOC : MainForm.Penny30mRxOC); break;
                                case OpBand.M20: C2 = (byte)((MainForm.PTT || MainForm.MOX_set) ? MainForm.Penny20mTxOC : MainForm.Penny20mRxOC); break;
                                case OpBand.M17: C2 = (byte)((MainForm.PTT || MainForm.MOX_set) ? MainForm.Penny17mTxOC : MainForm.Penny17mRxOC); break;
                                case OpBand.M15: C2 = (byte)((MainForm.PTT || MainForm.MOX_set) ? MainForm.Penny15mTxOC : MainForm.Penny15mRxOC); break;
                                case OpBand.M12: C2 = (byte)((MainForm.PTT || MainForm.MOX_set) ? MainForm.Penny12mTxOC : MainForm.Penny12mRxOC); break;
                                case OpBand.M10: C2 = (byte)((MainForm.PTT || MainForm.MOX_set) ? MainForm.Penny10mTxOC : MainForm.Penny10mRxOC); break;
                                case OpBand.M6: C2 = (byte)((MainForm.PTT || MainForm.MOX_set) ? MainForm.Penny6mTxOC : MainForm.Penny6mRxOC); break;
                                case OpBand.GC:
                                default: C2 = 0x00; break;
                            }
                            C2 = (byte)((int)C2 << 1);  // Open Collector bits are  7... 1 
                        }
                        else
                        {
                            C2 = 0x00;  // default Open Collector outputs to off
                        }

                        send_state = 1; // select next state
                        break;

                    case 1:
                        // set CO  to  0x02 and send frequency for Penelope/PennyLane/Hermes Tx  (and Mercury/Hermes if not full Duplex)
                        C0 = 0x02;
                        // send frequency data
                        C1 = MainForm.frequency[3];
                        C2 = MainForm.frequency[2];
                        C3 = MainForm.frequency[1];
                        C4 = MainForm.frequency[0];

                        // check if Duplex or PennyLane or Hermes is selected - if so go to next state else loop back to starting state
                        send_state = MainForm.Duplex ? 2 : 3;
                        break;

                    case 2:
                        // Duplex is selected so set CO to  0x04 and send duplex frequency to Mercury_1/ Hermes 
                        C0 = 0x04;
                        C1 = MainForm.duplex_frequency[3];
                        C2 = MainForm.duplex_frequency[2];
                        C3 = MainForm.duplex_frequency[1];
                        C4 = MainForm.duplex_frequency[0];

                        send_state = 3;
                        break;

                    case 3:
                        // set Drive Level, Mic boost and Line in settings
                        C0 = 0x12;
                        C1 = Drive_Level;
                        C2 = C3 = C4 = 0;
                        if (MainForm.Hermes || MainForm.PenneyPresent || MainForm.PennyLane)
                        {
                            if (MainForm.LineIn)
                                C2 = (byte)0x02;
                            else if (MainForm.MicGain20dB)
                                C2 = (byte)0x01;
                            else
                                C2 = 0x00;

                            // if Penelope is present, always send a 0 byte.
                            // if Hermes or PennyLane, send Drive_Level (0-255)
                            C1 = MainForm.PenneyPresent ? (byte)0x00 : Drive_Level;
                        }
                        send_state = 0;  // loop back to start
                        break;
                }

                // check if we need to transmit 
                if (((MainForm.PTT || MainForm.MOX_set) && MainForm.KK_on) || (!MainForm.OnlyTxOnPTT && MainForm.KK_on))
                {
                    MainForm.PTTEnable = true;  // enable Tx
                }
                else
                {
                    CWnoteIndex = 0;  // reset the CW tone so next time it starts with a raised cosine profile
                    MainForm.PTTEnable = false;
                    MainForm.MOX.BackColor = SystemColors.Control;
                }

                // we need to know if the PTT/key has just been released so we can send the CW note tail and 
                // delay the release of PTT if Sequencing has been set

                if (LastPTT == true && MainForm.PTTEnable == false) // true if PTT or key just released
                {
                    PTTReleased = true;
                    PTTDelay = 0;  // reset PTT Sequence delay counter
                }

                if (MainForm.PTTEnable || PTTReleased)  // then we need to set bit 0 in C0
                {
                    C0 += 1;                        // set PTT to Ozy true
                    MainForm.MOX.BackColor = Color.Green;    // indicate we are transmitting 
                }

                pntr = (frame_number * 512) + bufferOffset;     // start past the 8 bytes header before the data

                to_Device[pntr] = sync;
                to_Device[++pntr] = sync;
                to_Device[++pntr] = sync;

                to_Device[++pntr] = C0;
                to_Device[++pntr] = C1;
                to_Device[++pntr] = C2;
                to_Device[++pntr] = C3;
                to_Device[++pntr] = C4;

                for (x = 8; x < 512; x += 8)        // fill out one 512-byte frame
                {
                    AudioRing.Read(ref SampleReal, ref SampleImag);

                    // use the following rather than BitConverter.GetBytes since it uses less CPU 
                    int IntValue;
                    IntValue = (int)(scaleOut * SampleReal);
                    to_Device[++pntr] = (byte)(IntValue >> 8);        // left hi
                    to_Device[++pntr] = (byte)(IntValue & 0xff);  // left lo

                    IntValue = (int)(scaleOut * SampleImag);
                    to_Device[++pntr] = (byte)(IntValue >> 8);    // right hi
                    to_Device[++pntr] = (byte)(IntValue & 0xff);  // right lo

                    // send a Tune tone if TUN button is pressed
                    if (MainForm.TUN_set)
                    {
                        switch (MainForm.CurrentMode)
                        {
                            case OperMode.USB:
                            case OperMode.CWU:
                            case OperMode.AM:
                            case OperMode.FM:
                                {
                                    I_data = (short)(TxGain * CWnoteSin[CWnoteIndex]);  // TxGain sets output level
                                    Q_data = (short)(TxGain * CWnoteCos[CWnoteIndex]);
                                    if (++CWnoteIndex >= 80) CWnoteIndex = 0;
                                }
                                break;

                            case OperMode.LSB:  // need to swap the phase of I&Q if LSB signal
                            case OperMode.CWL:
                                {
                                    I_data = (short)(TxGain * CWnoteCos[CWnoteIndex]);   // TxGain sets output level
                                    Q_data = (short)(TxGain * CWnoteSin[CWnoteIndex]);
                                    if (++CWnoteIndex >= 80) CWnoteIndex = 0;
                                }
                                break;
                        }
                    }


                    // need to separate the code that tests for the PTT Enabled and Released so we can delay the PTT 
                    // release for sequencing

                    else if (PTTReleased)
                    {
                        switch (MainForm.CurrentMode)
                        {
                            case OperMode.CWU:
                                // send the CW note with raised cosine tail
                                {
                                    I_data = (short)(TxGain * RCCWnoteSin[CWtailIndex + 320]);  // TxGain sets output level
                                    Q_data = (short)(TxGain * RCCWnoteCos[CWtailIndex + 320]);

                                    if (++CWtailIndex >= 240)  // tail has been sent so apply any PTT Sequencing
                                    {
                                        CWtailIndex = 239;     // send a zero sample next time
                                        if (++PTTDelay >= (20 + MainForm.DelayPTT) * 48)       // PTT delay set by user,
                                        {
                                            PTTReleased = false;                // tail has been sent
                                            CWtailIndex = 0;                    // reset counters ready for next note
                                            PTTDelay = 0;
                                            RFDelay = 0;
                                            MainForm.MOX.BackColor = SystemColors.Control; // set MOX button colour to normal
                                        }
                                    }
                                }
                                break;

                            case OperMode.CWL:
                                // send the CW note with raised cosine tail
                                {
                                    // swap I & Q to give LSB 
                                    I_data = (short)(TxGain * RCCWnoteCos[CWtailIndex + 320]);   // TxGain sets output level
                                    Q_data = (short)(TxGain * RCCWnoteSin[CWtailIndex + 320]);

                                    if (++CWtailIndex >= 240)  // tail has been sent so apply any PTT Sequencing
                                    {
                                        CWtailIndex = 239;     // send a zero sample next time
                                        if (++PTTDelay >= (20 + MainForm.DelayPTT) * 48)       // PTT delay set by user, 
                                        {
                                            PTTReleased = false;                // tail has been sent
                                            CWtailIndex = 0;                    // reset counters ready for next note
                                            PTTDelay = 0;
                                            RFDelay = 0;
                                            MainForm.MOX.BackColor = SystemColors.Control; // set MOX button colour to normal
                                        }
                                    }
                                }
                                break;

                            // Add PTT sequencing for all other modes by default
                            default:    // Hold PTT for DelayPTT mS
                                if (++PTTDelay >= (20 + MainForm.DelayPTT) * 48)         // PTT delay set by user, I&Q continues for 20mS
                                {
                                    PTTReleased = false;                        // delay done
                                    PTTDelay = 0;
                                    //RFDelay = 0;
                                    MainForm.MOX.BackColor = SystemColors.Control; // set MOX button colour to normal
                                }
                                break;
                        }
                    }

                    else if (MainForm.PTTEnable)
                    {
                        // convert mic data from float (+/- 1) to 2's complement and set drive level
                        switch (MainForm.CurrentMode)
                        {
                            case OperMode.FM:
                                if (TransmitAudioRing.Count > 0)
                                {
                                    TransmitAudioRing.Read(ref SampleReal, ref SampleImag);
                                    cvtmod2freq = (float)MainForm.FM_deviation * 2.0 * Math.PI / 48000.0;  // peak deviation is 2 x FM_deviation value
                                    oscphase += SampleReal * cvtmod2freq;
                                    if (oscphase > TwoPi) oscphase -= TwoPi;
                                    if (oscphase < -TwoPi) oscphase += TwoPi;
                                    I_data = (short)(MicScaleOut * Math.Cos(oscphase));
                                    Q_data = (short)(MicScaleOut * Math.Sin(oscphase));
                                    PTTReleased = false;
                                }
                                break;

                            case OperMode.USB:
                                if (TransmitAudioRing.Count > 0)
                                {
                                    TransmitAudioRing.Read(ref SampleReal, ref SampleImag);
                                    // convert to 16 bit integer
                                    I_data = (short)(MicScaleOut * SampleReal);
                                    Q_data = (short)(MicScaleOut * SampleImag);
                                    PTTReleased = false;
                                }
                                break;

                            case OperMode.LSB:
                                if (TransmitAudioRing.Count > 0)
                                {
                                    TransmitAudioRing.Read(ref SampleReal, ref SampleImag);
                                    // convert to 16 bit integer
                                    Q_data = (short)(MicScaleOut * SampleReal);
                                    I_data = (short)(MicScaleOut * SampleImag);
                                    PTTReleased = false;
                                }
                                break;

                            case OperMode.CWU:
                                // send the raised cosine start and then a regular CW note
                                {
                                    if (MainForm.PTT || MainForm.MOX_set)  // only send a tone if a dot or dash is active
                                    {
                                        I_data = (short)(TxGain * RCCWnoteSin[CWnoteIndex]);  // TxGain sets output level
                                        Q_data = (short)(TxGain * RCCWnoteCos[CWnoteIndex]);
                                    }
                                    else  // Don't send any RF 
                                    {
                                        I_data = 0; Q_data = 0;
                                        CWnoteIndex = 0;   // so next note starts with raised cosine shape
                                    }

                                    if (RFDelay >= (MainForm.DelayRF - 5) * 48)              // Delay sending note until Sequencing done
                                    {                                               // 5mS is the USB etc delay
                                        if (++CWnoteIndex >= 320) CWnoteIndex = 240;
                                    }
                                    else RFDelay++;
                                }
                                break;

                            case OperMode.CWL:

                                // send the raised cosine start and then a regular CW note
                                {
                                    if (MainForm.PTT || MainForm.MOX_set)  // only send a tone if a dot or dash is active
                                    {
                                        // swap I & Q to give LSB 
                                        I_data = (short)(TxGain * RCCWnoteCos[CWnoteIndex]);   // TxGain sets output level
                                        Q_data = (short)(TxGain * RCCWnoteSin[CWnoteIndex]);
                                    }
                                    else  // Don't send any RF 
                                    {
                                        I_data = 0; Q_data = 0;
                                        CWnoteIndex = 0;   // so next note starts with raised cosine shape
                                    }

                                    if (RFDelay >= (MainForm.DelayRF - 5) * 48)              // Delay sending note until Sequencing done
                                    {                                               // 5mS is the USB etc delay
                                        if (++CWnoteIndex >= 320) CWnoteIndex = 240;
                                    }
                                    else RFDelay++;
                                }
                                break;

                            // generate AM - no plate transformers or leaky grids in sight!
                            case OperMode.AM:
                            case OperMode.SAM:
                                if (TransmitAudioRing.Count > 0)
                                {
                                    TransmitAudioRing.Read(ref SampleReal, ref SampleImag);
                                    I_data = (short)(MicScaleOut * 0.4f * (1 + SampleReal));
                                    Q_data = 0;
                                    IntValue = 0;
                                    PTTReleased = false;
                                }
                                break;

                            default:
                                IntValue = 0;
                                PTTReleased = false;
                                break;
                        }
                    }

                    // send I & Q data to Qzy 
                    to_Device[++pntr] = (byte)(I_data >> 8);      // I_data[0];
                    to_Device[++pntr] = (byte)(I_data & 0xff);    // I_data[1];
                    to_Device[++pntr] = (byte)(Q_data >> 8);      // Q_data[0];
                    to_Device[++pntr] = (byte)(Q_data & 0xff);    // Q_data[1];  
                } // end for

                LastPTT = MainForm.PTTEnable; // save the last PTT/key state so we can tell when it is released 
            }
        }

        public virtual void Start() { }
        public virtual void Stop() { }
        public virtual void SetMicGain() { }
        public virtual void Close() { }
        public virtual void ProcessWideBandData(ref byte[] EP4buf) { }
        protected virtual void Data_send(bool force) { }
        public virtual bool CheckVersions() { return true; }
        public virtual void SpecialI2CProcessing() { }
    }
}
