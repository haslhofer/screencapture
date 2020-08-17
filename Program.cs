using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;


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

            //bool result = User32.SetProcessDPIAware();

            bool result = SHCore.SetProcessDpiAwareness(SHCore.PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware);
            var setDpiError = Marshal.GetLastWin32Error();

            if (directory == String.Empty) {directory = @"c:\data\temp\";}

        

            //Retrieve Monitor configuration, so we can enumerate all displays
            MonitorHelper h = new MonitorHelper();
            var displays = h.GetDisplays();

            
            
            ScreenCapture sc = new ScreenCapture();
            
            do
            {

                List<MonitorInfo> monitorInfos = new List<MonitorInfo>();

                DateTime capturedTime = DateTime.Now;
                int deviceCount = 0;

                List<ScreenText> ocrResults = new List<ScreenText>();

                foreach (var aDisplay in displays)
                {
                    MonitorInfo aMonitorInfo = new MonitorInfo();
                    aMonitorInfo.ID = deviceCount;

                    aMonitorInfo.Rect = new ScreenRectangle(aDisplay.MonitorArea);
                    
                    string deviceName = aDisplay.DeviceName;
                    // Get DC from device name
                    Image img = sc.CaptureWindowFromDevice(deviceName, aDisplay.ScreenWidth, aDisplay.ScreenHeight);
                    string path = System.IO.Path.Combine(directory, GetFileName(deviceCount.ToString(), capturedTime));
                    img.Save(path, ImageFormat.Jpeg);
                    aMonitorInfo.ImageFullPath = path;
                    Console.WriteLine("Captured at " + path);

                    List<ScreenText> ocrText =  OcrHelper.GetScreenTexts(path, deviceCount);
                    ocrResults.AddRange(ocrText);

                    deviceCount++;
                    monitorInfos.Add(aMonitorInfo);
                }

                ScreenState s = new ScreenState();
                s.AppInfos =  ProcessHelper.GetAppInfoList();
                s.MonitorInfos = monitorInfos;
                s.TextOnScreen = ocrResults;

                //Serialize
                string pathJson = System.IO.Path.Combine(directory, GetFileNameJson(capturedTime));
                string jsonString = JsonSerializer.Serialize(s);
                File.WriteAllText(pathJson, jsonString);

                if (loopforever)
                {
                    System.Threading.Thread.Sleep(sleepDefaultMS);
                }
            }
            while(loopforever);

            return 0;
        }

      
        private static string GetFileNameJson(DateTime captureTime)
        {
            long ticks = captureTime.Ticks;
            return "screenstate_" + ticks.ToString().Trim() + ".json";
        }

        private static string GetFileName(string display, DateTime captureTime)
        {
            long ticks = captureTime.Ticks;
            return "capture_" + ticks.ToString().Trim() + "_"+ display + ".jpeg";
        }
    }
}

