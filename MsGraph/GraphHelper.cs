using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace GraphTutorial
{
    public class PatchCommand
    {
        //The target to update
        public string Target { get; set; }

        //The action to perform which can be insert, append or replace
        public string Action { get; set; }

        //The new HTML content to be added to the page for the action
        public string Content { get; set; }
    }
    public class GraphHelper
    {
        private static GraphServiceClient graphClient;
        public static void Initialize(IAuthenticationProvider authProvider)
        {
            graphClient = new GraphServiceClient(authProvider);
        }

        public static async Task<IOnenoteNotebooksCollectionPage> GetMeAsync2()
        {
            try
            {
                // GET /me
                return await graphClient.Me.Onenote.Notebooks.Request().GetAsync();

            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error getting signed-in user: {ex.Message}");
                return null;
            }
        }


        public static async Task<string> PostYesImages(string bearer)
        {
            string requestUrl = $"https://graph.microsoft.com/v1.0/me/onenote/pages";
            const string imagePartName = "image1";
            const string imagePartName2 = "image2";

            var allpages = await graphClient.Me.Onenote.Pages.Request().GetAsync();
            var firstPage = allpages[0];

            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(requestUrl),
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + bearer);

            string date = DateTime.Now.ToString("o");
            string simpleHtml = "<html>" +
                                "<head>" +
                                "<title>A simple page created with an image on it</title>" +
                                "<meta name=\"created\" content=\"" + date + "\" />" +
                                "</head>" +
                                "<body>" +
                                "<h1>This is a page with an image on it</h1>" +
                                "<img src=\"name:" + imagePartName +
                                "\" alt=\"A beautiful logo\" width=\"3840\" height=\"1600\" />" +
                                "<img src=\"name:" + imagePartName2 +
                                "\" alt=\"A beautiful logo2\" width=\"3840\" height=\"1600\" />" +
                                "</body>" +
                                "</html>";

            //FileStream f = new FileStream(@"C:\Users\gerhas\Pictures\smallpic.JPG", FileMode.Open);
            FileStream f = new FileStream(@"C:\data\temp2\capture_Image_637403620958932705_0.jpg", FileMode.Open);
            FileStream f2 = new FileStream(@"C:\data\temp2\capture_Image_637403625691622583_0.jpg", FileMode.Open);


            using (var imageContent = new StreamContent(f))
            {
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                using (var imageContent2 = new StreamContent(f2))
                {
                    imageContent2.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                    var createMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                    {
                        Content = new MultipartFormDataContent
                        {
                            {new StringContent(simpleHtml, System.Text.Encoding.UTF8, "text/html"), "Presentation"},
                            {imageContent, imagePartName},
                            {imageContent2, imagePartName2}
                        }
                    };
                    var response = await client.SendAsync(createMessage);
                }

                // Must send the request within the using block, or the image stream will have been disposed.

            }

            f.Close();

            return "ok";
        }

        public static async Task<string> PostYesImage(string bearer)
        {
            string requestUrl = $"https://graph.microsoft.com/v1.0/me/onenote/pages";
            const string imagePartName = "image1";

            var allpages = await graphClient.Me.Onenote.Pages.Request().GetAsync();
            var firstPage = allpages[0];

            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(requestUrl),
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + bearer);

            string date = DateTime.Now.ToString("o");
            string simpleHtml = "<html>" +
                                "<head>" +
                                "<title>A simple page created with an image on it</title>" +
                                "<meta name=\"created\" content=\"" + date + "\" />" +
                                "</head>" +
                                "<body>" +
                                "<h1>This is a page with an image on it</h1>" +
                                "<img src=\"name:" + imagePartName +
                                "\" alt=\"A beautiful logo\" width=\"3840\" height=\"1600\" />" +
                                "</body>" +
                                "</html>";

            //FileStream f = new FileStream(@"C:\Users\gerhas\Pictures\smallpic.JPG", FileMode.Open);
            FileStream f = new FileStream(@"C:\data\temp2\capture_Image_637403620958932705_0.jpg", FileMode.Open);


            using (var imageContent = new StreamContent(f))
            {
                imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                var createMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Content = new MultipartFormDataContent
                    {
                        {new StringContent(simpleHtml, System.Text.Encoding.UTF8, "text/html"), "Presentation"},
                        {imageContent, imagePartName}
                    }
                };

                // Must send the request within the using block, or the image stream will have been disposed.
                var response = await client.SendAsync(createMessage);
            }

            f.Close();

            return "ok";
        }

        public static async Task<string> PostNoImage(string bearer)
        {
            string requestUrl = $"https://graph.microsoft.com/v1.0/me/onenote/pages";

            var allpages = await graphClient.Me.Onenote.Pages.Request().GetAsync();
            var firstPage = allpages[0];

            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri(requestUrl),
            };

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + bearer);

            string date = DateTime.Now.ToString("o");
            string simpleHtml = "<html>" +
                            "<head>" +
                            "<title>A simple page created from basic HTML-formatted text</title>" +
                            "<meta name=\"created\" content=\"" + date + "\" />" +
                            "</head>" +
                            "<body>" +
                            "<p>This is a page that just contains some simple <i>formatted</i> <b>text</b></p>" +
                            "<p>Here is a <a href=\"http://www.microsoft.com\">link</a></p>" +
                            "</body>" +
                            "</html>";

            var createMessage = new HttpRequestMessage(HttpMethod.Post, requestUrl)
            {
                Content = new StringContent(simpleHtml, System.Text.Encoding.UTF8, "text/html")
            };

            HttpResponseMessage response = await client.SendAsync(createMessage);

            return "ok";
        }

        public static async Task<string> AppendImage(string bearer)
        {

            byte[] bytes = System.IO.File.ReadAllBytes(@"C:\data\temp2\capture_Image_637403620958932705_0.jpg"); 
            //Get Base64 encoding of string
            string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);  //attempt5
            string imageDataURL = string.Format("data:image/jpeg;base64,{0}", base64String);  //attempt6

            string imgSrc = "'<img src=" + imageDataURL  + @" width=""200"" height=""800""/>'";



            var allpages = await graphClient.Me.Onenote.Pages.Request().GetAsync();
            var firstPage = allpages[0];
            string pageId = firstPage.Id;
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
            client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + bearer);
            HttpResponseMessage response = await client.SendAsync(req);

            return "done";


        }

        public static async Task<IOnenotePagesCollectionPage> GetMeAsyncPages(string bearer)
        {
            try
            {


                var allpages = await graphClient.Me.Onenote.Pages.Request().GetAsync();
                var firstPage = allpages[0];
                var newPage = new OnenotePage();


                foreach (var aPage in allpages)
                {

                    string pageId = aPage.Id;
                    string requestUrl = $"https://graph.microsoft.com/v1.0/me/onenote/pages/{pageId}/content";
                    string body = @"[{
                            'target': 'body',
                            'action': 'append',
                            'position': 'after',
                            'content': '<div> added new </div>'}]";

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
                    client.DefaultRequestHeaders.TryAddWithoutValidation("authorization", "Bearer " + bearer);
                    HttpResponseMessage response = await client.SendAsync(req);





                    Stream pageContent = await graphClient.Me.Onenote.Pages[aPage.Id]
                                        .Content // 
                                        .Request()
                                        .GetAsync();
                    StreamReader r = new StreamReader(pageContent);
                    string t = r.ReadToEnd();
                    System.Diagnostics.Debug.WriteLine(t);
                }

            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error getting signed-in user: {ex.Message}");
                return null;
            }

            return null;
        }


        public static async Task<User> GetMeAsync()
        {
            try
            {
                // GET /me
                return await graphClient.Me
                    .Request()
                    .Select(u => new
                    {
                        u.DisplayName,
                        u.MailboxSettings
                    })
                    .GetAsync();
            }
            catch (ServiceException ex)
            {
                Console.WriteLine($"Error getting signed-in user: {ex.Message}");
                return null;
            }
        }
    }
}
