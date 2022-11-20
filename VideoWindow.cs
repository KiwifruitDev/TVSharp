using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TVSharp
{
    // Token: 0x02000006 RID: 6
    public class VideoWindow : Form
    {
        // Token: 0x0600002D RID: 45 RVA: 0x000021AE File Offset: 0x000003AE
        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Token: 0x0600002E RID: 46 RVA: 0x000021D0 File Offset: 0x000003D0
        private void InitializeComponent()
        {
            this.videoPanel = new DrawScreen();
            base.SuspendLayout();
            this.videoPanel.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            this.videoPanel.Location = new Point(2, 3);
            this.videoPanel.Name = "videoPanel";
            this.videoPanel.Size = new Size(421, 323);
            this.videoPanel.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.AutoValidate = AutoValidate.EnableAllowFocusChange;
            base.ClientSize = new Size(424, 327);
            base.ControlBox = false;
            base.Controls.Add(this.videoPanel);
            base.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            base.Name = "VideoWindow";
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
            this.Text = "Video";
            base.ResizeBegin += this.VideoWindows_ResizeBegin;
            base.Resize += this.VideoWindows_Resize;
            base.ResumeLayout(false);
        }

        // Token: 0x0600002F RID: 47 RVA: 0x000022E9 File Offset: 0x000004E9
        public VideoWindow()
        {
            this.InitializeComponent();
        }

        // Token: 0x06000030 RID: 48 RVA: 0x000022F7 File Offset: 0x000004F7
        public void DrawPictures(byte[] buf, int pictureWidth, int pictureHeight)
        {
            this.videoPanel.ArrayToBitmap(buf, pictureWidth, pictureHeight);
            this.videoPanel.Refresh();
        }

        // Token: 0x06000031 RID: 49 RVA: 0x00002314 File Offset: 0x00000514
        private void VideoWindows_ResizeBegin(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            this._oldVideoWindowWidth = control.Size.Width;
            this._oldVideoWindowHeight = control.Size.Height;
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00002350 File Offset: 0x00000550
        private void VideoWindows_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            int num = control.Size.Width;
            int num2 = control.Size.Height;
            if (this._oldVideoWindowWidth != num && this._oldVideoWindowHeight == num2)
            {
                num2 = num / 4 * 3;
            }
            else if (this._oldVideoWindowWidth == num && this._oldVideoWindowHeight != num2)
            {
                num = num2 / 3 * 4;
            }
            else
            {
                num2 = num / 4 * 3;
            }
            control.Size = new Size(num, num2);
        }

        // Token: 0x0400000D RID: 13
        private IContainer components;

        // Token: 0x0400000E RID: 14
        private DrawScreen videoPanel;

        // Token: 0x0400000F RID: 15
        private int _oldVideoWindowWidth;

        // Token: 0x04000010 RID: 16
        private int _oldVideoWindowHeight;
    }
}
