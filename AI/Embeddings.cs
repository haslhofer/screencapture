using System;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;


namespace screencapture
{

    public class TextEmbedding
    {
        public string SourceText { get; set; }
        public double[] EmbeddingVector { get; set; }
    }

    public class Embeddings
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        //private static string EMBEDDING_SERVER = "http://127.0.0.1:5000/embedding";
        private static string EMBEDDING_SERVER = "http://20.55.5.215:80/embedding";

        
        private static bool IsSuccess(System.Net.HttpStatusCode code)
        {
            return ((int)code >= 200 && (int)code < 300);
        }
        public static async Task<TextEmbedding> GetEmbeddingFromText(string textToEmbed)
        {
            try
            {

                //var request = new { text = textToEmbed};

                string requestUrl = EMBEDDING_SERVER;

                string requestpayload = JsonConvert.SerializeObject(
                new
                {
                    text = textToEmbed

                });


                var content = new StringContent(requestpayload, System.Text.Encoding.UTF8, "application/json");
                HttpRequestMessage req = new HttpRequestMessage()
                {
                    Method = new HttpMethod("POST"),
                    Content = content,
                    RequestUri = new Uri(requestUrl)
                };
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new Uri(requestUrl),
                };

                HttpResponseMessage response = await client.SendAsync(req);
                if (response.IsSuccessStatusCode)
                {

                    List<double> listVector = await response.Content.ReadFromJsonAsync<List<double>>();

                    TextEmbedding res = new TextEmbedding() { SourceText = textToEmbed, EmbeddingVector = listVector.ToArray()};
                    return res;
                }
                else
                {
                    Logger.Error("Request to Embedding Server failed:" + response.StatusCode.ToString());
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }

            return null;


        }
    }
}