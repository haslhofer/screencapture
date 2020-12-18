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
    public class TakeScreenshotWorker : IOAWorker
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private long _lastWorkItemProcessed = 0;
        private long _highWaterMarkOkToTrim = 0;
        private void TakeScreenShot(WorkItem triggeredWorkItem)
        {
            Console.WriteLine("Triggered take screen shot");


            var aDisplay = Program._MonitorToWatch;
            //Take Screenshot
            Image img = ScreenCapture.CaptureWindowFromDevice(aDisplay.DeviceName, aDisplay.ScreenWidth, aDisplay.ScreenHeight);
            Program._OaDisplayUx.SetImage(img);

            WorkItem w = WorkItem.GetGenericWorkItem(WorkItemType.ScreenShotTaken);
            w.WorkItemContext = img;
            lock (Program.ActionQueue)
            {
                Program.ActionQueue.Add(w);
            }
        }

        long IOAWorker.WaterMarkOkToTrim()
        {
            return _highWaterMarkOkToTrim;
        }
        public void TakeScreenShotsLoop()
        {
            try
            {

                while (true)
                {

                    WorkItem workItemToExecute = new WorkItem();
                    long highWaterMarkOkToTrim = _highWaterMarkOkToTrim;

                    lock (Program.ActionQueue)
                    {
                        foreach (var item in Program.ActionQueue)
                        {
                            if (item.CreatedTimeTick > highWaterMarkOkToTrim) highWaterMarkOkToTrim = item.CreatedTimeTick;

                            //Screenshot triggers on Kickoff, and Screenshotresults being available
                            if (item.TypeOfWorkItem == WorkItemType.Kickoff || item.TypeOfWorkItem == WorkItemType.ScreenShotTaken)
                            {
                                if (item.CreatedTimeTick > _lastWorkItemProcessed && item.CreatedTimeTick > workItemToExecute.CreatedTimeTick)
                                {
                                    workItemToExecute = item;
                                }
                            }
                        }


                    }

                    //Process if possible
                    if (workItemToExecute.CreatedTimeTick > 0)
                    {
                        TakeScreenShot(workItemToExecute);
                        //Set the new baseline for picking up the next work item
                        _lastWorkItemProcessed = workItemToExecute.CreatedTimeTick;

                    }

                    //The queue trimmer needs to know what the newest item is from where all older items can be removed because all processors have seen them
                    _highWaterMarkOkToTrim = highWaterMarkOkToTrim;

                    System.Threading.Thread.Sleep(10);

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }
    }
}

