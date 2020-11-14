using Novacode;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;


namespace screencapture
{

    public class NoteCapture
    {
        private static string NotesDirectory = @"C:\Users\gerhas\Documents\GitHub\CapturedNotes\";
        private static float MaxWidth = 600;
        public static void AddScreenshot(string hashtag, string pathToImage)
        {
            string docxPath = Path.Combine(NotesDirectory, hashtag + ".docx");

            
            
            using (DocX doc = GetOrCreateDoc(docxPath))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    System.Drawing.Image myImg = System.Drawing.Image.FromFile(pathToImage);
                    

                    myImg.Save(memoryStream, myImg.RawFormat);  // Save your picture in a memory stream.
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    Novacode.Image img = doc.AddImage(memoryStream); // Create image.
                    Paragraph p = doc.InsertParagraph("Test", false);
                    float scaleFactor = MaxWidth / myImg.Width;
                    Picture pic1 = img.CreatePicture((int)(myImg.Height * scaleFactor),(int)MaxWidth);     // Create picture.
                    p.InsertPicture(pic1, 0); // Insert picture into paragraph.

                    doc.Save();
                }
            }
        }

        private static DocX GetOrCreateDoc(string path)
        {
            if (!File.Exists(path))
            {
                return DocX.Create(path);
            }
            return DocX.Load(path);


        }
    }
}