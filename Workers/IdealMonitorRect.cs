using System.Drawing;
using System.Collections.Generic;
namespace screencapture
{
    public class IdealMonitorRect
    {
        private MonitorHelper.DisplayInfo _monitorToWatch;
        private Rectangle _bestRect;
        private Rectangle _monitorRect;
        public IdealMonitorRect(MonitorHelper.DisplayInfo monitorToWatch)
        {
            _monitorToWatch=monitorToWatch;


            ScreenRectangle monitorRect = new ScreenRectangle(_monitorToWatch.WorkArea);
            System.Drawing.Rectangle rMonitor = RecFromScreenRec(monitorRect);
            _monitorRect = rMonitor;
            _bestRect = rMonitor;
        }

        public Rectangle BestRectangle()
        {
            Rectangle final = new Rectangle(
                _bestRect.Left - _monitorRect.Left,
                _bestRect.Top- _monitorRect.Top,
                _bestRect.Width,
                _bestRect.Height
                );
            ;
            return final;

        }

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public void DetermineRect()
        {
            //Determine rec
            var appinfo = ProcessHelper.GetAppInfoList();
            _bestRect  = GetBestRectangleOfMonitor(_monitorToWatch, appinfo);

        }

         private static bool IsMinAppSize(System.Drawing.Rectangle r)
        {
            return r.Width > 10 && r.Height > 10;
        }

        private static System.Drawing.Rectangle RecFromScreenRec(ScreenRectangle r)
        {
            return new System.Drawing.Rectangle(r.Left, r.Top, r.Right - r.Left, r.Bottom - r.Top);
        }
        private static System.Drawing.Rectangle GetBestRectangleOfMonitor(MonitorHelper.DisplayInfo monitor, List<AppInfo> allApps)
        {
            bool isFirstRect = true;
            System.Drawing.Rectangle boundingRec = new Rectangle();

            ScreenRectangle monitorRect = new ScreenRectangle(monitor.WorkArea);
            System.Drawing.Rectangle rMonitor = RecFromScreenRec(monitorRect);
            System.Drawing.Rectangle rMonitor2 = new Rectangle(rMonitor.Left-15, rMonitor.Top-15, rMonitor.Width+30, rMonitor.Height+30);
            

            foreach (var anApp in allApps)
            {

                //Is the app fully contained in the window
                System.Drawing.Rectangle appRec = RecFromScreenRec(anApp.Rect);
                if (IsMinAppSize(appRec))
                {
                    if (rMonitor.Contains(appRec) || rMonitor2.Contains(appRec))
                    {
                        System.Diagnostics.Debug.WriteLine(anApp.AppName + "contained on Monitor");
                        if (isFirstRect)
                        {
                            boundingRec = appRec;
                            isFirstRect = false;
                        }
                        else
                        {
                            boundingRec = System.Drawing.Rectangle.Union(boundingRec, appRec);
                        }
                    }
                }

            }

            if (!IsMinAppSize(boundingRec)) return rMonitor;

            return boundingRec;


        }


    }
}
