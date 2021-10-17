using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class SubgameSelectEntry : BasePagesEntry
    {
        [JsonProperty("saveTheWorldUnowned", Order = -7)]
        public PagesMessage SaveTheWorldUnowned => this.SaveTheWorld;

        [JsonProperty("battleRoyale", Order = -7)]
        public PagesMessage BattleRoyale { get; set; }

        [JsonProperty("creative", Order = -7)]
        public PagesMessage Creative { get; set; }

        [JsonProperty("saveTheWorld", Order = -7)]
        public PagesMessage SaveTheWorld { get; set; }

        public SubgameSelectEntry()
          : base("subgameselectdata")
        {
        }
    }
}
