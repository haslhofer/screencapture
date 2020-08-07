using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;


namespace screencapture
{
    public class AppInfo
    {
        public string AppName {get;set;}
        public User32.RECT Rect {get;set;}
        

    }

    public class ProcessHelper
    {
        public static IEnumerable<AppInfo> GetProcessList()
        {

            //GetForground Window
            User32.GetForegroundWindow

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
                    if (!User32.IsIconic(windowHandle))
                    {
                        if (process.MainModule != null && process.MainModule.FileVersionInfo != null)
                        {
                            a.AppName = process.MainModule.FileVersionInfo.ProductName;
                        }
                        
                        IntPtr error = User32.GetWindowRect(windowHandle, ref rect);
                        System.Diagnostics.Debug.WriteLine(windowHandle);
                        a.Rect = rect;
                        results.Add(a);
                    }
                }
            }
            return results;
            
        }

    }
}
