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
        private static int sleepDefaultMS = 1;

        public static void CaptureAndWrite(
            OaDisplayUx ux
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

                //string path = System.IO.Path.Combine(directory, GetFileName(FileTypeForSerialization.Jpeg, capturedTime, deviceCount.ToString()));
                //img.Save(path, ImageFormat.Jpeg);

                ux.SetImage(img);


                Logger.Info("After capture Screenshot {ScreenNr}", deviceCount);

                //aMonitorInfo.ImageFullPath = path;
                //Console.WriteLine("Captured at " + path);

                //if (false)
                //{

                    //var res = ux.TryOcr().Result;

                    //Logger.Info("Before OCR {ScreenNr}", deviceCount);

                    //string ocrFinal = OcrHelperWindows.GetFullText(path);
                    //Do OCR
                    /* List<ScreenText> ocrText = OcrHelper.GetScreenTexts(path, deviceCount);
                    ocrResults.AddRange(ocrText); */
                    //Logger.Info("After OCR {ScreenNr}", deviceCount);

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

                    /* if (generateTextDump)
                    {
                        //if (ocrText != null && ocrText.Count > 0)
                        //{
                            Logger.Info("Before generate TextDump");

                            string filePath = @"C:\Users\gerhas\Documents\GitHub\hashtag\text\query.txt";

                            //string filePath = GetFileName(FileTypeForSerialization.RawText, capturedTime, deviceCount.ToString());
                            /* StringWriter writeToMemory = new StringWriter();
                            foreach (var aRes in ocrResults)
                            {
                                writeToMemory.Write(CleanString(aRes.Content) + " ");
                            } */

                            //string allText = CleanString(writeToMemory.ToString());
                            //string allText = CleanString(ocrFinal);

                            //skip beginning
                           /*  if (allText.Length > 50)
                            {
                                allText = allText.Substring(50);
                            }
 */
                            // StreamWriter writeToDisc = new StreamWriter(filePath);
                            // writeToDisc.Write(allText);
                            // writeToDisc.Flush();
                            // writeToDisc.Close();

                            // Logger.Info("After generate TextDump");

                            // string c0 = string.Empty;
                            // string c1 = string.Empty;
/* 
                            Logger.Info("Before run language model");
                            

                            var nerResult = LanguageModel.GetNerFromServer();

                            List<NerResponse> finalNer = new List<NerResponse>();
                            foreach (var aNer in nerResult)
                            {
                                if (NameClean.IsHighConfidenceName(aNer.text))
                                {
                                    finalNer.Add(aNer);
                                }
                                
                            }


                            List<ConfidenceScore> confScores = new List<ConfidenceScore>(); //LanguageModel.GetConfidenceFromServer();
                            if (confScores.Count >0)
                            {
                                c0 = confScores[0].GetDebug();
                            }
                            if (confScores.Count >1)
                            {
                                c1 = confScores[1].GetDebug();
                            }
                            Logger.Info("After run language model");

                            AssessmentResult r = new AssessmentResult();
                            r.ConfidenceScoreResults = confScores;
                            r.CapturedText = allText;
                            r.PathToImage = path;
                            r.RecognizedEntities = finalNer;


                            Program._ControllerUx.SetConfidence(r); */
                            //foreach (var aScore in confScores)
                         /*    {
                                Logger.Trace(aScore.GetDebug());
                            } */
                        //}
                    //}
 //*/


                    /*
                    // Re-render bitmap
                    Bitmap b = RenderImage.GetWhiteBitmap(aDisplay.ScreenWidth, aDisplay.ScreenHeight);
                    RenderImage.RenderBitmapFromTextSnippets(b, ocrText);
                    b.Save(@"c:\data\tes1.jpg", ImageFormat.Jpeg);
                    */
                //}
/*                 deviceCount++;
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
                File.WriteAllText(pathJson, jsonString); */

                
                System.Threading.Thread.Sleep(sleepDefaultMS);
                
            }
            while (true);


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

        private static string CleanString(string x)
        {
            bool prevIsWhiteSpace = false;

            StringWriter w = new StringWriter();
            foreach (char c in x)
            {
                char thisChar = c;
                if (((int)thisChar)>=128)
                {
                    thisChar  = (char)' ';
                }

                bool currIsWhiteSpace = char.IsWhiteSpace(thisChar);
                
                if (!(prevIsWhiteSpace && currIsWhiteSpace))
                {
                    w.Write(thisChar);
                }

                prevIsWhiteSpace = currIsWhiteSpace;

            }
            return w.ToString();
        }
    }
}
