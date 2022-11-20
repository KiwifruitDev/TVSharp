using System;

namespace TVSharp
{
    // Token: 0x02000014 RID: 20
    public class RtlSdrIO : IDisposable
    {
        // Token: 0x060000B3 RID: 179 RVA: 0x000057C8 File Offset: 0x000039C8
        ~RtlSdrIO()
        {
            this.Dispose();
        }

        // Token: 0x060000B4 RID: 180 RVA: 0x000057F4 File Offset: 0x000039F4
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        // Token: 0x060000B5 RID: 181 RVA: 0x000057FC File Offset: 0x000039FC
        public void SelectDevice(uint index)
        {
            this.Close();
            this._rtlDevice = new RtlDevice(index);
            this._rtlDevice.SamplesAvailable += this.rtlDevice_SamplesAvailable;
            this._rtlDevice.Frequency = this._frequency;
        }

        // Token: 0x17000024 RID: 36
        // (get) Token: 0x060000B6 RID: 182 RVA: 0x00005838 File Offset: 0x00003A38
        public RtlDevice Device
        {
            get
            {
                return this._rtlDevice;
            }
        }

        // Token: 0x060000B7 RID: 183 RVA: 0x00005840 File Offset: 0x00003A40
        public void Open()
        {
            DeviceDisplay[] activeDevices = DeviceDisplay.GetActiveDevices();
            foreach (DeviceDisplay deviceDisplay in activeDevices)
            {
                try
                {
                    this.SelectDevice(deviceDisplay.Index);
                    return;
                }
                catch (ApplicationException)
                {
                }
            }
            if (activeDevices.Length > 0)
            {
                throw new ApplicationException(activeDevices.Length + " compatible devices have been found but are all busy");
            }
            throw new ApplicationException("No compatible devices found");
        }

        // Token: 0x060000B8 RID: 184 RVA: 0x000058B4 File Offset: 0x00003AB4
        public void Close()
        {
            if (this._rtlDevice != null)
            {
                this._rtlDevice.Stop();
                this._rtlDevice.SamplesAvailable -= this.rtlDevice_SamplesAvailable;
                this._rtlDevice.Dispose();
                this._rtlDevice = null;
            }
        }

        // Token: 0x060000B9 RID: 185 RVA: 0x000058F4 File Offset: 0x00003AF4
        public void Start(SamplesReadyDelegate callback)
        {
            if (this._rtlDevice == null)
            {
                throw new ApplicationException("No device selected");
            }
            this._callback = callback;
            try
            {
                this._rtlDevice.Start();
            }
            catch
            {
                this.Open();
                this._rtlDevice.Start();
            }
        }

        // Token: 0x060000BA RID: 186 RVA: 0x0000594C File Offset: 0x00003B4C
        public void Stop()
        {
            this._rtlDevice.Stop();
        }

        // Token: 0x17000025 RID: 37
        // (get) Token: 0x060000BB RID: 187 RVA: 0x00005959 File Offset: 0x00003B59
        public double Samplerate
        {
            get
            {
                if (this._rtlDevice != null)
                {
                    return this._rtlDevice.Samplerate;
                }
                return 0.0;
            }
        }

        // Token: 0x17000026 RID: 38
        // (get) Token: 0x060000BC RID: 188 RVA: 0x0000597A File Offset: 0x00003B7A
        // (set) Token: 0x060000BD RID: 189 RVA: 0x00005983 File Offset: 0x00003B83
        public long Frequency
        {
            get
            {
                return (long)((ulong)this._frequency);
            }
            set
            {
                this._frequency = (uint)value;
                if (this._rtlDevice != null)
                {
                    this._rtlDevice.Frequency = this._frequency;
                }
            }
        }

        // Token: 0x060000BE RID: 190 RVA: 0x000059A6 File Offset: 0x00003BA6
        private unsafe void rtlDevice_SamplesAvailable(object sender, SamplesAvailableEventArgs e)
        {
            this._callback(this, e.Buffer, e.Length);
        }

        // Token: 0x04000093 RID: 147
        private RtlDevice _rtlDevice;

        // Token: 0x04000094 RID: 148
        private uint _frequency = 224000000U;

        // Token: 0x04000095 RID: 149
        private SamplesReadyDelegate _callback;
    }
}
