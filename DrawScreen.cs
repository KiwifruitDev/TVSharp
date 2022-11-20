using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TVSharp
{
    // Token: 0x0200000A RID: 10
    internal class DrawScreen : UserControl
    {
        // Token: 0x06000054 RID: 84 RVA: 0x0000260A File Offset: 0x0000080A
        public DrawScreen()
        {
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.UpdateStyles();
        }

        // Token: 0x06000055 RID: 85 RVA: 0x00002644 File Offset: 0x00000844
        public void ArrayToBitmap(byte[] buf, int width, int height)
        {
            if (this._rect.Width != width || this._rect.Height != height)
            {
                this._rect = new Rectangle(0, 0, width, height);
            }
            if (this._buffer == null || this._buffer.Width != width || this._buffer.Height != height)
            {
                this._buffer = new Bitmap(width, height, PixelFormat.Format8bppIndexed);
                this._buffer.Palette = this.GetGrayScalePalette();
            }
            BitmapData bitmapData = this._buffer.LockBits(this._rect, ImageLockMode.ReadWrite, PixelFormat.Format8bppIndexed);
            IntPtr scan = bitmapData.Scan0;
            Marshal.Copy(buf, 0, scan, buf.Length);
            this._buffer.UnlockBits(bitmapData);
        }

        // Token: 0x06000056 RID: 86 RVA: 0x000026FC File Offset: 0x000008FC
        private ColorPalette GetGrayScalePalette()
        {
            ColorPalette palette = this._buffer.Palette;
            Color[] entries = palette.Entries;
            for (int i = 0; i < 256; i++)
            {
                entries[i] = Color.FromArgb(i, i, i);
            }
            return palette;
        }

        // Token: 0x06000057 RID: 87 RVA: 0x00002744 File Offset: 0x00000944
        protected override void OnPaint(PaintEventArgs e)
        {
            if (this._buffer == null)
            {
                return;
            }
            int num = this._rect.Width;
            int num2 = this._rect.Height;
            int num3 = (int)((double)num * 0.15);
            int num4 = (int)((double)num2 * 0.05);
            num -= num3;
            num2 -= num4;
            e.Graphics.DrawImage(this._buffer, e.ClipRectangle, num3, num4, num, num2, GraphicsUnit.Pixel);
        }

        // Token: 0x06000058 RID: 88 RVA: 0x000027B2 File Offset: 0x000009B2
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        // Token: 0x04000020 RID: 32
        private const PixelFormat _pxf = PixelFormat.Format8bppIndexed;

        // Token: 0x04000021 RID: 33
        private Bitmap _buffer;

        // Token: 0x04000022 RID: 34
        private Rectangle _rect;
    }
}
