using Newtonsoft.Json;

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

        public SubgameInfoEntry() : base("SubgameInfo")
        {
        }
    }
}
