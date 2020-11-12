using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace screencapture
{
    public partial class ControllerUx : Form
    {
        private List<FloaterUx> _Floaters = new List<FloaterUx>();
        private Label score0 = new Label();
        private Label score1 = new Label();
        private int _Counter = 1;
        public ControllerUx()
        {

            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(300, 300);
            this.Opacity = 0.8;
            this.TopMost = true;

            score0.Text = "none";
            score0.Width = 300;

            score1.Text = "none";
            score1.Width = 300;
            score1.Location = new Point(0, 20);

            this.Controls.Add(score0);
            this.Controls.Add(score1);

            Button b = new Button();
            b.Text = "PressMe";
            b.Click += myButton_Click;

            this.Controls.Add(b);

        }

        void myButton_Click(Object sender, EventArgs e)
        {
            int startPos = _Counter * 10;
            _Counter++;

            FloaterUx ux = new FloaterUx(startPos, startPos, startPos + 100, startPos + 100, "HelloWorld");
            ux.Show();
            _Floaters.Add(ux);
        }

        public void SetConfidence(string c0, string c1)
        {
            if (this.InvokeRequired)
            {
                InvokeUI(() =>
                {
                    score0.Text = c0;
                    score1.Text = c1;
                });
            }
            else
            {
                score0.Text = c0;
                score1.Text = c1;
            }


        }

        public void DeleteFloaters()
        {
            foreach (var aFloater in _Floaters)
            {
                if (this.InvokeRequired)
                {
                    InvokeUI(() =>
                    {
                        aFloater.Close();
                    });
                }
                else
                {
                    aFloater.Close();
                }

            }
            _Floaters = new List<FloaterUx>();
        }

        public void AddFloater(int Left, int Top, int Right, int Bottom, string text)
        {
            if (this.InvokeRequired)
            {
                InvokeUI(() =>
                {

                    FloaterUx ux = new FloaterUx(Left, Top, Right, Bottom, text);
                    _Floaters.Add(ux);
                    ux.Show();



                });
            }
            else
            {
                FloaterUx ux = new FloaterUx(Top, Left, Bottom, Right, text);
                _Floaters.Add(ux);
                ux.Show();

                ;
            }

        }

        private void InvokeUI(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
        }





    }
}
