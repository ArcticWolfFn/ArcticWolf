using ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels.States.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels.States
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
