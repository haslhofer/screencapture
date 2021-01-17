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
    class SensorSnapshot
    {
        private enum SnapshotKind
        {
            Raw,
            Delta
        }

        private int[,] _values;
        private int _xdim;
        private int _ydim;
        private SnapshotKind _kind;
        private bool _isChanged = false;
        public SensorSnapshot(int xDim, int yDim)
        {
            _values = new int[xDim,yDim];
            _xdim = xDim;
            _ydim = yDim;
            _kind = SnapshotKind.Raw;
        }
        public SensorSnapshot(SensorSnapshot oldSnap, SensorSnapshot newSnap)
        {
            int sensorChanged = 0;
            _kind = SnapshotKind.Delta;
            bool isChanged = false;
            _xdim = oldSnap.Xdim;
            _ydim = oldSnap.Ydim;
            System.Diagnostics.Debug.Assert(oldSnap.Xdim == newSnap.Xdim);
            System.Diagnostics.Debug.Assert(oldSnap.Ydim == newSnap.Ydim);
            _values = new int[_xdim,_ydim];
            
            for (int x = 0; x < _xdim; x++)
            {
                for (int y = 0; y <_ydim; y++)
                {
                    int delta = oldSnap._values[x,y] - newSnap._values[x,y];
                    _values[x,y] = delta;
                    if (delta !=0)
                    {
                        isChanged = true;
                        sensorChanged ++;
                    }
                }

            }
            _isChanged = isChanged;
            double changedPercent = (double)sensorChanged / (double)(_xdim * _ydim);
            System.Diagnostics.Debug.WriteLine("Changed % " + changedPercent.ToString());

        }
        public bool IsChanged
        {
            get 
            {
                System.Diagnostics.Debug.Assert(_kind == SnapshotKind.Delta);
                return _isChanged;
            }
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
                for (int y = 0; y <_ydim; y++)
                {
                    int xPos =(int)((double)(bmp.Width) / (double)(_xdim) * x);
                    int yPos =(int)((double)(bmp.Height) / (double)(_ydim) * y);
                    Color color = bmp.GetPixel(xPos,yPos);
                    int colorArgb = color.ToArgb();
                    _values[x,y] = colorArgb;
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
        



        public MotionDetection() : base(new List<WorkItemType>() { WorkItemType.ScreenShotTaken })
        {
        }

      


        public override async Task<bool> DoWork(WorkItem triggeredWorkItem)
        {
            Logger.Info("Generate the snapshot matrix");
            Bitmap i = RenderImage.BitmapFromBmpByteArray((byte[])triggeredWorkItem.WorkItemContext);

            _thisSnapshot = new SensorSnapshot(SENSOR_HORIZONTAL, SENSOR_VERTICAL);
            _thisSnapshot.TakeSnapshot(i);

            if (_lastSnapshot != null)
            {
                SensorSnapshot delta = new SensorSnapshot(_lastSnapshot, _thisSnapshot);
                System.Diagnostics.Debug.WriteLine(delta.IsChanged.ToString());

            }

            //Do stuff
            _lastSnapshot = _thisSnapshot;
            _thisSnapshot = null;

            return true;

        }
    }
}

