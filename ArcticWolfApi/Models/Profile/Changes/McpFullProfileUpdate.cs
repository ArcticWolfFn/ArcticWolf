using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Profile.Changes
{
    public class McpFullProfileUpdate : McpChange
    {
        [JsonProperty("profile")]
        public Profile Profile { get; set; }

        public McpFullProfileUpdate(Profile profile) : base("fullProfileUpdate")
        {
            Profile = profile;
        }
    }
}
