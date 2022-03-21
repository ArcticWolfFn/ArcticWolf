using Newtonsoft.Json;

namespace ArcticWolf.Apis.NiteStats.Models
{
    public class AccessTokenResponse
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("lastUpdated")]
        public int LastUpdated { get; set; }
    }
}
