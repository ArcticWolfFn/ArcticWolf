using BotCord.Models;
using BotCord.Models.Enums;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Commands
{
    public class HelpCommand : CommandBase
    {
        public override void Handle(SocketMessage msg, string[] msg_args)
        {
            if (msg_args.Length < 2)
            {
                ShowAllAvailableCommands(msg);
            }
        }

        private static void ShowAllAvailableCommands(SocketMessage msg)
        {
            EmbedBuilder eBuilder = new()
            {
                Color = Color.Blue,
                Author = new EmbedAuthorBuilder().WithName($"{ Controllers.DiscordController.DiscordClient.CurrentUser.Username } Commands")
            };
            string response = "";

            foreach (CommandInfo cmdinfo in Managers.CommandsManager.CommandsList)
            {
                response += "`" + Controllers.ConfigController.Config.BotPrefix + cmdinfo.ChatCommand + "` – " + cmdinfo.CommandDetails;

                if (cmdinfo.IsStaffOnly)
                {
                    response += " (**STAFF ONLY**)";
                }

                response += "\n";
            }

            eBuilder.Description = response;
            msg.Channel.SendMessageAsync("", false, eBuilder.Build());
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
