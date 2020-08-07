using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;




namespace screencapture
{
    public class User32
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd,IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowRect(IntPtr hWnd,ref RECT rect);
        [DllImport("user32.dll", SetLastError=true)]
        public static extern bool SetProcessDPIAware();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public  static extern IntPtr GetForegroundWindow();

    }


    public class SHCore
    {
        public enum PROCESS_DPI_AWARENESS
        {
            Process_DPI_Unaware = 0,
            Process_System_DPI_Aware = 1,
            Process_Per_Monitor_DPI_Aware = 2
        }
        [DllImport("SHCore.dll", SetLastError = true)]
        public static extern bool SetProcessDpiAwareness(PROCESS_DPI_AWARENESS awareness);
    }
}