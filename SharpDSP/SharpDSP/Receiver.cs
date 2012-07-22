//==============================================================
//Written by: Philip A Covington, N8VB
//
//This software is licensed under the GNU General Public License
//==============================================================
//receiver.cs
//implements receiver functions for SDR
//==============================================================

/*
This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2007, 2008 Philip A Covington
Copyright (C) 2011-2012 Warren Pratt NR0V (wcpAGC code)

This program is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

The author can be reached by email at

p.covington@gmail.com
 * 
 * 
 * Modified 4 January 2012 by George Byrkit to add a Mutex that protects
 * the RX process from variable changes.  The RX process typically happens
 * on a non-UI thread, where the variable changes occur on/from the UI thread.
 * At this time, Warren Pratt's (NR0V) AGC method was added.  dagc.cs was replaced
 * by wcpagc.cs.

*/
using System;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;

namespace SharpDSP2._1
{	
	[Serializable()]
	public class Receiver
	{
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern void OutputDebugString(string text);

		#region Private members

        internal DSPBuffer rxbuffer;        
        internal Filter filter;
		internal Oscillator local_osc;
        internal Oscillator spec_local_osc;
#if false
		// replaced by WCPAGC... 
        internal DAgc agc;
#endif
        internal WCPAGC agc;
        internal BlockNoiseBlanker block_noise_blanker;
        internal AveragingNoiseBlanker ave_noise_blanker;
		internal Squelch squelch;
		internal SignalMeter meter;
		internal PowerSpectrumSignal power_spectrum;
		internal PLL pll;
		internal NoiseFilter noise_filter;
        internal InterferenceFilter interference_filter;
		internal Output output_mode;
        internal DCCorrector dcc;
        internal LoadableFilter cfir;
        internal OutbandPowerSpectrumSignal ops;
        internal BlockNoiseBlanker bnb_spec;
        internal AveragingNoiseBlanker anb_spec;
                
		#endregion

		#region Constructor

		public Receiver()
		{
            DSPState state =  new DSPState();

//            state.ServerConfigObject = sc;
//            state.DSPBlockSize = sc.DSPBlockSize;
//            state.DSPSampleRate = sc.DSPSampleRate;
//            state.PowerSpectrumBlockSize = sc.SpecBlockSize;

            rxbuffer = new DSPBuffer(state);

            dcc = new DCCorrector(ref rxbuffer);

            // Filter
            filter = new Filter(ref rxbuffer);
            
            // Local Oscillator (channel)
            local_osc = new Oscillator(ref rxbuffer);
            local_osc.LocalOscillatorOn = true;

            // Local Oscillator (channel)
            spec_local_osc = new Oscillator(ref rxbuffer);
            spec_local_osc.LocalOscillatorOn = true;

            // AGC
            //agc = new DAgc(ref rxbuffer);
            agc = new WCPAGC(ref rxbuffer);

            // Block Noise Blanker
            block_noise_blanker = new BlockNoiseBlanker(ref rxbuffer);
            bnb_spec = new BlockNoiseBlanker(ref rxbuffer);

            // Average Noise Blanker
            ave_noise_blanker = new AveragingNoiseBlanker(ref rxbuffer);
            anb_spec = new AveragingNoiseBlanker(ref rxbuffer);

            // Squelch
            squelch = new Squelch(ref rxbuffer);

            // Signal Metering
            meter = new SignalMeter(ref rxbuffer);

            // Power Spectrum
            power_spectrum = new PowerSpectrumSignal(ref rxbuffer);

            // PLL
            pll = new PLL(ref rxbuffer);

            // Noise Filter
            noise_filter = new NoiseFilter(ref rxbuffer);

            // Interference Filter
            interference_filter = new InterferenceFilter(ref rxbuffer);

            // Output
            output_mode = new Output(ref rxbuffer);

            cfir = new LoadableFilter(state.SpectrumAquireBlockSize, 64);
            cfir.LoadFilter(new FilterCoeffSet(64).GetFilterCoeffSet());

//            ops = new OutbandPowerSpectrumSignal(state.PowerSpectrumBlockSize);
		}

		#endregion

        #region Mutual Exclusion
        // use ReceiverMutex.WaitOne() to claim, ReceiverMutex.ReleaseMutex() to release
        private Mutex ReceiverMutex = new Mutex();
        #endregion

        #region Public members

        /// <summary>
        /// Now uses a Mutex object around all the processing, so that updates of
        /// properties or fields do NOT occur while a block is being processed.
        /// (George Byrkit, 4 January 2012)
        /// </summary>
        /// <param name="inbuffer">input buffer, a complex (CPX) array</param>
        /// <param name="outbuffer">output buffer, a complex (CPX) array</param>
        unsafe public void DoDSPProcess(ref CPX[] inbuffer, ref CPX[] outbuffer)
		{
            try
            {
                ReceiverMutex.WaitOne();

                rxbuffer.Fill(ref inbuffer);

                #region Do Noiseblankers

                if (block_noise_blanker.BlockNBSwitchOn)
                {
                    block_noise_blanker.Process();
                }

                if (ave_noise_blanker.AveNBSwitchOn)
                {
                    ave_noise_blanker.Process();
                }

                #endregion

                #region Local Oscillator

                local_osc.Process();

                #endregion

                #region Power Spectrum before filter

                power_spectrum.Process();   //djm uncommented

                #endregion

                #region Filter

                filter.Process();

                #endregion

                #region Metering after filter

                meter.Process();

                #endregion

                #region Do AGC

#if false
                // find max real and imaginary components for warren and log them (using OutputDebugString)
                float maxReal = 0.0F;
                float maxImaginary = 0.0F;
                float temp;
                for (int i = 0; i < rxbuffer.State.DSPBlockSize; ++i)
                {
                    temp = Math.Abs(rxbuffer.cpx[i].real);
                    if (temp > maxReal)
                    {
                        maxReal = temp;
                    }
                    temp = Math.Abs(rxbuffer.cpx[i].imag);
                    if (temp > maxImaginary)
                    {
                        maxImaginary = temp;
                    }
                }
                string logMsg = String.Format("AGC Buffer max real = {0}, max imaginary = {1}\n", maxReal, maxImaginary);
                OutputDebugString(logMsg);
                //Console.Write(logMsg);
#endif

                agc.Process();

                #endregion

                #region Squelch

                squelch.Process();

                #endregion

                #region Do Demod

                pll.Process();

                #endregion

                if (noise_filter.NoiseFilterSwitchOn) noise_filter.Process();
                if (interference_filter.InterferenceFilterSwitchOn) interference_filter.Process();

                #region Do Output

                output_mode.Process();

                #endregion

                outbuffer = (CPX[])rxbuffer.cpx.Clone();
            }
            finally
            {
                ReceiverMutex.ReleaseMutex();
            }
		}    

		public Receiver CloneSubRX(Receiver obj)
		{
			Receiver rcv_new = null;
            //Copier copyit = new Copier();
            //rcv_new = (Receiver)copyit.CopyObject(obj);
            //rcv_new.rx_type = RXType.SubRX;
            return rcv_new;
		}
        
		#endregion

        #region Public Properties

        public DSPState DSPStateObj
        {
            get { return rxbuffer.State; } 
        }
               
        public float FilterFrequencyLow
        {
            get { return filter.FilterFrequencyLow; }
            set 
            {
                ReceiverMutex.WaitOne();
                filter.FilterFrequencyLow = value;
                ReceiverMutex.ReleaseMutex();
            }
        }
        
        public float FilterFrequencyHigh
        {
            get { return filter.FilterFrequencyHigh; }
            set
            {
                ReceiverMutex.WaitOne();
                filter.FilterFrequencyHigh = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        public float LOFrequency
        {
            get { return local_osc.LOFrequency; }
            set {
                ReceiverMutex.WaitOne();
                local_osc.LOFrequency = value;
                ReceiverMutex.ReleaseMutex();
            }
        }
		public float SquelchMeterOffset
		{
            get { return squelch.SquelchMeterOffset; }
			set
			{
                ReceiverMutex.WaitOne();
                squelch.SquelchMeterOffset = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		public float SquelchGainOffset
		{
            get { return squelch.SquelchGainOffset; }
			set
			{
                ReceiverMutex.WaitOne();
                squelch.SquelchGainOffset = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		public float SquelchAttnOffset
		{
            get { return squelch.SquelchAttnOffset; }
			set
			{
                ReceiverMutex.WaitOne();
                squelch.SquelchAttnOffset = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		public bool SquelchOn
		{
            get { return squelch.SquelchOn; }
			set
			{
                ReceiverMutex.WaitOne();
                squelch.SquelchOn = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		public float SquelchThreshold
		{
            get { return squelch.SquelchThreshold; }
			set
			{
                ReceiverMutex.WaitOne();
                squelch.SquelchThreshold = value;
                ReceiverMutex.ReleaseMutex();
            }
		}
				
        public bool BlockNBSwitchOn
        {
            get { return block_noise_blanker.BlockNBSwitchOn; }
            set
            {
                ReceiverMutex.WaitOne();
                block_noise_blanker.BlockNBSwitchOn = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

		public float BlockNBThreshold
		{
            get { return block_noise_blanker.BlockNBThreshold; }
			set
			{
                ReceiverMutex.WaitOne();
                block_noise_blanker.BlockNBThreshold = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

        public bool AveNBSwitchOn
        {
            get { return ave_noise_blanker.AveNBSwitchOn; }
            set
            {
                ReceiverMutex.WaitOne();
                ave_noise_blanker.AveNBSwitchOn = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

		public float AveNBThreshold
		{
            get { return ave_noise_blanker.AveNBThreshold; }
			set
			{
                ReceiverMutex.WaitOne();
                ave_noise_blanker.AveNBThreshold = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

        public AGCType_e AGCMode
		{
            get { return agc.getAgc_mode(); }
			set
			{
                ReceiverMutex.WaitOne();
                agc.setAgc_mode(value);
                ReceiverMutex.ReleaseMutex();
            }
		}

        public double AGCFixedGain
        {
            get { return agc.FixedGain; }
            set
            {
                ReceiverMutex.WaitOne();
                agc.FixedGain = value;
                ReceiverMutex.ReleaseMutex();
            }
        }
        public double AGCFixedGainDB
        {
            get { return agc.FixedGainDb; }
            set
            {
                ReceiverMutex.WaitOne();
                agc.FixedGainDb = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        public double AGCMaximumGain
        {
            get { return agc.MaximumGain; }
            set
            {
                ReceiverMutex.WaitOne();
                agc.MaximumGain = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        public double AGCMaximumGainDB
        {
            get { return agc.MaximumGainDb; }
            set
            {
                ReceiverMutex.WaitOne();
                agc.MaximumGainDb = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// used to get/set the dB level on the spectral (panadapter) display
        /// </summary>
        public double AGCThreshDB
        {
            get { return agc.getAGCThreshDb(filter.FilterFrequencyHigh, filter.FilterFrequencyLow, block_size); }
            set
            {
                ReceiverMutex.WaitOne();
                agc.setAGCThreshDb(filter.FilterFrequencyHigh, filter.FilterFrequencyLow, block_size, value);
                ReceiverMutex.ReleaseMutex();
            }
        }

        public double AGCHangTime
		{
            get { return agc.HangTime; }
			set
			{
                ReceiverMutex.WaitOne();
                agc.HangTime = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

        public double AGCHangThreshold
        {
            get { return agc.HangThresh; }
            set
            {
                ReceiverMutex.WaitOne();
                agc.HangThresh = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        public double AGCHangLevel
        {
            get { return agc.HangLevelDb; }
            set
            {
                ReceiverMutex.WaitOne();
                agc.HangLevelDb = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Slope is in dB
        /// </summary>
        public double AGCSlope
        {
            get { return agc.VarGainDb; }
            set
            {
                ReceiverMutex.WaitOne();
                agc.VarGainDb = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        public AGC_Status AGCStatus
        {
            get { return agc.AGCStatus; }
        }

        public bool AGCHangEnable
        {
            get { return agc.AGCHangEnable; }
            set
            {
                agc.AGCHangEnable = value;
            }
        }

        public double AGCAttackTime
		{
            get { return agc.TauAttack; }
			set
			{
                ReceiverMutex.WaitOne();
                agc.TauAttack = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

        public double AGCDecayTime
        {
            get { return agc.TauDecay; }
            set
            {
                ReceiverMutex.WaitOne();
                agc.TauDecay = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        public bool InterferenceFilterSwitchOn
        {
            get { return interference_filter.InterferenceFilterSwitchOn; }
            set
            {
                ReceiverMutex.WaitOne();
                interference_filter.InterferenceFilterSwitchOn = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

		public float InterferenceFilterAdaptationRate
		{
            get { return interference_filter.InterferenceFilterAdaptationRate; }
			set
			{
                ReceiverMutex.WaitOne();
                interference_filter.InterferenceFilterAdaptationRate = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		public int InterferenceFilterDelay
		{
            get { return interference_filter.InterferenceFilterDelay; }
			set
			{
                ReceiverMutex.WaitOne();
                interference_filter.InterferenceFilterDelay = value;
                ReceiverMutex.ReleaseMutex();
            }			
		}

		public float InterferenceFilterLeakage
		{
            get { return interference_filter.InterferenceFilterLeakage; }
			set
			{
                ReceiverMutex.WaitOne();
                interference_filter.InterferenceFilterLeakage = value;
                ReceiverMutex.ReleaseMutex();
            }
		}
		
		public int InterferenceFilterAdaptiveFilterSize
		{
            get { return interference_filter.InterferenceFilterAdaptiveFilterSize; }
			set
			{
                ReceiverMutex.WaitOne();
                interference_filter.InterferenceFilterAdaptiveFilterSize = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

        public bool NoiseFilterSwitchOn
        {
            get { return noise_filter.NoiseFilterSwitchOn; }
            set
            {
                ReceiverMutex.WaitOne();
                noise_filter.NoiseFilterSwitchOn = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

		public int NoiseFilterDelay
		{
            get { return noise_filter.NoiseFilterDelay; }
			set
			{
                ReceiverMutex.WaitOne();
                noise_filter.NoiseFilterDelay = value;
                ReceiverMutex.ReleaseMutex();
            }			
		}

		public float NoiseFilterLeakage
		{
            get { return noise_filter.NoiseFilterLeakage; }
			set
			{
                ReceiverMutex.WaitOne();
                noise_filter.NoiseFilterLeakage = value;
                ReceiverMutex.ReleaseMutex();
            }
		}
		
		public int NoiseFilterAdaptiveFilterSize
		{
            get { return noise_filter.NoiseFilterAdaptiveFilterSize; }
			set
			{
                ReceiverMutex.WaitOne();
                noise_filter.NoiseFilterAdaptiveFilterSize = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

        public float NoiseFilterAdaptationRate
        {
            get { return noise_filter.NoiseFilterAdaptationRate; }
            set
            {
                ReceiverMutex.WaitOne();
                noise_filter.NoiseFilterAdaptationRate = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        private float osc_freq = 11025F;
		public float LocalOscillatorFrequency
		{
			get { return osc_freq; }
			set
			{
                ReceiverMutex.WaitOne();
                osc_freq = value;
				local_osc.LOFrequency = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private int iq_gain_value = 1;
		public int IQGainValue
		{
			get { return iq_gain_value; }
			set
			{
                ReceiverMutex.WaitOne();
                iq_gain_value = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private int iq_phase_value = 0;
		public int IQPhaseValue
		{
			get { return iq_phase_value; }
			set
			{
                ReceiverMutex.WaitOne();
                iq_phase_value = value;
                ReceiverMutex.ReleaseMutex();
            }
		}
				
		private float samplerate = 48000F;
		public float SampleRate
		{
			get { return samplerate; }
			set
			{
                ReceiverMutex.WaitOne();
                samplerate = value;
                rxbuffer.State.DSPSampleRate = samplerate;
                agc.setSample_rate((int)samplerate);
                ReceiverMutex.ReleaseMutex();
            }
		}
								
		private WindowType_e window_type = WindowType_e.BLACKMANHARRIS_WINDOW;
		public WindowType_e WindowType
		{
			get { return window_type; }
			set
			{
                ReceiverMutex.WaitOne();
                window_type = value;
                ReceiverMutex.ReleaseMutex();
            }
		}
		
		private bool binaural_mode_value = false;
		public bool BinauralMode
		{
			get { return binaural_mode_value; }
			set
			{
                ReceiverMutex.WaitOne();
                binaural_mode_value = value;
                output_mode.BinAuralMode = value;
                ReceiverMutex.ReleaseMutex();
            }
		}



		
		private int block_size = 2048;
		public int BlockSize
		{
			get { return block_size; }
			set
			{
                ReceiverMutex.WaitOne();
                block_size = value;
				fft_size = block_size * 2;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private int fft_size = 4096;
		public int FFTSize
		{
			get { return fft_size; }
			set
			{
                ReceiverMutex.WaitOne();
                fft_size = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private float rx_volume_l = 0.25F;
		public float VolumeLeft
		{
			get { return rx_volume_l; }
			set
			{
                ReceiverMutex.WaitOne();
                rx_volume_l = value;
                output_mode.VolumeLeft = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private float rx_volume_r = 0.25F;
		public float VolumeRight
		{
			get { return rx_volume_r; }
			set
			{
                ReceiverMutex.WaitOne();
                rx_volume_r = value;
                output_mode.VolumeRight = value;
                ReceiverMutex.ReleaseMutex();
            }
		}
		
        //private Mode current_mode = DSPMode_e.AM;
        //private Mode mode = DSPMode_e.AM;
        //public Mode CurrentMode
        //{
        //    get { return current_mode; }
        //    set
        //    {
        //        current_mode = value;
        //        mode = value;
        //        if (current_mode == DSPMode_e.SPEC)
        //            power_spectrum..Mode = PowerSpectrumMode.SPECMODE; //force power_spec position to before filter			                
        //    }
        //}

		private WindowType_e current_window = WindowType_e.BLACKMANHARRIS_WINDOW;
		public WindowType_e CurrentWindow
		{
			get { return current_window; }
			set
			{
                ReceiverMutex.WaitOne();
                current_window = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private int rx_display_low = 100;
		public int RXDisplayLow
		{
			get { return rx_display_low; }
			set {
                ReceiverMutex.WaitOne();
                rx_display_low = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private int rx_display_high = 2800;
		public int RXDisplayHigh
		{
			get { return rx_display_high; }
			set {
                ReceiverMutex.WaitOne();
                rx_display_high = value;
                ReceiverMutex.ReleaseMutex();
            }
		}
		
        //private int rx_filter_low_cut = 100;
        //public int RXFilterLowCut
        //{
        //    get { return rx_filter_low_cut; }
        //    set
        //    {
        //        rx_filter_low_cut = value;
        //        this.UpdateRXDisplayVars();
        //        filter.MakeFilter(this.rx_filter_low_cut, this.rx_filter_high_cut, this.SampleRate, FilterType.BandPass, (WindowType)this.WindowType);                 
        //    }
        //}

        //private int rx_filter_high_cut = 2800;
        //public int RXFilterHighCut
        //{
        //    get { return rx_filter_high_cut; }
        //    set
        //    {
        //        rx_filter_high_cut = value;
        //        this.UpdateRXDisplayVars();
        //        filter.MakeFilter(this.rx_filter_low_cut, this.rx_filter_high_cut, this.SampleRate, FilterType.BandPass, (WindowType)this.WindowType);
        //    }
        //}
				
				
		private float current_dds_freq = 0F;
		public float CurrentDDSFreq
		{
			get { return current_dds_freq; }
			set
			{
                ReceiverMutex.WaitOne();
                current_dds_freq = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private float am_pll_freq = 0F;         // is likewise 0.0F in PLL.cs
		public float AMPLLFrequency
		{
			get { return am_pll_freq; }
			set
			{
                ReceiverMutex.WaitOne();
                am_pll_freq = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private float am_pll_lo_limit = -500F;  // is -1000F in PLL.cs
		public float AMPLLLowLimit
		{
			get { return am_pll_lo_limit; }
			set
			{
                ReceiverMutex.WaitOne();
                am_pll_lo_limit = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private float am_pll_hi_limit = 500F;   // is +1000F in PLL.cs
		public float AMPLLHighLimit
		{
			get { return am_pll_hi_limit; }
			set
			{
                ReceiverMutex.WaitOne();
                am_pll_hi_limit = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private float am_pll_bandwidth = 400F;  // is 500F in PLL.cs
		public float AMPLLBandwidth
		{
			get { return am_pll_bandwidth; }
			set
			{
                ReceiverMutex.WaitOne();
                am_pll_bandwidth = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

        // set synchronous AM parameters as the PLL object's parameters
        public void SetSAMMode()
        {
            ReceiverMutex.WaitOne();
            pll.PLLBandwidth = AMPLLBandwidth;
            pll.PLLHighLimit = AMPLLHighLimit;
            pll.PLLLowLimit = AMPLLLowLimit;
            pll.PLLFrequency = AMPLLFrequency;
            ReceiverMutex.ReleaseMutex();
        }

        // set FM parameters as the PLL object's parameters
        public void SetFMMode()
        {
            ReceiverMutex.WaitOne();
            pll.PLLBandwidth = FMPLLBandwidth;
            pll.PLLHighLimit = FMPLLHighLimit;
            pll.PLLLowLimit = FMPLLLowLimit;
            pll.PLLFrequency = FMPLLFrequency;
            ReceiverMutex.ReleaseMutex();
        }

        private float fm_pll_freq = 0F;
		public float FMPLLFrequency
		{
			get { return fm_pll_freq; }
			set
			{
                ReceiverMutex.WaitOne();
                fm_pll_freq = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private float fm_pll_lo_limit = -6000F;
		public float FMPLLLowLimit
		{
			get { return fm_pll_lo_limit; }
			set
			{
                ReceiverMutex.WaitOne();
                fm_pll_lo_limit = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private float fm_pll_hi_limit = 6000F;
		public float FMPLLHighLimit
		{
			get { return fm_pll_hi_limit; }
			set
			{
                ReceiverMutex.WaitOne();
                fm_pll_hi_limit = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

		private float fm_pll_bandwidth = 10000F;
		public float FMPLLBandwidth
		{
			get { return fm_pll_bandwidth; }
			set
			{
                ReceiverMutex.WaitOne();
                fm_pll_bandwidth = value;
                ReceiverMutex.ReleaseMutex();
            }
		}

        //private RXType rx_type = RXType.MainRX;
        //public RXType ReceiverType
        //{
        //    get { return rx_type; }			
        //}

		private RXOutputRoute_e rx_route = RXOutputRoute_e.None;
		public RXOutputRoute_e ReceiverOutputRoute
		{
			get { return rx_route; }
			set
			{
                ReceiverMutex.WaitOne();
                rx_route = value;
                output_mode.RXOutputRoute = value;
                ReceiverMutex.ReleaseMutex();
            }
		}
        public bool PowerSpectrumOn
        {
            get { return power_spectrum.SpectrumSwitchOn; }
            set
            {
                ReceiverMutex.WaitOne();
                power_spectrum.SpectrumSwitchOn = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        public int PowerSpectrumUpdateRate
        {
            get { return power_spectrum.UpdatesPerSecond; }
            set
            {
                ReceiverMutex.WaitOne();
                power_spectrum.UpdatesPerSecond = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        public bool PowerSpectrumAveragingOn
        {
            get { return power_spectrum.PowerSpectrumAveragingOn; }
            set
            {
                ReceiverMutex.WaitOne();
                power_spectrum.PowerSpectrumAveragingOn = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        public float PowerSpectrumSmoothingFactor
        {
            get { return power_spectrum.PowerSpectrumSmoothingFactor; }
            set {
                ReceiverMutex.WaitOne();
                power_spectrum.PowerSpectrumSmoothingFactor = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        public float PowerSpectrumCorrection
        {
            get { return power_spectrum.PowerSpectrumCorrection; }
            set {
                ReceiverMutex.WaitOne();
                power_spectrum.PowerSpectrumCorrection = value;
                ReceiverMutex.ReleaseMutex();
            }
        }

        public PowerSpectrumSignal DSPPowerSpectrumObj
        {
            get { return power_spectrum; }
        }


        //private PowerSpectrumMode ps_mode = PowerSpectrumMode_e.OFF;
        //public PowerSpectrumMode PowerSpectrumMode
        //{
        //    get { return ps_mode; }
        //    set
        //    {
        //        ps_mode = value;
        //        power_spectrum..Mode = value;
        //        if (this.CurrentMode == Mode.SPEC)
        //            power_spectrum.Mode = PowerSpectrumMode.SPECMODE; //force power_spec position to before filter
        //    }
        //========================================

        // added 15 June 2009 by George Byrkit, K9TRV, to support access to the SMeter data that 'Receive' accumulates
        public float SMeterInstValue
        {
            get { return meter.InstValue; }
        }


        public float SMeterAvgValue
        {
            get { return meter.AvgValue; }
        }
        // END added 15 June 2009 by George Byrkit, K9TRV, to support access to the SMeter data that 'Receive' accumulates

        #endregion
    }
}
