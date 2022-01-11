using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Content
{
    public class BattleRoyaleNewsEntry : BasePagesEntry
    {
        [JsonProperty("news", Order = -7)]
        public BattleRoyaleNews News { get; set; }

        public BattleRoyaleNewsEntry(params object[] motds) : base("battleroyalenews")
        {
            News = new BattleRoyaleNews(motds);
        }
    }
}
