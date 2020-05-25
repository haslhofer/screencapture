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

            User32.SetProcessDPIAware();

            if (directory == String.Empty) {directory = @"c:\data\temp\";}

            //Retrieve Monitor configuration, so we can enumerate all displays
            MonitorHelper h = new MonitorHelper();
            var displays = h.GetDisplays();
            ScreenCapture sc = new ScreenCapture();
            
            do
            {
                DateTime capturedTime = DateTime.Now;
                int deviceCount = 0;
                foreach (var aDisplay in displays)
                {
                    string deviceName = aDisplay.DeviceName;
                    // Get DC from device name
                    Image img = sc.CaptureWindowFromDevice(deviceName, aDisplay.ScreenWidth, aDisplay.ScreenHeight);
                    string path = System.IO.Path.Combine(directory, GetFileName(deviceCount.ToString(), capturedTime));
                    img.Save(path, ImageFormat.Jpeg);
                    Console.WriteLine("Captured at " + path);
                    deviceCount++;
                }
                if (loopforever)
                {
                    System.Threading.Thread.Sleep(sleepDefaultMS);
                }
            }
            while(loopforever);

            return 0;
        }

      

        private static string GetFileName(string display, DateTime captureTime)
        {
            long ticks = captureTime.Ticks;
            return "capture_" + ticks.ToString().Trim() + "_"+ display + ".jpeg";
        }
    }
}
