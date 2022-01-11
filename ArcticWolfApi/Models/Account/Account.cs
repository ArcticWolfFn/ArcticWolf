using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Account
{
    public class Account
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("externalAuths")]
        public object ExternalAuths => new object();
    }
}
