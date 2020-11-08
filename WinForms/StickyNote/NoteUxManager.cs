using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace screencapture
{
    public class NoteUxManager
    {
        StickyNote _note;

        public NoteUxManager()
        {
            _note = new StickyNote();
            _note.Show();

        }
        public void ShowNote(NoteReference note, ScreenText text)
        {
            _note.InitNote(note);
            _note.ChangePos(text.Position.Left + 30, text.Position.Top + 30);
            _note.ShowNote();
        }
        public void HideNotes()
        {
            if (_note != null)
            {
                _note.HideNote();
            }
        }
    }
}