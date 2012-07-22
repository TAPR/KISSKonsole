/*  wcpagc.cs

This file is part of a program that implements a Software-Defined Radio.

Copyright (C) 2011, 2012 Warren Pratt, NR0V

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

The author can be reached by email at  

warren@wpratt.com

or by paper mail at

Warren Pratt
11303 Empire Grade
Santa Cruz, CA  95060

*/

using System;
using System.Collections.Generic;
using System.Text;

namespace SharpDSP2._1
{
    public class WCPAGC : IDSPObject
    {
        #region Variables
        private AGCType_e agc_mode;
        private int sample_rate;
        private double fixed_gain;

        private double tau_attack;
        private double tau_decay;
        private int n_tau;
        private double max_gain;
        private double var_gain;
        private double min_volts;
        private double max_input;
        private double out_targ;
        private double out_target;
        private double inv_max_input;
        private double slope_constant;

        private int out_index;
        private int in_index;
        private int attack_buffsize;

        private const int ring_buffsize = 19200;
        private CPX[] ring = new CPX[ring_buffsize];
        private double ring_max;
        private double[] abs_ring = new double[ring_buffsize];

        private double attack_mult;
        private double decay_mult;
        private double volts;
        private double save_volts;

        private CPX out_sample;
        private double abs_out_sample;
        private int state;

        private double tau_fast_backaverage;
        private double fast_backmult;
        private double onemfast_backmult;
        private double fast_backaverage;
        private double tau_fast_decay;
        private double fast_decay_mult;
        private double pop_ratio;

        private double hang_backaverage;
        private double tau_hang_backmult;
        private double hang_backmult;
        private double onemhang_backmult;
        private int hang_counter;
        private double hangtime;
        private double hang_thresh;
        private double hang_level;

        private double tau_hang_decay;
        private double hang_decay_mult;
        private int decay_type;

        // average of the absolute value of a sin wave of magnitude 1.0
        private const double SinAverage = 0.637;

        private DSPBuffer d = null;
        private DSPState s = null;
        #endregion

        #region Constructor
        public WCPAGC(ref DSPBuffer dsp_buffer_obj)
        {
            this.d = dsp_buffer_obj;
            this.s = d.State;

            //initialization
            InitWcpAGC();
         
            //defaults
            agc_mode = AGCType_e.agcLong;
            sample_rate = 48000;
            fixed_gain = 1000;
            n_tau = 4;
            tau_attack = 0.001;
            tau_decay = 0.250;
            tau_fast_decay = 0.005;
            tau_fast_backaverage = 0.250;
            pop_ratio = 5;
            out_targ = 1.0;
            var_gain = 1.0;
            max_gain = 100000.0;
            tau_hang_decay = 0.100;

#if false
            max_input = 1.0;
#else
            // Warren NR0V reports that this change needs to be made in order for things to be
            // in the right position on the screen and other factors.
            max_input = 500.0;
#endif

            hang_thresh = 0.01;
            hangtime = 0.250;
            tau_hang_backmult = 0.500;

            //setup calculated variables
            LoadWcpAGC();
        }
        #endregion

        #region get_set_methods

        /// <summary>
        /// public void setAgc_mode(int Agc_mode)
        /// Should also add a 'custom' mode where tau_decay can be set, along with tau_attack, maybe
        /// </summary>
        /// <param name="Agc_mode">0 for fixed gain, i.e., AGC is OFF, 1 for 'long', 2 for 'slow', 3 for 'medium', 4 for 'fast'</param>
        public void setAgc_mode(AGCType_e Agc_mode)
            //0 for fixed gain, i.e., AGC is OFF
        {
            if ((agc_mode == 0) && (Agc_mode != 0))
            {
                InitWcpAGC();
            }

            switch (Agc_mode)
            {
                case AGCType_e.agcOff:     //AGC off
                    break;

                case AGCType_e.agcLong:     //long
                    agcHangEnable = true;
                    hangtime = 2.0;
                    tau_decay = 2.0;
                    break;

                case AGCType_e.agcSlow:     //slow
                    agcHangEnable = true;
                    hangtime = 1.0;
                    tau_decay = 0.500;
                    break;

                case AGCType_e.agcMedium:     //medium
                    agcHangEnable = false;
                    hangtime = 0.0;
                    tau_decay = 0.250;
                    break;

                case AGCType_e.agcFast:     //fast
                    agcHangEnable = false;
                    hangtime = 0.0;
                    tau_decay = 0.050;
                    break;

                case AGCType_e.agcUser:     // custom...
                    agcHangEnable = true;
                    hangtime = 2.0;
                    tau_decay = 2.0;
                    break;

                default:
                    agcHangEnable = true;
                    hangtime = 2.0;
                    tau_decay = 2.0;
                    break;
            }

            agc_mode = Agc_mode;

            LoadWcpAGC();
        }

        public AGCType_e getAgc_mode()
        {
            return agc_mode;
        }

        /// <summary>
        /// fixed_gain when AGC is OFF (set to 'fixed'), linear
        /// </summary>
        public double FixedGain
        {
            get { return fixed_gain; }
            set { fixed_gain = value; }
        }

        /// <summary>
        /// fixed_gain when AGC is OFF (set to 'fixed'), in dB
        /// </summary>
        public double FixedGainDb
        {
            get
            {
                return 20.0 * Math.Log10(fixed_gain);
            }
            set { fixed_gain = Math.Pow(10.0, value / 20.0); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Sample_rate">sample rate.  Likely 48,000 or 96,000 or 192,000</param>
        public void setSample_rate(int Sample_rate)
        {
            sample_rate = Sample_rate;
            InitWcpAGC();
            LoadWcpAGC();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tau_attack">attack time constant in SECONDS</param>
        public double TauAttack
        {
            get { return tau_attack; }
            set
            {
                tau_attack = value;
                LoadWcpAGC();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Tau_decay">decay time constant in SECONDS</param>
        public double TauDecay
        {
            get { return tau_decay; }
            set
            {
                tau_decay = value;
                LoadWcpAGC();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Max_gain">maximum AGC gain = gain below the "knee", linear</param>
        public double MaximumGain
        {
            get { return max_gain;  }
            set
            {
                max_gain = value;
                if (Double.IsNaN(max_gain) || Double.IsInfinity(max_gain) || (max_gain <= 0))
                {
                    // illegal values!
                    throw new ArgumentOutOfRangeException("MaximumGain: new value is not valid!");
                }
                LoadWcpAGC();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Max_gaindB">maximum AGC gain = gain below the "knee", in dB</param>
        public double MaximumGainDb
        {
            get
            {
                return 20.0 * Math.Log10(max_gain);
            }
            set
            {
                max_gain = Math.Pow(10.0, value / 20.0);
                if (Double.IsNaN(max_gain) || Double.IsInfinity(max_gain) || (max_gain <= 0))
                {
                    // illegal values!
                    throw new ArgumentOutOfRangeException("MaximumGain: new value is not valid!");
                }
                LoadWcpAGC();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Var_gain">variable AGC gain = "Slope", linear</param>
        public double VarGain
        {
            get { return var_gain;  }
            set
            {
                var_gain = value;
                LoadWcpAGC();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Var_gaindB">variable AGC gain = "Slope", in dB</param>
        public double VarGainDb
        {
            get { return 20.0 * Math.Log10(var_gain); }
            set
            {
                var_gain = Math.Pow(10.0, value / 20.0); ;
                LoadWcpAGC();
            }
        }

        /// <summary>
        /// for hang threshold slider, range 0.0 to 1.0
        /// </summary>
        /// <returns></returns>
        public double HangThresh
        {
            get { return hang_thresh; }
            set
            {
                hang_thresh = value;
                LoadWcpAGC();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Hangtime">hangtime in SECONDS</param>
        public double HangTime
        {
            get { return hangtime; }
            set
            {
                hangtime = value;
                LoadWcpAGC();
            }
        }

        /// <summary>
        /// for line on bandscope
        /// </summary>
        /// <returns></returns>
        public double HangLevelDb
        {
            get {return 20.0 * Math.Log10(hang_level / SinAverage); }
            set {             
                if (max_input > min_volts)
                {
                    // BUG: found by Warren, 15 Jan 2012:  'SinAverage' term NOT needed!
                    // double convert = SinAverage * Math.Pow(10.0, value / 20.0);
                    double convert = Math.Pow(10.0, value / 20.0);
                    double tmp = Math.Max(1.0e-8, (convert - min_volts) / (max_input - min_volts));
                    hang_thresh = 1.0 + 0.125 * Math.Log10(tmp);
                }
                else
                {
                    hang_thresh = 1.0;
                }

                LoadWcpAGC();
            }
        }

        /// <summary>
        /// for line on bandscope
        /// </summary>
        /// <param name="filt_high"></param>
        /// <param name="filt_low"></param>
        /// <param name="spec_size"></param>
        /// <returns></returns>
        public double getAGCThreshDb(double filt_high, double filt_low, int spec_size)
        {
            double noise_offset = 10.0 * Math.Log10(Math.Abs(filt_high - filt_low) * spec_size / sample_rate);
            return 20.0 * Math.Log10(min_volts) - noise_offset;
        }

        /// <summary>
        /// for line on bandscope
        /// </summary>
        /// <param name="filt_high"></param>
        /// <param name="filt_low"></param>
        /// <param name="spec_size"></param>
        /// <param name="thresh"></param>
        public void setAGCThreshDb(double filt_high, double filt_low, int spec_size, double thresh)
        {
            double noise_offset = 10.0 * Math.Log10(Math.Abs(filt_high - filt_low) * spec_size / sample_rate);
            max_gain = out_target / (var_gain * Math.Pow(10.0, (thresh + noise_offset) / 20.0));

            LoadWcpAGC();
        }

        public AGC_Status AGCStatus
        {
            get { return AGC_Status.None; }
        }

        #endregion

        #region Private Methods

        private void InitWcpAGC()
        {
            out_index = -1;
            ring_max = 0.0;
            volts = 0.0;
            save_volts = 0.0;
            fast_backaverage = 0.0;
            hang_backaverage = 0.0;
            hang_counter = 0;
            state = 0;
            Array.Clear(abs_ring, 0, 19200);
            for (int i = 0; i < ring_buffsize; i++)
            {
                ring[i].real = 0.0f;
                ring[i].imag = 0.0f;
            }
            out_sample.real = 0.0f;
            out_sample.imag = 0.0f;
            abs_out_sample = 0.0f;
            decay_type = 0;
        }

        private bool agcHangEnable = true;
        public bool AGCHangEnable
        {
            get { return agcHangEnable; }
            set
            {
                agcHangEnable = value;
                LoadWcpAGC();
            }
        }

        private void LoadWcpAGC()
        {
            double tmp;
            attack_buffsize = (int)Math.Ceiling(sample_rate * n_tau * tau_attack);

            in_index = attack_buffsize + out_index;

            attack_mult = 1.0 - Math.Exp(-1.0 / (sample_rate * tau_attack));
            decay_mult = 1.0 - Math.Exp(-1.0 / (sample_rate * tau_decay));
            fast_decay_mult = 1.0 - Math.Exp(-1.0 / (sample_rate * tau_fast_decay));
            fast_backmult = 1.0 - Math.Exp(-1.0 / (sample_rate * tau_fast_backaverage));
            onemfast_backmult = 1.0 - fast_backmult;

            out_target = out_targ * (1.0 - Math.Exp(-n_tau)) * 0.99;
            min_volts = out_target / (var_gain * max_gain);

            tmp = Math.Log10(out_target / (max_input * var_gain * max_gain));
            if (tmp == 0.0)
                tmp = 1e-16;
            slope_constant = (out_target * (1.0 - 1.0 / var_gain)) / tmp;

            inv_max_input = 1.0 / max_input;

            tmp = Math.Pow(10.0, ((agcHangEnable ? hang_thresh : 1.0) - 1.0) / 0.125);
            hang_level = (max_input * tmp + (out_target / (var_gain * max_gain)) * (1.0 - tmp)) * SinAverage;

            hang_backmult = 1.0 - Math.Exp(-1.0 / (sample_rate * tau_hang_backmult));
            onemhang_backmult = 1.0 - hang_backmult;

            hang_decay_mult = 1.0 - Math.Exp(-1.0 / (sample_rate * tau_hang_decay));
        }
        #endregion

        #region AGC Method

        public void Process()
        {
            if (agc_mode == AGCType_e.agcOff)
            {
                this.d.Scale((float)fixed_gain, (float)fixed_gain);
                return;
            }

            WcpAGC();
        }

        public void WcpAGC()
        {
            CPX[] buff = this.d.cpx;
            int i, j, k;
            double mult;

            for (i = 0; i < this.s.DSPBlockSize; i++)
            {
                if (++out_index >= ring_buffsize)
                    out_index -= ring_buffsize;
                if (++in_index >= ring_buffsize)
                    in_index -= ring_buffsize;

                out_sample = ring[out_index];
                abs_out_sample = abs_ring[out_index];
                ring[in_index] = buff[i];
                abs_ring[in_index] = Math.Max(Math.Abs(ring[in_index].real), Math.Abs(ring[in_index].imag));

                fast_backaverage = fast_backmult * abs_out_sample + onemfast_backmult * fast_backaverage;
                hang_backaverage = hang_backmult * abs_out_sample + onemhang_backmult * hang_backaverage;

                if ((abs_out_sample >= ring_max) && (abs_out_sample > 0))
                {
                    ring_max = 0.0;
                    k = out_index;
                    for (j = 0; j < attack_buffsize; j++)
                    {
                        if (++k == ring_buffsize)
                            k = 0;
                        if (abs_ring[k] > ring_max)
                            ring_max = abs_ring[k];
                    }
                }
                if (abs_ring[in_index] > ring_max)
                    ring_max = abs_ring[in_index];

                if (hang_counter > 0)
                    --hang_counter;

                switch (state)
                {
                    case 0:
                        {
                            if (ring_max >= volts)
                            {
                                volts += (ring_max - volts) * attack_mult;
                            }
                            else
                            {
                                if (volts > pop_ratio * fast_backaverage)
                                {
                                    state = 1;
                                    volts += (ring_max - volts) * fast_decay_mult;
                                }
                                else
                                {
                                    if (hang_backaverage > hang_level)
                                    {
                                        state = 2;
                                        hang_counter = (int)(hangtime * sample_rate);
                                        decay_type = 1;
                                    }
                                    else
                                    {
                                        state = 3;
                                        volts += (ring_max - volts) * decay_mult;
                                        decay_type = 0;
                                    }
                                }
                            }
                            break;
                        }

                    case 1:
                        {
                            if (ring_max >= volts)
                            {
                                state = 0;
                                volts += (ring_max - volts) * attack_mult;
                            }
                            else
                            {
                                if (volts > save_volts)
                                {
                                    volts += (ring_max - volts) * fast_decay_mult;
                                }
                                else
                                {
                                    if (hang_counter > 0)
                                    {
                                        state = 2;
                                    }
                                    else
                                    {
                                        if (decay_type == 0)
                                        {
                                            state = 3;
                                            volts += (ring_max - volts) * decay_mult;
                                        }
                                        else
                                        {
                                            state = 4;
                                            volts += (ring_max - volts) * hang_decay_mult;
                                        }
                                    }
                                }
                            }
                            break;
                        }

                    case 2:
                        {
                            if (ring_max >= volts)
                            {
                                state = 0;
                                save_volts = volts;
                                volts += (ring_max - volts) * attack_mult;
                            }
                            else
                            {
                                if (hang_counter == 0)
                                {
                                    state = 4;
                                    volts += (ring_max - volts) * hang_decay_mult;
                                }
                            }
                            break;
                        }

                    case 3:
                        {
                            if (ring_max >= volts)
                            {
                                state = 0;
                                save_volts = volts;
                                volts += (ring_max - volts) * attack_mult;
                            }
                            else
                            {
                                volts += (ring_max - volts) * decay_mult;
                            }
                            break;
                        }
                    case 4:
                        {
                            if (ring_max >= volts)
                            {
                                state = 0;
                                save_volts = volts;
                                volts += (ring_max - volts) * attack_mult;
                            }
                            else
                            {
                                volts += (ring_max - volts) * hang_decay_mult;
                            }
                            break;
                        }
                } // end switch on state

                if (volts < min_volts)
                    volts = min_volts;
                
                mult = (out_target - slope_constant * Math.Min(0.0, Math.Log10 (inv_max_input * volts))) / volts;
                buff[i].real = (float)(out_sample.real * mult);
                buff[i].imag = (float)(out_sample.imag * mult);
            }
        }
        #endregion
    }
}
