using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ArcticWolfApi.Models.Content
{
    public class EmergencyNoticeEntry : BasePagesEntry
    {
        [JsonProperty("news", Order = -7)]
        public BattleRoyaleNews News { get; set; }

        public EmergencyNoticeEntry(params PagesMessage[] messages) : base("emergencynotice")
        {
            News = new BattleRoyaleNews(Array.Empty<object>())
            {
                Messages = ((IEnumerable<PagesMessage>)messages).Select(x => new PagesMessage(x.Message.Title, x.Message.Body).Message).ToList()
            };
        }
    }
}
