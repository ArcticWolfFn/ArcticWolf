using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Profile.Changes
{
    public class McpStatModified : McpChange
    {
        [JsonProperty("name")]
        public string Stat { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }

        public McpStatModified(string stat, object value) : base("statModified")
        {
            Stat = stat;
            Value = value;
        }
    }
}
