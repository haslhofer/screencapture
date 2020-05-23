/* using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;


namespace screencapture
{

    public class MonitorHelper
    {
        private const int CCHDEVICENAME = 32;


        [DllImport("user32.dll")]
        static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip,
        MonitorEnumDelegate lpfnEnum, IntPtr dwData);

        delegate bool MonitorEnumDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        /// <summary>
        /// The struct that contains the display information
        /// </summary>
        public class DisplayInfo
        {
            public string Availability { get; set; }
            public string ScreenHeight { get; set; }
            public string ScreenWidth { get; set; }
            public Rect MonitorArea { get; set; }
            public Rect WorkArea { get; set; }
        }

        /// <summary>
        /// Collection of display information
        /// </summary>
        public class DisplayInfoCollection : List<DisplayInfo>
        {
        }

    /// <summary>
    /// Returns the number of Displays using the Win32 functions
    /// </summary>
    /// <returns>collection of Display Info</returns>
    public DisplayInfoCollection GetDisplays()
    {
    DisplayInfoCollection col = new DisplayInfoCollection();

    EnumDisplayMonitors( IntPtr.Zero, IntPtr.Zero,
        delegate (IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor,  IntPtr dwData)
        {
        MonitorInfo mi = new MonitorInfo();
        mi.size = (uint)Marshal.SizeOf(mi);
        bool success = GetMonitorInfo(hMonitor, ref mi);
        if (success)
        {
            DisplayInfo di = new DisplayInfo();
            di.ScreenWidth = (mi.monitor.right - mi.monitor.left).ToString();
            di.ScreenHeight = (mi.monitor.bottom - mi.monitor.top).ToString();
            di.MonitorArea = mi.monitor;
            di.WorkArea = mi.work;
            di.Availability = mi.flags.ToString();
            col.Add(di);
        }
        return true;
         }, IntPtr.Zero );
     return col;
    }
    }

    
} */