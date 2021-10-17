using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class BackgroundData
    {
        public BackgroundData(string stage = null, string key = null)
        {
            this.Stage = stage;
            this.Key = key;
        }

        [JsonProperty("stage", NullValueHandling = NullValueHandling.Ignore)]
        public string Stage { get; set; }

        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        [JsonProperty("_type")]
        public string Type => "DynamicBackground";
    }
}
