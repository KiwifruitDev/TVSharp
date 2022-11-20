using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace TVSharp
{
    public class MainForm : Form
    {
        // Token: 0x0600005E RID: 94 RVA: 0x000028D4 File Offset: 0x00000AD4
        public MainForm()
        {
            this.InitializeComponent();
            this.videoWindow = new VideoWindow();
            this._settings = this._settingsPersister.ReadSettings();
            this.frequencyNumericUpDown_ValueChanged(null, null);
            try
            {
                this._rtlDevice.Open();
                DeviceDisplay[] activeDevices = DeviceDisplay.GetActiveDevices();
                this.deviceComboBox.Items.Clear();
                this.deviceComboBox.Items.AddRange(activeDevices);
                this.deviceComboBox.SelectedIndex = 0;
                this.deviceComboBox_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void startBtn_Click(object sender, EventArgs e)
        {
            if (!this._isDecoding)
            {
                this.StartDecoding();
            }
            else
            {
                this.StopDecoding();
            }
            this.startBtn.Text = (this._isDecoding ? "Stop" : "Start");
            this.deviceComboBox.Enabled = !this._rtlDevice.Device.IsStreaming;
        }

        // Token: 0x06000060 RID: 96 RVA: 0x000029F0 File Offset: 0x00000BF0
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this._isDecoding)
            {
                this.StopDecoding();
            }
            this._settings.Contrast = this.contrastTrackBar.Value;
            this._settings.Brightnes = this.brightnesTrackBar.Value;
            this._settings.Frequency = this.frequencyNumericUpDown.Value;
            this._settings.FrequencyCorrection = (int)this.frequencyCorrectionNumericUpDown.Value;
            this._settings.Samplerate = this.samplerateComboBox.SelectedIndex;
            this._settings.ProgramAgc = this.programAgcCheckBox.Checked;
            this._settings.TunerAgc = this.tunerAgcCheckBox.Checked;
            this._settings.RtlAgc = this.rtlAgcCheckBox.Checked;
            this._settings.DetectorLevel = this._detectorLevelCoef;
            this._settings.AutoPositionCorrecion = this._autoPositionCorrect;
            this._settings.InverseVideo = this._inverseVideo;
            this._settingsPersister.PersistSettings(this._settings);
        }

        // Token: 0x06000061 RID: 97 RVA: 0x00002B04 File Offset: 0x00000D04
        private void deviceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DeviceDisplay deviceDisplay = (DeviceDisplay)this.deviceComboBox.SelectedItem;
            if (deviceDisplay != null)
            {
                try
                {
                    this._rtlDevice.SelectDevice(deviceDisplay.Index);
                    this._rtlDevice.Frequency = 100000000L;
                    this._rtlDevice.Device.Samplerate = 2000000U;
                    this._rtlDevice.Device.UseRtlAGC = false;
                    this._rtlDevice.Device.UseTunerAGC = false;
                    this._initialized = true;
                }
                catch (Exception ex)
                {
                    this.deviceComboBox.SelectedIndex = -1;
                    this._initialized = false;
                    MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
                this.ConfigureDevice();
                this.ConfigureGUI();
            }
        }

        // Token: 0x06000062 RID: 98 RVA: 0x00002BD4 File Offset: 0x00000DD4
        private void tunerGainTrackBar_Scroll(object sender, EventArgs e)
        {
            if (!this._initialized)
            {
                return;
            }
            int num = this._rtlDevice.Device.SupportedGains[this.tunerGainTrackBar.Value];
            this._rtlDevice.Device.TunerGain = num;
            this.gainLabel.Text = (double)num / 10.0 + " dB";
        }

        // Token: 0x06000063 RID: 99 RVA: 0x00002C40 File Offset: 0x00000E40
        private void frequencyCorrectionNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!this._initialized)
            {
                return;
            }
            this._rtlDevice.Device.FrequencyCorrection = (int)this.frequencyCorrectionNumericUpDown.Value;
            this.frequencyCorrectionTrackBar.Value = (int)this.frequencyCorrectionNumericUpDown.Value;
        }

        // Token: 0x06000064 RID: 100 RVA: 0x00002C94 File Offset: 0x00000E94
        private void ConfigureGUI()
        {
            this.startBtn.Enabled = this._initialized;
            if (!this._initialized)
            {
                return;
            }
            this.videoWindow.Visible = true;
            this.tunerTypeLabel.Text = this._rtlDevice.Device.TunerType.ToString();
            this.tunerGainTrackBar.Maximum = this._rtlDevice.Device.SupportedGains.Length - 1;
            this.tunerGainTrackBar.Value = this.tunerGainTrackBar.Maximum;
            if (this._settings.Samplerate < 0)
            {
                this._settings.Samplerate = 2;
            }
            this.samplerateComboBox.SelectedIndex = this._settings.Samplerate;
            this.samplerateComboBox_SelectedIndexChanged(null, null);
            this.brightnesTrackBar.Value = this._settings.Brightnes;
            this.brightnesTrackBar_Scroll(null, null);
            this.contrastTrackBar.Value = this._settings.Contrast;
            this.contrastTrackBar_Scroll(null, null);
            this.frequencyCorrectionNumericUpDown.Value = this._settings.FrequencyCorrection;
            this.frequencyCorrectionNumericUpDown_ValueChanged(null, null);
            this.frequencyNumericUpDown.Value = this._settings.Frequency;
            this.frequencyNumericUpDown_ValueChanged(null, null);
            this.rtlAgcCheckBox.Checked = this._settings.RtlAgc;
            this.rtlAgcCheckBox_CheckedChanged(null, null);
            this.tunerAgcCheckBox.Checked = this._settings.TunerAgc;
            this.tunerAgcCheckBox_CheckedChanged(null, null);
            this.programAgcCheckBox.Checked = this._settings.ProgramAgc;
            this.programAgcCheckBox_CheckedChanged(null, null);
            this.autoSincCheckBox.Checked = this._settings.AutoPositionCorrecion;
            this.autoSincCheckBox_CheckedChanged(null, null);
            this.inverseCheckBox.Checked = this._settings.InverseVideo;
            this.inverseCheckBox_CheckedChanged(null, null);
            for (int i = 0; i < this.deviceComboBox.Items.Count; i++)
            {
                DeviceDisplay deviceDisplay = (DeviceDisplay)this.deviceComboBox.Items[i];
                if (deviceDisplay.Index == this._rtlDevice.Device.Index)
                {
                    this.deviceComboBox.SelectedIndex = i;
                    return;
                }
            }
        }

        // Token: 0x06000065 RID: 101 RVA: 0x00002EC3 File Offset: 0x000010C3
        private void ConfigureDevice()
        {
            this.frequencyCorrectionNumericUpDown_ValueChanged(null, null);
            this.tunerGainTrackBar_Scroll(null, null);
        }

        // Token: 0x06000066 RID: 102 RVA: 0x00002ED8 File Offset: 0x000010D8
        private unsafe void StartDecoding()
        {
            this._detectorLevelCoef = this._settings.DetectorLevel;
            if (this._detectorLevelCoef == 0f)
            {
                this._detectorLevelCoef = 0.77f;
            }
            this._pictureHeight = this._lineInFrame;
            this._pictureWidth = (int)(this._sampleRate / (double)this._countryCoeff);
            this._pictureCentr = this._pictureWidth / 2;
            this._detectImpulsePeriod = this._pictureWidth / 4;
            int num = this._pictureWidth * this._pictureHeight;
            this._grayScaleValues = new byte[num];
            this._videoWindowArray = new byte[num];
            try
            {
                this._rtlDevice.Start(new SamplesReadyDelegate(this.rtl_SamplesAvailable));
            }
            catch (Exception ex)
            {
                this.StopDecoding();
                MessageBox.Show("Unable to start RTL device\n" + ex.Message);
                return;
            }
            this._isDecoding = true;
        }

        // Token: 0x06000067 RID: 103 RVA: 0x00002FC0 File Offset: 0x000011C0
        private void StopDecoding()
        {
            this._rtlDevice.Stop();
            this._isDecoding = false;
            this._grayScaleValues = null;
        }

        // Token: 0x06000068 RID: 104 RVA: 0x00002FDC File Offset: 0x000011DC
        private unsafe void rtl_SamplesAvailable(object sender, Complex* buf, int length)
        {
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < length; i++)
            {
                sbyte real = buf[i].Real;
                sbyte imag = buf[i].Imag;
                num = Math.Max((int)real, num);
                int num3 = (int)Math.Sqrt((double)(real * real + imag * imag));
                num2 = Math.Max(num3, num2);
                if (this._inverseVideo)
                {
                    num3 = this._maxSignalLevel - num3;
                }
                this.DrawPixel(num3);
            }
            this._maxSignalLevel = (int)((double)this._maxSignalLevel * 0.9 + (double)num2 * 0.1);
            this._detectLevel = Convert.ToInt32((float)this._maxSignalLevel * this._detectorLevelCoef);
            this._blackLevel = Convert.ToInt32((float)this._maxSignalLevel * 0.7f);
            this._coeff = 255f / (float)this._blackLevel;
            this._agcSignalLevel = num;
            if (this._bufferIsFull)
            {
                lock (MainForm.locker)
                {
                    Array.Copy(this._grayScaleValues, this._videoWindowArray, this._grayScaleValues.Length);
                    this._bufferIsFull = false;
                }
            }
        }

        // Token: 0x06000069 RID: 105 RVA: 0x0000311C File Offset: 0x0000131C
        private void DrawPixel(int mag)
        {
            if (mag > this._detectLevel)
            {
                this._pixelCounter++;
            }
            else
            {
                if (this._pixelCounter > 5)
                {
                    if (this._pixelCounter > this._detectImpulsePeriod)
                    {
                        this._autoCorrectY = this._y;
                    }
                    else if (this._pixelCounter < this._detectImpulsePeriod)
                    {
                        this._autoCorrectX = this._x - this._pixelCounter;
                    }
                }
                this._pixelCounter = 0;
            }
            if (this._x >= this._pictureWidth)
            {
                this._y += 2;
                this._x = 0;
            }
            if (this._y == this._pictureHeight)
            {
                this._y = 0;
                this.FineFrequencyCorrection_Tick();
            }
            if (this._y > this._pictureHeight)
            {
                this._y = 1;
                this.FineFrequencyCorrection_Tick();
                this._bufferIsFull = true;
            }
            int num = (this._y + this._correctY) % this._pictureHeight * this._pictureWidth + (this._x + this._correctX) % this._pictureWidth;
            float num2 = (float)(this._blackLevel - mag) * this._coeff * this._contrast;
            num2 += (float)this._bright;
            if (num2 > 255f)
            {
                num2 = 255f;
            }
            if (num2 < 0f)
            {
                num2 = 0f;
            }
            this._grayScaleValues[num] = (byte)num2;
            this._x++;
        }

        // Token: 0x0600006A RID: 106 RVA: 0x00003278 File Offset: 0x00001478
        private void FineFrequencyCorrection_Tick()
        {
            this._fineTick += this._fineFrequencyCorrection;
            if (this._fineTick > 200)
            {
                this._correctX = (this._correctX + 1) % this._pictureWidth;
                this._fineTick = 0;
                return;
            }
            if (this._fineTick < -200)
            {
                this._correctX--;
                if (this._correctX < 0)
                {
                    this._correctX = this._pictureWidth;
                }
                this._fineTick = 0;
            }
        }

        // Token: 0x0600006B RID: 107 RVA: 0x000032FC File Offset: 0x000014FC
        private void fpsTimer_Tick(object sender, EventArgs e)
        {
            if (!this._isDecoding)
            {
                return;
            }
            lock (MainForm.locker)
            {
                this.videoWindow.DrawPictures(this._videoWindowArray, this._pictureWidth, this._pictureHeight);
            }
        }

        // Token: 0x0600006C RID: 108 RVA: 0x00003354 File Offset: 0x00001554
        private void frequencyNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (!this._initialized)
            {
                return;
            }
            this._rtlDevice.Device.Frequency = (uint)(this.frequencyNumericUpDown.Value * 1000000m);
            this.videoWindow.Text = Convert.ToString(this.frequencyNumericUpDown.Value);
        }

        // Token: 0x0600006D RID: 109 RVA: 0x000033B4 File Offset: 0x000015B4
        private void brightnesTrackBar_Scroll(object sender, EventArgs e)
        {
            this._bright = this.brightnesTrackBar.Value;
        }

        // Token: 0x0600006E RID: 110 RVA: 0x000033C7 File Offset: 0x000015C7
        private void contrastTrackBar_Scroll(object sender, EventArgs e)
        {
            this._contrast = (float)this.contrastTrackBar.Value / 10f;
        }

        // Token: 0x0600006F RID: 111 RVA: 0x000033E4 File Offset: 0x000015E4
        private void samplerateComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!this._initialized)
            {
                return;
            }
            bool isDecoding = this._isDecoding;
            if (isDecoding)
            {
                this.StopDecoding();
            }
            string s = this.samplerateComboBox.Items[this.samplerateComboBox.SelectedIndex].ToString().Split(new char[]
            {
                ' '
            })[0];
            this._sampleRate = double.Parse(s, CultureInfo.InvariantCulture) * 1000000.0;
            this._rtlDevice.Device.Samplerate = (uint)this._sampleRate;
            if (this.samplerateComboBox.SelectedIndex > 4)
            {
                this._countryCoeff = 15734.25f;
                this._lineInFrame = 525;
            }
            else
            {
                this._countryCoeff = 15625f;
                this._lineInFrame = 625;
            }
            if (isDecoding)
            {
                this.StartDecoding();
            }
        }

        // Token: 0x06000070 RID: 112 RVA: 0x000034B6 File Offset: 0x000016B6
        private void tunerAgcCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this._rtlDevice.Device.UseTunerAGC = this.tunerAgcCheckBox.Checked;
            this.tunerGainTrackBar.Enabled = !this.tunerAgcCheckBox.Checked;
        }

        // Token: 0x06000071 RID: 113 RVA: 0x000034EC File Offset: 0x000016EC
        private void rtlAgcCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this._rtlDevice.Device.UseRtlAGC = this.rtlAgcCheckBox.Checked;
        }

        // Token: 0x06000072 RID: 114 RVA: 0x0000350C File Offset: 0x0000170C
        private void programAgcCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.rtlAgcCheckBox.Enabled = !this.programAgcCheckBox.Checked;
            this.tunerAgcCheckBox.Enabled = !this.programAgcCheckBox.Checked;
            this.tunerGainTrackBar.Enabled = this.programAgcCheckBox.Checked;
            if (this.programAgcCheckBox.Checked)
            {
                this.tunerGainTrackBar.Enabled = true;
                this._rtlDevice.Device.UseTunerAGC = false;
                this._rtlDevice.Device.UseRtlAGC = false;
                return;
            }
            this.tunerAgcCheckBox_CheckedChanged(null, null);
            this.rtlAgcCheckBox_CheckedChanged(null, null);
        }

        // Token: 0x06000073 RID: 115 RVA: 0x000035B0 File Offset: 0x000017B0
        private void frequencyCorrectionTimer_Tick(object sender, EventArgs e)
        {
            if (!this._isDecoding)
            {
                return;
            }
            if (this._autoFrequencyCorrection)
            {
                if (this._frequencyCorrection < this._autoCorrectX)
                {
                    this.frequencyCorrectionNumericUpDown.Value = ++this.frequencyCorrectionNumericUpDown.Value % 200m;
                    this._counterAutoFreqCorrect = 0;
                }
                else if (this._frequencyCorrection > this._autoCorrectX)
                {
                    this.frequencyCorrectionNumericUpDown.Value = --this.frequencyCorrectionNumericUpDown.Value % 200m;
                    this._counterAutoFreqCorrect = 0;
                }
                else if (this._frequencyCorrection == this._autoCorrectX)
                {
                    this._counterAutoFreqCorrect++;
                    if (this._counterAutoFreqCorrect == 3)
                    {
                        this.autoFrequencyCorrectionButton_Click(null, null);
                    }
                }
                this._frequencyCorrection = this._autoCorrectX;
            }
            if (this._autoPositionCorrect)
            {
                this._correctX = this._pictureWidth - this._autoCorrectX;
                this._correctY = this._pictureHeight - this._autoCorrectY;
            }
            if (this.programAgcCheckBox.Checked)
            {
                if (this._agcSignalLevel > 125 && this._isDecoding)
                {
                    if (this.tunerGainTrackBar.Value > this.tunerGainTrackBar.Minimum)
                    {
                        this.tunerGainTrackBar.Value = this.tunerGainTrackBar.Value - 1;
                        this.tunerGainTrackBar_Scroll(null, null);
                    }
                }
                else if (this._agcSignalLevel < 90 && this._isDecoding && this.programAgcCheckBox.Checked && this.tunerGainTrackBar.Value < this.tunerGainTrackBar.Maximum)
                {
                    this.tunerGainTrackBar.Value = this.tunerGainTrackBar.Value + 1;
                    this.tunerGainTrackBar_Scroll(null, null);
                }
            }
            if (this._agcSignalLevel > 125)
            {
                this.label2.Text = "Gain Overload";
                return;
            }
            this.label2.Text = "Gain";
        }

        // Token: 0x06000074 RID: 116 RVA: 0x00003798 File Offset: 0x00001998
        private void xCorrectionTrackBar_Scroll(object sender, EventArgs e)
        {
            this._correctX = this.xCorrectionTrackBar.Value;
        }

        // Token: 0x06000075 RID: 117 RVA: 0x000037AB File Offset: 0x000019AB
        private void yCorrectionTrackBar_Scroll(object sender, EventArgs e)
        {
            this._correctY = this.yCorrectionTrackBar.Value * 2;
        }

        // Token: 0x06000076 RID: 118 RVA: 0x000037C0 File Offset: 0x000019C0
        private void frequencyCorrectionTrackBar_Scroll(object sender, EventArgs e)
        {
            this.frequencyCorrectionNumericUpDown.Value = this.frequencyCorrectionTrackBar.Value;
            this.frequencyCorrectionNumericUpDown_ValueChanged(null, null);
        }

        // Token: 0x06000077 RID: 119 RVA: 0x000037E8 File Offset: 0x000019E8
        private void autoSincCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this._autoPositionCorrect = this.autoSincCheckBox.Checked;
            this.xCorrectionTrackBar.Enabled = !this._autoPositionCorrect;
            this.yCorrectionTrackBar.Enabled = !this._autoPositionCorrect;
            if (!this._autoPositionCorrect)
            {
                this.xCorrectionTrackBar.Value = this._correctX;
                this.yCorrectionTrackBar.Value = this._correctY / 2;
            }
        }

        // Token: 0x06000078 RID: 120 RVA: 0x0000385A File Offset: 0x00001A5A
        private void autoFrequencyCorrectionButton_Click(object sender, EventArgs e)
        {
            this._autoFrequencyCorrection = !this._autoFrequencyCorrection;
            if (this._autoFrequencyCorrection)
            {
                this.autoFrequencyCorrectionButton.Text = "Stop correction";
                return;
            }
            this.autoFrequencyCorrectionButton.Text = "Auto correction";
        }

        // Token: 0x06000079 RID: 121 RVA: 0x00003894 File Offset: 0x00001A94
        private void fineFrequencyCorrectTrackBar_Scroll(object sender, EventArgs e)
        {
            this._fineFrequencyCorrection = this.fineFrequencyCorrectTrackBar.Value;
        }

        // Token: 0x0600007A RID: 122 RVA: 0x000038A7 File Offset: 0x00001AA7
        private void inverseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this._inverseVideo = this.inverseCheckBox.Checked;
        }

        // Token: 0x0600007B RID: 123 RVA: 0x000038BA File Offset: 0x00001ABA
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Token: 0x0600007C RID: 124 RVA: 0x000038DC File Offset: 0x00001ADC
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.startBtn = new System.Windows.Forms.Button();
            this.fpsTimer = new System.Windows.Forms.Timer(this.components);
            this.gainLabel = new System.Windows.Forms.Label();
            this.frequencyCorrectionNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tunerGainTrackBar = new System.Windows.Forms.TrackBar();
            this.tunerTypeLabel = new System.Windows.Forms.Label();
            this.deviceComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.frequencyNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.samplerateComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.brightnesTrackBar = new System.Windows.Forms.TrackBar();
            this.contrastTrackBar = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tunerAgcCheckBox = new System.Windows.Forms.CheckBox();
            this.rtlAgcCheckBox = new System.Windows.Forms.CheckBox();
            this.programAgcCheckBox = new System.Windows.Forms.CheckBox();
            this.frequencyCorrectionTimer = new System.Windows.Forms.Timer(this.components);
            this.frequencyCorrectionTrackBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.fineFrequencyCorrectTrackBar = new System.Windows.Forms.TrackBar();
            this.xCorrectionTrackBar = new System.Windows.Forms.TrackBar();
            this.yCorrectionTrackBar = new System.Windows.Forms.TrackBar();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.frequencyCorrectionGroupBox = new System.Windows.Forms.GroupBox();
            this.autoFrequencyCorrectionButton = new System.Windows.Forms.Button();
            this.positionGroupBox = new System.Windows.Forms.GroupBox();
            this.autoSincCheckBox = new System.Windows.Forms.CheckBox();
            this.tunerGroupBox = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.inverseCheckBox = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyCorrectionNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tunerGainTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightnesTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyCorrectionTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fineFrequencyCorrectTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xCorrectionTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.yCorrectionTrackBar)).BeginInit();
            this.frequencyCorrectionGroupBox.SuspendLayout();
            this.positionGroupBox.SuspendLayout();
            this.tunerGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // startBtn
            // 
            this.startBtn.Enabled = false;
            this.startBtn.Location = new System.Drawing.Point(322, 429);
            this.startBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(43, 95);
            this.startBtn.TabIndex = 0;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            // 
            // fpsTimer
            // 
            this.fpsTimer.Enabled = true;
            this.fpsTimer.Interval = 50;
            // 
            // gainLabel
            // 
            this.gainLabel.Location = new System.Drawing.Point(211, 122);
            this.gainLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.gainLabel.Name = "gainLabel";
            this.gainLabel.Size = new System.Drawing.Size(79, 15);
            this.gainLabel.TabIndex = 26;
            this.gainLabel.Text = "1000dB";
            this.gainLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // frequencyCorrectionNumericUpDown
            // 
            this.frequencyCorrectionNumericUpDown.Location = new System.Drawing.Point(222, 21);
            this.frequencyCorrectionNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.frequencyCorrectionNumericUpDown.Minimum = new decimal(new int[] {
            200,
            0,
            0,
            -2147483648});
            this.frequencyCorrectionNumericUpDown.Name = "frequencyCorrectionNumericUpDown";
            this.frequencyCorrectionNumericUpDown.Size = new System.Drawing.Size(64, 23);
            this.frequencyCorrectionNumericUpDown.TabIndex = 4;
            this.frequencyCorrectionNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 122);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 22;
            this.label2.Text = "Gain";
            // 
            // tunerGainTrackBar
            // 
            this.tunerGainTrackBar.AutoSize = false;
            this.tunerGainTrackBar.Location = new System.Drawing.Point(21, 141);
            this.tunerGainTrackBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tunerGainTrackBar.Maximum = 10000;
            this.tunerGainTrackBar.Name = "tunerGainTrackBar";
            this.tunerGainTrackBar.Size = new System.Drawing.Size(270, 35);
            this.tunerGainTrackBar.TabIndex = 3;
            // 
            // tunerTypeLabel
            // 
            this.tunerTypeLabel.Location = new System.Drawing.Point(182, 20);
            this.tunerTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.tunerTypeLabel.Name = "tunerTypeLabel";
            this.tunerTypeLabel.Size = new System.Drawing.Size(108, 15);
            this.tunerTypeLabel.TabIndex = 29;
            this.tunerTypeLabel.Text = "E4000";
            this.tunerTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // deviceComboBox
            // 
            this.deviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deviceComboBox.FormattingEnabled = true;
            this.deviceComboBox.Location = new System.Drawing.Point(7, 38);
            this.deviceComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.deviceComboBox.Name = "deviceComboBox";
            this.deviceComboBox.Size = new System.Drawing.Size(283, 23);
            this.deviceComboBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 20;
            this.label1.Text = "Device";
            // 
            // frequencyNumericUpDown
            // 
            this.frequencyNumericUpDown.DecimalPlaces = 3;
            this.frequencyNumericUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.frequencyNumericUpDown.Location = new System.Drawing.Point(145, 490);
            this.frequencyNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.frequencyNumericUpDown.Name = "frequencyNumericUpDown";
            this.frequencyNumericUpDown.Size = new System.Drawing.Size(160, 29);
            this.frequencyNumericUpDown.TabIndex = 30;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(18, 493);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 24);
            this.label5.TabIndex = 31;
            this.label5.Text = "Frequency";
            // 
            // samplerateComboBox
            // 
            this.samplerateComboBox.FormattingEnabled = true;
            this.samplerateComboBox.Items.AddRange(new object[] {
            "3.0 MSPS",
            "2.5 MSPS pal secam",
            "2.0 MSPS pal secam",
            "1.5 MSPS pal secam",
            "1.0 MSPS pal secam",
            "2.517480 MSPS ntsc",
            "1.951047 MSPS ntsc",
            "1.573425 MSPS ntsc",
            "1.006992 MSPS ntsc"});
            this.samplerateComboBox.Location = new System.Drawing.Point(100, 68);
            this.samplerateComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.samplerateComboBox.Name = "samplerateComboBox";
            this.samplerateComboBox.Size = new System.Drawing.Size(190, 23);
            this.samplerateComboBox.TabIndex = 32;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 72);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 15);
            this.label3.TabIndex = 33;
            this.label3.Text = "SampleRate";
            // 
            // brightnesTrackBar
            // 
            this.brightnesTrackBar.Location = new System.Drawing.Point(320, 33);
            this.brightnesTrackBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.brightnesTrackBar.Maximum = 100;
            this.brightnesTrackBar.Minimum = -100;
            this.brightnesTrackBar.Name = "brightnesTrackBar";
            this.brightnesTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.brightnesTrackBar.Size = new System.Drawing.Size(45, 142);
            this.brightnesTrackBar.TabIndex = 36;
            this.brightnesTrackBar.TickFrequency = 20;
            this.brightnesTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            // 
            // contrastTrackBar
            // 
            this.contrastTrackBar.Location = new System.Drawing.Point(320, 201);
            this.contrastTrackBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.contrastTrackBar.Maximum = 20;
            this.contrastTrackBar.Minimum = 1;
            this.contrastTrackBar.Name = "contrastTrackBar";
            this.contrastTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.contrastTrackBar.Size = new System.Drawing.Size(45, 159);
            this.contrastTrackBar.TabIndex = 37;
            this.contrastTrackBar.TickFrequency = 2;
            this.contrastTrackBar.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.contrastTrackBar.Value = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(316, 10);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 15);
            this.label6.TabIndex = 38;
            this.label6.Text = "Brightnes";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(318, 179);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 15);
            this.label7.TabIndex = 39;
            this.label7.Text = "Contrast";
            // 
            // tunerAgcCheckBox
            // 
            this.tunerAgcCheckBox.AutoSize = true;
            this.tunerAgcCheckBox.Location = new System.Drawing.Point(62, 100);
            this.tunerAgcCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tunerAgcCheckBox.Name = "tunerAgcCheckBox";
            this.tunerAgcCheckBox.Size = new System.Drawing.Size(56, 19);
            this.tunerAgcCheckBox.TabIndex = 40;
            this.tunerAgcCheckBox.Text = "Tuner";
            this.tunerAgcCheckBox.UseVisualStyleBackColor = true;
            // 
            // rtlAgcCheckBox
            // 
            this.rtlAgcCheckBox.AutoSize = true;
            this.rtlAgcCheckBox.Location = new System.Drawing.Point(132, 100);
            this.rtlAgcCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.rtlAgcCheckBox.Name = "rtlAgcCheckBox";
            this.rtlAgcCheckBox.Size = new System.Drawing.Size(44, 19);
            this.rtlAgcCheckBox.TabIndex = 41;
            this.rtlAgcCheckBox.Text = "RTL";
            this.rtlAgcCheckBox.UseVisualStyleBackColor = true;
            // 
            // programAgcCheckBox
            // 
            this.programAgcCheckBox.AutoSize = true;
            this.programAgcCheckBox.Location = new System.Drawing.Point(194, 100);
            this.programAgcCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.programAgcCheckBox.Name = "programAgcCheckBox";
            this.programAgcCheckBox.Size = new System.Drawing.Size(72, 19);
            this.programAgcCheckBox.TabIndex = 42;
            this.programAgcCheckBox.Text = "Program";
            this.programAgcCheckBox.UseVisualStyleBackColor = true;
            // 
            // frequencyCorrectionTimer
            // 
            this.frequencyCorrectionTimer.Enabled = true;
            this.frequencyCorrectionTimer.Interval = 200;
            // 
            // frequencyCorrectionTrackBar
            // 
            this.frequencyCorrectionTrackBar.AutoSize = false;
            this.frequencyCorrectionTrackBar.Location = new System.Drawing.Point(82, 51);
            this.frequencyCorrectionTrackBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.frequencyCorrectionTrackBar.Maximum = 200;
            this.frequencyCorrectionTrackBar.Minimum = -200;
            this.frequencyCorrectionTrackBar.Name = "frequencyCorrectionTrackBar";
            this.frequencyCorrectionTrackBar.Size = new System.Drawing.Size(204, 35);
            this.frequencyCorrectionTrackBar.TabIndex = 45;
            this.frequencyCorrectionTrackBar.TickFrequency = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 51);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(51, 15);
            this.label4.TabIndex = 46;
            this.label4.Text = "Roughly";
            // 
            // fineFrequencyCorrectTrackBar
            // 
            this.fineFrequencyCorrectTrackBar.AutoSize = false;
            this.fineFrequencyCorrectTrackBar.Location = new System.Drawing.Point(82, 92);
            this.fineFrequencyCorrectTrackBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.fineFrequencyCorrectTrackBar.Maximum = 20;
            this.fineFrequencyCorrectTrackBar.Minimum = -20;
            this.fineFrequencyCorrectTrackBar.Name = "fineFrequencyCorrectTrackBar";
            this.fineFrequencyCorrectTrackBar.Size = new System.Drawing.Size(204, 35);
            this.fineFrequencyCorrectTrackBar.TabIndex = 47;
            this.fineFrequencyCorrectTrackBar.TickFrequency = 2;
            // 
            // xCorrectionTrackBar
            // 
            this.xCorrectionTrackBar.AutoSize = false;
            this.xCorrectionTrackBar.Location = new System.Drawing.Point(82, 48);
            this.xCorrectionTrackBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.xCorrectionTrackBar.Maximum = 200;
            this.xCorrectionTrackBar.Name = "xCorrectionTrackBar";
            this.xCorrectionTrackBar.Size = new System.Drawing.Size(204, 35);
            this.xCorrectionTrackBar.TabIndex = 48;
            this.xCorrectionTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // yCorrectionTrackBar
            // 
            this.yCorrectionTrackBar.AutoSize = false;
            this.yCorrectionTrackBar.Location = new System.Drawing.Point(82, 90);
            this.yCorrectionTrackBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.yCorrectionTrackBar.Maximum = 400;
            this.yCorrectionTrackBar.Name = "yCorrectionTrackBar";
            this.yCorrectionTrackBar.Size = new System.Drawing.Size(204, 35);
            this.yCorrectionTrackBar.TabIndex = 49;
            this.yCorrectionTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 92);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 15);
            this.label9.TabIndex = 46;
            this.label9.Text = "Fine";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(26, 48);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 15);
            this.label10.TabIndex = 46;
            this.label10.Text = "X";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(26, 90);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(14, 15);
            this.label11.TabIndex = 46;
            this.label11.Text = "Y";
            // 
            // frequencyCorrectionGroupBox
            // 
            this.frequencyCorrectionGroupBox.Controls.Add(this.autoFrequencyCorrectionButton);
            this.frequencyCorrectionGroupBox.Controls.Add(this.label4);
            this.frequencyCorrectionGroupBox.Controls.Add(this.frequencyCorrectionNumericUpDown);
            this.frequencyCorrectionGroupBox.Controls.Add(this.fineFrequencyCorrectTrackBar);
            this.frequencyCorrectionGroupBox.Controls.Add(this.frequencyCorrectionTrackBar);
            this.frequencyCorrectionGroupBox.Controls.Add(this.label9);
            this.frequencyCorrectionGroupBox.Location = new System.Drawing.Point(14, 201);
            this.frequencyCorrectionGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.frequencyCorrectionGroupBox.Name = "frequencyCorrectionGroupBox";
            this.frequencyCorrectionGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.frequencyCorrectionGroupBox.Size = new System.Drawing.Size(301, 141);
            this.frequencyCorrectionGroupBox.TabIndex = 50;
            this.frequencyCorrectionGroupBox.TabStop = false;
            this.frequencyCorrectionGroupBox.Text = "Frequency correction";
            // 
            // autoFrequencyCorrectionButton
            // 
            this.autoFrequencyCorrectionButton.Location = new System.Drawing.Point(21, 17);
            this.autoFrequencyCorrectionButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.autoFrequencyCorrectionButton.Name = "autoFrequencyCorrectionButton";
            this.autoFrequencyCorrectionButton.Size = new System.Drawing.Size(125, 27);
            this.autoFrequencyCorrectionButton.TabIndex = 48;
            this.autoFrequencyCorrectionButton.Text = "Auto correction";
            this.autoFrequencyCorrectionButton.UseVisualStyleBackColor = true;
            // 
            // positionGroupBox
            // 
            this.positionGroupBox.Controls.Add(this.autoSincCheckBox);
            this.positionGroupBox.Controls.Add(this.label10);
            this.positionGroupBox.Controls.Add(this.label11);
            this.positionGroupBox.Controls.Add(this.yCorrectionTrackBar);
            this.positionGroupBox.Controls.Add(this.xCorrectionTrackBar);
            this.positionGroupBox.Location = new System.Drawing.Point(14, 348);
            this.positionGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.positionGroupBox.Name = "positionGroupBox";
            this.positionGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.positionGroupBox.Size = new System.Drawing.Size(301, 135);
            this.positionGroupBox.TabIndex = 48;
            this.positionGroupBox.TabStop = false;
            this.positionGroupBox.Text = "Position correction";
            // 
            // autoSincCheckBox
            // 
            this.autoSincCheckBox.AutoSize = true;
            this.autoSincCheckBox.Location = new System.Drawing.Point(29, 22);
            this.autoSincCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.autoSincCheckBox.Name = "autoSincCheckBox";
            this.autoSincCheckBox.Size = new System.Drawing.Size(52, 19);
            this.autoSincCheckBox.TabIndex = 48;
            this.autoSincCheckBox.Text = "Auto";
            this.autoSincCheckBox.UseVisualStyleBackColor = true;
            // 
            // tunerGroupBox
            // 
            this.tunerGroupBox.Controls.Add(this.label8);
            this.tunerGroupBox.Controls.Add(this.label1);
            this.tunerGroupBox.Controls.Add(this.tunerTypeLabel);
            this.tunerGroupBox.Controls.Add(this.deviceComboBox);
            this.tunerGroupBox.Controls.Add(this.programAgcCheckBox);
            this.tunerGroupBox.Controls.Add(this.gainLabel);
            this.tunerGroupBox.Controls.Add(this.rtlAgcCheckBox);
            this.tunerGroupBox.Controls.Add(this.samplerateComboBox);
            this.tunerGroupBox.Controls.Add(this.tunerAgcCheckBox);
            this.tunerGroupBox.Controls.Add(this.label2);
            this.tunerGroupBox.Controls.Add(this.label3);
            this.tunerGroupBox.Controls.Add(this.tunerGainTrackBar);
            this.tunerGroupBox.Location = new System.Drawing.Point(14, 0);
            this.tunerGroupBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tunerGroupBox.Name = "tunerGroupBox";
            this.tunerGroupBox.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tunerGroupBox.Size = new System.Drawing.Size(301, 193);
            this.tunerGroupBox.TabIndex = 51;
            this.tunerGroupBox.TabStop = false;
            this.tunerGroupBox.Text = "Tuner";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 100);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(31, 15);
            this.label8.TabIndex = 43;
            this.label8.Text = "AGC";
            // 
            // inverseCheckBox
            // 
            this.inverseCheckBox.AutoSize = true;
            this.inverseCheckBox.Location = new System.Drawing.Point(336, 397);
            this.inverseCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.inverseCheckBox.Name = "inverseCheckBox";
            this.inverseCheckBox.Size = new System.Drawing.Size(15, 14);
            this.inverseCheckBox.TabIndex = 52;
            this.inverseCheckBox.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(324, 372);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(44, 15);
            this.label12.TabIndex = 53;
            this.label12.Text = "Inverse";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 531);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.inverseCheckBox);
            this.Controls.Add(this.tunerGroupBox);
            this.Controls.Add(this.positionGroupBox);
            this.Controls.Add(this.frequencyCorrectionGroupBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.contrastTrackBar);
            this.Controls.Add(this.brightnesTrackBar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.frequencyNumericUpDown);
            this.Controls.Add(this.startBtn);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TVSharp";
            ((System.ComponentModel.ISupportInitialize)(this.frequencyCorrectionNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tunerGainTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.brightnesTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.contrastTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.frequencyCorrectionTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fineFrequencyCorrectTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xCorrectionTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.yCorrectionTrackBar)).EndInit();
            this.frequencyCorrectionGroupBox.ResumeLayout(false);
            this.frequencyCorrectionGroupBox.PerformLayout();
            this.positionGroupBox.ResumeLayout(false);
            this.positionGroupBox.PerformLayout();
            this.tunerGroupBox.ResumeLayout(false);
            this.tunerGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        // Token: 0x04000025 RID: 37
        private readonly RtlSdrIO _rtlDevice = new RtlSdrIO();

        // Token: 0x04000026 RID: 38
        private bool _isDecoding;

        // Token: 0x04000027 RID: 39
        private bool _initialized;

        // Token: 0x04000028 RID: 40
        private byte[] _grayScaleValues;

        // Token: 0x04000029 RID: 41
        private byte[] _videoWindowArray;

        // Token: 0x0400002A RID: 42
        private int _detectLevel;

        // Token: 0x0400002B RID: 43
        private float _detectorLevelCoef;

        // Token: 0x0400002C RID: 44
        private int _detectImpulsePeriod;

        // Token: 0x0400002D RID: 45
        private int _x;

        // Token: 0x0400002E RID: 46
        private int _y;

        // Token: 0x0400002F RID: 47
        private int _pictureWidth;

        // Token: 0x04000030 RID: 48
        private int _pictureHeight;

        // Token: 0x04000031 RID: 49
        private int _correctX;

        // Token: 0x04000032 RID: 50
        private int _correctY;

        // Token: 0x04000033 RID: 51
        private int _autoCorrectX;

        // Token: 0x04000034 RID: 52
        private int _autoCorrectY;

        // Token: 0x04000035 RID: 53
        private int _counterAutoFreqCorrect;

        // Token: 0x04000036 RID: 54
        private bool _autoPositionCorrect;

        // Token: 0x04000037 RID: 55
        private bool _autoFrequencyCorrection;

        // Token: 0x04000038 RID: 56
        private int _fineFrequencyCorrection;

        // Token: 0x04000039 RID: 57
        private int _fineTick;

        // Token: 0x0400003A RID: 58
        private int _pictureCentr;

        // Token: 0x0400003B RID: 59
        private float _countryCoeff;

        // Token: 0x0400003C RID: 60
        private int _lineInFrame;

        // Token: 0x0400003D RID: 61
        private int _pixelCounter;

        // Token: 0x0400003E RID: 62
        private int _maxSignalLevel;

        // Token: 0x0400003F RID: 63
        private int _agcSignalLevel;

        // Token: 0x04000040 RID: 64
        private int _blackLevel;

        // Token: 0x04000041 RID: 65
        private int _bright;

        // Token: 0x04000042 RID: 66
        private float _contrast;

        // Token: 0x04000043 RID: 67
        private float _coeff;

        // Token: 0x04000044 RID: 68
        private double _sampleRate;

        // Token: 0x04000045 RID: 69
        private int _frequencyCorrection;

        // Token: 0x04000046 RID: 70
        private SettingsMemoryEntry _settings;

        // Token: 0x04000047 RID: 71
        private readonly SettingsPersister _settingsPersister = new SettingsPersister();

        // Token: 0x04000048 RID: 72
        private VideoWindow videoWindow;

        // Token: 0x04000049 RID: 73
        private bool _inverseVideo;

        // Token: 0x0400004A RID: 74
        private bool _bufferIsFull;

        // Token: 0x0400004B RID: 75
        private static object locker = new object();

        // Token: 0x0400004C RID: 76
        private IContainer components;

        // Token: 0x0400004D RID: 77
        private Button startBtn;

        // Token: 0x0400004E RID: 78
        private System.Windows.Forms.Timer fpsTimer;

        // Token: 0x0400004F RID: 79
        private Label gainLabel;

        // Token: 0x04000050 RID: 80
        private NumericUpDown frequencyCorrectionNumericUpDown;

        // Token: 0x04000051 RID: 81
        private Label label2;

        // Token: 0x04000052 RID: 82
        private TrackBar tunerGainTrackBar;

        // Token: 0x04000053 RID: 83
        private Label tunerTypeLabel;

        // Token: 0x04000054 RID: 84
        private ComboBox deviceComboBox;

        // Token: 0x04000055 RID: 85
        private Label label1;

        // Token: 0x04000056 RID: 86
        private NumericUpDown frequencyNumericUpDown;

        // Token: 0x04000057 RID: 87
        private Label label5;

        // Token: 0x04000058 RID: 88
        private ComboBox samplerateComboBox;

        // Token: 0x04000059 RID: 89
        private Label label3;

        // Token: 0x0400005A RID: 90
        private TrackBar brightnesTrackBar;

        // Token: 0x0400005B RID: 91
        private TrackBar contrastTrackBar;

        // Token: 0x0400005C RID: 92
        private Label label6;

        // Token: 0x0400005D RID: 93
        private Label label7;

        // Token: 0x0400005E RID: 94
        private CheckBox tunerAgcCheckBox;

        // Token: 0x0400005F RID: 95
        private CheckBox rtlAgcCheckBox;

        // Token: 0x04000060 RID: 96
        private CheckBox programAgcCheckBox;

        // Token: 0x04000061 RID: 97
        private System.Windows.Forms.Timer frequencyCorrectionTimer;

        // Token: 0x04000062 RID: 98
        private TrackBar frequencyCorrectionTrackBar;

        // Token: 0x04000063 RID: 99
        private Label label4;

        // Token: 0x04000064 RID: 100
        private TrackBar fineFrequencyCorrectTrackBar;

        // Token: 0x04000065 RID: 101
        private TrackBar xCorrectionTrackBar;

        // Token: 0x04000066 RID: 102
        private TrackBar yCorrectionTrackBar;

        // Token: 0x04000067 RID: 103
        private Label label9;

        // Token: 0x04000068 RID: 104
        private Label label10;

        // Token: 0x04000069 RID: 105
        private Label label11;

        // Token: 0x0400006A RID: 106
        private GroupBox frequencyCorrectionGroupBox;

        // Token: 0x0400006B RID: 107
        private GroupBox positionGroupBox;

        // Token: 0x0400006C RID: 108
        private CheckBox autoSincCheckBox;

        // Token: 0x0400006D RID: 109
        private GroupBox tunerGroupBox;

        // Token: 0x0400006E RID: 110
        private Label label8;

        // Token: 0x0400006F RID: 111
        private Button autoFrequencyCorrectionButton;

        // Token: 0x04000070 RID: 112
        private CheckBox inverseCheckBox;

        // Token: 0x04000071 RID: 113
        private Label label12;
    }
}
