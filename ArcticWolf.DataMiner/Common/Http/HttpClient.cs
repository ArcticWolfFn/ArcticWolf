using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Common.Http
{
    public class HttpClient
    {
        private const string LOG_PREFIX = "Http";

        public HttpResponse Request(string url, RequestMethod method = RequestMethod.GET)
        {
            HttpResponse response = new HttpResponse();

            WebClient client = new();

            try
            {
                Log.Debug($"Creating a {method} request to '{url}'", LOG_PREFIX);

                switch (method)
                {
                    case RequestMethod.GET:
                        response.Content = client.DownloadString(url);
                        break;

                    case RequestMethod.POST:
                        throw new Exception("POST is not supported yet");
                }
                response.Success = true;
            } 
            catch (WebException ex)
            {
                Log.Error($"Request to '{url}' failed", LOG_PREFIX);
                Log.Error("Request Status: " + ex.Status, LOG_PREFIX);
                Log.Error("Error Message: " + ex.Message, LOG_PREFIX);
                response.Success = false;
            }
            catch (Exception ex)
            {
                Log.Error($"Request to '{url}' failed", LOG_PREFIX);
                Log.Error("Error Message: " + ex.Message, LOG_PREFIX);
                Log.Error("StackTrace: " + ex.StackTrace, LOG_PREFIX);
                response.Success = false;
            }

            if (client.ResponseHeaders != null)
            {
                foreach (string header in client.ResponseHeaders)
                {
                    string value = "";
                    foreach (string item in client.ResponseHeaders.GetValues(header))
                        value += item + ",";

                    Log.Debug($"Response Header: {header} Value: '{value}'", LOG_PREFIX);
                }
            }

            return response;
        }
    }

    public struct HttpResponse
    {
        public string Content { get; set; }
        public bool Success { get; set; }
    }
}
