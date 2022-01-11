using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Content
{
    public class BackgroundData
    {
        [JsonProperty("stage", NullValueHandling = NullValueHandling.Ignore)]
        public string Stage { get; set; }

        [JsonProperty("key", NullValueHandling = NullValueHandling.Ignore)]
        public string Key { get; set; }

        [JsonProperty("_type")]
        public string Type => "DynamicBackground";

        public BackgroundData(string stage = null, string key = null)
        {
            Stage = stage;
            Key = key;
        }
    }
}
