/*
 * K.I.S.S. Konsole for High Performance Software Defined Radio
 *
 * 
 * Copyright (C) 2009 Phil Harman, VK6APH
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 * 
 *  V1.0.0 
 */

/*
 * Change log 
 * 
 * 20 May 2009 - Added Sampling Rate control
 * 21 May 2009 - Added Noise Blanker Thresholds
 * 22 May 2009 - Added Bandscope Controls
 * 23 May 2009 - Added IQ Gain control
 * 30 Jun 2010 - Added Network Adapter selection
 * 14 Feb 2011 - Added DelayPTT control
 * 
 * 
 * 
 * TODO: Remove focus from control once used
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;   // use View > Output to see debug messages


namespace KISS_Konsole
{
    public partial class SetupForm : Form
    {
        Form1 MainForm;             // reference to Form1

        bool suspendUpdate = false;

        public void SetHPSDRVersions()
        {
            FX2CodeVersion.Text = MainForm.Ozy_version;
            MercuryCodeVersion.Text = "V" + ((float)MainForm.Merc_version / 10f).ToString();   // Version number of Mercury FPGA code
            PenelopeCodeVersion.Text = "V" + ((float)MainForm.Penny_version / 10f).ToString(); // Version number of Penelope FPGA code
            OzyCodeVersion.Text = "V" + ((float)MainForm.Ozy_FPGA_version / 10f).ToString();   // Version number of Ozy FPGA code
        }

        private void SafeUpDownValueChange(decimal newValue, NumericUpDown control)
        {
            if (newValue < control.Minimum)
                newValue = control.Minimum;
            if (newValue > control.Maximum)
                newValue = control.Maximum;

            control.Value = newValue;
        }

        private void SafeTrackBarValueChange(int newValue, TrackBar control)
        {
            if (newValue < control.Minimum)
                newValue = control.Minimum;
            if (newValue > control.Maximum)
                newValue = control.Maximum;

            control.Value = newValue;
        }

        public void ChangeAGCTimes()
        {
            SafeUpDownValueChange(Convert.ToDecimal(MainForm.AGCAttackTime), numericUpDownAGCAttackTime);
            SafeUpDownValueChange(Convert.ToDecimal(MainForm.AGCDecayTime), numericUpDownAGCDecayTime);
            SafeUpDownValueChange(Convert.ToDecimal(MainForm.AGCHangTime), numericUpDownAGCHangTime);
        }

        public bool CustomRXAGCEnabled
        {
            set
            {
                numericUpDownAGCAttackTime.Enabled = value;
                numericUpDownAGCDecayTime.Enabled = value;
                numericUpDownAGCHangTime.Enabled = value;

                if (value)
                {
                    numericUpDownAGCAttackTime_ValueChanged(this, EventArgs.Empty);
                    numericUpDownAGCDecayTime_ValueChanged(this, EventArgs.Empty);
                    numericUpDownAGCHangTime_ValueChanged(this, EventArgs.Empty);
                }
            }
        }

        public void ChangeFixedGain(double newFixedGain)
        {
            suspendUpdate = true;
            SafeUpDownValueChange(Convert.ToDecimal(newFixedGain), numericUpDownAGCFixedGain);
            suspendUpdate = false;
        }

        public void ChangeMaxGain(double newMaxGain)
        {
            suspendUpdate = true;
            SafeUpDownValueChange(Convert.ToDecimal(newMaxGain), numericUpDownAGCMaxGain);
            suspendUpdate = false;
        }

        public void ChangeHangThresh(int newValue)
        {
            suspendUpdate = true;
            SafeTrackBarValueChange(newValue, trackBarAGCHangThreshold);
            suspendUpdate = false;
        }

        public bool AGCHangEnabled
        {
            set
            {
                trackBarAGCHangThreshold.Enabled = value;
            }
        }

        public SetupForm(Form1 Parent) // get reference from Form1 so we know where variables are 
        {
            InitializeComponent();

            MainForm = Parent;

            suspendUpdate = true;

            // display Average  state, on/off
            this.Average.Checked = MainForm.rcvr.PowerSpectrumAveragingOn;  
            // display Smoothing Factor, convert from float 0.01 - 0.99 to integer 1 - 99
            float smooth = MainForm.rcvr.PowerSpectrumSmoothingFactor * 100f;
            this.Smoothing.Value = (int)smooth;
            if (Average.Checked)   // only enable Smoothing control if Average checked
                Smoothing.Enabled = true;
            else Smoothing.Enabled = false;
            // display Noise Blanker Thresholds, convert 0.1 - 20 to 1 - 200 to display on control
            float temp1 = MainForm.rcvr.BlockNBThreshold * 10f;
            float temp2 = MainForm.rcvr.AveNBThreshold * 10f;
            this.NB1Threshold.Value = (int)temp1;       // set NB1 Threshold, this forces control to run 
            this.NB2Threshold.Value = (int)temp2;       // set NB2 Threshold, this forces control to run
            // display Bandscope update rate 
            this.UpdateRate.Value = MainForm.Rate;
            // display IQScale setting 
            this.IQScale.Value = MainForm.IQScale;
            // set I display on/off
            Ion.Checked = MainForm.ShowI;
            // set Q display on/off
            Qon.Checked = MainForm.ShowQ;
            // set Waterfall AGC on/off
            WaterfallAGC.Checked = MainForm.WaterfallAGC;
            // set Mic Gain selected
            chkMicGain.Checked = MainForm.MicGain20dB;
            // set Atlas10MHz selected
            Atlas10MHz.Checked = MainForm.Atlas10MHz;
            // set Penelope10MHz selected
            Penelope10MHz.Checked = MainForm.Penelope10MHz;
            // set Mercury10MHz slected
            Mercury10MHz.Checked = MainForm.Mercury10MHz;
            // set Exchalibur selected
            chkExcalibur.Checked = MainForm.Excalibur;
            // set High filter value
            TxFilterHigh.Value = (int) MainForm.TxFilterHigh;
            // set Low filter value
            TxFilterLow.Value = (int)MainForm.TxFilterLow;

            // set the gains per band
            Gain160m.Value = MainForm.Gain160m;
            Gain80m.Value  = MainForm.Gain80m;
            Gain60m.Value  = MainForm.Gain60m;
            Gain40m.Value  = MainForm.Gain40m;
            Gain30m.Value  = MainForm.Gain30m;
            Gain20m.Value  = MainForm.Gain20m;
            Gain17m.Value  = MainForm.Gain17m;
            Gain15m.Value = MainForm.Gain15m;
            Gain12m.Value  = MainForm.Gain12m;
            Gain10m.Value  = MainForm.Gain10m;
            Gain6m.Value   = MainForm.Gain6m;

            // set Full Duplex state
            chkMuteRxOnPTT.Checked = !MainForm.Duplex;
            chkOnlyTxOnPTT.Checked = MainForm.OnlyTxOnPTT;

            // set Tune power percentage
            TuneLevel.Value = MainForm.TuneLevel;

            // display callsign
            textBoxCallSign.Text = MainForm.Your_call;

            // ANF filter
            numericUpDownANFAdaptiveFilterSize.Value = MainForm.ANFAdaptiveFilterSize;
            numericUpDownANFLeakage.Value = (decimal)MainForm.ANFLeakage;
            numericUpDownANFDelay.Value = MainForm.ANFDelay;
            numericUpDownANFAdaptationRate.Value = (decimal)MainForm.ANFAdaptationRate;

            // NR filter
            numericUpDownNRAdaptiveFilterSize.Value = MainForm.NRAdaptiveFilterSize;
            numericUpDownNRLeakage.Value = (decimal)MainForm.NRLeakage;
            numericUpDownNRDelay.Value = MainForm.NRDelay;
            numericUpDownNRAdaptationRate.Value = (decimal)MainForm.NRAdaptationRate;

            // Bandscope settings
            GridMax.Value = MainForm.GridMax;
            GridMin.Value = MainForm.GridMin;
            GridStep.Value = MainForm.GridStep;

            // AGC settings
            numericUpDownAGCSlope.Value = Convert.ToDecimal(MainForm.AGCSlope);
            numericUpDownAGCMaxGain.Value = Convert.ToDecimal(MainForm.AGCMaximumGainDB);
            ChangeAGCTimes();
            SafeTrackBarValueChange(MainForm.AGCHangThreshold, trackBarAGCHangThreshold);
            numericUpDownAGCFixedGain.Value = Convert.ToDecimal(MainForm.AGCFixedGainDB);
            AGCHangEnabled = MainForm.AGCHangEnabled;

            // Waterfall settings
            WaterFall_High.Value = Convert.ToDecimal(MainForm.WaterfallHighThreshold);
            WaterFall_Low.Value = Convert.ToDecimal(MainForm.WaterfallLowThreshold);

            //Penny Open Collector settings
            chkExtCtrlEnable.Checked = MainForm.PennyOC;

            // Penny Open Collector receive settings
            checkBoxesPenOC160mRX.Value = MainForm.Penny160mRxOC;
            checkBoxesPenOC80mRX.Value = MainForm.Penny80mRxOC;
            checkBoxesPenOC60mRX.Value = MainForm.Penny60mRxOC;
            checkBoxesPenOC40mRX.Value = MainForm.Penny40mRxOC;
            checkBoxesPenOC30mRX.Value = MainForm.Penny30mRxOC;
            checkBoxesPenOC20mRX.Value = MainForm.Penny20mRxOC;
            checkBoxesPenOC17mRX.Value = MainForm.Penny17mRxOC;
            checkBoxesPenOC15mRX.Value = MainForm.Penny15mRxOC;
            checkBoxesPenOC12mRX.Value = MainForm.Penny12mRxOC;
            checkBoxesPenOC10mRX.Value = MainForm.Penny10mRxOC;
            checkBoxesPenOC6mRX.Value = MainForm.Penny6mRxOC;

            // Penny Open Collector transmit settings
            checkBoxesPenOC160mTX.Value = MainForm.Penny160mTxOC;
            checkBoxesPenOC80mTX.Value = MainForm.Penny80mTxOC;
            checkBoxesPenOC60mTX.Value = MainForm.Penny60mTxOC;
            checkBoxesPenOC40mTX.Value = MainForm.Penny40mTxOC;
            checkBoxesPenOC30mTX.Value = MainForm.Penny30mTxOC;
            checkBoxesPenOC20mTX.Value = MainForm.Penny20mTxOC;
            checkBoxesPenOC17mTX.Value = MainForm.Penny17mTxOC;
            checkBoxesPenOC15mTX.Value = MainForm.Penny15mTxOC;
            checkBoxesPenOC12mTX.Value = MainForm.Penny12mTxOC;
            checkBoxesPenOC10mTX.Value = MainForm.Penny10mTxOC;
            checkBoxesPenOC6mTX.Value = MainForm.Penny6mTxOC;

            alexUserControl160m.Value = MainForm.Alex160mState;
            alexUserControl80m.Value = MainForm.Alex80mState;
            alexUserControl60m.Value = MainForm.Alex60mState;
            alexUserControl40m.Value = MainForm.Alex40mState;
            alexUserControl30m.Value = MainForm.Alex30mState;
            alexUserControl20m.Value = MainForm.Alex20mState;
            alexUserControl17m.Value = MainForm.Alex17mState;
            alexUserControl15m.Value = MainForm.Alex15mState;
            alexUserControl12m.Value = MainForm.Alex12mState;
            alexUserControl10m.Value = MainForm.Alex10mState;
            alexUserControl6m.Value = MainForm.Alex6mState;
            alexUserControlGC.Value = MainForm.AlexGCState;

            // Display Metis IP address
            UpdateEthernetInfo();
            cbAttemptFastConnect.Checked = MainForm.DoFastEthernetConnect;

            // display number of IP adaptes
            IP_adapters.Text = MainForm.numberOfIPAdapters;

            // display list of Network Interfaces
            listNetworkInterfaces.Text = MainForm.Network_interfaces;

            if (MainForm.KKDevice == KKMethod.USB)
            {
                // remove the ethernet page so it does not show
                tabControl1.TabPages.Remove(tabPage7);

                // disable the 'hermes' choice
                chkHermes.Enabled = false;
            }

            // set FM deviation value
            FM_deviation.Value = MainForm.FM_deviation;

            // set PTT Delay
            DelayPTT.Value = MainForm.DelayPTT;

            // set the checkboxes and enable the tab for alex if present
            chkAlex.Checked = MainForm.Alex;
            groupBoxAlex.Enabled = chkAlex.Checked;

            // Apollo should only be present if Hermes is present.
            if (chkHermes.Checked)
            {
                chkApollo.Checked = MainForm.Apollo;
            }

            
            // END OF SUSPENDED UPDATE ZONE!
            suspendUpdate = false;

            // set Penelope present
            chkPenelope.Checked = MainForm.PenneyPresent;
            // set PennyLane present
            chkPennyLane.Checked = MainForm.PennyLane;
            // We need to enable or disable relevant options if Penny is present so force update of control
            chkPenelope_CheckedChanged(this, EventArgs.Empty);
            // We need to enable or disable relevant options if PennyLane is present so force update of control
            chkPennyLane_CheckedChanged(this, EventArgs.Empty);

            if (MainForm.KKDevice != KKMethod.USB)
            {
                // set Hermes present
                chkHermes.Checked = MainForm.Hermes;
                // We need to enable or disable relevant options if Hermes is present so force update of control
                chkHermes_CheckedChanged(this, EventArgs.Empty);
            }

            // set Excalibur present
            chkExcalibur.Checked = MainForm.Excalibur;
            // We need to enable or disable relevant options if Excalibur is present so force update of control
            chkExcalibur_CheckedChanged(this, EventArgs.Empty);
            // set Line In selected
            chkLineIn.Checked = MainForm.LineIn;
            chkLineIn_CheckedChanged(this, EventArgs.Empty);

            checkBoxSkipVersionCheck.Checked = MainForm.SkipVersionCheck;
            checkBoxAllow16kWidebandSamples.Checked = MainForm.Allow16kWidebandSamples;

            SetHPSDRVersions();
        }

        public void UpdateEthernetInfo()
        {
            Metis_IP.Text = MainForm.Metis_IP_address;
            MetisMACaddressTextBox.Text = MainForm.MetisMAC;
            tbHostIP.Text = MainForm.EthernetHostIPAddress;
        }

        // convert selected SampleRate.Text to an integer and send to the MainForm & DSP 
        public void SampleRate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.SampleRate = Int32.Parse(this.SampleRate.Text); // 48000,96000 or 192000
            MainForm.rcvr.SampleRate = MainForm.SampleRate;      // change the DSP sample rate

            // set the bandwidth since dependent on sample rate
            MainForm.AdjustFilterByMode();
        }

        // Threshold value control is from 1 to 200, convert to float as 0.1 to 20
        private void NB1Threshold_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.rcvr.BlockNBThreshold = (float)((float)NB1Threshold.Value / 10f);
        }

        // Threshold value control is from 1 to 200, convert to float as 0.1 to 20
        private void NB2Threshold_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.rcvr.AveNBThreshold = (float)((float)NB2Threshold.Value / 10f);
        }

        // Turn Bandscope Averaging on/off
        private void Average_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.rcvr.PowerSpectrumAveragingOn = Average.Checked;
            if (Average.Checked)            // only display Smoothing control if Average is on 
                Smoothing.Enabled = true;
            else Smoothing.Enabled = false; 
        }

        // Set Bandscope Smoothing Factor as float 0.1 to 20
        private void Smoothing_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.rcvr.PowerSpectrumSmoothingFactor = (float)((float)Smoothing.Value / 100f); 
        }

        // Set Bandscope update rate 
        public void UpdateRate_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Rate = (int)UpdateRate.Value;
        }
        
        // Display I channel when checked
        public void Ion_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.ShowI = Ion.Checked;
        }

        // Display Q channel when checked
        private void Qon_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.ShowQ = Qon.Checked;
        }
        
        // Adjust gain of I&Q display 
        public void IQScale_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.IQScale = (int)this.IQScale.Value;
        }

        private void textBoxCallSign_TextChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Your_call = textBoxCallSign.Text;
            MainForm.UpdateFormTitle();
        }

        public void GridMax_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.GridMax = (int)this.GridMax.Value;            
        }

        public void GridMin_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.GridMin = (int)this.GridMin.Value;
        }

        public void GridStep_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.GridStep = (int)this.GridStep.Value;
        }

        private void numericUpDownANFAdaptiveFilterSize_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.ANFAdaptiveFilterSize = (int)numericUpDownANFAdaptiveFilterSize.Value;
        }

        private void numericUpDownANFDelay_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.ANFDelay = (int)numericUpDownANFDelay.Value;
        }

        private void numericUpDownANFAdaptationRate_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.ANFAdaptationRate = (float)numericUpDownANFAdaptationRate.Value;
        }

        private void numericUpDownANFLeakage_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.ANFLeakage = (float)numericUpDownANFLeakage.Value;
        }

        private void numericUpDownNRAdaptiveFilterSize_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.NRAdaptiveFilterSize = (int)numericUpDownNRAdaptiveFilterSize.Value;
        }

        private void numericUpDownNRDelay_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.NRDelay = (int)numericUpDownNRDelay.Value;
        }

        private void numericUpDownNRAdaptationRate_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.NRAdaptationRate = (float)numericUpDownNRAdaptationRate.Value;
        }

        private void numericUpDownNRLeakage_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.NRLeakage = (float)numericUpDownNRLeakage.Value;
        }

        private void numericUpDownAGCMaxGain_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.UpdateMaxGainAndSlider((double)numericUpDownAGCMaxGain.Value);
        }

        private void numericUpDownAGCHangTime_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.AGCHangTime = (float)numericUpDownAGCHangTime.Value;
        }

        private void numericUpDownAGCSlope_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.AGCSlope = (float)numericUpDownAGCSlope.Value;
        }

        private void numericUpDownAGCAttackTime_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.AGCAttackTime = (float)numericUpDownAGCAttackTime.Value;
        }

        private void numericUpDownAGCDecayTime_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.AGCDecayTime = (float)numericUpDownAGCDecayTime.Value;
        }

        private void trackBarAGCHangThreshold_Scroll(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.AGCHangThreshold = trackBarAGCHangThreshold.Value;
        }

        private void Setupform_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        public void WaterFall_Low_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.WaterfallLowThreshold = (int)WaterFall_Low.Value;
        }

        private void WaterFall_High_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.WaterfallHighThreshold = (int)WaterFall_High.Value;
        }

        // turn on Waterfall AGC when selected
        public void WaterfallAGC_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.WaterfallAGC = WaterfallAGC.Checked;
            if (WaterfallAGC.Checked)            // only display control if AGC is off 
                WaterFall_Low.Enabled = false;
            else WaterFall_Low.Enabled = true; 
        }


        // enable Hermes if selected and all relevant controls & disable Penny 
        private void chkHermes_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            if (chkHermes.Checked)
            {
                // disable selection of Penny and PennyLane
                chkPennyLane.Checked = false;
                chkPennyLane.Enabled = false;
                chkPenelope.Checked = false;
                chkPenelope.Enabled = false;
                chkExcalibur.Checked = false;
                Code_Version.Text = "Hermes";
            }

            // *** add Ozy/Magister or Metis selected here 
            else
            {
                // Hermes not selected so enable selection of Penelope and PennyLane
                chkPenelope.Enabled = true;
                chkPennyLane.Enabled = true;
                Code_Version.Text = "Metis";  
            }

            // Update Penny's status
            chkPenelope_CheckedChanged(this, EventArgs.Empty);
            // Update PennyLane's status
            chkPennyLane_CheckedChanged(this, EventArgs.Empty);
            // Update Excalibur's status 
            chkExcalibur_CheckedChanged(this, EventArgs.Empty);
            // Update Mic gain control
            chkMicGain_CheckedChanged(this, EventArgs.Empty);
            // Update ExtCtrlGroup
            chkExtCtrlEnable_CheckedChanged(this, EventArgs.Empty);
            // Update GainByBand
            GainByBandGroup_EnabledChanged(this, EventArgs.Empty);
            // Update Tx filter group
            TransmitFilterGroup_EnabledChanged(this, EventArgs.Empty);
            // Update Tune Group
            TuneGroup_EnabledChanged(this, EventArgs.Empty);
            // Update Rx 0n PTT enable 
            chkMuteRxOnPTT_EnabledChanged(this, EventArgs.Empty);
            // Update Tx on PTT enable 
            chkOnlyTxOnPTT_EnabledChanged(this, EventArgs.Empty);
            // let the mainform know of the changes of Hermes status
            MainForm.Hermes = chkHermes.Checked;
        }

        // enable Penelope if selected and all relevant controls, disable PennyLane
        public void chkPenelope_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            // if Penny is present then enable  relevant options
            if (chkPenelope.Checked)
            {
                Penelope10MHz.Enabled = true;
                //Code_Version.Text = "Ozy/Magister";
                chkPennyLane.Checked = false;      // disable PennyLane
            }
            else if (!chkPennyLane.Checked)        // Penny is not present so disable all relevant options
            {                                      // if PennyLane also not present
                Penelope10MHz.Checked = false;  // disable 10MHz clock from Penny
                Penelope10MHz.Enabled = false;
                PennyOC.Enabled = false;
            }
            // force update of Penny 10MHz clock selection
            Penelope10MHz_CheckedChanged(this, EventArgs.Empty);
            // Update mic status
            chkMicGain_CheckedChanged(this, EventArgs.Empty);
            // Update Excalibure's status 
            chkExcalibur_CheckedChanged(this, EventArgs.Empty);
            // Update ExtCtrlGroup
            chkExtCtrlEnable_CheckedChanged(this, EventArgs.Empty);
            // Update GainByBand
            GainByBandGroup_EnabledChanged(this, EventArgs.Empty);
            // Update Tx filter group
            TransmitFilterGroup_EnabledChanged(this, EventArgs.Empty);
            // Update Tune Group
            TuneGroup_EnabledChanged(this, EventArgs.Empty);
            // Update Rx 0n PTT enable 
            chkMuteRxOnPTT_EnabledChanged(this, EventArgs.Empty);
            // Update Tx on PTT enable 
            chkOnlyTxOnPTT_EnabledChanged(this, EventArgs.Empty);
            // let the mainform know of the changes of Penny status
            MainForm.PenneyPresent = chkPenelope.Checked;
        }

        // enable PennyLane if selected and all relevant controls, disable Penelope
        private void chkPennyLane_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            // if PennyLane is present then enable  relevant options
            if (chkPennyLane.Checked)
            {
                Penelope10MHz.Enabled = true;
                //Code_Version.Text = "Ozy/Magister";
                chkPenelope.Checked = false;   // disable Penelope
            }
            else if (!chkPenelope.Checked)     // PennyLane is not present so disable all relevant options
            {                                  // if Penelope also not present
                Penelope10MHz.Checked = false;  // disable 10MHz clock from Penny
                Penelope10MHz.Enabled = false;
                PennyOC.Enabled = false;
            }
            // force update of Penny 10MHz clock selection
            Penelope10MHz_CheckedChanged(this, EventArgs.Empty);
            // Update mic status
            chkMicGain_CheckedChanged(this, EventArgs.Empty);
            // Update Excalibure's status 
            chkExcalibur_CheckedChanged(this, EventArgs.Empty);
            // Update ExtCtrlGroup
            chkExtCtrlEnable_CheckedChanged(this, EventArgs.Empty);
            // Update GainByBand
            GainByBandGroup_EnabledChanged(this, EventArgs.Empty);
            // Update Tx filter group
            TransmitFilterGroup_EnabledChanged(this, EventArgs.Empty);
            // Update Tune Group
            TuneGroup_EnabledChanged(this, EventArgs.Empty);
            // Update Rx 0n PTT enable 
            chkMuteRxOnPTT_EnabledChanged(this, EventArgs.Empty);
            // Update Tx on PTT enable 
            chkOnlyTxOnPTT_EnabledChanged(this, EventArgs.Empty);
            // let the mainform know of the changes of Penny status
            MainForm.PennyLane = chkPennyLane.Checked;

        }

        public void chkMicGain_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.MicGain20dB = chkMicGain.Checked;

            if (chkPenelope.Checked || chkPennyLane.Checked || chkHermes.Checked) // *** does Hermes have line input ? ***
            {
                chkMicGain.Enabled = true;
                chkLineIn.Enabled = true;
                MainForm.SetMicGain();  // select new mic gain setting
            }
            else
            {
                chkMicGain.Enabled = false;
                chkLineIn.Enabled = false;
            }
        }

        public void Penelope10MHz_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penelope10MHz = Penelope10MHz.Checked;
        }

        public void Atlas10MHz_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Atlas10MHz = Atlas10MHz.Checked;
        }

        public void Mercury10MHz_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            if (Penelope10MHz.Checked) MainForm.Penelope10MHz = true;  // since the could be an instant where no clock is selected
            MainForm.Mercury10MHz = Mercury10MHz.Checked;
        }


        private void chkExcalibur_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            chkExcalibur.Enabled = chkHermes.Checked ? false : true;

            // if Excalibur is enabled  or Hermes selected then disable all other 10MHz clock options
            if (chkExcalibur.Checked || chkHermes.Checked)
            {
                Atlas10MHz.Enabled = false;
                Atlas10MHz.Checked = false;
                Penelope10MHz.Enabled = false;
                Penelope10MHz.Checked = false;
                Mercury10MHz.Enabled = false;
                Mercury10MHz.Checked = false;
            }
            else 
            {
                Atlas10MHz.Enabled = true;                
                Mercury10MHz.Enabled = true;
                if(chkPenelope.Checked || chkPennyLane.Checked)
                    Penelope10MHz.Enabled = true;
            }

            // update the Mainform with the current settings
            MainForm.Excalibur = chkExcalibur.Checked;
            MainForm.Atlas10MHz = Atlas10MHz.Checked;
            MainForm.Penelope10MHz = Penelope10MHz.Checked;
            MainForm.Mercury10MHz = Mercury10MHz.Checked;
        }

        public void TxFilterHigh_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.TxFilterHigh = (float)TxFilterHigh.Value;
            MainForm.AdjustFilterByMode();  // force read of new value
        }

        private void TxFilterLow_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.TxFilterLow = (float)TxFilterLow.Value;
            MainForm.AdjustFilterByMode(); // force read of new value 
        }

        private void Gain160m_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Gain160m = (int)Gain160m.Value;
            MainForm.UpdateBandGain();
        }

        private void Gain80m_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Gain80m = (int)Gain80m.Value;
            MainForm.UpdateBandGain();
        }

        private void Gain60m_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Gain60m = (int)Gain60m.Value;
            MainForm.UpdateBandGain();
        }

        private void Gain40m_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Gain40m = (int)Gain40m.Value;
            MainForm.UpdateBandGain();
        }

        private void Gain30m_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Gain30m = (int)Gain30m.Value;
            MainForm.UpdateBandGain();
        }

        private void Gain20m_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Gain20m = (int)Gain20m.Value;
            MainForm.UpdateBandGain();
        }

        private void Gain17m_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Gain17m = (int)Gain17m.Value;
            MainForm.UpdateBandGain();
        }

        private void Gain15m_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Gain15m = (int)Gain15m.Value;
            MainForm.UpdateBandGain();
        }

        private void Gain12m_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Gain12m = (int)Gain12m.Value;
            MainForm.UpdateBandGain();
        }

        private void Gain10m_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Gain10m = (int)Gain10m.Value;
            MainForm.UpdateBandGain();
        }

        private void Gain6m_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Gain6m = (int)Gain6m.Value;
            MainForm.UpdateBandGain();
        }

        private void chkMuteRxOnPTT_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Duplex = !chkMuteRxOnPTT.Checked;
            MainForm.setVolumeSquelched(); // force update of receiver audio mute
        }

        private void TuneLevel_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.TuneLevel = (int)TuneLevel.Value;
        }

        private void Set_CWPitch_ValueChanged(object sender, EventArgs e)
        {
            MainForm.CWPitch = (int)Set_CWPitch.Value;      // if user types in a number, get the new value 
            MainForm.AdjustFilterByMode();                  // force read of new value
        }

        // select all Open Collector controls
        private void chkExtCtrlEnable_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            chkExtCtrlEnable.Enabled = (chkPenelope.Checked || chkPennyLane.Checked || chkHermes.Checked) ? true : false;

            if (chkExtCtrlEnable.Checked && (chkPenelope.Checked || chkPennyLane.Checked || chkHermes.Checked))
            {
                PennyOC.Enabled = true;
            }
            else
            {
                PennyOC.Enabled = false;
            }

            MainForm.PennyOC = chkExtCtrlEnable.Checked;
        }


        // Penny Receive Open Collector settings

        private void checkBoxesPenOC6mRX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny6mRxOC = checkBoxesPenOC6mRX.Value;
        }

        private void checkBoxesPenOC10mRX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny10mRxOC = checkBoxesPenOC10mRX.Value;
        }

        private void checkBoxesPenOC12mRX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny12mRxOC = checkBoxesPenOC12mRX.Value;
        }

        private void checkBoxesPenOC15mRX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny15mRxOC = checkBoxesPenOC15mRX.Value;
        }

        private void checkBoxesPenOC17mRX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny17mRxOC = checkBoxesPenOC17mRX.Value;
        }

        private void checkBoxesPenOC20mRX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny20mRxOC = checkBoxesPenOC20mRX.Value;
        }

        private void checkBoxPenOC30mRX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny30mRxOC = checkBoxesPenOC30mRX.Value;
        }

        private void checkBoxesPenOC40mRX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny40mRxOC = checkBoxesPenOC40mRX.Value;
        }

        private void checkBoxesPenOC60mRX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny60mRxOC = checkBoxesPenOC60mRX.Value;
        }

        private void checkBoxesPenOC80mRX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny80mRxOC = checkBoxesPenOC80mRX.Value;
        }

        private void checkBoxesPenOC160mRX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny160mRxOC = checkBoxesPenOC160mRX.Value;
        }

        // Penny Transmit Open Collector settings

        private void checkBoxesPenOC6mTX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny6mTxOC = checkBoxesPenOC6mTX.Value;
        }

        private void checkBoxesPenOC10mTX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny10mTxOC = checkBoxesPenOC10mTX.Value;
        }

        private void checkBoxesPenOC12mTX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny12mTxOC = checkBoxesPenOC12mTX.Value;
        }

        private void checkBoxesPenOC15mTX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny15mTxOC = checkBoxesPenOC15mTX.Value;
        }

        private void checkBoxesPenOC17mTX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny17mTxOC = checkBoxesPenOC17mTX.Value;
        }

        private void checkBoxesPenOC20mTX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny20mTxOC = checkBoxesPenOC20mTX.Value;
        }

        private void checkBoxesPenOC30mTX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny30mTxOC = checkBoxesPenOC30mTX.Value;
        }

        private void checkBoxesPenOC40mTX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny40mTxOC = checkBoxesPenOC40mTX.Value;
        }

        private void checkBoxesPenOC60mTX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny60mTxOC = checkBoxesPenOC60mTX.Value;
        }

        private void checkBoxesPenOC80mTX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny80mTxOC = checkBoxesPenOC80mTX.Value;
        }

        private void checkBoxesPenOC160mTX_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Penny160mTxOC = checkBoxesPenOC160mTX.Value;
        }

        private void chkOnlyTxOnPTT_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.OnlyTxOnPTT = chkOnlyTxOnPTT.Checked;
        }

        private void GainByBandGroup_EnabledChanged(object sender, EventArgs e)
        {
            if (chkPenelope.Checked || chkPennyLane.Checked || chkHermes.Checked)
                GainByBandGroup.Enabled = true;
            else
                GainByBandGroup.Enabled = false;
        }

        private void TransmitFilterGroup_EnabledChanged(object sender, EventArgs e)
        {
            if (chkPenelope.Checked || chkPennyLane.Checked || chkHermes.Checked)
                TransmitFilterGroup.Enabled = true;
            else
                TransmitFilterGroup.Enabled = false;
        }

        private void TuneGroup_EnabledChanged(object sender, EventArgs e)
        {
            if (chkPenelope.Checked || chkPennyLane.Checked || chkHermes.Checked)
                TuneGroup.Enabled = true;
            else
                TuneGroup.Enabled = false;
        }

        private void chkMuteRxOnPTT_EnabledChanged(object sender, EventArgs e)
        {
            if (chkPenelope.Checked || chkPennyLane.Checked || chkHermes.Checked)
                chkMuteRxOnPTT.Enabled = true;
            else
                chkMuteRxOnPTT.Enabled = false;
        }

        private void chkOnlyTxOnPTT_EnabledChanged(object sender, EventArgs e)
        {
            if (chkPenelope.Checked || chkPennyLane.Checked || chkHermes.Checked)
                chkOnlyTxOnPTT.Enabled = true;
            else
                chkOnlyTxOnPTT.Enabled = false;
        }

        private void FM_deviation_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.FM_deviation = (int)FM_deviation.Value;
        }

        private void chkLineIn_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            if ((chkPenelope.Checked || chkPennyLane.Checked) && chkLineIn.Checked)
                MainForm.LineIn = true;
            else
                MainForm.LineIn = false;
        }

        private void DelayPTT_ValueChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            else MainForm.DelayPTT = (int)DelayPTT.Value;
        }

        private void alexUserControl6m_StateChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Alex6mState = alexUserControl6m.Value;
        }

        private void alexUserControl10m_StateChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Alex10mState = alexUserControl10m.Value;
        }

        private void alexUserControl12m_StateChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Alex12mState = alexUserControl12m.Value;
        }

        private void alexUserControl15m_StateChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Alex15mState = alexUserControl15m.Value;
        }

        private void alexUserControl17m_StateChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Alex17mState = alexUserControl17m.Value;
        }

        private void alexUserControl20m_StateChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Alex20mState = alexUserControl20m.Value;
        }

        private void alexUserControl30m_StateChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Alex30mState = alexUserControl30m.Value;
        }

        private void alexUserControl40m_StateChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Alex40mState = alexUserControl40m.Value;
        }

        private void alexUserControl60m_StateChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Alex60mState = alexUserControl60m.Value;
        }

        private void alexUserControl80m_StateChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Alex80mState = alexUserControl80m.Value;
        }

        private void alexUserControl160m_StateChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Alex160mState = alexUserControl160m.Value;
        }

        private void alexUserControlGC_StateChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.AlexGCState = alexUserControlGC.Value;
        }

        private void chkApollo_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Apollo = chkApollo.Checked;
        }

        private void chkAlex_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Alex = chkAlex.Checked;
            groupBoxAlex.Enabled = chkAlex.Checked;
        }

        private void checkBoxSkipVersionCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.SkipVersionCheck = checkBoxSkipVersionCheck.Checked;
        }

        // changing this value requires stopping and restarting the 'radio'
        private void checkBoxAllow16kWidebandSamples_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.Allow16kWidebandSamples = checkBoxAllow16kWidebandSamples.Checked;
        }

        private void cbAttemptFastConnect_CheckedChanged(object sender, EventArgs e)
        {
            if (suspendUpdate)
            {
                return;
            }

            MainForm.DoFastEthernetConnect = cbAttemptFastConnect.Checked;
        }
    }
}
