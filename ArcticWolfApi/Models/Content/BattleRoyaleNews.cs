using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ArcticWolfApi.Models.Content
{
    public class BattleRoyaleNews
    {
        [JsonProperty("messages")]
        public List<PagesMessageBase> Messages { get; set; }

        [JsonProperty("motds")]
        public List<object> MOTDS { get; set; }

        public BattleRoyaleNews(params object[] motds)
        {
            Messages = ((IEnumerable<object>)motds)
                .Select(x =>
                {
                    BattleRoyaleNewsMOTD battleRoyaleNewsMotd = (BattleRoyaleNewsMOTD)x;
                    return new PagesMessage(battleRoyaleNewsMotd.Title, battleRoyaleNewsMotd.Body, battleRoyaleNewsMotd.TileImage).Message;
                })
                .Where(x => x != null).ToList();

            MOTDS = ((IEnumerable<object>)motds).ToList();
        }
    }
}
