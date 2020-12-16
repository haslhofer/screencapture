using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;
using Windows.Media.Ocr;
using Windows.Graphics.Imaging;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Windows.Storage;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;


namespace screencapture
{

    public class OcrHelperWindows
    {
        private static OcrEngine _ocrEngine;

        public static void InitOCr()
        {
            _ocrEngine = OcrEngine.TryCreateFromUserProfileLanguages();
        }
        public static string GetFullText(string pathToImg)
        {
            //execute powershell cmdlets or scripts using command arguments as process
            ProcessStartInfo processInfo = new ProcessStartInfo();
            processInfo.FileName = @"powershell.exe";
            //execute powershell script using script file
            processInfo.Arguments = @"& { C:\Users\gerhas\Documents\GitHub\screencapture\Powershell\ocrtest2.ps1 " + pathToImg + "}";
            //execute powershell command

            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;
            processInfo.UseShellExecute = false;
            processInfo.CreateNoWindow = true;

            //start powershell process using process start info
            Process process = new Process();
            process.StartInfo = processInfo;
            process.Start();

            string result = process.StandardOutput.ReadToEnd();

            Root2 myDeserializedClass = JsonConvert.DeserializeObject<Root2>(result);
            return myDeserializedClass.Text;

        }

        public static async Task<string> GetFullText2(string pathToImg)
        {

            Bitmap bmp = new Bitmap(pathToImg);
            int width = bmp.Width;
            int height = bmp.Height;


            using (Graphics g = Graphics.FromImage(bmp))
            {
                // copy rectangle from screen (doesn't include cursor)

                using (var stream = new Windows.Storage.Streams.InMemoryRandomAccessStream())
                {
                    //These steps to get to a SoftwareBitmap are aweful! 
                    bmp.Save(stream.AsStream(), System.Drawing.Imaging.ImageFormat.Bmp);//choose the specific image format by your own bitmap source
                    Windows.Graphics.Imaging.BitmapDecoder decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
                    SoftwareBitmap bitmap = await decoder.GetSoftwareBitmapAsync();

                    OcrResult ocrResult = await _ocrEngine.RecognizeAsync(bitmap);
                    for (int i = 0; i < ocrResult.Lines.Count; i++)
                    {
                        OcrLine line = ocrResult.Lines[i];


                    }
                }
            }
            return string.Empty;


        }

    }
}