using System;
using System.Runtime.InteropServices;
using System.Drawing;

using System.Drawing.Imaging;




namespace screencapture
{
    
    class Program
    {
        private static int sleepDefaultMS = 1000;


        static int Main(
            string directory = "",
            bool loopforever = false
            )
        {
            if (directory == String.Empty) {directory = @"c:\data\";}
         
            do
            {
                ScreenCapture sc = new ScreenCapture();
                // capture entire screen, and save it to a file
                Image img = sc.CaptureScreen();
                // display image in a Picture control named imageDisplay
                // capture this window, and save it
                //sc.CaptureWindowToFile(this.Handle,"C:\\temp2.gif",ImageFormat.Gif);
                string path = System.IO.Path.Combine(directory, GetFileName());
                sc.CaptureScreenToFile(path, ImageFormat.Jpeg);
                Console.WriteLine("Captured at " + path);
                if (loopforever)
                {
                    System.Threading.Thread.Sleep(sleepDefaultMS);
                }
            }
            while(loopforever);
            return 0;
        }

        private static string GetFileName()
        {
            long ticks = DateTime.Now.Ticks;
            return "capture_" + ticks.ToString().Trim() + ".jpeg";
        }
    }
}
