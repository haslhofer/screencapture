namespace screencapture
{
    public class QueueCleaner
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public static void CleanQueue()
        {
            long topCull = long.MaxValue;
            //Figure out lowest watermark 
            foreach (var item in Program.WorkerList)
            {
                if (item.WaterMarkOkToTrim() <topCull) topCull = item.WaterMarkOkToTrim();
                lock (Program.ActionQueue)
                {
                    Logger.Info("Culling queue at tick:" + topCull);
                    Logger.Info("before cull queue length:" + Program.ActionQueue.Count.ToString());
                    Program.ActionQueue.RemoveAll(wi => wi.CreatedTimeTick < topCull);
                    Logger.Info("After cull queue length:" + Program.ActionQueue.Count.ToString());
                }
                System.GC.Collect();
            }
        }
    }
}
