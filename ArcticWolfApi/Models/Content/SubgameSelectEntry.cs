using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Content
{
    public class SubgameSelectEntry : BasePagesEntry
    {
        [JsonProperty("saveTheWorldUnowned", Order = -7)]
        public PagesMessage SaveTheWorldUnowned => SaveTheWorld;

        [JsonProperty("battleRoyale", Order = -7)]
        public PagesMessage BattleRoyale { get; set; }

        [JsonProperty("creative", Order = -7)]
        public PagesMessage Creative { get; set; }

        [JsonProperty("saveTheWorld", Order = -7)]
        public PagesMessage SaveTheWorld { get; set; }

        public SubgameSelectEntry() : base("subgameselectdata")
        {
        }
    }
}
