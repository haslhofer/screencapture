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

        public void SetImage(Image img)
        {
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

        private void InvokeUI(Action a)
        {
            this.BeginInvoke(new MethodInvoker(a));
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
            this.buttonCaptureScreen = new System.Windows.Forms.Button();
            this.screenCapture = new System.Windows.Forms.PictureBox();
            this.statusTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.screenCapture)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonCaptureScreen
            // 
            this.buttonCaptureScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCaptureScreen.Location = new System.Drawing.Point(1043, 611);
            this.buttonCaptureScreen.Name = "buttonCaptureScreen";
            this.buttonCaptureScreen.Size = new System.Drawing.Size(145, 47);
            this.buttonCaptureScreen.TabIndex = 0;
            this.buttonCaptureScreen.Text = "Capture Screen";
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
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 670);
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

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();


        private async void CaptureClick_EventHandler(Object sender,EventArgs e)
        {

            Logger.Info("Before write captured screenshot to OneNote");

            Image imageToWrite = screenCapture.Image;
            string pageId = Configurator.DestinationOneNote.PageId;
            string res = await OneNoteCapture.AppendImage(imageToWrite, pageId);
            Logger.Info("After write captured screenshot to OneNote");

            MessageBox.Show(res, "Captured page", MessageBoxButtons.OK);

        }
    }
}