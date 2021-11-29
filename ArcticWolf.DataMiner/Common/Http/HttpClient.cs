using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ArcticWolf.DataMiner.Common.Http
{
    public class HttpClient
    {
        private const string LOG_PREFIX = "Http";
        private WebHeaderCollection _defaultHeaders = new();

        public HttpClient(WebHeaderCollection defaultHeaders = null)
        {
            if (defaultHeaders != null)
            {
                _defaultHeaders = defaultHeaders;
            }
        }

        public HttpResponse Request(string url, RequestMethod method = RequestMethod.GET)
        {
            HttpResponse result = new HttpResponse();

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = method.ToString().ToUpper();
            request.UserAgent = "ArcticWolf.DataMiner v1.0.0 DevAlpha";
            request.Accept = "application/json";
            request.Headers = _defaultHeaders;

            Log.Debug($"Creating a {method} request to '{url}'", LOG_PREFIX);

            // ToDo: add support for POST requests
            /*if (postdata != null)
            {
                string s = "";
                foreach (string str2 in postdata.Keys)
                {
                    string[] textArray1 = new string[] { s, HttpUtility.UrlEncode(str2), "=", HttpUtility.UrlEncode(postdata[str2]), "&" };
                    s = string.Concat(textArray1);
                }
                byte[] bytes = Encoding.ASCII.GetBytes(s);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                try
                {
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                catch (Exception ex)
                {
                    LogController.Write(LogPrefix + "Failed to get the request Stream. URL: " + url + " | Error: " + ex.Message, LogController.LogType.Error);
                    return_str.Add("status_code", "REQUEST_FAILED");
                    return_str.Add("response", "REQUEST_FAILED");
                    return return_str;
                }
            }*/

            HttpWebResponse response = null;
            string statusCode = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                statusCode = ((int)response.StatusCode).ToString();
            }
            catch (WebException ex)
            {

                if (ex.Status.ToString() == "OK")
                {
                    response = ex.Response as HttpWebResponse;
                    statusCode = ((int)response.StatusCode).ToString();
                }
            }
            if (response != null)
            {
                Stream resStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(resStream);
                string response_data = sr.ReadToEnd();
                sr.Close();
                resStream.Close();
                Log.Debug($"Status code: {statusCode}", LOG_PREFIX);
                Log.Verbose($"Response data: {response_data}", LOG_PREFIX);

                result.Content = response_data;
                result.Success = true;
            }
            else
            {
                Log.Error($"Request to '{url}' failed. ", LOG_PREFIX);
            }
            return result;
        }
    }

    public struct HttpResponse
    {
        public string Content { get; set; }
        public bool Success { get; set; }
    }
}
