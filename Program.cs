using System;
using System.Runtime.InteropServices;
using System.Drawing;

using System.Drawing.Imaging;




namespace screencapture
{
    
    class Program
    {

        static void Main(string[] args)
        {
            
            ScreenCapture sc = new ScreenCapture();
            // capture entire screen, and save it to a file
            Image img = sc.CaptureScreen();
            // display image in a Picture control named imageDisplay
            // capture this window, and save it
            //sc.CaptureWindowToFile(this.Handle,"C:\\temp2.gif",ImageFormat.Gif);
            sc.CaptureScreenToFile(@"C:\data\test.jpg", ImageFormat.Jpeg);
            Console.WriteLine("Hello World Gerald!");
        }
    }
}
