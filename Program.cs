using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Drawing.Imaging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Windows.Forms;
using NLog;


namespace screencapture
{

    class Program
    {


        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public static MonitorHelper.DisplayInfo _MonitorToWatch;
        public static ConcurrentQueue<ScreenState> _CaptureItems = new ConcurrentQueue<ScreenState>();
        public static List<NoteReference> _NoteReferences = new List<NoteReference>();


        public static OaDisplayUx _OaDisplayUx;
        public static List<WorkItem> ActionQueue = new List<WorkItem>();
        public static List<GenericWorker> WorkerList = new List<GenericWorker>();



        [STAThread]
        static int Main(
            int MonitorIndex = 1
            )
        {



            ConfigureLogging();
            Logger.Info("Startup");
            Logger.Info("Monitor to use:" + MonitorIndex.ToString());

            bool result = SHCore.SetProcessDpiAwareness(SHCore.PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware);
            var setDpiError = Marshal.GetLastWin32Error();

            //Authenticate with MS Graph
            Logger.Info("Preparing for OneNoteAuthentication");
            OneNoteCapture.Init();

            //Init which pages we'll use
            Configurator.Init();

            //Determine the screen to watch

            MonitorHelper h = new MonitorHelper();
            var displays = h.GetDisplays();
            Logger.Info("App assumes there are two monitors. # Displays detected:" + displays.Count.ToString());

            _MonitorToWatch = displays[MonitorIndex];

            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _OaDisplayUx = new OaDisplayUx();
            Logger.Info("Before Show Ux");
            _OaDisplayUx.Show();

            //Set up queue to deal with incoming events
            lock (ActionQueue)
            {
                ActionQueue.Add(WorkItem.GetGenericWorkItem(WorkItemType.Kickoff));
            }

            ScreenshotWorker screenShotworker = new ScreenshotWorker();
            WorkerList.Add(screenShotworker);
            ImageCacheWorker cacheWorker = new ImageCacheWorker();
            WorkerList.Add(cacheWorker);

            foreach (var worker in WorkerList)
            {
                Task.Run(() =>
                {
                    worker.WorkerLoop();
                });

            }


            Task.Run(() =>
            {
                while (true)
                {
                    QueueCleaner.CleanQueue();
                    Thread.Sleep(100);
                }
            });

            



            //Thread myCaptureThread = new Thread(() => CaptureAndWorker.CaptureAndWrite(_OaDisplayUx));
            //myCaptureThread.Start();

            //Thread myShowNote = new Thread(() => ShowNoteWorker.ShowNotes());
            //myShowNote.Start();

            Application.Run();


            //Console.ReadLine();


            return 0;
        }

        private static void ConfigureLogging()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = "file.txt" };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            NLog.LogManager.Configuration = config;
        }


        private static void TestPython()
        {
            LanguageModel.RunCmd(@"c:\Users\gerhas\Documents\GitHub\hashtag\test.py", string.Empty, @"c:\Users\gerhas\Documents\GitHub\hashtag");
        }



    }
}

