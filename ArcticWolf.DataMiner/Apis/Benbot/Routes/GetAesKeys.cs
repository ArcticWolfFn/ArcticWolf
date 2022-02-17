using ArcticWolf.DataMiner.Common.Http;
using ArcticWolf.DataMiner.Common.Json;
using ArcticWolf.DataMiner.Models.Apis.Benbot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Apis.Benbot.Routes
{
    public class GetAesKeys
    {
        public bool SupportsPreviousFnVersions => true;

        private HttpClient _client;

        private const string LOG_PREFIX = "Benbot|Aes";

        public GetAesKeys(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Gets the aes keys for the specified or current version
        /// </summary>
        public AesResponse Get(string version = null)
        {
            string url = "https://benbot.app/api/v1/aes";
            if (version != null)
            {
                url += "?version=" + version;
            }

            HttpResponse response = _client.Request(url);
            if (!response.Success)
            {
                Log.Error("Request to retrieve aes data was not successful!", LOG_PREFIX);
                return null;
            }

            AesResponse aesResponse = JsonDeserializer.Deserialize<AesResponse>(response.Content);

            return aesResponse;
        }
    }
}
