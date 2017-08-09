namespace KISS_Konsole
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.chkSpec = new System.Windows.Forms.CheckBox();
            this.display_freq = new System.Windows.Forms.TextBox();
            this.trackBarSetFrequency = new System.Windows.Forms.TrackBar();
            this.BandSelect = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.stepSize = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Preamp = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.SyncLED = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.AGCTrackBar = new System.Windows.Forms.TrackBar();
            this.AGCSpeed = new System.Windows.Forms.ComboBox();
            this.BandwidthTrackBar = new System.Windows.Forms.TrackBar();
            this.ANF = new System.Windows.Forms.CheckBox();
            this.NR = new System.Windows.Forms.CheckBox();
            this.NB1 = new System.Windows.Forms.CheckBox();
            this.NB2 = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.Mode = new System.Windows.Forms.ComboBox();
            this.chkWideSpec = new System.Windows.Forms.CheckBox();
            this.ADCoverloadButton = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.VolumeTrackBar = new System.Windows.Forms.TrackBar();
            this.StoreFreq = new System.Windows.Forms.Button();
            this.RecallFreq = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deviceConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataLoggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.specifyFilenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableDataLoggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chkWaterFall = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxSpectrum = new System.Windows.Forms.PictureBox();
            this.pictureBoxWideband = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Squelch_setting = new System.Windows.Forms.Label();
            this.Squelch_level = new System.Windows.Forms.TrackBar();
            this.Filter_squelch = new System.Windows.Forms.CheckBox();
            this.Bandscope_squelch = new System.Windows.Forms.CheckBox();
            this.DriveLevel = new System.Windows.Forms.TrackBar();
            this.Drive = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelSMeter = new System.Windows.Forms.Label();
            this.MOX = new System.Windows.Forms.Button();
            this.TUN = new System.Windows.Forms.Button();
            this.OnOffButton = new System.Windows.Forms.Button();
            this.labelFilterWidth = new System.Windows.Forms.Label();
            this.NoiseGateLevel = new System.Windows.Forms.TrackBar();
            this.chkNoiseGate = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.VOXHangTime = new System.Windows.Forms.NumericUpDown();
            this.MicrophoneGain = new System.Windows.Forms.TrackBar();
            this.labelClipLED = new System.Windows.Forms.Label();
            this.chkVOX = new System.Windows.Forms.CheckBox();
            this.VOXLevel = new System.Windows.Forms.TrackBar();
            this.label5 = new System.Windows.Forms.Label();
            this.ProcGaindB = new System.Windows.Forms.Label();
            this.chkBassCut = new System.Windows.Forms.CheckBox();
            this.ProcessorGain = new System.Windows.Forms.TrackBar();
            this.chkClipper = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.chkMicAGC = new System.Windows.Forms.CheckBox();
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            this.textBoxForwardPower = new System.Windows.Forms.Label();
            this.textBoxReversePower = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.labelAGCGain = new System.Windows.Forms.Label();
            this.labelVolume = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxNoiseGate = new System.Windows.Forms.Label();
            this.textBoxVOXLevel = new System.Windows.Forms.Label();
            this.textBoxMicGain = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelTopStatus = new System.Windows.Forms.Panel();
            this.labelFocus = new System.Windows.Forms.Label();
            this.saveFileDialogDataLogging = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSetFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AGCTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BandwidthTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeTrackBar)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpectrum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWideband)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Squelch_level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DriveLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoiseGateLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VOXHangTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MicrophoneGain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VOXLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessorGain)).BeginInit();
            this.panelBottom.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panelTopStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkSpec
            // 
            this.chkSpec.AutoSize = true;
            this.chkSpec.Location = new System.Drawing.Point(634, 83);
            this.chkSpec.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSpec.Name = "chkSpec";
            this.chkSpec.Size = new System.Drawing.Size(148, 24);
            this.chkSpec.TabIndex = 23;
            this.chkSpec.Text = "Show Spectrum";
            this.chkSpec.UseVisualStyleBackColor = true;
            this.chkSpec.CheckedChanged += new System.EventHandler(this.chkSpec_CheckedChanged);
            // 
            // display_freq
            // 
            this.display_freq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.display_freq.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.display_freq.ForeColor = System.Drawing.Color.Crimson;
            this.display_freq.Location = new System.Drawing.Point(436, 12);
            this.display_freq.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.display_freq.Name = "display_freq";
            this.display_freq.Size = new System.Drawing.Size(278, 53);
            this.display_freq.TabIndex = 43;
            this.display_freq.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // trackBarSetFrequency
            // 
            this.trackBarSetFrequency.AutoSize = false;
            this.trackBarSetFrequency.LargeChange = 0;
            this.trackBarSetFrequency.Location = new System.Drawing.Point(0, 0);
            this.trackBarSetFrequency.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.trackBarSetFrequency.Maximum = 55000000;
            this.trackBarSetFrequency.Name = "trackBarSetFrequency";
            this.trackBarSetFrequency.Size = new System.Drawing.Size(1536, 37);
            this.trackBarSetFrequency.SmallChange = 0;
            this.trackBarSetFrequency.TabIndex = 44;
            this.trackBarSetFrequency.TickFrequency = 1000000;
            this.trackBarSetFrequency.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBarSetFrequency.Scroll += new System.EventHandler(this.trackBarSetFrequency_Scroll);
            // 
            // BandSelect
            // 
            this.BandSelect.AllowDrop = true;
            this.BandSelect.FormattingEnabled = true;
            this.BandSelect.Items.AddRange(new object[] {
            "160m",
            "80m",
            "40m",
            "30m",
            "20m",
            "17m",
            "15m",
            "12m",
            "10m",
            "6m",
            "GC"});
            this.BandSelect.Location = new System.Drawing.Point(351, 60);
            this.BandSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BandSelect.Name = "BandSelect";
            this.BandSelect.Size = new System.Drawing.Size(90, 28);
            this.BandSelect.TabIndex = 46;
            this.BandSelect.SelectedIndexChanged += new System.EventHandler(this.BandSelect_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(296, 63);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 20);
            this.label3.TabIndex = 47;
            this.label3.Text = "Band";
            // 
            // stepSize
            // 
            this.stepSize.FormattingEnabled = true;
            this.stepSize.Items.AddRange(new object[] {
            "1Hz",
            "10Hz",
            "100Hz",
            "1kHz"});
            this.stepSize.Location = new System.Drawing.Point(351, 105);
            this.stepSize.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.stepSize.Name = "stepSize";
            this.stepSize.Size = new System.Drawing.Size(90, 28);
            this.stepSize.TabIndex = 48;
            this.stepSize.SelectedIndexChanged += new System.EventHandler(this.stepSize_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(266, 108);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 20);
            this.label4.TabIndex = 49;
            this.label4.Text = "Step Size";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(726, 38);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 20);
            this.label10.TabIndex = 54;
            this.label10.Text = "Frequency";
            // 
            // Preamp
            // 
            this.Preamp.AutoSize = true;
            this.Preamp.Location = new System.Drawing.Point(838, 155);
            this.Preamp.Name = "Preamp";
            this.Preamp.Size = new System.Drawing.Size(90, 24);
            this.Preamp.TabIndex = 56;
            this.Preamp.Text = "Preamp";
            this.Preamp.UseVisualStyleBackColor = true;
            this.Preamp.CheckedChanged += new System.EventHandler(this.Preamp_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(465, 63);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(44, 20);
            this.label11.TabIndex = 59;
            this.label11.Text = "AGC";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(458, 108);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(49, 20);
            this.label12.TabIndex = 60;
            this.label12.Text = "Mode";
            // 
            // SyncLED
            // 
            this.SyncLED.Location = new System.Drawing.Point(120, 42);
            this.SyncLED.Name = "SyncLED";
            this.SyncLED.Size = new System.Drawing.Size(58, 35);
            this.SyncLED.TabIndex = 63;
            this.SyncLED.Text = "Sync";
            this.SyncLED.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(274, 160);
            this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(63, 20);
            this.label15.TabIndex = 70;
            this.label15.Text = "Volume";
            // 
            // AGCTrackBar
            // 
            this.AGCTrackBar.AutoSize = false;
            this.AGCTrackBar.Location = new System.Drawing.Point(351, 232);
            this.AGCTrackBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AGCTrackBar.Maximum = 80;
            this.AGCTrackBar.Minimum = -60;
            this.AGCTrackBar.Name = "AGCTrackBar";
            this.AGCTrackBar.Size = new System.Drawing.Size(225, 43);
            this.AGCTrackBar.TabIndex = 71;
            this.AGCTrackBar.TickFrequency = 10;
            this.AGCTrackBar.Value = 70;
            this.AGCTrackBar.Scroll += new System.EventHandler(this.AGCTrackBar_Scroll);
            // 
            // AGCSpeed
            // 
            this.AGCSpeed.FormattingEnabled = true;
            this.AGCSpeed.Items.AddRange(new object[] {
            "OFF",
            "Long",
            "Slow",
            "Med",
            "Fast",
            "User"});
            this.AGCSpeed.Location = new System.Drawing.Point(524, 60);
            this.AGCSpeed.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AGCSpeed.Name = "AGCSpeed";
            this.AGCSpeed.Size = new System.Drawing.Size(86, 28);
            this.AGCSpeed.TabIndex = 72;
            this.AGCSpeed.SelectedIndexChanged += new System.EventHandler(this.AGCSpeed_SelectedIndexChanged);
            // 
            // BandwidthTrackBar
            // 
            this.BandwidthTrackBar.AutoSize = false;
            this.BandwidthTrackBar.Location = new System.Drawing.Point(878, 223);
            this.BandwidthTrackBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.BandwidthTrackBar.Maximum = 20000;
            this.BandwidthTrackBar.Name = "BandwidthTrackBar";
            this.BandwidthTrackBar.Size = new System.Drawing.Size(156, 43);
            this.BandwidthTrackBar.TabIndex = 73;
            this.BandwidthTrackBar.TickFrequency = 500;
            this.BandwidthTrackBar.Scroll += new System.EventHandler(this.BandwidthTrackBar_Scroll);
            this.BandwidthTrackBar.ValueChanged += new System.EventHandler(this.BandwidthTrackBar_ValueChanged);
            // 
            // ANF
            // 
            this.ANF.AutoSize = true;
            this.ANF.Location = new System.Drawing.Point(838, 86);
            this.ANF.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ANF.Name = "ANF";
            this.ANF.Size = new System.Drawing.Size(67, 24);
            this.ANF.TabIndex = 74;
            this.ANF.Text = "ANF";
            this.ANF.UseVisualStyleBackColor = true;
            this.ANF.CheckedChanged += new System.EventHandler(this.ANF_CheckedChanged);
            // 
            // NR
            // 
            this.NR.AutoSize = true;
            this.NR.Location = new System.Drawing.Point(927, 86);
            this.NR.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NR.Name = "NR";
            this.NR.Size = new System.Drawing.Size(58, 24);
            this.NR.TabIndex = 75;
            this.NR.Text = "NR";
            this.NR.UseVisualStyleBackColor = true;
            this.NR.CheckedChanged += new System.EventHandler(this.NR_CheckedChanged);
            // 
            // NB1
            // 
            this.NB1.AutoSize = true;
            this.NB1.Location = new System.Drawing.Point(838, 120);
            this.NB1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NB1.Name = "NB1";
            this.NB1.Size = new System.Drawing.Size(66, 24);
            this.NB1.TabIndex = 76;
            this.NB1.Text = "NB1";
            this.NB1.UseVisualStyleBackColor = true;
            this.NB1.CheckedChanged += new System.EventHandler(this.NB1_CheckedChanged);
            // 
            // NB2
            // 
            this.NB2.AutoSize = true;
            this.NB2.Location = new System.Drawing.Point(927, 120);
            this.NB2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NB2.Name = "NB2";
            this.NB2.Size = new System.Drawing.Size(66, 24);
            this.NB2.TabIndex = 77;
            this.NB2.Text = "NB2";
            this.NB2.UseVisualStyleBackColor = true;
            this.NB2.CheckedChanged += new System.EventHandler(this.NB2_CheckedChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(273, 235);
            this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(83, 20);
            this.label16.TabIndex = 78;
            this.label16.Text = "AGC-Gain";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(789, 223);
            this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(89, 20);
            this.label17.TabIndex = 79;
            this.label17.Text = "Filter Width";
            // 
            // Mode
            // 
            this.Mode.FormattingEnabled = true;
            this.Mode.Items.AddRange(new object[] {
            "AM",
            "SAM",
            "FM",
            "USB",
            "LSB",
            "CWU",
            "CWL"});
            this.Mode.Location = new System.Drawing.Point(524, 103);
            this.Mode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Mode.Name = "Mode";
            this.Mode.Size = new System.Drawing.Size(86, 28);
            this.Mode.TabIndex = 80;
            this.Mode.SelectedIndexChanged += new System.EventHandler(this.Mode_SelectedIndexChanged);
            // 
            // chkWideSpec
            // 
            this.chkWideSpec.AutoSize = true;
            this.chkWideSpec.Location = new System.Drawing.Point(634, 111);
            this.chkWideSpec.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkWideSpec.Name = "chkWideSpec";
            this.chkWideSpec.Size = new System.Drawing.Size(188, 24);
            this.chkWideSpec.TabIndex = 85;
            this.chkWideSpec.Text = "Show Wide Spectrum";
            this.chkWideSpec.UseVisualStyleBackColor = true;
            this.chkWideSpec.CheckedChanged += new System.EventHandler(this.chkWideSpec_CheckedChanged);
            // 
            // ADCoverloadButton
            // 
            this.ADCoverloadButton.Location = new System.Drawing.Point(188, 42);
            this.ADCoverloadButton.Name = "ADCoverloadButton";
            this.ADCoverloadButton.Size = new System.Drawing.Size(58, 35);
            this.ADCoverloadButton.TabIndex = 86;
            this.ADCoverloadButton.Text = "ADC";
            this.ADCoverloadButton.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // VolumeTrackBar
            // 
            this.VolumeTrackBar.AutoSize = false;
            this.VolumeTrackBar.LargeChange = 0;
            this.VolumeTrackBar.Location = new System.Drawing.Point(334, 160);
            this.VolumeTrackBar.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.VolumeTrackBar.Maximum = 100;
            this.VolumeTrackBar.Name = "VolumeTrackBar";
            this.VolumeTrackBar.Size = new System.Drawing.Size(260, 43);
            this.VolumeTrackBar.SmallChange = 0;
            this.VolumeTrackBar.TabIndex = 69;
            this.VolumeTrackBar.TickFrequency = 10;
            this.VolumeTrackBar.Scroll += new System.EventHandler(this.SetVolume);
            // 
            // StoreFreq
            // 
            this.StoreFreq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StoreFreq.Location = new System.Drawing.Point(8, 25);
            this.StoreFreq.Name = "StoreFreq";
            this.StoreFreq.Size = new System.Drawing.Size(56, 35);
            this.StoreFreq.TabIndex = 93;
            this.StoreFreq.Tag = "";
            this.StoreFreq.Text = "STO";
            this.StoreFreq.UseVisualStyleBackColor = true;
            this.StoreFreq.Click += new System.EventHandler(this.StoreFreq_Click);
            // 
            // RecallFreq
            // 
            this.RecallFreq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RecallFreq.Location = new System.Drawing.Point(69, 25);
            this.RecallFreq.Name = "RecallFreq";
            this.RecallFreq.Size = new System.Drawing.Size(56, 35);
            this.RecallFreq.TabIndex = 94;
            this.RecallFreq.Text = "RCL";
            this.RecallFreq.UseVisualStyleBackColor = true;
            this.RecallFreq.Click += new System.EventHandler(this.RecallFreq_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.StoreFreq);
            this.groupBox1.Controls.Add(this.RecallFreq);
            this.groupBox1.Location = new System.Drawing.Point(634, 198);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(134, 69);
            this.groupBox1.TabIndex = 96;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Quick Memory";
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Black;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(1059, 12);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(210, 62);
            this.pictureBox2.TabIndex = 98;
            this.pictureBox2.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupToolStripMenuItem,
            this.deviceConfigToolStripMenuItem,
            this.dataLoggingToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 3, 0, 3);
            this.menuStrip1.Size = new System.Drawing.Size(1533, 35);
            this.menuStrip1.TabIndex = 101;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(70, 29);
            this.setupToolStripMenuItem.Text = "Setup";
            this.setupToolStripMenuItem.Click += new System.EventHandler(this.setupToolStripMenuItem_Click);
            // 
            // deviceConfigToolStripMenuItem
            // 
            this.deviceConfigToolStripMenuItem.Name = "deviceConfigToolStripMenuItem";
            this.deviceConfigToolStripMenuItem.Size = new System.Drawing.Size(156, 29);
            this.deviceConfigToolStripMenuItem.Text = "Connection Type";
            this.deviceConfigToolStripMenuItem.Click += new System.EventHandler(this.deviceConfigToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(74, 29);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // dataLoggingToolStripMenuItem
            // 
            this.dataLoggingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.specifyFilenameToolStripMenuItem,
            this.enableDataLoggingToolStripMenuItem});
            this.dataLoggingToolStripMenuItem.Name = "dataLoggingToolStripMenuItem";
            this.dataLoggingToolStripMenuItem.Size = new System.Drawing.Size(132, 29);
            this.dataLoggingToolStripMenuItem.Text = "Data Logging";
            // 
            // specifyFilenameToolStripMenuItem
            // 
            this.specifyFilenameToolStripMenuItem.Name = "specifyFilenameToolStripMenuItem";
            this.specifyFilenameToolStripMenuItem.Size = new System.Drawing.Size(225, 30);
            this.specifyFilenameToolStripMenuItem.Text = "Specify filename";
            this.specifyFilenameToolStripMenuItem.Click += new System.EventHandler(this.specifyFilenameToolStripMenuItem_Click);
            // 
            // enableDataLoggingToolStripMenuItem
            // 
            this.enableDataLoggingToolStripMenuItem.Name = "enableDataLoggingToolStripMenuItem";
            this.enableDataLoggingToolStripMenuItem.Size = new System.Drawing.Size(225, 30);
            this.enableDataLoggingToolStripMenuItem.Text = "Enable";
            this.enableDataLoggingToolStripMenuItem.Click += new System.EventHandler(this.enableDataLoggingToolStripMenuItem_Click);
            // 
            // chkWaterFall
            // 
            this.chkWaterFall.AutoSize = true;
            this.chkWaterFall.Location = new System.Drawing.Point(634, 137);
            this.chkWaterFall.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkWaterFall.Name = "chkWaterFall";
            this.chkWaterFall.Size = new System.Drawing.Size(142, 24);
            this.chkWaterFall.TabIndex = 102;
            this.chkWaterFall.Text = "Show Waterfall";
            this.chkWaterFall.UseVisualStyleBackColor = true;
            this.chkWaterFall.CheckedChanged += new System.EventHandler(this.chkWaterFall_CheckedChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 114);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pictureBoxSpectrum);
            this.splitContainer1.Panel1MinSize = 0;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBoxWideband);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(1533, 732);
            this.splitContainer1.SplitterDistance = 359;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 2;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // pictureBoxSpectrum
            // 
            this.pictureBoxSpectrum.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxSpectrum.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxSpectrum.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBoxSpectrum.MinimumSize = new System.Drawing.Size(1152, 320);
            this.pictureBoxSpectrum.Name = "pictureBoxSpectrum";
            this.pictureBoxSpectrum.Size = new System.Drawing.Size(1536, 359);
            this.pictureBoxSpectrum.TabIndex = 0;
            this.pictureBoxSpectrum.TabStop = false;
            this.pictureBoxSpectrum.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxSpectrum_Paint);
            this.pictureBoxSpectrum.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSpectrum_MouseDown);
            this.pictureBoxSpectrum.MouseEnter += new System.EventHandler(this.pictureBoxSpectrum_MouseEnter);
            this.pictureBoxSpectrum.MouseLeave += new System.EventHandler(this.pictureBoxSpectrum_MouseLeave);
            this.pictureBoxSpectrum.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSpectrum_MouseMove);
            this.pictureBoxSpectrum.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxSpectrum_MouseUp);
            // 
            // pictureBoxWideband
            // 
            this.pictureBoxWideband.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBoxWideband.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxWideband.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBoxWideband.MinimumSize = new System.Drawing.Size(1152, 325);
            this.pictureBoxWideband.Name = "pictureBoxWideband";
            this.pictureBoxWideband.Size = new System.Drawing.Size(1536, 367);
            this.pictureBoxWideband.TabIndex = 0;
            this.pictureBoxWideband.TabStop = false;
            this.pictureBoxWideband.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxWideband_Paint);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Squelch_setting);
            this.groupBox2.Controls.Add(this.Squelch_level);
            this.groupBox2.Controls.Add(this.Filter_squelch);
            this.groupBox2.Controls.Add(this.Bandscope_squelch);
            this.groupBox2.Location = new System.Drawing.Point(16, 186);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(232, 122);
            this.groupBox2.TabIndex = 104;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Squelch";
            // 
            // Squelch_setting
            // 
            this.Squelch_setting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Squelch_setting.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Squelch_setting.Location = new System.Drawing.Point(168, 77);
            this.Squelch_setting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Squelch_setting.Name = "Squelch_setting";
            this.Squelch_setting.Size = new System.Drawing.Size(40, 28);
            this.Squelch_setting.TabIndex = 3;
            // 
            // Squelch_level
            // 
            this.Squelch_level.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Squelch_level.AutoSize = false;
            this.Squelch_level.BackColor = System.Drawing.SystemColors.Control;
            this.Squelch_level.Cursor = System.Windows.Forms.Cursors.Default;
            this.Squelch_level.LargeChange = 2;
            this.Squelch_level.Location = new System.Drawing.Point(9, 77);
            this.Squelch_level.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Squelch_level.Maximum = 500;
            this.Squelch_level.Minimum = -1500;
            this.Squelch_level.Name = "Squelch_level";
            this.Squelch_level.Size = new System.Drawing.Size(150, 31);
            this.Squelch_level.TabIndex = 2;
            this.Squelch_level.TickStyle = System.Windows.Forms.TickStyle.None;
            this.Squelch_level.Value = -1000;
            this.Squelch_level.Scroll += new System.EventHandler(this.Squelch_level_Scroll);
            // 
            // Filter_squelch
            // 
            this.Filter_squelch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Filter_squelch.AutoSize = true;
            this.Filter_squelch.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Filter_squelch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Filter_squelch.Location = new System.Drawing.Point(138, 36);
            this.Filter_squelch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Filter_squelch.Name = "Filter_squelch";
            this.Filter_squelch.Size = new System.Drawing.Size(70, 24);
            this.Filter_squelch.TabIndex = 1;
            this.Filter_squelch.Text = "Filter";
            this.Filter_squelch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Filter_squelch.UseVisualStyleBackColor = true;
            this.Filter_squelch.CheckedChanged += new System.EventHandler(this.Filter_squelch_CheckedChanged);
            // 
            // Bandscope_squelch
            // 
            this.Bandscope_squelch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Bandscope_squelch.AutoSize = true;
            this.Bandscope_squelch.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.Bandscope_squelch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bandscope_squelch.Location = new System.Drawing.Point(9, 36);
            this.Bandscope_squelch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Bandscope_squelch.Name = "Bandscope_squelch";
            this.Bandscope_squelch.Size = new System.Drawing.Size(116, 24);
            this.Bandscope_squelch.TabIndex = 0;
            this.Bandscope_squelch.Text = "Bandscope";
            this.Bandscope_squelch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Bandscope_squelch.UseVisualStyleBackColor = true;
            this.Bandscope_squelch.CheckedChanged += new System.EventHandler(this.Bandscope_squelch_CheckedChanged);
            // 
            // DriveLevel
            // 
            this.DriveLevel.AutoSize = false;
            this.DriveLevel.BackColor = System.Drawing.SystemColors.Control;
            this.DriveLevel.Cursor = System.Windows.Forms.Cursors.Default;
            this.DriveLevel.LargeChange = 2;
            this.DriveLevel.Location = new System.Drawing.Point(26, 148);
            this.DriveLevel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DriveLevel.Maximum = 100;
            this.DriveLevel.Name = "DriveLevel";
            this.DriveLevel.Size = new System.Drawing.Size(150, 31);
            this.DriveLevel.TabIndex = 4;
            this.DriveLevel.TickStyle = System.Windows.Forms.TickStyle.None;
            this.DriveLevel.Value = 50;
            this.DriveLevel.Scroll += new System.EventHandler(this.DriveLevel_Scroll);
            // 
            // Drive
            // 
            this.Drive.Location = new System.Drawing.Point(183, 148);
            this.Drive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Drive.Name = "Drive";
            this.Drive.ReadOnly = true;
            this.Drive.Size = new System.Drawing.Size(40, 26);
            this.Drive.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(76, 128);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 20);
            this.label2.TabIndex = 105;
            this.label2.Text = "Drive";
            // 
            // labelSMeter
            // 
            this.labelSMeter.Location = new System.Drawing.Point(752, 26);
            this.labelSMeter.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSMeter.Name = "labelSMeter";
            this.labelSMeter.Size = new System.Drawing.Size(291, 35);
            this.labelSMeter.TabIndex = 106;
            this.labelSMeter.Text = "SMeter:";
            this.labelSMeter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MOX
            // 
            this.MOX.Location = new System.Drawing.Point(120, 86);
            this.MOX.Name = "MOX";
            this.MOX.Size = new System.Drawing.Size(58, 35);
            this.MOX.TabIndex = 107;
            this.MOX.Text = "MOX";
            this.MOX.UseVisualStyleBackColor = true;
            this.MOX.Click += new System.EventHandler(this.MOX_Click);
            // 
            // TUN
            // 
            this.TUN.Location = new System.Drawing.Point(188, 86);
            this.TUN.Name = "TUN";
            this.TUN.Size = new System.Drawing.Size(58, 35);
            this.TUN.TabIndex = 108;
            this.TUN.Text = "TUN";
            this.TUN.UseVisualStyleBackColor = true;
            this.TUN.Click += new System.EventHandler(this.TUN_Click);
            // 
            // OnOffButton
            // 
            this.OnOffButton.Location = new System.Drawing.Point(52, 42);
            this.OnOffButton.Name = "OnOffButton";
            this.OnOffButton.Size = new System.Drawing.Size(58, 35);
            this.OnOffButton.TabIndex = 109;
            this.OnOffButton.Text = "OFF";
            this.OnOffButton.UseVisualStyleBackColor = true;
            this.OnOffButton.Click += new System.EventHandler(this.OnOffButton_Click);
            // 
            // labelFilterWidth
            // 
            this.labelFilterWidth.AutoSize = true;
            this.labelFilterWidth.Location = new System.Drawing.Point(932, 265);
            this.labelFilterWidth.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFilterWidth.Name = "labelFilterWidth";
            this.labelFilterWidth.Size = new System.Drawing.Size(46, 20);
            this.labelFilterWidth.TabIndex = 133;
            this.labelFilterWidth.Text = "width";
            // 
            // NoiseGateLevel
            // 
            this.NoiseGateLevel.AutoSize = false;
            this.NoiseGateLevel.BackColor = System.Drawing.SystemColors.Control;
            this.NoiseGateLevel.Cursor = System.Windows.Forms.Cursors.Default;
            this.NoiseGateLevel.LargeChange = 1;
            this.NoiseGateLevel.Location = new System.Drawing.Point(34, 226);
            this.NoiseGateLevel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NoiseGateLevel.Maximum = 100;
            this.NoiseGateLevel.Name = "NoiseGateLevel";
            this.NoiseGateLevel.Size = new System.Drawing.Size(150, 31);
            this.NoiseGateLevel.TabIndex = 150;
            this.NoiseGateLevel.TickStyle = System.Windows.Forms.TickStyle.None;
            this.NoiseGateLevel.Scroll += new System.EventHandler(this.NoiseGateLevel_Scroll);
            // 
            // chkNoiseGate
            // 
            this.chkNoiseGate.AutoSize = true;
            this.chkNoiseGate.Location = new System.Drawing.Point(10, 231);
            this.chkNoiseGate.Name = "chkNoiseGate";
            this.chkNoiseGate.Size = new System.Drawing.Size(22, 21);
            this.chkNoiseGate.TabIndex = 149;
            this.chkNoiseGate.UseVisualStyleBackColor = true;
            this.chkNoiseGate.CheckedChanged += new System.EventHandler(this.chkNoiseGate_CheckedChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(291, 117);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(48, 40);
            this.label13.TabIndex = 148;
            this.label13.Text = "Hang\r\n (mS)";
            // 
            // VOXHangTime
            // 
            this.VOXHangTime.Location = new System.Drawing.Point(231, 122);
            this.VOXHangTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.VOXHangTime.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.VOXHangTime.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.VOXHangTime.Name = "VOXHangTime";
            this.VOXHangTime.Size = new System.Drawing.Size(57, 26);
            this.VOXHangTime.TabIndex = 147;
            this.VOXHangTime.Value = new decimal(new int[] {
            400,
            0,
            0,
            0});
            // 
            // MicrophoneGain
            // 
            this.MicrophoneGain.AutoSize = false;
            this.MicrophoneGain.BackColor = System.Drawing.SystemColors.Control;
            this.MicrophoneGain.Cursor = System.Windows.Forms.Cursors.Default;
            this.MicrophoneGain.LargeChange = 2;
            this.MicrophoneGain.Location = new System.Drawing.Point(34, 174);
            this.MicrophoneGain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MicrophoneGain.Maximum = 142;
            this.MicrophoneGain.Name = "MicrophoneGain";
            this.MicrophoneGain.Size = new System.Drawing.Size(150, 31);
            this.MicrophoneGain.TabIndex = 146;
            this.MicrophoneGain.TickStyle = System.Windows.Forms.TickStyle.None;
            this.MicrophoneGain.Scroll += new System.EventHandler(this.MicrophoneGain_Scroll);
            // 
            // labelClipLED
            // 
            this.labelClipLED.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelClipLED.Location = new System.Drawing.Point(231, 177);
            this.labelClipLED.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelClipLED.Name = "labelClipLED";
            this.labelClipLED.Size = new System.Drawing.Size(71, 24);
            this.labelClipLED.TabIndex = 145;
            this.labelClipLED.Text = "Clip";
            this.labelClipLED.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkVOX
            // 
            this.chkVOX.AutoSize = true;
            this.chkVOX.Location = new System.Drawing.Point(10, 126);
            this.chkVOX.Name = "chkVOX";
            this.chkVOX.Size = new System.Drawing.Size(22, 21);
            this.chkVOX.TabIndex = 143;
            this.chkVOX.UseVisualStyleBackColor = true;
            this.chkVOX.CheckedChanged += new System.EventHandler(this.chkVOX_CheckedChanged);
            // 
            // VOXLevel
            // 
            this.VOXLevel.AutoSize = false;
            this.VOXLevel.BackColor = System.Drawing.SystemColors.Control;
            this.VOXLevel.Cursor = System.Windows.Forms.Cursors.Default;
            this.VOXLevel.LargeChange = 1;
            this.VOXLevel.Location = new System.Drawing.Point(34, 122);
            this.VOXLevel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.VOXLevel.Maximum = 100;
            this.VOXLevel.Name = "VOXLevel";
            this.VOXLevel.Size = new System.Drawing.Size(150, 31);
            this.VOXLevel.TabIndex = 142;
            this.VOXLevel.TickStyle = System.Windows.Forms.TickStyle.None;
            this.VOXLevel.Scroll += new System.EventHandler(this.VOXLevel_Scroll);
            this.VOXLevel.ValueChanged += new System.EventHandler(this.VOXLevel_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(226, 75);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 20);
            this.label5.TabIndex = 140;
            this.label5.Text = "dB";
            // 
            // ProcGaindB
            // 
            this.ProcGaindB.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ProcGaindB.Location = new System.Drawing.Point(182, 69);
            this.ProcGaindB.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ProcGaindB.Name = "ProcGaindB";
            this.ProcGaindB.Size = new System.Drawing.Size(42, 31);
            this.ProcGaindB.TabIndex = 139;
            // 
            // chkBassCut
            // 
            this.chkBassCut.AutoSize = true;
            this.chkBassCut.Location = new System.Drawing.Point(10, 18);
            this.chkBassCut.Name = "chkBassCut";
            this.chkBassCut.Size = new System.Drawing.Size(100, 24);
            this.chkBassCut.TabIndex = 138;
            this.chkBassCut.Text = "Bass Cut";
            this.chkBassCut.UseVisualStyleBackColor = true;
            this.chkBassCut.CheckedChanged += new System.EventHandler(this.chkBassCut_CheckedChanged);
            // 
            // ProcessorGain
            // 
            this.ProcessorGain.AutoSize = false;
            this.ProcessorGain.BackColor = System.Drawing.SystemColors.Control;
            this.ProcessorGain.Cursor = System.Windows.Forms.Cursors.Default;
            this.ProcessorGain.LargeChange = 1;
            this.ProcessorGain.Location = new System.Drawing.Point(34, 69);
            this.ProcessorGain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ProcessorGain.Maximum = 100;
            this.ProcessorGain.Minimum = 10;
            this.ProcessorGain.Name = "ProcessorGain";
            this.ProcessorGain.Size = new System.Drawing.Size(150, 31);
            this.ProcessorGain.TabIndex = 137;
            this.ProcessorGain.TickStyle = System.Windows.Forms.TickStyle.None;
            this.ProcessorGain.Value = 10;
            this.ProcessorGain.Scroll += new System.EventHandler(this.ProcessorGain_Scroll);
            this.ProcessorGain.ValueChanged += new System.EventHandler(this.ProcessorGain_ValueChanged);
            // 
            // chkClipper
            // 
            this.chkClipper.AutoSize = true;
            this.chkClipper.Location = new System.Drawing.Point(10, 74);
            this.chkClipper.Name = "chkClipper";
            this.chkClipper.Size = new System.Drawing.Size(22, 21);
            this.chkClipper.TabIndex = 136;
            this.chkClipper.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkClipper.UseVisualStyleBackColor = true;
            this.chkClipper.CheckedChanged += new System.EventHandler(this.chkClipper_CheckedChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(34, 206);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.label14.Size = new System.Drawing.Size(97, 20);
            this.label14.TabIndex = 152;
            this.label14.Text = "Noise Gate";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(34, 154);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.label9.Size = new System.Drawing.Size(79, 20);
            this.label9.TabIndex = 125;
            this.label9.Text = "Mic Gain";
            // 
            // timer2
            // 
            this.timer2.Interval = 400;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 400;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(34, 102);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.label8.Size = new System.Drawing.Size(51, 20);
            this.label8.TabIndex = 153;
            this.label8.Text = "VOX";
            // 
            // chkMicAGC
            // 
            this.chkMicAGC.AutoSize = true;
            this.chkMicAGC.Location = new System.Drawing.Point(122, 18);
            this.chkMicAGC.Name = "chkMicAGC";
            this.chkMicAGC.Size = new System.Drawing.Size(98, 24);
            this.chkMicAGC.TabIndex = 154;
            this.chkMicAGC.Text = "Mic AGC";
            this.chkMicAGC.UseVisualStyleBackColor = true;
            // 
            // timer4
            // 
            this.timer4.Tick += new System.EventHandler(this.timer4_Tick);
            // 
            // textBoxForwardPower
            // 
            this.textBoxForwardPower.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBoxForwardPower.Location = new System.Drawing.Point(104, 26);
            this.textBoxForwardPower.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.textBoxForwardPower.Name = "textBoxForwardPower";
            this.textBoxForwardPower.Size = new System.Drawing.Size(105, 31);
            this.textBoxForwardPower.TabIndex = 155;
            // 
            // textBoxReversePower
            // 
            this.textBoxReversePower.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBoxReversePower.Location = new System.Drawing.Point(302, 26);
            this.textBoxReversePower.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.textBoxReversePower.Name = "textBoxReversePower";
            this.textBoxReversePower.Size = new System.Drawing.Size(105, 31);
            this.textBoxReversePower.TabIndex = 156;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(15, 32);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(76, 20);
            this.label18.TabIndex = 157;
            this.label18.Text = "FWD Pwr";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(218, 32);
            this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(73, 20);
            this.label19.TabIndex = 158;
            this.label19.Text = "REV Pwr";
            // 
            // labelAGCGain
            // 
            this.labelAGCGain.AutoSize = true;
            this.labelAGCGain.Location = new System.Drawing.Point(438, 274);
            this.labelAGCGain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelAGCGain.Name = "labelAGCGain";
            this.labelAGCGain.Size = new System.Drawing.Size(39, 20);
            this.labelAGCGain.TabIndex = 159;
            this.labelAGCGain.Text = "gain";
            // 
            // labelVolume
            // 
            this.labelVolume.AutoSize = true;
            this.labelVolume.Location = new System.Drawing.Point(438, 198);
            this.labelVolume.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelVolume.Name = "labelVolume";
            this.labelVolume.Size = new System.Drawing.Size(28, 20);
            this.labelVolume.TabIndex = 160;
            this.labelVolume.Text = "vol";
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.groupBox3);
            this.panelBottom.Controls.Add(this.labelVolume);
            this.panelBottom.Controls.Add(this.chkWaterFall);
            this.panelBottom.Controls.Add(this.labelAGCGain);
            this.panelBottom.Controls.Add(this.chkSpec);
            this.panelBottom.Controls.Add(this.label3);
            this.panelBottom.Controls.Add(this.stepSize);
            this.panelBottom.Controls.Add(this.label4);
            this.panelBottom.Controls.Add(this.BandSelect);
            this.panelBottom.Controls.Add(this.Preamp);
            this.panelBottom.Controls.Add(this.AGCSpeed);
            this.panelBottom.Controls.Add(this.label11);
            this.panelBottom.Controls.Add(this.groupBox1);
            this.panelBottom.Controls.Add(this.label12);
            this.panelBottom.Controls.Add(this.ANF);
            this.panelBottom.Controls.Add(this.SyncLED);
            this.panelBottom.Controls.Add(this.NR);
            this.panelBottom.Controls.Add(this.VolumeTrackBar);
            this.panelBottom.Controls.Add(this.label15);
            this.panelBottom.Controls.Add(this.AGCTrackBar);
            this.panelBottom.Controls.Add(this.BandwidthTrackBar);
            this.panelBottom.Controls.Add(this.NB1);
            this.panelBottom.Controls.Add(this.NB2);
            this.panelBottom.Controls.Add(this.label16);
            this.panelBottom.Controls.Add(this.label17);
            this.panelBottom.Controls.Add(this.Mode);
            this.panelBottom.Controls.Add(this.chkWideSpec);
            this.panelBottom.Controls.Add(this.ADCoverloadButton);
            this.panelBottom.Controls.Add(this.groupBox2);
            this.panelBottom.Controls.Add(this.DriveLevel);
            this.panelBottom.Controls.Add(this.Drive);
            this.panelBottom.Controls.Add(this.label2);
            this.panelBottom.Controls.Add(this.MOX);
            this.panelBottom.Controls.Add(this.TUN);
            this.panelBottom.Controls.Add(this.OnOffButton);
            this.panelBottom.Controls.Add(this.labelFilterWidth);
            this.panelBottom.Controls.Add(this.label10);
            this.panelBottom.Controls.Add(this.trackBarSetFrequency);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 846);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1533, 326);
            this.panelBottom.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBoxNoiseGate);
            this.groupBox3.Controls.Add(this.textBoxVOXLevel);
            this.groupBox3.Controls.Add(this.textBoxMicGain);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.chkMicAGC);
            this.groupBox3.Controls.Add(this.chkBassCut);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.NoiseGateLevel);
            this.groupBox3.Controls.Add(this.chkNoiseGate);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.VOXHangTime);
            this.groupBox3.Controls.Add(this.MicrophoneGain);
            this.groupBox3.Controls.Add(this.labelClipLED);
            this.groupBox3.Controls.Add(this.chkVOX);
            this.groupBox3.Controls.Add(this.VOXLevel);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.ProcGaindB);
            this.groupBox3.Controls.Add(this.ProcessorGain);
            this.groupBox3.Controls.Add(this.chkClipper);
            this.groupBox3.Location = new System.Drawing.Point(1034, 38);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(344, 269);
            this.groupBox3.TabIndex = 161;
            this.groupBox3.TabStop = false;
            // 
            // textBoxNoiseGate
            // 
            this.textBoxNoiseGate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBoxNoiseGate.Location = new System.Drawing.Point(182, 226);
            this.textBoxNoiseGate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.textBoxNoiseGate.Name = "textBoxNoiseGate";
            this.textBoxNoiseGate.Size = new System.Drawing.Size(42, 31);
            this.textBoxNoiseGate.TabIndex = 160;
            // 
            // textBoxVOXLevel
            // 
            this.textBoxVOXLevel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBoxVOXLevel.Location = new System.Drawing.Point(182, 122);
            this.textBoxVOXLevel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.textBoxVOXLevel.Name = "textBoxVOXLevel";
            this.textBoxVOXLevel.Size = new System.Drawing.Size(42, 31);
            this.textBoxVOXLevel.TabIndex = 158;
            // 
            // textBoxMicGain
            // 
            this.textBoxMicGain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.textBoxMicGain.Location = new System.Drawing.Point(182, 174);
            this.textBoxMicGain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.textBoxMicGain.Name = "textBoxMicGain";
            this.textBoxMicGain.Size = new System.Drawing.Size(42, 31);
            this.textBoxMicGain.TabIndex = 156;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 48);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(147, 20);
            this.label1.TabIndex = 155;
            this.label1.Text = "Speech Processor";
            // 
            // panelTopStatus
            // 
            this.panelTopStatus.AutoSize = true;
            this.panelTopStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelTopStatus.Controls.Add(this.labelFocus);
            this.panelTopStatus.Controls.Add(this.label18);
            this.panelTopStatus.Controls.Add(this.label19);
            this.panelTopStatus.Controls.Add(this.pictureBox2);
            this.panelTopStatus.Controls.Add(this.display_freq);
            this.panelTopStatus.Controls.Add(this.textBoxReversePower);
            this.panelTopStatus.Controls.Add(this.labelSMeter);
            this.panelTopStatus.Controls.Add(this.textBoxForwardPower);
            this.panelTopStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTopStatus.Location = new System.Drawing.Point(0, 35);
            this.panelTopStatus.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelTopStatus.Name = "panelTopStatus";
            this.panelTopStatus.Size = new System.Drawing.Size(1533, 79);
            this.panelTopStatus.TabIndex = 1;
            // 
            // labelFocus
            // 
            this.labelFocus.AutoSize = true;
            this.labelFocus.Location = new System.Drawing.Point(0, 0);
            this.labelFocus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFocus.Name = "labelFocus";
            this.labelFocus.Size = new System.Drawing.Size(0, 20);
            this.labelFocus.TabIndex = 0;
            // 
            // saveFileDialogDataLogging
            // 
            this.saveFileDialogDataLogging.DefaultExt = "dat";
            this.saveFileDialogDataLogging.Filter = "Data Files|*.dat";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1533, 1172);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelTopStatus);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(894, 730);
            this.Name = "Form1";
            this.Text = "KISS Konsole";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSetFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AGCTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BandwidthTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VolumeTrackBar)).EndInit();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSpectrum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWideband)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Squelch_level)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DriveLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NoiseGateLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VOXHangTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MicrophoneGain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VOXLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProcessorGain)).EndInit();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panelTopStatus.ResumeLayout(false);
            this.panelTopStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkSpec;
        private System.Windows.Forms.TextBox display_freq;
        private System.Windows.Forms.TrackBar trackBarSetFrequency;
        private System.Windows.Forms.ComboBox BandSelect;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox stepSize;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox Preamp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.Button SyncLED;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TrackBar AGCTrackBar;
        private System.Windows.Forms.ComboBox AGCSpeed;
        private System.Windows.Forms.TrackBar BandwidthTrackBar;
        private System.Windows.Forms.CheckBox ANF;
        private System.Windows.Forms.CheckBox NR;
        private System.Windows.Forms.CheckBox NB1;
        private System.Windows.Forms.CheckBox NB2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox Mode;
        private System.Windows.Forms.CheckBox chkWideSpec;
        private System.Windows.Forms.Button ADCoverloadButton;
        public System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TrackBar VolumeTrackBar;
        private System.Windows.Forms.Button StoreFreq;
        private System.Windows.Forms.Button RecallFreq;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkWaterFall;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBoxSpectrum;
        private System.Windows.Forms.PictureBox pictureBoxWideband;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox Bandscope_squelch;
        private System.Windows.Forms.TrackBar Squelch_level;
        private System.Windows.Forms.CheckBox Filter_squelch;
        private System.Windows.Forms.Label Squelch_setting;
        private System.Windows.Forms.TrackBar DriveLevel;
        private System.Windows.Forms.TextBox Drive;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelSMeter;
        public System.Windows.Forms.Button MOX;
        private System.Windows.Forms.Button TUN;
        private System.Windows.Forms.Button OnOffButton;
        private System.Windows.Forms.Label labelFilterWidth;
        private System.Windows.Forms.TrackBar NoiseGateLevel;
        private System.Windows.Forms.CheckBox chkNoiseGate;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.NumericUpDown VOXHangTime;
        private System.Windows.Forms.TrackBar MicrophoneGain;
        private System.Windows.Forms.Label labelClipLED;
        private System.Windows.Forms.CheckBox chkVOX;
        private System.Windows.Forms.TrackBar VOXLevel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label ProcGaindB;
        private System.Windows.Forms.CheckBox chkBassCut;
        private System.Windows.Forms.TrackBar ProcessorGain;
        private System.Windows.Forms.CheckBox chkClipper;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.Timer timer2;
        public System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.CheckBox chkMicAGC;
        public System.Windows.Forms.Timer timer4;
        private System.Windows.Forms.ToolStripMenuItem deviceConfigToolStripMenuItem;
        private System.Windows.Forms.Label textBoxForwardPower;
        private System.Windows.Forms.Label textBoxReversePower;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Label labelAGCGain;
        private System.Windows.Forms.Label labelVolume;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelTopStatus;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label textBoxMicGain;
        private System.Windows.Forms.Label textBoxNoiseGate;
        private System.Windows.Forms.Label textBoxVOXLevel;
        private System.Windows.Forms.Label labelFocus;
        private System.Windows.Forms.ToolStripMenuItem dataLoggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem specifyFilenameToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem enableDataLoggingToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialogDataLogging;
    }
}

