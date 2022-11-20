using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TVSharp
{
    // Token: 0x02000005 RID: 5
    public class NativeMethods
    {
        // Token: 0x0600000F RID: 15
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint rtlsdr_get_device_count();

        // Token: 0x06000010 RID: 16
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl, EntryPoint = "rtlsdr_get_device_name")]
        private static extern IntPtr rtlsdr_get_device_name_native(uint index);

        // Token: 0x06000011 RID: 17 RVA: 0x0000218C File Offset: 0x0000038C
        public static string rtlsdr_get_device_name(uint index)
        {
            IntPtr ptr = NativeMethods.rtlsdr_get_device_name_native(index);
            return Marshal.PtrToStringAnsi(ptr);
        }

        // Token: 0x06000012 RID: 18
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_device_usb_strings(uint index, StringBuilder manufact, StringBuilder product, StringBuilder serial);

        // Token: 0x06000013 RID: 19
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_open(out IntPtr dev, uint index);

        // Token: 0x06000014 RID: 20
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_close(IntPtr dev);

        // Token: 0x06000015 RID: 21
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_xtal_freq(IntPtr dev, uint rtlFreq, uint tunerFreq);

        // Token: 0x06000016 RID: 22
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_xtal_freq(IntPtr dev, out uint rtlFreq, out uint tunerFreq);

        // Token: 0x06000017 RID: 23
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_usb_strings(IntPtr dev, StringBuilder manufact, StringBuilder product, StringBuilder serial);

        // Token: 0x06000018 RID: 24
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_center_freq(IntPtr dev, uint freq);

        // Token: 0x06000019 RID: 25
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint rtlsdr_get_center_freq(IntPtr dev);

        // Token: 0x0600001A RID: 26
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_freq_correction(IntPtr dev, int ppm);

        // Token: 0x0600001B RID: 27
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_freq_correction(IntPtr dev);

        // Token: 0x0600001C RID: 28
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_tuner_gains(IntPtr dev, [In][Out] int[] gains);

        // Token: 0x0600001D RID: 29
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern RtlSdrTunerType rtlsdr_get_tuner_type(IntPtr dev);

        // Token: 0x0600001E RID: 30
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_tuner_gain(IntPtr dev, int gain);

        // Token: 0x0600001F RID: 31
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_get_tuner_gain(IntPtr dev);

        // Token: 0x06000020 RID: 32
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_tuner_gain_mode(IntPtr dev, int manual);

        // Token: 0x06000021 RID: 33
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_agc_mode(IntPtr dev, int on);

        // Token: 0x06000022 RID: 34
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_direct_sampling(IntPtr dev, int on);

        // Token: 0x06000023 RID: 35
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_offset_tuning(IntPtr dev, int on);

        // Token: 0x06000024 RID: 36
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_sample_rate(IntPtr dev, uint rate);

        // Token: 0x06000025 RID: 37
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern uint rtlsdr_get_sample_rate(IntPtr dev);

        // Token: 0x06000026 RID: 38
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_set_testmode(IntPtr dev, int on);

        // Token: 0x06000027 RID: 39
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_reset_buffer(IntPtr dev);

        // Token: 0x06000028 RID: 40
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_read_sync(IntPtr dev, IntPtr buf, int len, out int nRead);

        // Token: 0x06000029 RID: 41
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_wait_async(IntPtr dev, Delegates cb, IntPtr ctx);

        // Token: 0x0600002A RID: 42
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_read_async(IntPtr dev, Delegates cb, IntPtr ctx, uint bufNum, uint bufLen);

        // Token: 0x0600002B RID: 43
        [DllImport("rtlsdr", CallingConvention = CallingConvention.Cdecl)]
        public static extern int rtlsdr_cancel_async(IntPtr dev);

        // Token: 0x0400000C RID: 12
        private const string LibRtlSdr = "rtlsdr";
    }
}
