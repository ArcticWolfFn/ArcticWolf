using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Profile
{
    public class ItemAttributes
    {
        [JsonProperty("max_level_bonus")]
        public int MaxLevelBonus => 1;

        [JsonProperty("level")]
        public int Level => 1;

        [JsonProperty("item_seen")]
        public bool ItemSeen => true;

        [JsonProperty("xp")]
        public int Xp => 0;

        [JsonProperty("variants", NullValueHandling = NullValueHandling.Ignore)]
        public List<ItemVariant> Variants { get; set; }

        [JsonProperty("favorite")]
        public bool Favorite => false;
    }
}
