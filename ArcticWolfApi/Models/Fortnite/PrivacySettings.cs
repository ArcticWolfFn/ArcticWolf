using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Fortnite
{
    public class PrivacySettings
    {
        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("optOutOfPublicLeaderboards")]
        public bool OptOutOfPublicLeaderboards => false;

        public PrivacySettings(string accountId) => this.AccountId = accountId;
    }
}
