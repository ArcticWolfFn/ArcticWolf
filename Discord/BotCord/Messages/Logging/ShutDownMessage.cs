using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Messages.Logging
{
    public class ShutDownMessage : LogMessageBase
    {
        public static Embed GetMessage(string cause = "Unknown")
        {
            EmbedBuilder embedBuilder = new()
            {
                Title = "Bot is shutting down...",
                Color = Color.Red
            };
            embedBuilder.AddField("Cause: ", cause);
            embedBuilder.AddField("Occured at: ", DateTime.UtcNow + " (UTC)");

            return embedBuilder.Build();
        }

        public void SendToLogChannel(string cause = "Unknown")
        {
            if (!CanSendToLogChannel())
            {
                return;
            }

            LogMessageChannel.SendMessageAsync(null, false, GetMessage(cause));
        }
    }
}
