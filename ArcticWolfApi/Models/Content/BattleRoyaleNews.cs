using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class BattleRoyaleNews
    {
        public BattleRoyaleNews(params object[] motds)
        {
            this.Messages = ((IEnumerable<object>)motds).Select<object, PagesMessageBase>((Func<object, PagesMessageBase>)(x =>
            {
                BattleRoyaleNewsMOTD battleRoyaleNewsMotd = (BattleRoyaleNewsMOTD)x;
                return new PagesMessage(battleRoyaleNewsMotd.Title, battleRoyaleNewsMotd.Body, battleRoyaleNewsMotd.TileImage).Message;
            })).Where<PagesMessageBase>((Func<PagesMessageBase, bool>)(x => x != null)).ToList<PagesMessageBase>();
            this.MOTDS = ((IEnumerable<object>)motds).ToList<object>();
        }

        [JsonProperty("messages")]
        public List<PagesMessageBase> Messages { get; set; }

        [JsonProperty("motds")]
        public List<object> MOTDS { get; set; }
    }
}
