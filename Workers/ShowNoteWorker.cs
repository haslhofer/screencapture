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
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        private static int sleepDefaultMS = 50;

        public static void ShowNotes()
        {
            bool loopforever = true;

            do
            {
                ScreenState s;
                if (Program._CaptureItems.TryDequeue(out s))
                {
                    Logger.Info("Found work item to dequeue");
                    System.Diagnostics.Debug.WriteLine(s.ToString());

                    NoteReference foundNote; //which note produced a hit
                    ScreenText screenText; //what is the location and text on the screen that produced a hit

                    GetMatchingNote(s, out foundNote, out screenText);
                    
                    if (foundNote!=null)
                    {
                        Logger.Info("Found Note text");
                        //System.Diagnostics.Debug.WriteLine("Found ");
                        Program._NoteUxManager.ShowNote(foundNote, screenText);
                        //Program._NoteForm.ChangePos(foundScreenText.Position.Left, foundScreenText.Position.Top);
                        

                    }
                    else
                    {
                        Logger.Info("Did not find screen text" );
                        Program._NoteUxManager.HideNotes();
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
        private static void GetMatchingNote(ScreenState s, out NoteReference foundNote, out ScreenText matchingScreenText)
        {
            foundNote = null;
            matchingScreenText = null;

            foreach (var aNote in Program._NoteReferences)
            {
                foreach (var screenText in s.TextOnScreen)
                {
                    if (screenText.Content.ToLower().Contains(aNote.Anchor))
                    {
                        foundNote = aNote; 
                        matchingScreenText = screenText;
                        return;
                    }
                }
            }
            return;
        }

        
    }
}
