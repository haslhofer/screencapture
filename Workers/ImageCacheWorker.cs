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
    class ImageCacheItem
    {
        public long TickTimeTaken {get;set;}
        public Image ImageTaken {get;set;}
    }

    public class ImageCacheWorker : GenericWorker
    {

        
        private List<ImageCacheItem> _cache = new List<ImageCacheItem>();
        private long _ticksLastItemCached = 0;
        private long TICKS_TO_WAIT = new TimeSpan(0,0,10).Ticks; //Every 10 seconds
        private long TICKS_TO_CACHE = new TimeSpan(0,1,0).Ticks; //Cache images for a minute
        
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public ImageCacheWorker() : base (new List<WorkItemType>() {WorkItemType.ScreenShotTaken})
        {   
        }
        
        public override void DoWork(WorkItem triggeredWorkItem)
        {
            Logger.Info("Check if screenshot should be cached");

            long ticksNow = DateTime.Now.Ticks;

            TrimCache(ticksNow);

            if (_ticksLastItemCached < (ticksNow - TICKS_TO_WAIT))
            {
                _ticksLastItemCached = ticksNow;
                ImageCacheItem cacheItem = new ImageCacheItem();
                cacheItem.TickTimeTaken = ticksNow;
                cacheItem.ImageTaken = (Image)triggeredWorkItem.WorkItemContext;
                _cache.Add(cacheItem);
            }

        }

        private void TrimCache(long currentTick)
        {

            Logger.Info("# cached items before cull:" + _cache.Count.ToString());
            _cache.RemoveAll(wi => wi.TickTimeTaken < currentTick - TICKS_TO_CACHE);
            Logger.Info("# cached items after cull:" + _cache.Count.ToString());
        }
    }
}

