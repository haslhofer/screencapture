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
    public abstract class GenericWorker 
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private long _lastWorkItemProcessed = 0;
        private long _highWaterMarkOkToTrim = 0;
        public abstract Task<bool> DoWork(WorkItem triggeredWorkItem);

        private List<WorkItemType> _triggers;
        
        public GenericWorker(List<WorkItemType> triggers)
        {
            _triggers = triggers;
        }

        public long WaterMarkOkToTrim()
        {
            return _highWaterMarkOkToTrim;
        }
        public async void WorkerLoop()
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
                            if (_triggers.Contains(item.TypeOfWorkItem))
                            {
                                //look for the newest item that matches the type, and is older than the last processed item
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
                        bool isSuccess = await DoWork(workItemToExecute);
                        //Set the new baseline for picking up the next work item
                        if (!isSuccess)
                        {
                            Logger.Error("Task performed not successful");
                        }
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

