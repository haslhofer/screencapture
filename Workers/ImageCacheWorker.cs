using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Linq;
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
        public long TickTimeTaken { get; set; }
        public Image ImageTaken { get; set; }
    }

    public class ImageCacheWorker : GenericWorker
    {

        public enum CacheMode
        {
            Capture,
            Retrieve
        }

        private List<ImageCacheItem> _cache = new List<ImageCacheItem>();
        private long _ticksLastItemCached = 0;
        private long TICKS_TO_WAIT = new TimeSpan(0, 0, 10).Ticks; //Every 10 seconds
        private long TICKS_TO_CACHE = new TimeSpan(0, 3, 0).Ticks; //Cache images for three minutes
        private CacheMode _cacheMode = CacheMode.Capture;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();



        public ImageCacheWorker() : base(new List<WorkItemType>() { WorkItemType.ScreenShotTaken })
        {
        }

        public CacheMode GetCacheMode()
        {
            return _cacheMode;
        }

        public long StartRetrieve()
        {
            _cacheMode = CacheMode.Retrieve;
            return _ticksLastItemCached;
        }

        public void ResumeCache()
        {
            _cacheMode = CacheMode.Capture;
        }

        private void EnsureThatRetrieve()
        {
            if (_cacheMode == CacheMode.Capture)
            throw new Exception("Can only retrieve items if mode has been changed away from capture");
        }

        public Image GetFromToken(long token)
        {
            EnsureThatRetrieve();
            lock (_cache)
            {
                var res = from x in _cache where x.TickTimeTaken == token select x.ImageTaken;
                return res.FirstOrDefault();
            }
        }

        public Tuple<Image, long> GetPreviousFromToken(long token)
        {
            EnsureThatRetrieve();
            lock (_cache)
            {
                //Select all older ones
                var prev = from x in _cache where x.TickTimeTaken < token select x;
                var cacheItem = prev.OrderByDescending(i => i.TickTimeTaken).FirstOrDefault();
                if (cacheItem == null) return null;
                return new Tuple<Image, long>(cacheItem.ImageTaken, cacheItem.TickTimeTaken);
            }

        }

        public Tuple<Image, long> GetNextFromToken(long token)
        {
            EnsureThatRetrieve();
            lock (_cache)
            {
                //Select all older ones
                var prev = from x in _cache where x.TickTimeTaken > token select x;
                var cacheItem = prev.OrderBy(i => i.TickTimeTaken).FirstOrDefault();
                if (cacheItem == null) return null;
                return new Tuple<Image, long>(cacheItem.ImageTaken, cacheItem.TickTimeTaken);
            }
        }

        public override void DoWork(WorkItem triggeredWorkItem)
        {
            if (_cacheMode == CacheMode.Capture)
            {
                lock (_cache)
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
            }

        }

        private void TrimCache(long currentTick)
        {
            lock (_cache)
            {
                Logger.Info("# cached items before cull:" + _cache.Count.ToString());
                _cache.RemoveAll(wi => wi.TickTimeTaken < currentTick - TICKS_TO_CACHE);
                Logger.Info("# cached items after cull:" + _cache.Count.ToString());
            }
        }
    }
}

