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
    class DeltaSnapshot
    {
        private long[,] _values = null;
        private int _xdim;
        private int _ydim;

        private static long RELEVANT_HISTORY_TICK = new TimeSpan(0, 0, 10).Ticks;

        public void OverlaySnapshot(System.Drawing.Bitmap bmp)
        {

            long min_changed_date = DateTime.Now.Ticks - RELEVANT_HISTORY_TICK;

            //Take pixels
            for (int x = 0; x < _xdim; x++)
            {
                for (int y = 0; y < _ydim; y++)
                {

                    if (_values[x, y] > min_changed_date) //Sensor was changed recently
                    {
                        int xPos = (int)((double)(bmp.Width) / (double)(_xdim) * x);
                        int yPos = (int)((double)(bmp.Height) / (double)(_ydim) * y);
                        RenderImage.SetMarkerAtPosition(bmp, xPos, yPos);
                        
                    }
                }
            }

        }

        public void Update(SensorSnapshot oldSnap, SensorSnapshot newSnap)
        {
            if (_values == null)
            {

                _xdim = oldSnap.Xdim;
                _ydim = oldSnap.Ydim;
                System.Diagnostics.Debug.Assert(oldSnap.Xdim == newSnap.Xdim);
                System.Diagnostics.Debug.Assert(oldSnap.Ydim == newSnap.Ydim);
                _values = new long[_xdim, _ydim];
            }

            long marker = DateTime.Now.Ticks;

            for (int x = 0; x < _xdim; x++)
            {
                for (int y = 0; y < _ydim; y++)
                {
                    int delta = oldSnap.SnapshotValues[x, y] - newSnap.SnapshotValues[x, y];
                    if (delta != 0)
                    {
                        _values[x, y] = marker;
                    }
                }

            }
        }

    }
    class SensorSnapshot
    {

        private int[,] _values;
        private int _xdim;
        private int _ydim;

        public SensorSnapshot(int xDim, int yDim)
        {
            _values = new int[xDim, yDim];
            _xdim = xDim;
            _ydim = yDim;
        }


        public int[,] SnapshotValues
        {
            get
            {
                return _values;
            }
        }
        public int Xdim
        {
            get
            {
                return _xdim;
            }
        }
        public int Ydim
        {
            get
            {
                return _ydim;
            }
        }

        // take sensor values from Image
        public void TakeSnapshot(System.Drawing.Bitmap bmp)
        {
            //Take pixels
            for (int x = 0; x < _xdim; x++)
            {
                for (int y = 0; y < _ydim; y++)
                {
                    int xPos = (int)((double)(bmp.Width) / (double)(_xdim) * x);
                    int yPos = (int)((double)(bmp.Height) / (double)(_ydim) * y);
                    Color color = bmp.GetPixel(xPos, yPos);
                    int colorArgb = color.ToArgb();
                    _values[x, y] = colorArgb;
                }

            }
        }

    }
    public class MotionDetection : GenericWorker
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static int SENSOR_HORIZONTAL = 100;
        private static int SENSOR_VERTICAL = 100;

        private SensorSnapshot _lastSnapshot = null;
        private SensorSnapshot _thisSnapshot;
        private DeltaSnapshot _delta = null;




        public MotionDetection() : base(new List<WorkItemType>() { WorkItemType.ScreenShotTaken })
        {
            _delta = new DeltaSnapshot();
        }


        public void OverlayDeltaToBitmap(Bitmap b)
        {
            _delta.OverlaySnapshot(b);
        }


        public override async Task<bool> DoWork(WorkItem triggeredWorkItem)
        {
            Logger.Info("Generate the snapshot matrix");
            Bitmap i = RenderImage.BitmapFromBmpByteArray((byte[])triggeredWorkItem.WorkItemContext);

            _thisSnapshot = new SensorSnapshot(SENSOR_HORIZONTAL, SENSOR_VERTICAL);
            _thisSnapshot.TakeSnapshot(i);

            if (_lastSnapshot != null)
            {
                _delta.Update(_lastSnapshot, _thisSnapshot);
            }

            //Do stuff
            _lastSnapshot = _thisSnapshot;
            _thisSnapshot = null;

            return true;

        }
    }
}

