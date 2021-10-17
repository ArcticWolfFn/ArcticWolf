using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class SubgameInfoEntry : BasePagesEntry
    {
        [JsonProperty("battleroyale")]
        public SubgameInfo BattleRoyale { get; set; }

        [JsonProperty("savetheworld")]
        public SubgameInfo SaveTheWorld { get; set; }

        [JsonProperty("creative")]
        public SubgameInfo Creative { get; set; }

        public SubgameInfoEntry()
          : base("SubgameInfo")
        {
        }
    }
}
