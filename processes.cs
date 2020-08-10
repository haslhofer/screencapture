using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;


namespace screencapture
{
    

    public class ProcessHelper
    {
        public static List<AppInfo> GetAppInfoList()
        {

            //GetForground Window
            IntPtr foregroundWindow = User32.GetForegroundWindow();
            System.Diagnostics.Debug.WriteLine("Foreground window handle:" + foregroundWindow.ToString());


            List<AppInfo> results = new List<AppInfo>();
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                AppInfo a = new AppInfo();
                a.AppName = process.MainWindowTitle;
                //Get Coordinates
                User32.RECT rect = new User32.RECT();

                IntPtr windowHandle = process.MainWindowHandle;
                if (windowHandle != IntPtr.Zero)
                {
                    a.IsForeGroundWindow = (windowHandle == foregroundWindow);
                    System.Diagnostics.Debug.WriteLine("Handle:" + windowHandle.ToString());

                    if (!User32.IsIconic(windowHandle))
                    {
                        if (process.MainModule != null && process.MainModule.FileVersionInfo != null)
                        {
                            a.AppName = process.MainModule.FileVersionInfo.ProductName;
                        }
                        System.Diagnostics.Debug.WriteLine("App Name:" + a.AppName);


                        IntPtr error = User32.GetWindowRect(windowHandle, ref rect);
                        System.Diagnostics.Debug.WriteLine(windowHandle);
                        a.Rect = new ScreenRectangle(rect);
                        results.Add(a);
                    }
                }
            }
            return results;
            
        }

    }
}
