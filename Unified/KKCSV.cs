using System;
using System.Collections.Generic;
using System.IO;

namespace KISS_Konsole
{
    partial class Form1
    {
        private void CreateKKCSV(string KKCSVName)
        {
            // set all data accordingly
            Your_call = "your call here";
            SampleRate = 48000;
            stepSize.Text = "10Hz";
            Mode.Text = "USB";
            VolumeTrackBar.Value = 8;

            SafeTrackBarValueChange(43, AGCTrackBar);
            AGCSpeed.Text = "Long";
            AGCFixedGainDB = 20.0;
            AGCMaximumGainDB = 50.0;
            AGCHangThreshold = 50;
            AGCSlope = 0.0;
            UserAGCHangTime = 1.0;    // 1 ms
            UserAGCAttackTime = 10.0;
            UserAGCDecayTime = 10.0;

            BandwidthTrackBar.Value = 3382;
            set_frequency_160 = 1836056;
            set_frequency_80 = 3635928;
            set_frequency_40 = 7135010;
            set_frequency_30 = 10120082;
            set_frequency_20 = 14266100;
            set_frequency_17 = 18091270;
            set_frequency_15 = 21045856;
            set_frequency_12 = 24950010;
            set_frequency_10 = 28040534;
            set_frequency_6 = 50257000;
            set_frequency_GC = 16998470;
            set_frequency.Value = 14266100;
            BandSelect.Text = "40m";
            ANF.Checked = false;
            NR.Checked = false;
            NB1.Checked = false;
            NB2.Checked = false;
            rcvr.BlockNBThreshold = 8.8F;
            rcvr.AveNBThreshold = 9.0F;
            rcvr.PowerSpectrumAveragingOn = true;
            rcvr.PowerSpectrumSmoothingFactor = 0.6F;
            Rate = 15;
            Preamp_160 = false;
            Preamp_80 = false;
            Preamp_40 = false;
            Preamp_30 = false;
            Preamp_20 = false;
            Preamp_17 = false;
            Preamp_15 = false;
            Preamp_10 = false;
            Preamp_6 = false;
            Preamp_GC = false;

            KKDevice = KKMethod.Ethernet;
            Alex = false;
            Apollo = false;

            // 65 is the default value for AlexState. It corresponds to TXAnt1 (64) | RXAnt1 (1) = 65
            Alex160mState = DefaultAlexState;
            Alex80mState = DefaultAlexState;
            Alex60mState = DefaultAlexState;
            Alex40mState = DefaultAlexState;
            Alex30mState = DefaultAlexState;
            Alex20mState = DefaultAlexState;
            Alex17mState = DefaultAlexState;
            Alex15mState = DefaultAlexState;
            Alex12mState = DefaultAlexState;
            Alex10mState = DefaultAlexState;
            Alex6mState = DefaultAlexState;
            AlexGCState = DefaultAlexState;

            SkipVersionCheck = false;
            Allow16kWidebandSamples = true;
            DoFastEthernetConnect = false;
            EthernetHostIPAddress = "";
            Metis_IP_address = "";

            // create the file
            WriteKKCSV(KKCSVName);
        }

        private void WriteKKCSV(string KKCSVName)
        {
            // save the current settings to KK.csv
            List<string> lines = new List<string>();        // create a List<string> to hold all the output lines

            // each line of the output file, in output order
            lines.Add("Users Call," + Your_call);
            lines.Add("KKDevice," + ((int)KKDevice).ToString());
            lines.Add("Band," + BandSelect.Text);
            lines.Add("Sample Rate," + SampleRate);
            lines.Add("Step Size," + stepSize.Text);
            lines.Add("Mode," + Mode.Text);
            lines.Add("Volume," + VolumeTrackBar.Value);
            lines.Add("Bandwidth," + BandwidthTrackBar.Value);
            lines.Add("Last Freq 160m," + set_frequency_160);
            lines.Add("Last Freq 80m," + set_frequency_80);
            lines.Add("Last Freq 40m," + set_frequency_40);
            lines.Add("Last Freq 30m," + set_frequency_30);
            lines.Add("Last Freq 20m," + set_frequency_20);
            lines.Add("Last Freq 17m," + set_frequency_17);
            lines.Add("Last Freq 15m," + set_frequency_15);
            lines.Add("Last Freq 12m," + set_frequency_12);
            lines.Add("Last Freq 10m," + set_frequency_10);
            lines.Add("Last Freq 6m," + set_frequency_6);
            lines.Add("Last Freq GC," + set_frequency_GC);
            lines.Add("Last frequency," + set_frequency.Value);
            // we need to store the following controls as true or false, first we convert from bool to a string
            lines.Add("ANF," + (ANF.Checked).ToString());
            lines.Add("NR," + (NR.Checked).ToString());
            lines.Add("NB1," + (NB1.Checked).ToString());
            lines.Add("NB2," + (NB2.Checked).ToString());
            lines.Add("NB1 Threshold," + rcvr.BlockNBThreshold.ToString(nfi)); // use nfi to make floats region independent 
            lines.Add("NB2 Threshold," + rcvr.AveNBThreshold.ToString(nfi));
            lines.Add("Bandscope Average," + rcvr.PowerSpectrumAveragingOn.ToString());
            lines.Add("Bandscope Smooth," + rcvr.PowerSpectrumSmoothingFactor.ToString(nfi));
            lines.Add("Rate," + Rate.ToString());
            // save the preamp setting for each band
            lines.Add("Preamp 160," + Preamp_160.ToString());
            lines.Add("Preamp 80," + Preamp_80.ToString());
            lines.Add("Preamp 40," + Preamp_40.ToString());
            lines.Add("Preamp 30," + Preamp_30.ToString());
            lines.Add("Preamp 20," + Preamp_20.ToString());
            lines.Add("Preamp 17," + Preamp_17.ToString());
            lines.Add("Preamp 15," + Preamp_15.ToString());
            lines.Add("Preamp 12," + Preamp_12.ToString());
            lines.Add("Preamp 10," + Preamp_10.ToString());
            lines.Add("Preamp 6," + Preamp_6.ToString());
            lines.Add("Preamp GC," + Preamp_GC.ToString());
            // end of original kk.csv file. beginning of values that LookupKKCSVValue should be used on
            lines.Add("ANFAdaptiveFilterSize," + ANFAdaptiveFilterSize.ToString(nfi));
            lines.Add("ANFDelay," + ANFDelay.ToString(nfi));
            lines.Add("ANFAdaptationRate," + ANFAdaptationRate.ToString(nfi));
            lines.Add("ANFLeakage," + ANFLeakage.ToString(nfi));
            lines.Add("NRAdaptiveFilterSize," + NRAdaptiveFilterSize.ToString(nfi));
            lines.Add("NRDelay," + NRDelay.ToString(nfi));
            lines.Add("NRAdaptationRate," + NRAdaptationRate.ToString(nfi));
            lines.Add("NRLeakage," + NRLeakage.ToString(nfi));
            lines.Add("Bandscope Grid Max," + GridMax.ToString(nfi));
            lines.Add("Bandscope Grid Min," + GridMin.ToString(nfi));
            lines.Add("Bandscope Grid Step," + GridStep.ToString(nfi));

            lines.Add("AGC Speed," + AGCSpeed.Text);
            lines.Add("AGC Fixed Gain," + AGCFixedGainDB.ToString(nfi));
            lines.Add("AGC Hang Threshold," + AGCHangThreshold.ToString(nfi));
            lines.Add("AGC Max Gain," + AGCMaximumGainDB.ToString(nfi));
            lines.Add("AGC Slope," + AGCSlope.ToString(nfi));
            lines.Add("AGC Attack Time," + UserAGCAttackTime.ToString(nfi));    // only variable for 'user' AGC
            lines.Add("AGC Decay Time," + UserAGCDecayTime.ToString(nfi));    // only variable for 'user' AGC
            lines.Add("AGC Hang Time," + UserAGCHangTime.ToString(nfi));    // only variable for 'user' AGC

            lines.Add("WaterfallHigh," + WaterfallHighThreshold.ToString(nfi));
            lines.Add("WaterfallLow," + WaterfallLowThreshold.ToString(nfi));
            lines.Add("WaterFall AGC," + WaterfallAGC.ToString(nfi));

            lines.Add("Squelch level," + Squelch_level.Value.ToString(nfi));

            lines.Add("Hermes," + Hermes.ToString(nfi));
            lines.Add("PennyLane," + PennyLane.ToString(nfi));
            lines.Add("Penny Present," + PenneyPresent.ToString(nfi));
            lines.Add("Mic Gain 20dB," + MicGain20dB.ToString(nfi));
            lines.Add("Atlas 10MHz," + Atlas10MHz.ToString(nfi));
            lines.Add("Mercury 10MHz," + Mercury10MHz.ToString(nfi));
            lines.Add("Penelope 10MHz," + Penelope10MHz.ToString(nfi));
            lines.Add("Excalibur Present," + Excalibur.ToString(nfi));

            lines.Add("Tx Filter High," + TxFilterHigh.ToString(nfi));
            lines.Add("Tx Filter Low," + TxFilterLow.ToString(nfi));

            lines.Add("Drive Level," + DriveLevel.Value.ToString(nfi));

            lines.Add("Band Gain 160m," + Gain160m.ToString(nfi));
            lines.Add("Band Gain 80m," + Gain80m.ToString(nfi));
            lines.Add("Band Gain 60m," + Gain60m.ToString(nfi));
            lines.Add("Band Gain 40m," + Gain40m.ToString(nfi));
            lines.Add("Band Gain 30m," + Gain30m.ToString(nfi));
            lines.Add("Band Gain 20m," + Gain20m.ToString(nfi));
            lines.Add("Band Gain 17m," + Gain17m.ToString(nfi));
            lines.Add("Band Gain 15m," + Gain15m.ToString(nfi));
            lines.Add("Band Gain 12m," + Gain12m.ToString(nfi));
            lines.Add("Band Gain 10m," + Gain10m.ToString(nfi));
            lines.Add("Band Gain 6m," + Gain6m.ToString(nfi));

            lines.Add("Full Duplex," + Duplex.ToString(nfi));
            lines.Add("Only Tx on PTT," + OnlyTxOnPTT.ToString(nfi));

            lines.Add("Tune Level," + TuneLevel.ToString(nfi));
            lines.Add("CWPitch," + CWPitch.ToString(nfi));

            lines.Add("Penny OC Enable," + PennyOC.ToString(nfi));
            lines.Add("Penny OC 160mTx," + Penny160mTxOC.ToString(nfi));
            lines.Add("Penny OC 80mTx," + Penny80mTxOC.ToString(nfi));
            lines.Add("Penny OC 60mTx," + Penny60mTxOC.ToString(nfi));
            lines.Add("Penny OC 40mTx," + Penny40mTxOC.ToString(nfi));
            lines.Add("Penny OC 30mTx," + Penny30mTxOC.ToString(nfi));
            lines.Add("Penny OC 20mTx," + Penny20mTxOC.ToString(nfi));
            lines.Add("Penny OC 17mTx," + Penny17mTxOC.ToString(nfi));
            lines.Add("Penny OC 15mTx," + Penny15mTxOC.ToString(nfi));
            lines.Add("Penny OC 12mTx," + Penny12mTxOC.ToString(nfi));
            lines.Add("Penny OC 10mTx," + Penny10mTxOC.ToString(nfi));
            lines.Add("Penny OC 6mTx," + Penny6mTxOC.ToString(nfi));

            lines.Add("Penny OC 160mRx," + Penny160mRxOC.ToString(nfi));
            lines.Add("Penny OC 80mRx," + Penny80mRxOC.ToString(nfi));
            lines.Add("Penny OC 60mRx," + Penny60mRxOC.ToString(nfi));
            lines.Add("Penny OC 40mRx," + Penny40mRxOC.ToString(nfi));
            lines.Add("Penny OC 30mRx," + Penny30mRxOC.ToString(nfi));
            lines.Add("Penny OC 20mRx," + Penny12mRxOC.ToString(nfi));
            lines.Add("Penny OC 17mRx," + Penny17mRxOC.ToString(nfi));
            lines.Add("Penny OC 15mRx," + Penny15mRxOC.ToString(nfi));
            lines.Add("Penny OC 12mRx," + Penny12mRxOC.ToString(nfi));
            lines.Add("Penny OC 10mRx," + Penny10mRxOC.ToString(nfi));
            lines.Add("Penny OC 6mRx," + Penny6mRxOC.ToString(nfi));

            lines.Add("DelayRF," + DelayRF.ToString(nfi));
            lines.Add("DelayPTT," + DelayPTT.ToString(nfi));

            lines.Add("FM deviation," + FM_deviation.ToString(nfi));
            lines.Add("Speech Processor," + chkClipper.Checked.ToString(nfi));
            lines.Add("Processor Gain," + ProcessorGain.Value.ToString(nfi));
            lines.Add("Bass Cut," + chkBassCut.Checked.ToString(nfi));

            lines.Add("VOX On," + chkVOX.Checked.ToString(nfi));
            lines.Add("VOX Level," + VOXLevel.Value.ToString(nfi));
            VOXHang = (int)VOXHangTime.Value;
            lines.Add("VOX Hang," + VOXHang.ToString(nfi));
            lines.Add("Mic Gain," + MicrophoneGain.Value.ToString(nfi));
            lines.Add("Processor Gain," + ProcessorGain.Value.ToString(nfi));
            lines.Add("Line In," + LineIn.ToString(nfi));
            lines.Add("Last MAC," + MetisMAC);
            lines.Add("Mic AGC," + chkMicAGC.Checked.ToString(nfi));
            lines.Add("Noise Gate," + chkNoiseGate.Checked.ToString(nfi));
            lines.Add("Noise Gate Level," + NoiseGateLevel.Value.ToString(nfi));
            lines.Add("Alex," + Alex.ToString(nfi));
            lines.Add("Apollo," + Apollo.ToString(nfi));

            lines.Add("Alex160mState," + Alex160mState.ToString(nfi));
            lines.Add("Alex80mState," + Alex80mState.ToString(nfi));
            lines.Add("Alex60mState," + Alex60mState.ToString(nfi));
            lines.Add("Alex40mState," + Alex40mState.ToString(nfi));
            lines.Add("Alex30mState," + Alex30mState.ToString(nfi));
            lines.Add("Alex20mState," + Alex20mState.ToString(nfi));
            lines.Add("Alex17mState," + Alex17mState.ToString(nfi));
            lines.Add("Alex15mState," + Alex15mState.ToString(nfi));
            lines.Add("Alex12mState," + Alex12mState.ToString(nfi));
            lines.Add("Alex10mState," + Alex10mState.ToString(nfi));
            lines.Add("Alex6mState," + Alex6mState.ToString(nfi));
            lines.Add("AlexGCState," + AlexGCState.ToString(nfi));
            lines.Add("SkipVersionCheck," + SkipVersionCheck.ToString(nfi));    // convert bool to 'true'/'false'
            lines.Add("Allow16kWidebandSamples," + Allow16kWidebandSamples.ToString(nfi));    // convert bool to 'true'/'false'
            lines.Add("DoFastEthernetConnect," + DoFastEthernetConnect.ToString(nfi));    // convert bool to 'true'/'false'
            lines.Add("EthernetHostIPAddress," + EthernetHostIPAddress.ToString(nfi));  // string
            lines.Add("Metis_IP_address," + Metis_IP_address.ToString(nfi));            // string

            // write all the lines
            File.WriteAllLines(KKCSVName, lines.ToArray());
        }

        public string LookupKKCSVValue(string key, string defaultValue, List<string> value)
        {
            for (int i = 0; i < value.Count; i += 2)
            {
                string s = value[i];
                // search all the even index entries for a key that matches.  If there is one, return
                // the next list entry
                if (s.Equals(key, StringComparison.InvariantCulture))
                {
                    // they match.  return the next element in value
                    return value[i + 1];
                }
            }

            // not found.  Return default value instead
            return defaultValue;
        }

        public double LookupKKCSVValue(string key, double defaultValue, List<string> value)
        {
            for (int i = 0; i < value.Count; i += 2)
            {
                string s = value[i];
                // search all the even index entries for a key that matches.  If there is one, return
                // the next list entry
                if (s.Equals(key, StringComparison.InvariantCulture))
                {
                    // they match.  return the next element in value
                    try
                    {
                        return (float)Convert.ToDouble(value[i + 1], nfi);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }

            // not found.  Return default value instead
            return defaultValue;
        }

        public int LookupKKCSVValue(string key, int defaultValue, List<string> value)
        {
            for (int i = 0; i < value.Count; i += 2)
            {
                string s = value[i];
                // search all the even index entries for a key that matches.  If there is one, return
                // the next list entry
                if (s.Equals(key, StringComparison.InvariantCulture))
                {
                    // they match.  return the next element in value
                    try
                    {
                        return Convert.ToInt32(value[i + 1], nfi);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }

            // not found.  Return default value instead
            return defaultValue;
        }

        public bool LookupKKCSVValue(string key, bool defaultValue, List<string> value)
        {
            for (int i = 0; i < value.Count; i += 2)
            {
                string s = value[i];
                // search all the even index entries for a key that matches.  If there is one, return
                // the next list entry
                if (s.Equals(key, StringComparison.InvariantCulture))
                {
                    // they match.  return the next element in value
                    try
                    {
                        return Convert.ToBoolean(value[i + 1], nfi);
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }

            // not found.  Return default value instead
            return defaultValue;
        }

        public void ReadKKCSV(string KKCSVName)
        {
            // read the file into an array so we can process each entry
            var lines = File.ReadAllLines(KKCSVName);
            List<string> value = new List<string>();

            // process the string array of data that was read
            foreach (string text in lines)
            {
                /* This gets around the problem of the locale of the user as far as saving floating point
                 * values which might (because of locale) have commas in the numbers, thus creating
                 * more entries in the array than desired, because of mis-parsing based on the extra commas.
                 * We also get to use a List<string> so that we don't need to know how big it is before we process
                 * data.  It indexes the same as 'string[] value' did.
                 */
                int p = text.IndexOf(",");      // search for the comma after the data element name
                if (p != -1)
                {
                    // we found a comma.  Split the line into 'before' and 'after' the comma
                    // before the comma is the data name
                    // after the comma is its value
                    value.Add(text.Substring(0, p));
                    value.Add(text.Substring(p + 1));
                }
            }

            // Assign the values in KK.csv to their respective variables
            //TODO: Save radio state on a per-band basis

            Your_call = LookupKKCSVValue("Users Call", Your_call, value);

            // get what type last run used (Ethernet or USB)
            int kkDevice = LookupKKCSVValue("KKDevice", (int)KKDevice, value);
            try
            {
                KKDevice = (KKMethod)kkDevice;
            }
            catch
            {
                KKDevice = KKMethod.Ethernet;
            }

            // is it by design, or an error, that BandSelect.Text isn't set to this value?
            BandText = LookupKKCSVValue("Band", BandText, value);
            SampleRate = LookupKKCSVValue("Sample Rate", SampleRate, value);
            stepSize.Text = LookupKKCSVValue("Step Size", stepSize.Text, value);

            // this next line forces ChangeMode to be called, which sets ModeText
            Mode.Text = LookupKKCSVValue("Mode", Mode.Text, value);
            SafeTrackBarValueChange( LookupKKCSVValue("Volume", VolumeTrackBar.Value, value), VolumeTrackBar);

            UserAGCAttackTime = LookupKKCSVValue("AGC Attack Time", UserAGCAttackTime, value);
            UserAGCDecayTime = LookupKKCSVValue("AGC Decay Time", UserAGCDecayTime, value);
            UserAGCHangTime = LookupKKCSVValue("AGC Hang Time", UserAGCHangTime, value);
            AGCSpeed.Text = LookupKKCSVValue("AGC Speed", AGCSpeed.Text, value);
            AGCMaximumGainDB = LookupKKCSVValue("AGC Max Gain", AGCMaximumGainDB, value);
            AGCFixedGainDB = LookupKKCSVValue("AGC Fixed Gain", AGCFixedGainDB, value);
            SafeTrackBarValueChange((int)AGCMaximumGainDB, AGCTrackBar);
            AGCHangThreshold = LookupKKCSVValue("AGC Hang Threshold", AGCHangThreshold, value);
            AGCSlope = LookupKKCSVValue("AGC Slope", AGCSlope, value);

            SafeTrackBarValueChange( LookupKKCSVValue("Bandwidth", BandwidthTrackBar.Value, value), BandwidthTrackBar);

            // use last set frequency when starting
            set_frequency_160 = LookupKKCSVValue("Last Freq 160m", set_frequency_160, value);
            set_frequency_80 = LookupKKCSVValue("Last Freq 80m", set_frequency_80, value);
            set_frequency_40 = LookupKKCSVValue("Last Freq 40m", set_frequency_40, value);
            set_frequency_30 = LookupKKCSVValue("Last Freq 30m", set_frequency_30, value);
            set_frequency_20 = LookupKKCSVValue("Last Freq 20m", set_frequency_20, value);
            set_frequency_17 = LookupKKCSVValue("Last Freq 17m", set_frequency_17, value);
            set_frequency_15 = LookupKKCSVValue("Last Freq 15m", set_frequency_15, value);
            set_frequency_12 = LookupKKCSVValue("Last Freq 12m", set_frequency_12, value);
            set_frequency_10 = LookupKKCSVValue("Last Freq 10m", set_frequency_10, value);
            set_frequency_6 = LookupKKCSVValue("Last Freq 6m", set_frequency_6, value);
            set_frequency_GC = LookupKKCSVValue("Last Freq GC", set_frequency_GC, value);
            SafeTrackBarValueChange( LookupKKCSVValue("Last frequency", set_frequency.Value, value), set_frequency);

            ANF.Checked = LookupKKCSVValue("ANF", ANF.Checked, value);
            NR.Checked = LookupKKCSVValue("NR", NR.Checked, value);
            NB1.Checked = LookupKKCSVValue("NB1", NB1.Checked, value);
            NB2.Checked = LookupKKCSVValue("NB2", NB2.Checked, value);
            rcvr.BlockNBThreshold = (float)LookupKKCSVValue("NB1 Threshold", rcvr.BlockNBThreshold, value);
            rcvr.AveNBThreshold = (float)LookupKKCSVValue("NB2 Threshold", rcvr.AveNBThreshold, value);
            rcvr.PowerSpectrumAveragingOn = LookupKKCSVValue("Bandscope Average", rcvr.PowerSpectrumAveragingOn, value);
            rcvr.PowerSpectrumSmoothingFactor = (float)LookupKKCSVValue("Bandscope Smooth", rcvr.PowerSpectrumSmoothingFactor, value);
            Rate = LookupKKCSVValue("Rate", Rate, value);

            // save the preamp setting for each band
            Preamp_160 = LookupKKCSVValue("Preamp 160", Preamp_160, value);
            Preamp_80 = LookupKKCSVValue("Preamp 80", Preamp_80, value);
            Preamp_40 = LookupKKCSVValue("Preamp 40", Preamp_40, value);
            Preamp_30 = LookupKKCSVValue("Preamp 30", Preamp_30, value);
            Preamp_20 = LookupKKCSVValue("Preamp 20", Preamp_20, value);
            Preamp_17 = LookupKKCSVValue("Preamp 17", Preamp_17, value);
            Preamp_15 = LookupKKCSVValue("Preamp 15", Preamp_15, value);
            Preamp_12 = LookupKKCSVValue("Preamp 12", Preamp_12, value);
            Preamp_10 = LookupKKCSVValue("Preamp 10", Preamp_10, value);
            Preamp_6 = LookupKKCSVValue("Preamp 6", Preamp_6, value);
            Preamp_GC = LookupKKCSVValue("Preamp GC", Preamp_GC, value);

            ANFAdaptiveFilterSize = LookupKKCSVValue("ANFAdaptiveFilterSize", ANFAdaptiveFilterSize, value);
            ANFDelay = LookupKKCSVValue("ANFDelay", ANFDelay, value);
            ANFAdaptationRate = (float)LookupKKCSVValue("ANFAdaptationRate", ANFAdaptationRate, value);
            ANFLeakage = (float)LookupKKCSVValue("ANFLeakage", ANFLeakage, value);

            NRAdaptiveFilterSize = LookupKKCSVValue("NRAdaptiveFilterSize", NRAdaptiveFilterSize, value);
            NRDelay = LookupKKCSVValue("NRDelay", NRDelay, value);
            NRAdaptationRate = (float)LookupKKCSVValue("NRAdaptationRate", NRAdaptationRate, value);
            NRLeakage = (float)LookupKKCSVValue("NRLeakage", NRLeakage, value);

            GridMax = LookupKKCSVValue("Bandscope Grid Max", GridMax, value);
            GridMin = LookupKKCSVValue("Bandscope Grid Min", GridMin, value);
            GridStep = LookupKKCSVValue("Bandscope Grid Step", GridStep, value);

            WaterfallHighThreshold = LookupKKCSVValue("WaterfallHigh", WaterfallHighThreshold, value);
            WaterfallLowThreshold = LookupKKCSVValue("WaterfallLow", WaterfallLowThreshold, value);
            WaterfallAGC = LookupKKCSVValue("WaterFall AGC", WaterfallAGC, value);

            SafeTrackBarValueChange( LookupKKCSVValue("Squelch level", Squelch_level.Value, value), Squelch_level);
            Hermes = LookupKKCSVValue("Hermes", Hermes, value);
            PennyLane = LookupKKCSVValue("PennyLane", PennyLane, value);
            PenneyPresent = LookupKKCSVValue("Penny Present", PenneyPresent, value);
            MicGain20dB = LookupKKCSVValue("Mic Gain 20dB", MicGain20dB, value);

            Atlas10MHz = LookupKKCSVValue("Atlas 10MHz", Atlas10MHz, value);
            Mercury10MHz = LookupKKCSVValue("Mercury 10MHz", Mercury10MHz, value);
            Penelope10MHz = LookupKKCSVValue("Penelope 10MHz", Penelope10MHz, value);
            Excalibur = LookupKKCSVValue("Excalibur Present", Excalibur, value);

            TxFilterHigh = (float)LookupKKCSVValue("Tx Filter High", TxFilterHigh, value);
            TxFilterLow = (float)LookupKKCSVValue("Tx Filter Low", TxFilterLow, value);

            SafeTrackBarValueChange( LookupKKCSVValue("Drive Level", DriveLevel.Value, value), DriveLevel);

            Gain160m = LookupKKCSVValue("Band Gain 160m", Gain160m, value);
            Gain80m = LookupKKCSVValue("Band Gain 80m", Gain80m, value);
            Gain60m = LookupKKCSVValue("Band Gain 60m", Gain60m, value);
            Gain40m = LookupKKCSVValue("Band Gain 40m", Gain40m, value);
            Gain30m = LookupKKCSVValue("Band Gain 30m", Gain30m, value);
            Gain20m = LookupKKCSVValue("Band Gain 20m", Gain20m, value);
            Gain17m = LookupKKCSVValue("Band Gain 17m", Gain17m, value);
            Gain15m = LookupKKCSVValue("Band Gain 15m", Gain15m, value);
            Gain12m = LookupKKCSVValue("Band Gain 12m", Gain12m, value);
            Gain10m = LookupKKCSVValue("Band Gain 10m", Gain10m, value);
            Gain6m = LookupKKCSVValue("Band Gain 6m", Gain6m, value);

            Duplex = LookupKKCSVValue("Full Duplex", Duplex, value);
            OnlyTxOnPTT = LookupKKCSVValue("Only Tx on PTT", OnlyTxOnPTT, value);

            TuneLevel = LookupKKCSVValue("Tune Level", TuneLevel, value);

            CWPitch = LookupKKCSVValue("CWPitch", CWPitch, value);

            PennyOC = LookupKKCSVValue("Penny OC Enable", PennyOC, value);

            Penny160mTxOC = LookupKKCSVValue("Penny OC 160mTx", Penny160mTxOC, value);
            Penny80mTxOC = LookupKKCSVValue("Penny OC 80mTx", Penny80mTxOC, value);
            Penny60mTxOC = LookupKKCSVValue("Penny OC 60mTx", Penny60mTxOC, value);
            Penny40mTxOC = LookupKKCSVValue("Penny OC 40mTx", Penny40mTxOC, value);
            Penny30mTxOC = LookupKKCSVValue("Penny OC 30mTx", Penny30mTxOC, value);
            Penny20mTxOC = LookupKKCSVValue("Penny OC 20mTx", Penny20mTxOC, value);
            Penny17mTxOC = LookupKKCSVValue("Penny OC 17mTx", Penny17mTxOC, value);
            Penny15mTxOC = LookupKKCSVValue("Penny OC 15mTx", Penny15mTxOC, value);
            Penny12mTxOC = LookupKKCSVValue("Penny OC 12mTx", Penny12mTxOC, value);
            Penny10mTxOC = LookupKKCSVValue("Penny OC 10mTx", Penny10mTxOC, value);
            Penny6mTxOC = LookupKKCSVValue("Penny OC 6mTx", Penny6mTxOC, value);

            Penny160mRxOC = LookupKKCSVValue("Penny OC 160mRx", Penny160mRxOC, value);
            Penny80mRxOC = LookupKKCSVValue("Penny OC 80mRx", Penny80mRxOC, value);
            Penny60mRxOC = LookupKKCSVValue("Penny OC 60mRx", Penny60mRxOC, value);
            Penny40mRxOC = LookupKKCSVValue("Penny OC 40mRx", Penny40mRxOC, value);
            Penny30mRxOC = LookupKKCSVValue("Penny OC 30mRx", Penny30mRxOC, value);
            Penny20mRxOC = LookupKKCSVValue("Penny OC 20mRx", Penny20mRxOC, value);
            Penny17mRxOC = LookupKKCSVValue("Penny OC 17mRx", Penny17mRxOC, value);
            Penny15mRxOC = LookupKKCSVValue("Penny OC 15mRx", Penny15mRxOC, value);
            Penny12mRxOC = LookupKKCSVValue("Penny OC 12mRx", Penny12mRxOC, value);
            Penny10mRxOC = LookupKKCSVValue("Penny OC 10mRx", Penny10mRxOC, value);
            Penny6mRxOC = LookupKKCSVValue("Penny OC 6mRx", Penny6mRxOC, value);

            DelayRF = LookupKKCSVValue("DelayRF", DelayRF, value);
            DelayPTT = LookupKKCSVValue("DelayPTT", DelayPTT, value);

            FM_deviation = LookupKKCSVValue("FM deviation", FM_deviation, value);

            chkClipper.Checked = LookupKKCSVValue("Speech Processor", chkClipper.Checked, value);
            SafeTrackBarValueChange( LookupKKCSVValue("Processor Gain", ProcessorGain.Value, value), ProcessorGain);
            chkBassCut.Checked = LookupKKCSVValue("Bass Cut", chkBassCut.Checked, value);

            chkVOX.Checked = LookupKKCSVValue("VOX On", chkVOX.Checked, value);
            SafeTrackBarValueChange( LookupKKCSVValue("VOX Level", VOXLevel.Value, value), VOXLevel);
            SafeUpDownValueChange( LookupKKCSVValue("VOX Hang", VOXHang, value), VOXHangTime);
            SafeTrackBarValueChange( LookupKKCSVValue("Mic Gain", MicrophoneGain.Value, value), MicrophoneGain);
            SafeTrackBarValueChange( LookupKKCSVValue("Processor Gain", ProcessorGain.Value, value), ProcessorGain);
            LineIn = LookupKKCSVValue("Line In", LineIn, value);
            MetisMAC = LookupKKCSVValue("Last MAC", "", value);
            chkMicAGC.Checked = LookupKKCSVValue("Mic AGC", chkMicAGC.Checked, value);
            chkNoiseGate.Checked = LookupKKCSVValue("Noise Gate", chkNoiseGate.Checked, value);
            SafeTrackBarValueChange( LookupKKCSVValue("Noise Gate Level", NoiseGateLevel.Value, value), NoiseGateLevel);

            Alex = LookupKKCSVValue("Alex", Alex, value);
            Apollo = LookupKKCSVValue("Apollo", Apollo, value);

            // use the GetValidAlexState method so that defects in the retrieved value
            // can be fixed.  Common defects include getting a value of 0,
            // or other value where TX or RX antenna selector is 0.  If such a defect
            // is found, cause the TX or RX antenna selector to be 1.
            // This turns a value of 0 that is read in to '65' (decimal), which is a
            // 1 for Tx (64) and a 1 for Rx (1).
            GetValidAlexState("Alex160mState", ref Alex160mState, value);
            GetValidAlexState("Alex80mState", ref Alex80mState, value);
            GetValidAlexState("Alex60mState", ref Alex60mState, value);
            GetValidAlexState("Alex40mState", ref Alex40mState, value);
            GetValidAlexState("Alex30mState", ref Alex30mState, value);
            GetValidAlexState("Alex20mState", ref Alex20mState, value);
            GetValidAlexState("Alex17mState", ref Alex17mState, value);
            GetValidAlexState("Alex15mState", ref Alex15mState, value);
            GetValidAlexState("Alex12mState", ref Alex12mState, value);
            GetValidAlexState("Alex10mState", ref Alex10mState, value);
            GetValidAlexState("Alex6mState", ref Alex6mState, value);
            GetValidAlexState("AlexGCState", ref AlexGCState, value);

            SkipVersionCheck = LookupKKCSVValue("SkipVersionCheck", SkipVersionCheck, value);   // bool
            Allow16kWidebandSamples = LookupKKCSVValue("Allow16kWidebandSamples", Allow16kWidebandSamples, value);   // bool
            DoFastEthernetConnect = LookupKKCSVValue("DoFastEthernetConnect", DoFastEthernetConnect, value);   // bool
            EthernetHostIPAddress = LookupKKCSVValue("EthernetHostIPAddress", EthernetHostIPAddress, value);   // bool
            Metis_IP_address = LookupKKCSVValue("Metis_IP_address", Metis_IP_address, value);   // bool
        }

        void GetValidAlexState(string name, ref int state, List<string> value)
        {
            state = LookupKKCSVValue(name, state, value);
            if ((state & 0x3) == 0)
            {
                // if rx antenna selector is 0, set it to 1
                state |= 1;
            }
            if (((state >> 6) & 0x3) == 0)
            {
                // if tx antenna selector is 0, set it to 1
                state |= 64;
            }
        }
    }
}
