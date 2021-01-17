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
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace screencapture
{
    class ImageCacheItem
    {
        public long TickTimeTaken { get; set; }
        public Image ImageTaken { get; set; }
        public TextEmbedding TextEmbeddingOfScreen { get; set; }

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

      

        public override async Task<bool> DoWork(WorkItem triggeredWorkItem)
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
                        Bitmap i = RenderImage.BitmapFromBmpByteArray((byte[])triggeredWorkItem.WorkItemContext);

                        cacheItem.ImageTaken = i;

                        //string text = OcrHelperWindows.GetFullTextFromImage(i).GetAwaiter().GetResult();
                        //cacheItem.TextEmbeddingOfScreen = Embeddings.GetEmbeddingFromText(text).GetAwaiter().GetResult();


                        _cache.Add(cacheItem);
                    }
                }
            }

            return true;

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

        public List<Image> GetLastNCachedItems(int count)
        {
            List<Image> res = new List<Image>();
            lock (_cache)
            {
                res.AddRange((from x in _cache orderby x.TickTimeTaken descending select x.ImageTaken).Take(count));
            }

            return res;
        }
        public List<Image> GenerateTopNImages(int count)
        {
            _Dump();

            List<Image> res = new List<Image>();

            lock (_cache)
            {
                List<Tuple<double, int>> distances = new List<Tuple<double, int>>();
                for (int a = 0; a < _cache.Count - 1; a++)
                {
                    var v1 = _cache[a].TextEmbeddingOfScreen.EmbeddingVector;
                    var v2 = _cache[a + 1].TextEmbeddingOfScreen.EmbeddingVector;
                    var distance = MathNet.Numerics.Distance.Cosine(v1, v2);

                    Tuple<double, int> item = new Tuple<double, int>(distance, a);
                    distances.Add(item);

                    System.Diagnostics.Debug.WriteLine(a.ToString() + ":" + distance);
                }

                var indices = (from x in distances orderby x.Item1 descending select x).Take(count);

                List<int> offsets = new List<int>();
                foreach (var item in indices)
                {
                    offsets.Add(item.Item2);
                }


                foreach (int offset in offsets.Distinct())
                {
                    res.Add(_cache[offset].ImageTaken);

                }
            }

            return res;



        }

        public void _Dump()
        {
            int c = 0;
            lock (_cache)
            {
                // dump images
                foreach (var item in _cache)
                {
                    string pathJpg = @"c:\data\dump\item" + c.ToString() + ".jpg";
                    string pathTxt = @"c:\data\dump\item" + c.ToString() + ".txt";
                    item.ImageTaken.Save(pathJpg, ImageFormat.Jpeg);

                    StreamWriter w = new StreamWriter(pathTxt);
                    w.Write(item.TextEmbeddingOfScreen.SourceText);
                    w.Flush();
                    w.Close();

                    c++;
                }
                StreamWriter wr = new StreamWriter(@"C:\data\dump\distances.txt");
                for (int a = 0; a < _cache.Count - 1; a++)
                {

                    var v1 = _cache[a].TextEmbeddingOfScreen.EmbeddingVector;
                    var v2 = _cache[a + 1].TextEmbeddingOfScreen.EmbeddingVector;
                    var distance = MathNet.Numerics.Distance.Cosine(v1, v2);

                    wr.WriteLine(a.ToString() + ":" + distance.ToString());


                    System.Diagnostics.Debug.WriteLine(a.ToString() + ":" + distance);

                }
                wr.Close();
            }
        }
    }
}

