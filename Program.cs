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
        public static ConcurrentQueue<ScreenState> _CaptureItems = new ConcurrentQueue<ScreenState>();
        public static List<NoteReference> _NoteReferences = new List<NoteReference>();

        public static StickyNote _NoteForm;

        [STAThread]
        static int Main(
            string directory = "",
            bool loopforever = true,
            bool detectText = true,
            bool generateTextDump = false,
            //bool reRenderText = false,
            bool detectProcesses = false


            )
        {
            ConfigureLogging();
            Logger.Info("Startup");

            FillTestData();


            bool result = SHCore.SetProcessDpiAwareness(SHCore.PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware);
            var setDpiError = Marshal.GetLastWin32Error();

            if (directory == String.Empty) { directory = @"c:\data\temp2\"; }

            //Start workers

            Thread myCaptureThread = new Thread(() => CaptureAndWorker.CaptureAndWrite(directory, loopforever, detectText, generateTextDump, detectProcesses));
            myCaptureThread.Start();

            Thread myShowNote = new Thread(() => ShowNoteWorker.ShowNotes());
            myShowNote.Start();


            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _NoteForm = new StickyNote("Test");
            _NoteForm.Show();
            //Application.Run(_NoteForm);
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
            r1.Anchor = "wenjun";
            r1.Note = "Send a Note to Wenjun";
            _NoteReferences.Add(r1);
        }


    }
}

