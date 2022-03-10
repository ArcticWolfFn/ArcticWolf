using Newtonsoft.Json;

namespace ArcticWolf.Apis.Benbot.Models
{
    public class AesResponse
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("mainKey")]
        public string MainKey { get; set; }

        [JsonProperty("dynamicKeys")]
        public Dictionary<string, string> DynamicKeys { get; set; }
    }
}
