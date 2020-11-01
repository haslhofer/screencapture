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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(400, 100);
            TextBox b = new TextBox();
            //b.TextChanged += ChangePos;
            b.Text = "Hello";
            this.Controls.Add(b);

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
