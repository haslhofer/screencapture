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
    }
}