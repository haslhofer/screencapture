using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace screencapture
{
    public partial class OaDisplayUx : Form
    {

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public OaDisplayUx()
        {
            InitializeComponent();
        }



        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OaDisplayUx));

            this.components = new System.ComponentModel.Container();
            this.buttonCaptureScreen = new System.Windows.Forms.Button();
            this.screenCapture = new System.Windows.Forms.PictureBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            this.spoolForward = new System.Windows.Forms.Button();
            this.spoolBack = new System.Windows.Forms.Button();
            this.resumeMirror = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.screenCapture)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCaptureScreen
            // 
            this.buttonCaptureScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCaptureScreen.Location = new System.Drawing.Point(1133, 611);
            this.buttonCaptureScreen.Name = "buttonCaptureScreen";
            this.buttonCaptureScreen.Size = new System.Drawing.Size(50, 47);
            this.buttonCaptureScreen.TabIndex = 0;
            this.buttonCaptureScreen.Text = "📌";
            this.buttonCaptureScreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCaptureScreen.UseVisualStyleBackColor = true;
            this.buttonCaptureScreen.Click += CaptureClick_EventHandler;
            // 
            // screenCapture 
            // 
            this.screenCapture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.screenCapture.Location = new System.Drawing.Point(12, 12);
            this.screenCapture.Name = "screenCapture";
            this.screenCapture.Size = new System.Drawing.Size(1176, 593);
            this.screenCapture.TabIndex = 1;
            this.screenCapture.TabStop = false;
            this.screenCapture.SizeMode = PictureBoxSizeMode.Zoom;
            // 
            // statusTextBox
            // 
            this.statusTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusTextBox.Enabled = false;
            this.statusTextBox.Location = new System.Drawing.Point(12, 611);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(969, 26);
            this.statusTextBox.TabIndex = 3;

            // spoolForward
            // 
            this.spoolForward.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.spoolForward.Location = new System.Drawing.Point(1037, 611);
            this.spoolForward.Name = "spoolForward";
            this.spoolForward.Size = new System.Drawing.Size(37, 47);
            this.spoolForward.TabIndex = 4;
            this.spoolForward.Text = "▶️";
            this.spoolForward.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spoolForward.UseVisualStyleBackColor = true;
            this.spoolForward.Click += SpoolForwardClick_EventHandler;
            // 
            // spoolBack
            // 
            this.spoolBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));

            this.spoolBack.Location = new System.Drawing.Point(994, 611);
            this.spoolBack.Name = "spoolBack";
            this.spoolBack.Size = new System.Drawing.Size(37, 47);
            this.spoolBack.TabIndex = 5;
            this.spoolBack.Text = "◀️";
            this.spoolBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.spoolBack.UseVisualStyleBackColor = true;
            this.spoolBack.Click += SpoolBackClick_EventHandler;
            // 
            // resumeMirror
            // 
            this.resumeMirror.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.resumeMirror.Location = new System.Drawing.Point(1080, 611);
            this.resumeMirror.Name = "resumeMirror";
            this.resumeMirror.Size = new System.Drawing.Size(37, 47);
            this.resumeMirror.TabIndex = 6;
            this.resumeMirror.Text = "▶️▶️";
            this.resumeMirror.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resumeMirror.UseVisualStyleBackColor = true;
            this.resumeMirror.Click += ResumeMirrorClick_EventHandler;
            
            notifyIcon1 = new NotifyIcon(this.components);
             
            var embeddedProvider = new Microsoft.Extensions.FileProviders.EmbeddedFileProvider(System.Reflection.Assembly.GetExecutingAssembly());
            using (var reader = embeddedProvider.GetFileInfo("resources\\PDF.ico").CreateReadStream())
            {
                notifyIcon1.Icon  = new Icon(reader);
            }
            notifyIcon1.Visible = true;
            notifyIcon1.Text = "Observational Assistance";
            notifyIcon1.Click += OnNotify_Click;


            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 670);
            this.Controls.Add(this.resumeMirror);
            this.Controls.Add(this.spoolBack);
            this.Controls.Add(this.spoolForward);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.screenCapture);
            this.Controls.Add(this.buttonCaptureScreen);
            
            this.Name = "Observational Assistance";
            this.Text = "Observational Assistance";
            ((System.ComponentModel.ISupportInitialize)(this.screenCapture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCaptureScreen;
        private System.Windows.Forms.PictureBox screenCapture;
        private System.Windows.Forms.TextBox statusTextBox;

        private System.Windows.Forms.Button spoolForward;
        private System.Windows.Forms.Button spoolBack;
        private System.Windows.Forms.Button resumeMirror;
        private System.Windows.Forms.NotifyIcon notifyIcon1;

        private long _viewToken = 0;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();



        private async void OnNotify_Click(Object sender, EventArgs e)
        {
            CaptureClick_EventHandler(sender, e);
        }

        private async void CaptureClick_EventHandler(Object sender, EventArgs e)
        {
            Logger.Info("Before write captured screenshot to OneNote");
            //var img = Program.CacheWorker.GenerateTopNImages(2);

            Image imageToWrite = screenCapture.Image;
            var capResult = await GenerateSummary.CaptureSummaryToOneNote(imageToWrite);

            if (!capResult.IsSuccess)
            {
                MessageBox.Show(capResult.UserMessage, "Capture Image", MessageBoxButtons.OK);
            }
            else
            {
                this.statusTextBox.Text = capResult.UserMessage;
            }
        }

        private void SetStatusFromToken()
        {
            if (_viewToken == 0)
            {
                this.statusTextBox.Text = "";
            }
            else
            {
                DateTime d = new DateTime(_viewToken);
                this.statusTextBox.Text = d.ToLongTimeString();
            }
        }
        private void EnsureRetrieve()
        {
            if (Program.CacheWorker.GetCacheMode() == ImageCacheWorker.CacheMode.Capture)
            {
                _viewToken = Program.CacheWorker.StartRetrieve();
            }
        }
        private void SpoolBackClick_EventHandler(Object sender, EventArgs e)
        {
            EnsureRetrieve();
            var payload = Program.CacheWorker.GetPreviousFromToken(_viewToken);
            if (payload != null)
            {
                screenCapture.Image = payload.Item1;
                _viewToken = payload.Item2;
            }
            SetStatusFromToken();
        }
        private void SpoolForwardClick_EventHandler(Object sender, EventArgs e)
        {
            EnsureRetrieve();
            var payload = Program.CacheWorker.GetNextFromToken(_viewToken);
            if (payload != null)
            {
                screenCapture.Image = payload.Item1;
                _viewToken = payload.Item2;
            }
            SetStatusFromToken();
        }
        private void ResumeMirrorClick_EventHandler(Object sender, EventArgs e)
        {
            _viewToken = 0;
            Program.CacheWorker.ResumeCache();
            SetStatusFromToken();
        }

        public void SetImage(Bitmap img)
        {
            

            if (Program.CacheWorker.GetCacheMode() == ImageCacheWorker.CacheMode.Capture)
            {
                Program.MotionDetectionWorker.OverlayDeltaToBitmap(img);
                
                if (this.InvokeRequired)
                {
                    InvokeUI(() =>
                    {
                        screenCapture.Image = img;
                    });
                }
                else
                {
                    screenCapture.Image = img;
                }
            }

        }

        private void InvokeUI(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
        }


    }
}