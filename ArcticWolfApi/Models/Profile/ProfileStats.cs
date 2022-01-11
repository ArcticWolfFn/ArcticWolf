using Newtonsoft.Json;
namespace ArcticWolfApi.Models.Profile
{
    public class ProfileStats
    {
        [JsonProperty("attributes")]
        public object Attributes { get; set; }

        [JsonProperty("commandRevision")]
        public int CommandRevision { get; set; } = -1;
    }
}
