using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNitePlusBot.Messages.Staging
{
    public class StatsMessage
    {
        public static Embed GetMessage(Dictionary<string, uint> stats)
        {
            EmbedBuilder embedBuilder = new()
            {
                Title = "Server Statistics",
                Color = Color.DarkBlue
            };

            foreach (KeyValuePair<string, uint> entry in stats)
            {
                embedBuilder.AddField(entry.Key, entry.Value, true);
            }

            embedBuilder.WithCurrentTimestamp();

            return embedBuilder.Build();
        }
    }
}
