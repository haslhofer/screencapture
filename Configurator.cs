using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;


namespace screencapture
{
    
    public class Configurator
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    
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
    }
}