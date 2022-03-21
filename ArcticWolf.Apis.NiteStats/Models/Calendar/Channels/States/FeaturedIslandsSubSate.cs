using ArcticWolf.Apis.NiteStats.Models.Calendar.Channels.States.Models;
using Newtonsoft.Json;

namespace ArcticWolf.Apis.NiteStats.Models.Calendar.Channels.States
{
    public class FeaturedIslandsSubSate
    {
        [JsonProperty("islandCodes")]
        public List<string> IslandCodes { get; set; }

        [JsonProperty("playlistCuratedContent")]
        public PlaylistCuratedContent PlaylistCuratedContent { get; set; }

        [JsonProperty("playlistCuratedHub")]
        public Dictionary<string, string> PlaylistCuratedHub { get; set; }

        [JsonProperty("islandTemplates")]
        public List<object> IslandTemplates { get; set; }
    }
}
