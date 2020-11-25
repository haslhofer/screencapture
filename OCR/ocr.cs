using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using Tesseract;

namespace screencapture
{

    public class OcrHelper
    {
        private static TesseractEngine _tesseractEngine = null;
        public static List<ScreenText> GetScreenTexts(string pathToImg, int monitorID)
        {
            if (_tesseractEngine == null)
            {
                _tesseractEngine = new TesseractEngine(@"c:\data\tessdata", "eng");
            }
                
            //var img = Pix.LoadFromFile(@"C:\data\temp\capture_637324115193484215_1.jpeg");
            var img = Pix.LoadFromFile(pathToImg);
            List<ScreenText> results = new List<ScreenText>();
            using (var page = _tesseractEngine.Process(img,PageSegMode.Auto)) 
            {
                var iter = page.GetIterator();
                do
                {
                    Rect b;
                    if (iter.TryGetBoundingBox(PageIteratorLevel.Block,out b))
                    {
                        string text = iter.GetText(PageIteratorLevel.Block);
                        
                        ScreenText t = new ScreenText();
                        t.Content = text;
                        t.ImageID = monitorID;
                        t.Position = new ScreenRectangle(b);
                        results.Add(t);
                    }
                    
                } while (  iter.Next(PageIteratorLevel.Block));

                return results;

            }     
        }
    }
}