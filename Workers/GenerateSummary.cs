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
    public class GenerateSummary 
    {
        public class CaptureResult
        {
            public bool IsSuccess {get;set;}
            public string UserMessage {get;set;}
        }
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        private static Image GenerateThumbnails()
        {
            List<Image> cacheItems = Program.CacheWorker.GetLastNCachedItems(3);
            var img = RenderImage.HorizontalConcatenate(cacheItems, 1000);
            //img.Save(@"c:\data\dump\concat.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            return img;
        }
        public static async Task<CaptureResult> CaptureSummaryToOneNote(Image imageToWrite)
        {

            var imgThumb = GenerateThumbnails();

            string result = string.Empty;
            string pageId = Configurator.DestinationOneNote.PageId;

            bool isSuccess = await OneNoteCapture.AppendImage(imageToWrite, pageId);
            if (isSuccess)
            {
                 isSuccess = await OneNoteCapture.AppendImage(imgThumb, pageId);
            }

            CaptureResult c = new CaptureResult();
            c.IsSuccess = isSuccess;
            
            Logger.Info("After write captured screenshot to OneNote");
            if (!isSuccess)
            {
                c.UserMessage = "Failure when adding to OneNote";
            }
            else
            {
                c.UserMessage = "OneNote capture successful at " + DateTime.Now.ToLongTimeString();
            }

            return c;

        }
    }
}

