using System;
using System.Runtime.InteropServices;

namespace TVSharp
{
    // Token: 0x02000003 RID: 3
    // (Invoke) Token: 0x0600000C RID: 12
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void Delegates(byte* buf, uint len, IntPtr ctx);
    // Token: 0x02000010 RID: 16
    // (Invoke) Token: 0x060000A7 RID: 167
    public delegate void SamplesAvailableDelegate(object sender, SamplesAvailableEventArgs e);
    // Token: 0x02000013 RID: 19
    // (Invoke) Token: 0x060000B0 RID: 176
    public unsafe delegate void SamplesReadyDelegate(object sender, Complex* data, int length);
}
