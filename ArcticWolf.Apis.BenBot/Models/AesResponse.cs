using ArcticWolf.Apis.Base;
using Newtonsoft.Json;

namespace ArcticWolf.Apis.BenBot.Models
{
    public class AesResponse : IContainsVersion
    {
        [JsonProperty("mainKey")]
        public string MainKey { get; set; }

        [JsonProperty("dynamicKeys")]
        public Dictionary<string, string> DynamicKeys { get; set; }

        [JsonProperty("version")]
        public override string Version { get; set; }
    }
}
