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
using Microsoft.Extensions.FileProviders.Embedded;

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

        public static ImageCacheWorker CacheWorker;
        public static MotionDetection MotionDetectionWorker;

        public static IdealMonitorRect IdealMonitorRectWorker;

       

        [STAThread]
        static int Main(
            int MonitorIndex = 1
            )
        {

            /* string text1 = File.ReadAllText(@"C:\data\dump\item6.txt");
            string text2 = File.ReadAllText(@"C:\data\dump\item7.txt");

            var ax = Embeddings.GetEmbeddingFromText(text1).GetAwaiter().GetResult();
            var ay = Embeddings.GetEmbeddingFromText(text2).GetAwaiter().GetResult();
            var distance = MathNet.Numerics.Distance.Cosine(ax.EmbeddingVector, ay.EmbeddingVector); */


            ConfigureLogging();
            Logger.Info("Startup");
            Logger.Info("Monitor to use:" + MonitorIndex.ToString());

            OcrHelperWindows.InitOCr();

            bool result = SHCore.SetProcessDpiAwareness(SHCore.PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware);
            var setDpiError = Marshal.GetLastWin32Error();


            
            MonitorHelper h = new MonitorHelper();
            var displays = h.GetDisplays();
            Logger.Info("App assumes there are two monitors. # Displays detected:" + displays.Count.ToString());

            if (MonitorIndex >= displays.Count)
            {
                MonitorIndex = 0;
                Logger.Warn("fell back to monitoring Monitor 0");
            }

            _MonitorToWatch = displays[MonitorIndex];

            IdealMonitorRectWorker = new IdealMonitorRect(_MonitorToWatch);

         
            //Authenticate with MS Graph
            Logger.Info("Preparing for OneNoteAuthentication");
            OneNoteCapture.Init();

            //Init which pages we'll use
            Configurator.Init();

            //Determine the screen to watch


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

            //Capture screenshot
            ScreenshotWorker screenShotworker = new ScreenshotWorker();
            WorkerList.Add(screenShotworker);
            
            //Cache image for history feature
            ImageCacheWorker cacheWorker = new ImageCacheWorker();
            CacheWorker = cacheWorker;
            WorkerList.Add(cacheWorker);

            //Motion Detection
            MotionDetectionWorker  = new MotionDetection();
            WorkerList.Add(MotionDetectionWorker);

            //Perform OCR to determine embeddings
            //OcrWorker ocrWorker = new OcrWorker();
            //WorkerList.Add(ocrWorker);



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

             Task.Run(() =>
            {
                while (true)
                {
                    IdealMonitorRectWorker.DetermineRect();
                    Thread.Sleep(2000);
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

