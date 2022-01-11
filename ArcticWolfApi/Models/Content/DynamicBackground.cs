using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Content
{
    public class DynamicBackground : BasePagesEntry
    {
        [JsonProperty("backgrounds", Order = -7)]
        public Background Backgrounds { get; set; }

        public DynamicBackground() : base("dynamicbackgrounds")
        {
        }
    }
}
