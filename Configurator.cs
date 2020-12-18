using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;


namespace screencapture
{
    
    public class Configurator
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static string HashTagConfigPath =@"C:\Users\gerhas\Documents\GitHub\hashtag\text\config.txt";
        //private static List<string> OptedInPages = new List<string>() {"AI", "News", "Gerald Haslhofer", "Tim Franklin", "Vishnu Nath", "Shilpa Ranganathan", "Ryan McMinn", "Rachel Haslhofer", "Michael Howe"};
        //public static IEnumerable<OneNoteDescriptor> OptedInOneNotes {get;set;}

        private static string ONENOTE_DESTINATION_PAGE_NAME = "oacapture";
        public static OneNoteDescriptor DestinationOneNote {get;set;}


        public static void Init()
        {
            Logger.Info("Before retrieve OneNotePages");
            //Retrieve all OneNotePages, and see which ones are opted in
            var OneNote = OneNoteCapture.GetAllPages().Result;
            var optedIn = from x in OneNote  where x.PageTitle.ToLower() == ONENOTE_DESTINATION_PAGE_NAME select x;
            if (optedIn.First() != null) 
            {
                DestinationOneNote  = optedIn.First();
            }
            else
            {
                throw new System.Exception("Destination OneNote page with name 'OACapture' was not found");
            }
            Logger.Info("After retrieve OneNotePages");
        }

        /* public static void WriteConfig()
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
 */
/*         public static string GetPageIdFromHashTag(string tag)
        {
            var oneNotePage = from x in OptedInOneNotes where x.PageTitle.Equals(tag) select x;            
            return oneNotePage.First().PageId;
            
        } */


    }
}