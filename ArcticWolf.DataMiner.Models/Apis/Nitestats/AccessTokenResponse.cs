using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcticWolf.DataMiner.Models.Apis.Nitestats
{
    public class AccessTokenResponse
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("lastUpdated")]
        public int LastUpdated { get; set; }
    }
}
