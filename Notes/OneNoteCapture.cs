using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;


namespace screencapture
{
    public class OneNoteDescriptor
    {
        public string PageId {get;set;}
        public string PageTitle {get;set;}

    }

    public class OneNoteCapture
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static GraphServiceClient _graphClient;
        private static string _accessToken;
        
        public static void Init()
        {

            //Ensure app secrets are properly set
            //See tutorial https://docs.microsoft.com/en-us/graph/tutorials/dotnet-core?tutorial-step=3
            //
            //dotnet user-secrets set appId "YOUR_APP_ID_HERE"
            
            var appConfig = LoadAppSettings();

            if (appConfig == null)
            {
                Logger.Log(NLog.LogLevel.Fatal, "Missing or invalid appsettings.json...exiting");
                throw new System.Exception("Missing or invalid appsettings.json");
            }

            var appId = appConfig["appId"];
            string scopesString = "User.Read;MailboxSettings.Read;Calendars.ReadWrite;Notes.Create;Notes.Read;Notes.ReadWrite";
            var scopes = scopesString.Split(';');

            // Initialize the auth provider with values from appsettings.json
            var authProvider = new DeviceCodeAuthProvider(appId, scopes);
            
            // Request a token to sign in the user
            _accessToken = authProvider.GetAccessToken2().Result;
            _graphClient = new GraphServiceClient(authProvider);

        }
     

        public static async Task<List<OneNoteDescriptor>> GetAllPages()
        {
            List<OneNoteDescriptor> res = new List<OneNoteDescriptor>();
            var pages =  await _graphClient.Me.Onenote.Pages.Request().GetAsync(); 
            foreach (var aPage in pages)
            {
                OneNoteDescriptor d= new OneNoteDescriptor();
                d.PageId = aPage.Id;
                d.PageTitle = aPage.Title;
                res.Add(d);
            }
            return res;
        }

        public static async Task<string> AppendImage(string pathToImage, string pageId)
        {

            //byte[] bytes = System.IO.File.ReadAllBytes(@"C:\data\temp2\capture_Image_637403620958932705_0.jpg"); 
            byte[] bytes = System.IO.File.ReadAllBytes(pathToImage); 
            MemoryStream m = new MemoryStream(bytes);
            System.Drawing.Image myImg = System.Drawing.Image.FromStream(m);
            int width = myImg.Width;
            int height = myImg.Height;
            

            //Get Base64 encoding of string
            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);  //attempt5
            string imageDataURL = string.Format("data:image/jpeg;base64,{0}", base64String);  //attempt6

            string imgSrc = "'<img src=" + imageDataURL  + @" width="""+ width +@""" height=""" +height +@"""/>'";

            string requestUrl = $"https://graph.microsoft.com/v1.0/me/onenote/pages/{pageId}/content";
            string body = @"[{
                            'target': 'body',
                            'action': 'append',
                            'position': 'after',
                            'content': " + imgSrc + " }]";


            var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage req = new HttpRequestMessage()
            {
                Method = new HttpMethod("PATCH"),
                Content = content,
                RequestUri = new Uri(requestUrl)
            };
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(requestUrl),
            };
            client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + _accessToken);
            HttpResponseMessage response = await client.SendAsync(req);

            return response.StatusCode.ToString();

        }

        static IConfigurationRoot LoadAppSettings()
        {
            var appConfig = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            // Check for required settings
            if (string.IsNullOrEmpty(appConfig["appId"]) )
            {
                return null;
            }

            return appConfig;
        }
    }
}
