using System;
using System.Runtime.InteropServices;

namespace TVSharp
{
    // Token: 0x02000002 RID: 2
    public sealed class UnsafeBuffer : IDisposable
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        private unsafe UnsafeBuffer(Array buffer, int realLength, bool aligned)
        {
            this._buffer = buffer;
            this._handle = GCHandle.Alloc(this._buffer, GCHandleType.Pinned);
            this._ptr = (void*)this._handle.AddrOfPinnedObject();
            if (aligned)
            {
                this._ptr = (void*)((byte)this._ptr + 15L & -16L);
            }
            this._length = realLength;
        }

        // Token: 0x06000002 RID: 2 RVA: 0x000020B4 File Offset: 0x000002B4
        ~UnsafeBuffer()
        {
            this.Dispose();
        }

        // Token: 0x06000003 RID: 3 RVA: 0x000020E0 File Offset: 0x000002E0
        public unsafe void Dispose()
        {
            if (this._handle.IsAllocated)
            {
                this._handle.Free();
            }
            this._buffer = null;
            this._ptr = null;
            this._length = 0;
            GC.SuppressFinalize(this);
        }

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x06000004 RID: 4 RVA: 0x00002127 File Offset: 0x00000327
        public unsafe void* Address
        {
            get
            {
                return this._ptr;
            }
        }

        // Token: 0x17000002 RID: 2
        // (get) Token: 0x06000005 RID: 5 RVA: 0x0000212F File Offset: 0x0000032F
        public int Length
        {
            get
            {
                return this._length;
            }
        }

        // Token: 0x06000006 RID: 6 RVA: 0x00002137 File Offset: 0x00000337
        public unsafe static implicit operator void*(UnsafeBuffer unsafeBuffer)
        {
            return unsafeBuffer.Address;
        }

        // Token: 0x06000007 RID: 7 RVA: 0x0000213F File Offset: 0x0000033F
        public static UnsafeBuffer Create(int size)
        {
            return UnsafeBuffer.Create(1, size, true);
        }

        // Token: 0x06000008 RID: 8 RVA: 0x00002149 File Offset: 0x00000349
        public static UnsafeBuffer Create(int length, int sizeOfElement)
        {
            return UnsafeBuffer.Create(length, sizeOfElement, true);
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002154 File Offset: 0x00000354
        public static UnsafeBuffer Create(int length, int sizeOfElement, bool aligned)
        {
            byte[] buffer = new byte[length * sizeOfElement + (aligned ? 16 : 0)];
            return new UnsafeBuffer(buffer, length, aligned);
        }

        // Token: 0x0600000A RID: 10 RVA: 0x0000217B File Offset: 0x0000037B
        public static UnsafeBuffer Create(Array buffer)
        {
            return new UnsafeBuffer(buffer, buffer.Length, false);
        }

        // Token: 0x04000001 RID: 1
        private readonly GCHandle _handle;

        // Token: 0x04000002 RID: 2
        private unsafe void* _ptr;

        // Token: 0x04000003 RID: 3
        private int _length;

        // Token: 0x04000004 RID: 4
        private Array _buffer;
    }
}
