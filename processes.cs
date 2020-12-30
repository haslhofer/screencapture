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
                try
                {
                    AppInfo a = new AppInfo();
                    a.AppName = process.MainWindowTitle;
                    //Get Coordinates
                    User32.RECT rect = new User32.RECT();

                    IntPtr windowHandle = process.MainWindowHandle;
                    if (windowHandle != IntPtr.Zero)
                    {
                        if (HasWindowStyle(process))
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
                                if (!(a.AppName.Contains("Microsoft") && a.AppName.Contains("Windows") && a.AppName.Contains("System") && a.AppName.Contains("Operating")))
                                {
                                    if (!(a.AppName.Contains("Entertainment") && a.AppName.Contains("Platform")))
                                    {
                                        if (!(a.AppName.Contains("Windows Store")))
                                        {
                                            if (!(a.AppName.Contains("GlobalProtect")))
                                            {
                                                if (!(a.AppName.Contains("Cortana") || a.AppName.Contains("screencapture")))
                                                {
                                                    results.Add(a);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }

                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }

            }
            return results;

        }

        public static bool HasWindowStyle(Process p)
        {
            IntPtr hnd = p.MainWindowHandle;
            UInt32 WS_DISABLED = 0x8000000;
            int GWL_STYLE = -16;
            bool visible = false;
            if (hnd != IntPtr.Zero)
            {
                UInt32 style = (UInt32)User32.GetWindowLongPtr(hnd, GWL_STYLE);
                visible = ((style & WS_DISABLED) != WS_DISABLED);
            }
            return visible;
        }

    }
}
