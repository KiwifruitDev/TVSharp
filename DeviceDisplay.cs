using System;

namespace TVSharp
{
    // Token: 0x0200000D RID: 13
    public class DeviceDisplay
    {
        // Token: 0x17000012 RID: 18
        // (get) Token: 0x0600007E RID: 126 RVA: 0x000050E5 File Offset: 0x000032E5
        // (set) Token: 0x0600007F RID: 127 RVA: 0x000050ED File Offset: 0x000032ED
        public uint Index { get; private set; }

        // Token: 0x17000013 RID: 19
        // (get) Token: 0x06000080 RID: 128 RVA: 0x000050F6 File Offset: 0x000032F6
        // (set) Token: 0x06000081 RID: 129 RVA: 0x000050FE File Offset: 0x000032FE
        public string Name { get; set; }

        // Token: 0x06000082 RID: 130 RVA: 0x00005108 File Offset: 0x00003308
        public static DeviceDisplay[] GetActiveDevices()
        {
            uint num = NativeMethods.rtlsdr_get_device_count();
            DeviceDisplay[] array = new DeviceDisplay[num];
            for (uint num2 = 0U; num2 < num; num2 += 1U)
            {
                string name = NativeMethods.rtlsdr_get_device_name(num2);
                array[(int)((UIntPtr)num2)] = new DeviceDisplay
                {
                    Index = num2,
                    Name = name
                };
            }
            return array;
        }

        // Token: 0x06000083 RID: 131 RVA: 0x00005154 File Offset: 0x00003354
        public override string ToString()
        {
            return this.Name;
        }
    }
}
