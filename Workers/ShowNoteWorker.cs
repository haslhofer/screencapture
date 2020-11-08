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
                    Program._ControllerUx.DeleteFloaters();
                    Logger.Info("Found work item to dequeue");
                    System.Diagnostics.Debug.WriteLine(s.ToString());

                    NoteReference foundNote; //which note produced a hit
                    ScreenText screenText; //what is the location and text on the screen that produced a hit

                    var matches = GetMatchingNote(s);
                    if (matches.Count ==0 )
                    {
                        Logger.Info("Did not find note");
                    }
                    else
                    {
                        Logger.Info("!!! NOTE FOUND");
                    }
                    foreach (Tuple<NoteReference, ScreenText> match in matches)
                    {
                        Program._ControllerUx.AddFloater(match.Item2.Position.Left, match.Item2.Position.Top, match.Item2.Position.Right, match.Item2.Position.Bottom, match.Item1.Note);
                    }
                    
                    /* if (foundNote!=null)
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
                    } */
                }


                if (loopforever)
                {
                    System.Threading.Thread.Sleep(sleepDefaultMS);
                }
            }
            while (loopforever);


        }

        //Look at the screen and see if you can find an anchor;
        private static  List<Tuple<NoteReference, ScreenText>> GetMatchingNote(ScreenState s)
        {
            List<Tuple<NoteReference, ScreenText>> res = new List<Tuple<NoteReference, ScreenText>>();

            
            foreach (var aNote in Program._NoteReferences)
            {
                foreach (var screenText in s.TextOnScreen)
                {
                    if (screenText.Content.ToLower().Contains(aNote.Anchor))
                    {
                        res.Add(new Tuple<NoteReference, ScreenText>(aNote, screenText));
                        
                    }
                }
            }
            return res;
        }

        
    }
}
