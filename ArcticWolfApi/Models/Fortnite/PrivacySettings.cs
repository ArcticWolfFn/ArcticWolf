using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Fortnite
{
    public class PrivacySettings
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("optOutOfPublicLeaderboards")]
        public bool OptOutOfPublicLeaderboards => false;

        public PrivacySettings(string accountId) => AccountId = accountId;
    }
}
