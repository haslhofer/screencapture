using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace screencapture
{
    public class Configurator
    {
        private static string HashTagConfigPath =@"C:\Users\gerhas\Documents\GitHub\hashtag\text\config.txt";
        private static List<string> OptedInPages = new List<string>() {"AI", "News"};
        public static IEnumerable<OneNoteDescriptor> OptedInOneNotes {get;set;}


        public static void Init()
        {
            //Retrieve all OneNotePages, and see which ones are opted in
            var OneNote = OneNoteCapture.GetAllPages().Result;
            var optedIn = from x in OneNote  where OptedInPages.Contains(x.PageTitle) select x;
            OptedInOneNotes = optedIn;
            
        }

        public static void WriteConfig()
        {
            FileStream f = new FileStream(HashTagConfigPath, FileMode.Create);
            StreamWriter w = new StreamWriter(f);
            var hashTags = from x in OptedInOneNotes select x.PageTitle;
            foreach (string aHashTag in hashTags)
            {
                w.WriteLine(aHashTag);
            }
            w.Flush();
            w.Close();
            
            f.Close();
        }

        public static string GetPageIdFromHashTag(string tag)
        {
            var oneNotePage = from x in OptedInOneNotes where x.PageTitle.Equals(tag) select x;            
            return oneNotePage.First().PageId;
            
        }


    }
}