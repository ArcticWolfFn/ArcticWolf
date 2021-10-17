using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class BattleRoyaleNewsEntry : BasePagesEntry
    {
        public BattleRoyaleNewsEntry(params object[] motds)
          : base("battleroyalenews")
        {
            this.News = new BattleRoyaleNews(motds);
        }

        [JsonProperty("news", Order = -7)]
        public BattleRoyaleNews News { get; set; }
    }
}
