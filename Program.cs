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


namespace screencapture
{

    class Program
    {
        


        public static ConcurrentQueue<ScreenState> _CaptureItems = new ConcurrentQueue<ScreenState>();
        public static List<NoteReference> _NoteReferences = new List<NoteReference>();

        public static Form1 _NoteForm;

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

            FillTestData();


            bool result = SHCore.SetProcessDpiAwareness(SHCore.PROCESS_DPI_AWARENESS.Process_Per_Monitor_DPI_Aware);
            var setDpiError = Marshal.GetLastWin32Error();

            if (directory == String.Empty) { directory = @"c:\data\temp2\"; }

            //Start workers

            Thread myCaptureThread = new Thread(() => CaptureAndWorker.CaptureAndWrite(directory, loopforever, detectText, generateTextDump, detectProcesses));
            myCaptureThread.Start();

            Thread myShowNote = new Thread(() => ShowNoteWorker.ShowNotes());
            myShowNote.Start();


            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            _NoteForm = new Form1();
            Application.Run(_NoteForm);

            //Console.ReadLine();


            return 0;
        }

        private static void FillTestData()
        {
            NoteReference r1 = new NoteReference();
            r1.Anchor = "hundertwasser";
            r1.Note = "This is a note about haslhofer";
            _NoteReferences.Add(r1);
        }

     
    }
}

