using System;

namespace TVSharp
{
    // Token: 0x02000008 RID: 8
    public class SettingsMemoryEntry
    {
        // Token: 0x06000034 RID: 52 RVA: 0x00002420 File Offset: 0x00000620
        public SettingsMemoryEntry()
        {
        }

        // Token: 0x06000035 RID: 53 RVA: 0x00002428 File Offset: 0x00000628
        public SettingsMemoryEntry(SettingsMemoryEntry memoryEntry)
        {
            this._programAgc = memoryEntry._programAgc;
            this._rtlAgc = memoryEntry._rtlAgc;
            this._tunerAgc = memoryEntry._tunerAgc;
            this._autoPositionCorrection = memoryEntry._autoPositionCorrection;
            this._frequencyCorrection = memoryEntry._frequencyCorrection;
            this._samplerate = memoryEntry._samplerate;
            this._frequency = memoryEntry._frequency;
            this._detectorLevel = memoryEntry._detectorLevel;
            this._brightnes = memoryEntry._brightnes;
            this._contrast = memoryEntry._contrast;
            this._palSecamChannelFrequency = memoryEntry._palSecamChannelFrequency;
            this._ntscChannelFequency = memoryEntry._ntscChannelFequency;
            this._inverseVideo = memoryEntry._inverseVideo;
        }

        // Token: 0x17000003 RID: 3
        // (get) Token: 0x06000036 RID: 54 RVA: 0x000024D7 File Offset: 0x000006D7
        // (set) Token: 0x06000037 RID: 55 RVA: 0x000024DF File Offset: 0x000006DF
        public int Brightnes
        {
            get
            {
                return this._brightnes;
            }
            set
            {
                this._brightnes = value;
            }
        }

        // Token: 0x17000004 RID: 4
        // (get) Token: 0x06000038 RID: 56 RVA: 0x000024E8 File Offset: 0x000006E8
        // (set) Token: 0x06000039 RID: 57 RVA: 0x000024F0 File Offset: 0x000006F0
        public bool AutoPositionCorrecion
        {
            get
            {
                return this._autoPositionCorrection;
            }
            set
            {
                this._autoPositionCorrection = value;
            }
        }

        // Token: 0x17000005 RID: 5
        // (get) Token: 0x0600003A RID: 58 RVA: 0x000024F9 File Offset: 0x000006F9
        // (set) Token: 0x0600003B RID: 59 RVA: 0x00002501 File Offset: 0x00000701
        public long[] PalSecamChannelFrequency
        {
            get
            {
                return this._palSecamChannelFrequency;
            }
            set
            {
                this._palSecamChannelFrequency = value;
            }
        }

        // Token: 0x17000006 RID: 6
        // (get) Token: 0x0600003C RID: 60 RVA: 0x0000250A File Offset: 0x0000070A
        // (set) Token: 0x0600003D RID: 61 RVA: 0x00002512 File Offset: 0x00000712
        public long[] ntscChannelFrequency
        {
            get
            {
                return this._ntscChannelFequency;
            }
            set
            {
                this._ntscChannelFequency = value;
            }
        }

        // Token: 0x17000007 RID: 7
        // (get) Token: 0x0600003E RID: 62 RVA: 0x0000251B File Offset: 0x0000071B
        // (set) Token: 0x0600003F RID: 63 RVA: 0x00002523 File Offset: 0x00000723
        public bool ProgramAgc
        {
            get
            {
                return this._programAgc;
            }
            set
            {
                this._programAgc = value;
            }
        }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000040 RID: 64 RVA: 0x0000252C File Offset: 0x0000072C
        // (set) Token: 0x06000041 RID: 65 RVA: 0x00002534 File Offset: 0x00000734
        public bool TunerAgc
        {
            get
            {
                return this._tunerAgc;
            }
            set
            {
                this._tunerAgc = value;
            }
        }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x06000042 RID: 66 RVA: 0x0000253D File Offset: 0x0000073D
        // (set) Token: 0x06000043 RID: 67 RVA: 0x00002545 File Offset: 0x00000745
        public bool RtlAgc
        {
            get
            {
                return this._rtlAgc;
            }
            set
            {
                this._rtlAgc = value;
            }
        }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x06000044 RID: 68 RVA: 0x0000254E File Offset: 0x0000074E
        // (set) Token: 0x06000045 RID: 69 RVA: 0x00002556 File Offset: 0x00000756
        public int FrequencyCorrection
        {
            get
            {
                return this._frequencyCorrection;
            }
            set
            {
                this._frequencyCorrection = value;
            }
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x06000046 RID: 70 RVA: 0x0000255F File Offset: 0x0000075F
        // (set) Token: 0x06000047 RID: 71 RVA: 0x00002567 File Offset: 0x00000767
        public decimal Frequency
        {
            get
            {
                return this._frequency;
            }
            set
            {
                this._frequency = value;
            }
        }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x06000048 RID: 72 RVA: 0x00002570 File Offset: 0x00000770
        // (set) Token: 0x06000049 RID: 73 RVA: 0x00002578 File Offset: 0x00000778
        public int Samplerate
        {
            get
            {
                return this._samplerate;
            }
            set
            {
                this._samplerate = value;
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x0600004A RID: 74 RVA: 0x00002581 File Offset: 0x00000781
        // (set) Token: 0x0600004B RID: 75 RVA: 0x00002589 File Offset: 0x00000789
        public float DetectorLevel
        {
            get
            {
                return this._detectorLevel;
            }
            set
            {
                this._detectorLevel = value;
            }
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x0600004C RID: 76 RVA: 0x00002592 File Offset: 0x00000792
        // (set) Token: 0x0600004D RID: 77 RVA: 0x0000259A File Offset: 0x0000079A
        public int Contrast
        {
            get
            {
                return this._contrast;
            }
            set
            {
                this._contrast = value;
            }
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x0600004E RID: 78 RVA: 0x000025A3 File Offset: 0x000007A3
        // (set) Token: 0x0600004F RID: 79 RVA: 0x000025AB File Offset: 0x000007AB
        public bool InverseVideo
        {
            get
            {
                return this._inverseVideo;
            }
            set
            {
                this._inverseVideo = value;
            }
        }

        // Token: 0x04000011 RID: 17
        private decimal _frequency;

        // Token: 0x04000012 RID: 18
        private int _samplerate;

        // Token: 0x04000013 RID: 19
        private bool _programAgc;

        // Token: 0x04000014 RID: 20
        private bool _rtlAgc;

        // Token: 0x04000015 RID: 21
        private bool _tunerAgc;

        // Token: 0x04000016 RID: 22
        private bool _autoPositionCorrection;

        // Token: 0x04000017 RID: 23
        private float _detectorLevel;

        // Token: 0x04000018 RID: 24
        private int _frequencyCorrection;

        // Token: 0x04000019 RID: 25
        private int _contrast;

        // Token: 0x0400001A RID: 26
        private int _brightnes;

        // Token: 0x0400001B RID: 27
        private long[] _palSecamChannelFrequency;

        // Token: 0x0400001C RID: 28
        private long[] _ntscChannelFequency;

        // Token: 0x0400001D RID: 29
        private bool _inverseVideo;
    }
}
