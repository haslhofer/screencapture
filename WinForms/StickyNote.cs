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
    public partial class StickyNote : Form
    {
        private string _NoteText;
        public StickyNote(string NoteText)
        {
            _NoteText = NoteText;
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(400, 100);
            Label b = new Label();
            //b.TextChanged += ChangePos;
            b.Text = NoteText;
            this.Controls.Add(b);
            this.Opacity = 0.8;
            this.TopMost = true;

        }

        public void ChangePos(int x, int y)
        {
            InvokeUI(() => {
            this.Location = new Point(x,y);
            });
        }

        public void HideNote()
        {
              InvokeUI(() => {
            this.Hide();
            });

        }
        public void ShowNote()
        {
              InvokeUI(() => {
            this.Show();
            });

        }

        private void InvokeUI(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
        }

    }
}
