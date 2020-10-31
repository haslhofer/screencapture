using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;


namespace screencapture
{

    public class ShowNoteWorker
    {
        private static string _searchText = "haslhofer";
        
        private static int sleepDefaultMS = 50;

        public static void ShowNotes()
        {
            bool loopforever = true;

            do
            {
                ScreenState s;
                if (Program._CaptureItems.TryDequeue(out s))
                {
                    System.Diagnostics.Debug.WriteLine(s.ToString());
                    ScreenText foundScreenText = GetMatchingScreenText(s);
                    if (foundScreenText !=null)
                    {
                        System.Diagnostics.Debug.WriteLine("Found " + foundScreenText.Content);

                    }
                }


                if (loopforever)
                {
                    System.Threading.Thread.Sleep(sleepDefaultMS);
                }
            }
            while (loopforever);


        }

        //Look at the screen and see if you can find an anchor;
        private static ScreenText GetMatchingScreenText(ScreenState s)
        {
            foreach (var aNote in Program._NoteReferences)
            {
                foreach (var screenText in s.TextOnScreen)
                {
                    if (screenText.Content.ToLower().Contains(aNote.Anchor))
                    {
                        return screenText;
                    }
                }
            }
            return null;
        }

        
    }
}
