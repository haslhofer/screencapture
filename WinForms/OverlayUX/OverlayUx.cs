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
        public OverlayUx()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(100, 100);
            this.Opacity = 0.8;
            this.TopMost = true;
            _p = new PictureBox();
            _p.Size = new Size(1000, 1000);
            this.Controls.Add(_p);

        }

        public void SetBitmap(System.Drawing.Bitmap b)
        {
            if (this._p.InvokeRequired)
            {
                InvokeUI(() => {
                _p.Image = b;
                });
            
            }
            else
            {_p.Image = b;}
                
            
            
        }

        public void ChangePos(int x, int y)
        {
            InvokeUI(() => {
            this.Location = new Point(x,y);
            
            });
        }

        
        private void InvokeUI(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
        }

    }
}
