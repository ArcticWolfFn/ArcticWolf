using BotCord.Extensions;
using BotCord.Models;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.EventHandlers
{
    public static class MessageReceivedEvent
    {
        public static readonly string LogPrefix = nameof(MessageReceivedEvent);
        public static Task Handle(SocketMessage msg)
        {
            Log.Debug("New message (\"" + msg.Content + "\") from " + msg.Author.Username, LogPrefix);
            if (msg.Content.StartsWith(Controllers.ConfigController.Config.BotPrefix))
            {
                HandleCommand(msg);
            }
            return Task.CompletedTask;
        }

        private static void HandleCommand(SocketMessage msg)
        {
            string[] msg_args = msg.Content.Split(" ");
            msg_args[0] = msg_args[0].Replace(Controllers.ConfigController.Config.BotPrefix, "");

            IEnumerable<CommandInfo> foundCommands = Managers.CommandsManager.CommandsList.Where(x => x.ChatCommand == msg_args[0]);

            if (!foundCommands.Any())
            {
                return;
            }

            CommandInfo commandInfo = foundCommands.First();

            if (commandInfo.IsStaffOnly)
            {
                if (!msg.Author.IsStaff())
                {
                    // ToDo: respond with error message?
                    return;
                }
            }

            commandInfo.Command.Handle(msg, msg_args);
        }
    }
}
