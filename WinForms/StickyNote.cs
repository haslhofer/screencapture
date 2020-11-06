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
        private NoteReference _noteReference;
        Label _noteText;
        
        public StickyNote()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(400, 100);
            this.Opacity = 0.8;
            this.TopMost = true;
            _noteText = new Label();
            this.Controls.Add(_noteText);

        }

        public void InitNote(NoteReference r)
        {
            _noteReference = r;
            InvokeUI(() => {
                this._noteText.Text = r.Note;
            });
            
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
