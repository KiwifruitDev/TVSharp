using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace TVSharp
{
    // Token: 0x0200000F RID: 15
    public sealed class RtlDevice : IDisposable
    {
        // Token: 0x06000085 RID: 133 RVA: 0x00005164 File Offset: 0x00003364
        public RtlDevice(uint index)
        {
            this._index = index;
            int num = NativeMethods.rtlsdr_open(out this._dev, this._index);
            if (num != 0)
            {
                throw new ApplicationException("Cannot open RTL device. Is the device locked somewhere?");
            }
            int num2 = (this._dev == IntPtr.Zero) ? 0 : NativeMethods.rtlsdr_get_tuner_gains(this._dev, null);
            if (num2 < 0)
            {
                num2 = 0;
            }
            this._supportsOffsetTuning = (NativeMethods.rtlsdr_set_offset_tuning(this._dev, 0) != -2);
            this._supportedGains = new int[num2];
            if (num2 >= 0)
            {
                NativeMethods.rtlsdr_get_tuner_gains(this._dev, this._supportedGains);
            }
            this._name = NativeMethods.rtlsdr_get_device_name(this._index);
            this._gcHandle = GCHandle.Alloc(this);
        }

        // Token: 0x06000086 RID: 134 RVA: 0x00005248 File Offset: 0x00003448
        ~RtlDevice()
        {
            this.Dispose();
        }

        // Token: 0x06000087 RID: 135 RVA: 0x00005274 File Offset: 0x00003474
        public void Dispose()
        {
            this.Stop();
            NativeMethods.rtlsdr_close(this._dev);
            if (this._gcHandle.IsAllocated)
            {
                this._gcHandle.Free();
            }
            this._dev = IntPtr.Zero;
            GC.SuppressFinalize(this);
        }

        // Token: 0x14000001 RID: 1
        // (add) Token: 0x06000088 RID: 136 RVA: 0x000052B4 File Offset: 0x000034B4
        // (remove) Token: 0x06000089 RID: 137 RVA: 0x000052EC File Offset: 0x000034EC
        public event SamplesAvailableDelegate SamplesAvailable;

        // Token: 0x0600008A RID: 138 RVA: 0x00005324 File Offset: 0x00003524
        public void Start()
        {
            if (this._worker != null)
            {
                throw new ApplicationException("Already running");
            }
            int num = NativeMethods.rtlsdr_set_center_freq(this._dev, this._centerFrequency);
            if (num != 0)
            {
                throw new ApplicationException("Cannot access RTL device");
            }
            num = NativeMethods.rtlsdr_set_tuner_gain_mode(this._dev, this._useTunerAGC ? 0 : 1);
            if (num != 0)
            {
                throw new ApplicationException("Cannot access RTL device");
            }
            if (!this._useTunerAGC)
            {
                num = NativeMethods.rtlsdr_set_tuner_gain(this._dev, this._tunerGain);
                if (num != 0)
                {
                    throw new ApplicationException("Cannot access RTL device");
                }
            }
            num = NativeMethods.rtlsdr_reset_buffer(this._dev);
            if (num != 0)
            {
                throw new ApplicationException("Cannot access RTL device");
            }
            this._worker = new Thread(new ThreadStart(this.StreamProc));
            this._worker.Priority = ThreadPriority.Highest;
            this._worker.Start();
        }

        // Token: 0x0600008B RID: 139 RVA: 0x000053FA File Offset: 0x000035FA
        public void Stop()
        {
            if (this._worker == null)
            {
                return;
            }
            NativeMethods.rtlsdr_cancel_async(this._dev);
            if (this._worker.ThreadState == ThreadState.Running)
            {
                this._worker.Join();
            }
            this._worker = null;
        }

        // Token: 0x17000014 RID: 20
        // (get) Token: 0x0600008C RID: 140 RVA: 0x00005430 File Offset: 0x00003630
        public uint Index
        {
            get
            {
                return this._index;
            }
        }

        // Token: 0x17000015 RID: 21
        // (get) Token: 0x0600008D RID: 141 RVA: 0x00005438 File Offset: 0x00003638
        public string Name
        {
            get
            {
                return this._name;
            }
        }

        // Token: 0x17000016 RID: 22
        // (get) Token: 0x0600008E RID: 142 RVA: 0x00005440 File Offset: 0x00003640
        // (set) Token: 0x0600008F RID: 143 RVA: 0x00005448 File Offset: 0x00003648
        public uint Samplerate
        {
            get
            {
                return this._sampleRate;
            }
            set
            {
                this._sampleRate = value;
                if (this._dev != IntPtr.Zero)
                {
                    NativeMethods.rtlsdr_set_sample_rate(this._dev, this._sampleRate);
                }
            }
        }

        // Token: 0x17000017 RID: 23
        // (get) Token: 0x06000090 RID: 144 RVA: 0x00005475 File Offset: 0x00003675
        // (set) Token: 0x06000091 RID: 145 RVA: 0x00005480 File Offset: 0x00003680
        public uint Frequency
        {
            get
            {
                return this._centerFrequency;
            }
            set
            {
                this._centerFrequency = value;
                if (this._dev != IntPtr.Zero && NativeMethods.rtlsdr_set_center_freq(this._dev, this._centerFrequency) != 0)
                {
                    throw new ArgumentException("The frequency cannot be set: " + value);
                }
            }
        }

        // Token: 0x17000018 RID: 24
        // (get) Token: 0x06000092 RID: 146 RVA: 0x000054CF File Offset: 0x000036CF
        // (set) Token: 0x06000093 RID: 147 RVA: 0x000054D7 File Offset: 0x000036D7
        public bool UseRtlAGC
        {
            get
            {
                return this._useRtlAGC;
            }
            set
            {
                this._useRtlAGC = value;
                if (this._dev != IntPtr.Zero)
                {
                    NativeMethods.rtlsdr_set_agc_mode(this._dev, this._useRtlAGC ? 1 : 0);
                }
            }
        }

        // Token: 0x17000019 RID: 25
        // (get) Token: 0x06000094 RID: 148 RVA: 0x0000550A File Offset: 0x0000370A
        // (set) Token: 0x06000095 RID: 149 RVA: 0x00005512 File Offset: 0x00003712
        public bool UseTunerAGC
        {
            get
            {
                return this._useTunerAGC;
            }
            set
            {
                this._useTunerAGC = value;
                if (this._dev != IntPtr.Zero)
                {
                    NativeMethods.rtlsdr_set_tuner_gain_mode(this._dev, this._useTunerAGC ? 0 : 1);
                }
            }
        }

        // Token: 0x1700001A RID: 26
        // (get) Token: 0x06000096 RID: 150 RVA: 0x00005545 File Offset: 0x00003745
        // (set) Token: 0x06000097 RID: 151 RVA: 0x0000554D File Offset: 0x0000374D
        public SamplingMode SamplingMode
        {
            get
            {
                return this._samplingMode;
            }
            set
            {
                this._samplingMode = value;
                if (this._dev != IntPtr.Zero)
                {
                    NativeMethods.rtlsdr_set_direct_sampling(this._dev, (int)this._samplingMode);
                }
            }
        }

        // Token: 0x1700001B RID: 27
        // (get) Token: 0x06000098 RID: 152 RVA: 0x0000557A File Offset: 0x0000377A
        public bool SupportsOffsetTuning
        {
            get
            {
                return this._supportsOffsetTuning;
            }
        }

        // Token: 0x1700001C RID: 28
        // (get) Token: 0x06000099 RID: 153 RVA: 0x00005582 File Offset: 0x00003782
        // (set) Token: 0x0600009A RID: 154 RVA: 0x0000558A File Offset: 0x0000378A
        public bool UseOffsetTuning
        {
            get
            {
                return this._useOffsetTuning;
            }
            set
            {
                this._useOffsetTuning = value;
                if (this._dev != IntPtr.Zero)
                {
                    NativeMethods.rtlsdr_set_offset_tuning(this._dev, this._useOffsetTuning ? 1 : 0);
                }
            }
        }

        // Token: 0x1700001D RID: 29
        // (get) Token: 0x0600009B RID: 155 RVA: 0x000055BD File Offset: 0x000037BD
        public int[] SupportedGains
        {
            get
            {
                return this._supportedGains;
            }
        }

        // Token: 0x1700001E RID: 30
        // (get) Token: 0x0600009C RID: 156 RVA: 0x000055C5 File Offset: 0x000037C5
        // (set) Token: 0x0600009D RID: 157 RVA: 0x000055CD File Offset: 0x000037CD
        public int TunerGain
        {
            get
            {
                return this._tunerGain;
            }
            set
            {
                this._tunerGain = value;
                if (this._dev != IntPtr.Zero)
                {
                    NativeMethods.rtlsdr_set_tuner_gain(this._dev, this._tunerGain);
                }
            }
        }

        // Token: 0x1700001F RID: 31
        // (get) Token: 0x0600009E RID: 158 RVA: 0x000055FA File Offset: 0x000037FA
        // (set) Token: 0x0600009F RID: 159 RVA: 0x00005602 File Offset: 0x00003802
        public int FrequencyCorrection
        {
            get
            {
                return this._frequencyCorrection;
            }
            set
            {
                this._frequencyCorrection = value;
                if (this._dev != IntPtr.Zero)
                {
                    NativeMethods.rtlsdr_set_freq_correction(this._dev, this._frequencyCorrection);
                }
            }
        }

        // Token: 0x17000020 RID: 32
        // (get) Token: 0x060000A0 RID: 160 RVA: 0x0000562F File Offset: 0x0000382F
        public RtlSdrTunerType TunerType
        {
            get
            {
                if (!(this._dev == IntPtr.Zero))
                {
                    return NativeMethods.rtlsdr_get_tuner_type(this._dev);
                }
                return RtlSdrTunerType.Unknown;
            }
        }

        // Token: 0x17000021 RID: 33
        // (get) Token: 0x060000A1 RID: 161 RVA: 0x00005650 File Offset: 0x00003850
        public bool IsStreaming
        {
            get
            {
                return this._worker != null;
            }
        }

        // Token: 0x060000A2 RID: 162 RVA: 0x0000565E File Offset: 0x0000385E
        private void StreamProc()
        {
            NativeMethods.rtlsdr_read_async(this._dev, RtlDevice._rtlCallback, (IntPtr)this._gcHandle, 0U, RtlDevice._readLength);
        }

        // Token: 0x060000A3 RID: 163 RVA: 0x00005682 File Offset: 0x00003882
        private unsafe void ComplexSamplesAvailable(Complex* buffer, int length)
        {
            if (this.SamplesAvailable != null)
            {
                this._eventArgs.Buffer = buffer;
                this._eventArgs.Length = length;
                this.SamplesAvailable(this, this._eventArgs);
            }
        }

        // Token: 0x060000A4 RID: 164 RVA: 0x000056B8 File Offset: 0x000038B8
        private unsafe static void RtlSdrSamplesAvailable(byte* buf, uint len, IntPtr ctx)
        {
            GCHandle gchandle = GCHandle.FromIntPtr(ctx);
            if (!gchandle.IsAllocated)
            {
                return;
            }
            RtlDevice rtlDevice = (RtlDevice)gchandle.Target;
            int num = (int)(len / 2U);
            if (rtlDevice._iqBuffer == null || rtlDevice._iqBuffer.Length != num)
            {
                rtlDevice._iqBuffer = UnsafeBuffer.Create(num, sizeof(Complex));
                rtlDevice._iqPtr = (Complex*)rtlDevice._iqBuffer;
            }
            Complex* ptr = rtlDevice._iqPtr;
            for (int i = 0; i < num; i++)
            {
                ptr->Imag = (sbyte)(*(buf++) - 128);
                ptr->Real = (sbyte)(*(buf++) - 128);
                ptr++;
            }
            rtlDevice.ComplexSamplesAvailable(rtlDevice._iqPtr, rtlDevice._iqBuffer.Length);
        }

        // Token: 0x04000078 RID: 120
        private const uint DefaultFrequency = 183000000U;

        // Token: 0x04000079 RID: 121
        private const int DefaultSamplerate = 2000000;

        // Token: 0x0400007A RID: 122
        private readonly uint _index;

        // Token: 0x0400007B RID: 123
        private IntPtr _dev;

        // Token: 0x0400007C RID: 124
        private readonly string _name;

        // Token: 0x0400007D RID: 125
        private readonly int[] _supportedGains;

        // Token: 0x0400007E RID: 126
        private bool _useTunerAGC = true;

        // Token: 0x0400007F RID: 127
        private bool _useRtlAGC;

        // Token: 0x04000080 RID: 128
        private int _tunerGain;

        // Token: 0x04000081 RID: 129
        private uint _centerFrequency = 183000000U;

        // Token: 0x04000082 RID: 130
        private uint _sampleRate = 2000000U;

        // Token: 0x04000083 RID: 131
        private int _frequencyCorrection;

        // Token: 0x04000084 RID: 132
        private SamplingMode _samplingMode;

        // Token: 0x04000085 RID: 133
        private bool _useOffsetTuning;

        // Token: 0x04000086 RID: 134
        private readonly bool _supportsOffsetTuning;

        // Token: 0x04000087 RID: 135
        private GCHandle _gcHandle;

        // Token: 0x04000088 RID: 136
        private UnsafeBuffer _iqBuffer;

        // Token: 0x04000089 RID: 137
        private unsafe Complex* _iqPtr;

        // Token: 0x0400008A RID: 138
        private Thread _worker;

        // Token: 0x0400008B RID: 139
        private readonly SamplesAvailableEventArgs _eventArgs = new SamplesAvailableEventArgs();

        // Token: 0x0400008C RID: 140
        private unsafe static readonly Delegates _rtlCallback = new Delegates(RtlDevice.RtlSdrSamplesAvailable);

        // Token: 0x0400008D RID: 141
        private static readonly uint _readLength = 32768U;
    }
}
