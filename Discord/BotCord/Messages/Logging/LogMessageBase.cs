using BotCord.Controllers;
using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Messages.Logging
{
    public class LogMessageBase
    {
        protected IMessageChannel LogMessageChannel = null;

        protected bool CanSendToLogChannel()
        {
            if (DiscordController.DiscordClient.ConnectionState != ConnectionState.Connected)
            {
                LogController.WriteLine("[Discord/Logging] Couldn't send log message because the bot is not connected!", LogController.LogType.Error);
                return false;
            }

            LogMessageChannel = DiscordController.DiscordClient.GetChannel(ConfigController.Config.LogChannelID) as IMessageChannel;

            return true;
        }
    }
}
