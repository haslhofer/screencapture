using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace screencapture
{
    public partial class OverlayUx : Form
    {

        private PictureBox _p;
        public OverlayUx(int Top, int Left, int Bottom, int Right)
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Top, Left);
            this.Size = new Size(Right - Left, Bottom - Top);
            this.Opacity = 0.0;
            this.TopMost = true;
            _p = new PictureBox();
            _p.Size = new Size(Right - Left, Bottom - Top);
            this.Controls.Add(_p);

        }

        public void HideOverlay()
        {
            SetOpacity(0.1);
        }
        public void ShowOverlay()
        {
            SetOpacity(0.9);
        }

        private void SetOpacity(double opacity)
        {
            if (this._p.InvokeRequired)
            {
                InvokeUI(() =>
                {
                    this.Opacity = opacity;
                });
            }
            else
            {
                this.Opacity = opacity;
            }
        }


        public void SetBitmap(System.Drawing.Bitmap b)
        {
            if (this._p.InvokeRequired)
            {
                InvokeUI(() =>
                {
                    _p.Image = b;
                });

            }
            else
            { _p.Image = b; }



        }

        public void ChangePos(int x, int y)
        {
            InvokeUI(() =>
            {
                this.Location = new Point(x, y);

            });
        }


        private void InvokeUI(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
        }

    }
}
