using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Imaging;





namespace screencapture
{
    public class ScreenCapture
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
   
        /// <summary>
        /// Creates an Image object containing a screen shot of the entire desktop
        /// </summary>
        /// <returns></returns>
        public static Image CaptureScreen()
        {
            return CaptureWindow( User32.GetDesktopWindow() );
        }
        /// <summary>
        /// Creates an Image object containing a screen shot of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. (In windows forms, this is obtained by the Handle property)</param>
        /// <returns></returns>
       
        // [DllImport("gdi32.dll")]
        // static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, IntPtr lpInitData);


        public static Image CaptureWindowFromDevice(string deviceName, int x, int y, int width, int height)
        {

            
            IntPtr hdcSrc = GDI32.CreateDC(deviceName, null, null, IntPtr.Zero);
            
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc,width,height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest,hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest,0,0,width,height,hdcSrc,x,y,GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest,hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            GDI32.DeleteDC(hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;
        }

        public static Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = User32.GetWindowDC(handle);
            // get the size
            User32.RECT windowRect = new User32.RECT();
            User32.GetWindowRect(handle,ref windowRect);
            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;
            // create a device context we can copy to
            IntPtr hdcDest = GDI32.CreateCompatibleDC(hdcSrc);
            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = GDI32.CreateCompatibleBitmap(hdcSrc,width,height);
            // select the bitmap object
            IntPtr hOld = GDI32.SelectObject(hdcDest,hBitmap);
            // bitblt over
            GDI32.BitBlt(hdcDest,0,0,width,height,hdcSrc,0,0,GDI32.SRCCOPY);
            // restore selection
            GDI32.SelectObject(hdcDest,hOld);
            // clean up
            GDI32.DeleteDC(hdcDest);
            User32.ReleaseDC(handle,hdcSrc);
            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);
           
            // free up the Bitmap object
            GDI32.DeleteObject(hBitmap);
            return img;
        }
        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public static void CaptureWindowToFile(IntPtr handle, string filename, ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(filename,format);
        }
        /// <summary>
        /// Captures a screen shot of the entire desktop, and saves it to a file
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="format"></param>
        public static void CaptureScreenToFile(string filename, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(filename,format);
        }
    }
}
