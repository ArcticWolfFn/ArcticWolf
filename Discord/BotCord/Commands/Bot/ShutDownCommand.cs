using BotCord.Extensions;
using BotCord.Models.Enums;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Commands.Bot
{
    public class ShutDownCommand : CommandBase
    {
        public override void Handle(SocketMessage msg, string[] msg_args)
        {
            msg.Reply("https://giphy.com/gifs/latenightseth-lol-seth-meyers-lnsm-KctrWMQ7u9D2du0YmD");

            // otherwise the bot won't shutdown and freeze
            Task.Run(() =>
            {
                Managers.ControllerManager.ShutDownControllers("Command called by " + msg.Author.Username);
            });
        }

        public override StatusReport Init()
        {
            return StatusReport.OK;
        }

        public override StatusReport ShutDown()
        {
            return StatusReport.OK;
        }
    }
}
