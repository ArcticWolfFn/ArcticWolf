using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Fortnite
{
    public class VersionCheck
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        public VersionCheck(string type) => Type = type;
    }
}
