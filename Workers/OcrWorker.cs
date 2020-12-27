using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Threading.Tasks;


namespace screencapture
{
    public class OcrWorker : GenericWorker
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public OcrWorker() : base (new List<WorkItemType>() {WorkItemType.ScreenShotTaken})
        {   
        }
        
        public override async Task<bool> DoWork(WorkItem triggeredWorkItem)
        {
            Logger.Info("Triggered to OCR");
            Image img = (Image)triggeredWorkItem.WorkItemContext;
            string text = await OcrHelperWindows.GetFullTextFromImage(img);

            WorkItem w = WorkItem.GetGenericWorkItem(WorkItemType.OcrPerformed);
            w.WorkItemContext = text;
            lock (Program.ActionQueue)
            {
                Program.ActionQueue.Add(w);
            }
            return true;
        }
    }
}

