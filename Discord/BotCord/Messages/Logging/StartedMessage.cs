using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Messages.Logging
{
    public class StartedMessage : LogMessageBase
    {
        public static Embed GetMessage()
        {
            EmbedBuilder embedBuilder = new()
            {
                Title = "Bot started successfully!",
                Color = Color.Green,
                ThumbnailUrl = "https://cdn.vcc-online.eu/app/vcc-dc-bot/assets/success-check.png"
            };
            embedBuilder.AddField("Occured at: ", DateTime.UtcNow + " (UTC)");

            return embedBuilder.Build();
        }

        public void SendToLogChannel()
        {
            if (!CanSendToLogChannel())
            {
                return;
            }

            LogMessageChannel.SendMessageAsync(null, false, GetMessage());
        }
    }
}
