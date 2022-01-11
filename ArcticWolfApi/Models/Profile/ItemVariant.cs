using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArcticWolfApi.Models.Profile
{
    public class ItemVariant
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("active")]
        public string Active { get; set; }

        [JsonProperty("owned")]
        public List<string> Owned { get; set; }

        [JsonIgnore]
        public string Id { get; set; }
    }
}
