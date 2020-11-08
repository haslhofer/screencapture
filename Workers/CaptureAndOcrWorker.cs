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

    public class CaptureAndWorker
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        private enum FileTypeForSerialization
        {
            Jpeg = 1,
            FullOCR = 2,
            RawText = 3

        }
        private static int sleepDefaultMS = 1000;

        public static void CaptureAndWrite(
            string directory = "",
            bool loopforever = true,
            bool detectText = true,
            bool generateTextDump = false,
            //bool reRenderText = false,
            bool detectProcesses = false
            )
        {

            ScreenCapture sc = new ScreenCapture();

            do
            {

                List<MonitorInfo> monitorInfos = new List<MonitorInfo>();

                DateTime capturedTime = DateTime.Now;
                int deviceCount = 0;

                List<ScreenText> ocrResults = new List<ScreenText>();

                var aDisplay = Program._MonitorToWatch;

                
                MonitorInfo aMonitorInfo = new MonitorInfo();
                aMonitorInfo.ID = deviceCount;
                aMonitorInfo.Rect = new ScreenRectangle(aDisplay.MonitorArea);

                string deviceName = aDisplay.DeviceName;
                Logger.Info("Before capture Screenshot {ScreenNr}", deviceCount);

                //Take Screenshot
                Image img = sc.CaptureWindowFromDevice(deviceName, aDisplay.ScreenWidth, aDisplay.ScreenHeight);
                string path = System.IO.Path.Combine(directory, GetFileName(FileTypeForSerialization.Jpeg, capturedTime, deviceCount.ToString()));
                img.Save(path, ImageFormat.Jpeg);

                Logger.Info("After capture Screenshot {ScreenNr}", deviceCount);

                aMonitorInfo.ImageFullPath = path;
                Console.WriteLine("Captured at " + path);

                if (detectText)
                {
                    Logger.Info("Before OCR {ScreenNr}", deviceCount);
                    //Do OCR
                    List<ScreenText> ocrText = OcrHelper.GetScreenTexts(path, deviceCount);
                    ocrResults.AddRange(ocrText);
                    Logger.Info("After OCR {ScreenNr}", deviceCount);

                    /* //TBD
                    {
                        int count = 0;

                         foreach (var snip in ocrText)
                         {
                             Program._ControllerUx.AddFloater(snip.Position.Left, snip.Position.Top, snip.Position.Right,  snip.Position.Bottom, snip.Content);
                             //if (count > 10) break;
                             count++;

                         }
                    } */

                    //Reassemble pic
                    // var bmp = RenderImage.GetWhiteBitmap(aDisplay.MonitorArea.Right - aDisplay.MonitorArea.Left, aDisplay.MonitorArea.Bottom - aDisplay.MonitorArea.Top);
                    // RenderImage.RenderBitmapFromTextSnippets(bmp, ocrText);
                    // Program._OverlayUx.SetBitmap(bmp);
                    // Program._OverlayUx.ShowOverlay();
                    
                    
                    //Program._OverlayUx.Show();
                    //bmp.Save(System.IO.Path.Combine(directory, GetFileName(FileTypeForSerialization.Jpeg, capturedTime, deviceCount.ToString() + "debug")));
                    //

                    if (generateTextDump)
                    {
                        if (ocrText != null && ocrText.Count > 0)
                        {

                            string filePath = GetFileName(FileTypeForSerialization.RawText, capturedTime, deviceCount.ToString());
                            StreamWriter writeToDisc = new StreamWriter(filePath);

                            foreach (var aRes in ocrResults)
                            {
                                writeToDisc.Write(aRes.Content + " ");
                            }

                            writeToDisc.Flush();
                            writeToDisc.Close();
                        }
                    }



                    /*
                    // Re-render bitmap
                    Bitmap b = RenderImage.GetWhiteBitmap(aDisplay.ScreenWidth, aDisplay.ScreenHeight);
                    RenderImage.RenderBitmapFromTextSnippets(b, ocrText);
                    b.Save(@"c:\data\tes1.jpg", ImageFormat.Jpeg);
                    */
                }
                deviceCount++;
                monitorInfos.Add(aMonitorInfo);
                

                ScreenState s = new ScreenState();
                if (detectProcesses)
                {
                    s.AppInfos = ProcessHelper.GetAppInfoList();
                }
                else
                {
                    s.AppInfos = new List<AppInfo>();
                }
                s.MonitorInfos = monitorInfos;
                s.TextOnScreen = ocrResults;


                Program._CaptureItems.Enqueue(s);


                //Serialize
                string pathJson = System.IO.Path.Combine(directory, GetFileName(FileTypeForSerialization.FullOCR, capturedTime, string.Empty));
                string jsonString = JsonSerializer.Serialize(s);
                File.WriteAllText(pathJson, jsonString);

                if (loopforever)
                {
                    System.Threading.Thread.Sleep(sleepDefaultMS);
                }
            }
            while (loopforever);


        }

        private static string GetFileName(FileTypeForSerialization fts, DateTime captureTime, string optionalFileName)
        {
            long ticks = captureTime.Ticks;
            string optionalFileNameFinal = "";

            if (!string.IsNullOrEmpty(optionalFileName))
            {
                optionalFileNameFinal = "_" + optionalFileName;
            }
            return "capture_" +
                    GetSerializationFileStringFromType(fts) + "_" +
                    ticks.ToString().Trim() +
                    optionalFileNameFinal +
                    "." + GetSerializationExtensionFromType(fts);
        }

        private static string GetSerializationFileStringFromType(FileTypeForSerialization t)
        {
            switch (t)
            {
                case FileTypeForSerialization.FullOCR: return "OcrText";
                case FileTypeForSerialization.Jpeg: return "Image";
                case FileTypeForSerialization.RawText: return "RawText";
                default: throw new Exception("Unknown FileTypeForSerialization");
            }
        }
        private static string GetSerializationExtensionFromType(FileTypeForSerialization t)
        {
            switch (t)
            {
                case FileTypeForSerialization.FullOCR: return "json";
                case FileTypeForSerialization.Jpeg: return "jpg";
                case FileTypeForSerialization.RawText: return "txt";
                default: throw new Exception("Unknown FileTypeForSerialization");
            }
        }
    }
}
