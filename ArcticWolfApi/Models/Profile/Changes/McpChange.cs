using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Profile.Changes
{
    public class McpChange
    {
        [JsonProperty("changeType", Order = -2)]
        public string ChangeType { get; set; }

        public McpChange(string changeType) => ChangeType = changeType;
    }
}
