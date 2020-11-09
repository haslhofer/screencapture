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
    public partial class FloaterUx : Form
    {

        private Label _p;
        public FloaterUx(int Left, int Top, int Right,int Bottom, string text)
        {
            InitializeComponent();

            Size hardCodedSize = new Size(100,100);

            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Left, Top);
            //this.Size = new Size(Right - Left, Bottom - Top);
            this.Size = hardCodedSize;
            this.Opacity = 0.9;
            this.BackColor = Color.Yellow;
            this.TopMost = true;
            _p = new Label();
            _p.Paint += label1_Paint;
            _p.Font = new Font(FontFamily.GenericSansSerif, 14);
            
            _p.Text = text;
            //_p.BorderStyle = BorderStyle.FixedSingle;
            
            //_p.Size = new Size(Right - Left, Bottom - Top);
            _p.Size = hardCodedSize;

            this.Controls.Add(_p);

        }

        void label1_Paint(object sender, PaintEventArgs e)
        {
            //ControlPaint.DrawBorder(e.Graphics,_p.DisplayRectangle, Color.Blue, ButtonBorderStyle.Solid);
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


        /* public void SetBitmap(System.Drawing.Bitmap b)
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
        */
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
