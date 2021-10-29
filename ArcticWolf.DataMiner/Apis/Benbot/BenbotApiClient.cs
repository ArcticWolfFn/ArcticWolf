using ArcticWolf.DataMiner.Common.Http;
using ArcticWolf.DataMiner.Models.Apis.Benbot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Apis.Benbot
{
    public class BenbotApiClient
    {
        private const string LOG_PREFIX = "BenbotApi";
        private HttpClient _client;
        private static readonly WebHeaderCollection defaultHeaders = new()
        {
            { HttpRequestHeader.ContentType, "application/json" }
        };

        public BenbotApiClient()
        {
            Log.Information("Initialising...", LOG_PREFIX);
            _client = new HttpClient(defaultHeaders);

            GetStatus();
        }

        public void GetStatus()
        {
            HttpResponse response = _client.Request("https://benbot.app/api/v1/status");

            if (!response.Success)
            {
                Log.Error("Request to retrieve status data was not successful!", LOG_PREFIX);
                return;
            }

            StatusResponse statusResponse = JsonConvert.DeserializeObject<StatusResponse>(response.Content);

            Log.Information("Current FN CDN version is " + statusResponse.CurrentCdnVersion, LOG_PREFIX);
        }
    }
}
