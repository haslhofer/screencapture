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
        
        private PictureBox _pic = new PictureBox();
        private Size FormDefaultSize = new Size(1600, 800);
        private int MarginForButtons = 100;
        private Button _captureButton = new Button();


        public ControllerUx()
        {


            InitializeComponent();
            //this.StartPosition = FormStartPosition.Manual;
            //this.Location = new Point(300, 300);
            
            this.TopMost = true;
            
            this.Size = FormDefaultSize;
            

            
            _pic.SizeMode = PictureBoxSizeMode.Zoom;
            _pic.BorderStyle = BorderStyle.FixedSingle;

            SetPic();

            _captureButton.Text = "Capture";
            _captureButton.Height = MarginForButtons;
            _captureButton.Click += doCapture_click;
            
            SetButton();

            this.Controls.Add(_captureButton);
            this.Controls.Add(_pic);


            this.Resize += OnResize;



        }

        private void SetButton()
        {
            _captureButton.Location =new Point(0,0);
            _captureButton.Width = 200;

        }
        private void SetPic()
        {
            _pic.Location = new Point(0, MarginForButtons);
            _pic.Size = new Size(this.Width, this.Height - MarginForButtons);

        }



        private Point GetButtonLocation()
        {
            return new Point(0, 0);
        }

        protected void OnResize (object sender, System.EventArgs e)
        {
           SetPic();
           SetButton();

        }

        private void doCapture_click(Object sender, EventArgs e)
        {

            //ToDo

        }
        
        public void SetImage(Image img)
        {
            if (this.InvokeRequired)
            {
                InvokeUI(() =>
                {
                    _pic.Image = img;
                });
            }
            else
            {
                _pic.Image = img;
            }

        }

        public async Task<string> TryOcr()
        {
            string r = string.Empty;
            if (this.InvokeRequired)
            {
                InvokeUI(async () =>
                {
                    OcrHelperWindows.InitOCr();
                    r = await OcrHelperWindows.GetFullText2(@"C:\data\temp2\capture_Image_637403612543211215_0.jpg");


                });
            }
            else
            {
                r = String.Empty;
            }

            return r;

        }
        private void InvokeUI(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
        }





    }
}
