using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;


namespace screencapture
{

    public class ScreenRectangle
        {
            /// <summary>
            /// The x-coordinate of the upper-left corner of the rectangle.
            /// </summary>
            public int Left {get;set;}

            /// <summary>
            /// The y-coordinate of the upper-left corner of the rectangle.
            /// </summary>
            public int Top {get;set;}

            /// <summary>
            /// The x-coordinate of the lower-right corner of the rectangle.
            /// </summary>
            public int Right  {get;set;}

            /// <summary>
            /// The y-coordinate of the lower-right corner of the rectangle.
            /// </summary>
            public int Bottom  {get;set;}

            public ScreenRectangle(User32.RECT r)
            {
                this.Left = r.left;
                this.Right = r.right;
                this.Top = r.top;
                this.Bottom = r.bottom;
            }

            public ScreenRectangle(MonitorHelper.RectStruct  r)
            {
                this.Left = r.Left;
                this.Right = r.Right;
                this.Top = r.Top;
                this.Bottom = r.Bottom;
            }
        }



    public class AppInfo
    {
        public string AppName {get;set;}
        public ScreenRectangle Rect {get;set;}
        public bool IsForeGroundWindow {get;set;}

    }
    

    public class MonitorInfo
    {
        public ScreenRectangle Rect {get;set;}
        public string ImageFullPath {get;set;}
        
    }



    public class ScreenState
    {
        public List<AppInfo> AppInfos {get;set;}
        public List<MonitorInfo> MonitorInfos {get;set;}


    }
}