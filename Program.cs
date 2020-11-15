using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using System.Collections.Concurrent;
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
        //public static NoteUxManager _NoteUxManager; 
        //public static OverlayUx _OverlayUx;

        public static ControllerUx _ControllerUx;



        [STAThread]
        static int Main(
            string directory = "",
            bool loopforever = true,
            bool detectText = true,
            bool generateTextDump = true,
            //bool reRenderText = false,
            bool detectProcesses = false


            )
        {

            //LanguageModel.GetConfidenceFromServer();
            //TestPython();
            //return 0;


            ConfigureLogging();
            Logger.Info("Startup");

            FillTestData();

            bool result = SHCore.SetProcessDpiAwareness(SHCore.PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware);
            var setDpiError = Marshal.GetLastWin32Error();

            if (directory == String.Empty) { directory = @"c:\data\temp2\"; }

            //Determine the screen to watch
               
            MonitorHelper h = new MonitorHelper();
            var displays = h.GetDisplays();
            _MonitorToWatch= displays[1];

            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _ControllerUx = new ControllerUx();
            _ControllerUx.Show();
            //_ControllerUx.Hide();
            
            //_NoteUxManager= new NoteUxManager();

            //_OverlayUx = new OverlayUx(_MonitorToWatch.MonitorArea.Top,_MonitorToWatch.MonitorArea.Left,_MonitorToWatch.MonitorArea.Bottom, _MonitorToWatch.MonitorArea.Right);
            //_OverlayUx.Show();
            //_OverlayUx.HideOverlay();

            //Application.Run(_NoteForm);
            //Start workers

            Thread myCaptureThread = new Thread(() => CaptureAndWorker.CaptureAndWrite(directory, loopforever, detectText, generateTextDump, detectProcesses));
            myCaptureThread.Start();

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

        private static void FillTestData()
        {
            NoteReference r1 = new NoteReference();
            r1.Anchor = "samnerkar";
            r1.Note = "Ask about Satya AI365 slide deck";
            _NoteReferences.Add(r1);
        }

        private static void TestPython()
        {
            LanguageModel.RunCmd(@"c:\Users\gerhas\Documents\GitHub\hashtag\test.py", string.Empty, @"c:\Users\gerhas\Documents\GitHub\hashtag");
        }



    }
}

