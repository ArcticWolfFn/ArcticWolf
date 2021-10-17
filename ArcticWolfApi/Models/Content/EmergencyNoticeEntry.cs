using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class EmergencyNoticeEntry : BasePagesEntry
    {
        public EmergencyNoticeEntry(params PagesMessage[] messages)
          : base("emergencynotice")
        {
            this.News = new BattleRoyaleNews(Array.Empty<object>())
            {
                Messages = ((IEnumerable<PagesMessage>)messages).Select<PagesMessage, PagesMessageBase>((Func<PagesMessage, PagesMessageBase>)(x => new PagesMessage(x.Message.Title, x.Message.Body).Message)).ToList<PagesMessageBase>()
            };
        }

        [JsonProperty("news", Order = -7)]
        public BattleRoyaleNews News { get; set; }
    }
}
