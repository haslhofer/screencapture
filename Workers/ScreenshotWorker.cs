using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;


namespace screencapture
{
    public class ScreenshotWorker : GenericWorker
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public ScreenshotWorker() : base (new List<WorkItemType>() {WorkItemType.Kickoff, WorkItemType.ScreenShotTaken})
        {   
        }
        
        public override async Task<bool> DoWork(WorkItem triggeredWorkItem)
        {
            Logger.Info("Triggered take screen shot");


            var aDisplay = Program._MonitorToWatch;

            var Rec = Program.IdealMonitorRectWorker.BestRectangle();


            //Take Screenshot
            Tuple<Image, byte[]> imgTuple = ScreenCapture.CaptureWindowFromDevice(aDisplay.DeviceName, Rec.Left, Rec.Top, Rec.Width,Rec.Height);
            
            Program._OaDisplayUx.SetImage(imgTuple.Item1);

            WorkItem w = WorkItem.GetGenericWorkItem(WorkItemType.ScreenShotTaken);
            w.WorkItemContext = imgTuple.Item2;
            lock (Program.ActionQueue)
            {
                Program.ActionQueue.Add(w);
            }
            return true;
        }
    }
}

