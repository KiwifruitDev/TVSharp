using System;

namespace TVSharp
{
    // Token: 0x02000011 RID: 17
    public sealed class SamplesAvailableEventArgs : EventArgs
    {
        // Token: 0x17000022 RID: 34
        // (get) Token: 0x060000AA RID: 170 RVA: 0x0000579E File Offset: 0x0000399E
        // (set) Token: 0x060000AB RID: 171 RVA: 0x000057A6 File Offset: 0x000039A6
        public int Length { get; set; }

        // Token: 0x17000023 RID: 35
        // (get) Token: 0x060000AC RID: 172 RVA: 0x000057AF File Offset: 0x000039AF
        // (set) Token: 0x060000AD RID: 173 RVA: 0x000057B7 File Offset: 0x000039B7
        public unsafe Complex* Buffer { get; set; }
    }
}