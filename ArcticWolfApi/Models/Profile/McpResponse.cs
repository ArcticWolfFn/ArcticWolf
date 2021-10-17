using ArcticWolfApi.Models.Profile.Changes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Profile
{
    public class McpResponse
    {
        [JsonProperty("profileRevision")]
        public int ProfileRevision { get; set; }

        [JsonProperty("profileId")]
        public string ProfileId { get; set; }

        [JsonProperty("profileChangesBaseRevision")]
        public int ProfileChangesBaseRevision { get; set; }

        [JsonProperty("profileChanges")]
        public List<object> ProfileChanges { get; set; }

        [JsonProperty("profileCommandRevision")]
        public int ProfileCommandRevision { get; set; }

        [JsonProperty("serverTime")]
        public DateTime ServerTime { get; set; }

        [JsonProperty("responseVersion")]
        public int ResponseVersion { get; set; }

        public McpResponse(
          Profile profile,
          int rvn,
          string profileId,
          List<object> changes,
          bool isUpdated = false)
        {
            this.ProfileRevision = profile.Rvn;
            this.ProfileId = profileId;
            this.ProfileChangesBaseRevision = isUpdated ? profile.Rvn - 1 : profile.Rvn;
            if ((isUpdated ? (profile.Rvn - 1 == rvn ? 1 : 0) : (profile.Rvn == rvn ? 1 : 0)) != 0)
                this.ProfileChanges = changes;
            else
                this.ProfileChanges = new List<object>()
        {
          (object) new McpFullProfileUpdate(profile)
        };
            this.ProfileCommandRevision = profile.Rvn;
            this.ServerTime = DateTime.UtcNow.TrimDate();
            this.ResponseVersion = 1;
        }
    }
}
