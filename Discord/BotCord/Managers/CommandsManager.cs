using BotCord.Commands;
using BotCord.Commands.Bot;
using BotCord.Controllers;
using BotCord.Extensions;
using BotCord.Models;
using BotCord.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Managers
{
    /// <summary>
    /// This class manages the initialization and shutdown of all bot commands.
    /// </summary>
    public static class CommandsManager
    {
        private static readonly string LOG_PREFIX = $"[{ nameof(CommandsManager) }] ";

        /// <summary>
        /// Stores all controllers of this application for managing them.
        /// Register all controllers of this application here!
        /// </summary>
        public static readonly List<CommandInfo> CommandsList = new()
        {
            new CommandInfo(
                command: new HelpCommand(),
                initPriority: 1,
                shutDownPriority: 0,
                chatCommand: "help",
                commandDetails: "Displays a list of all available commands"),

            new CommandInfo(
                command: new ShutDownCommand(),
                initPriority: 0,
                shutDownPriority: 0,
                chatCommand: "shutdown",
                commandDetails: "Shutdowns the bot",
                isStaffOnly: true),
        };

        public static void InitCommands()
        {
            LogController.WriteLine(LOG_PREFIX + "Init Commands...");

            foreach (CommandInfo commandInfo in CommandsList.OrderBy(x => x.InitPriority))
            {
                InitCommand(commandInfo);
            }

            LogController.WriteLine(LOG_PREFIX + "Init Commands done");
        }

        private static void InitCommand(CommandInfo commandInfo)
        {
            string commandName = commandInfo.Command.GetClassName();

            LogController.WriteLine(LOG_PREFIX + $"Initialising { commandName } ...", LogController.LogType.Debug);

            StatusReport controllerStatusResponse = commandInfo.Command.Init();

            if (controllerStatusResponse != StatusReport.OK)
            {
                LogController.WriteLine(LOG_PREFIX + $"{ commandName } returned " + controllerStatusResponse.ToString(), LogController.LogType.Error);
            }
            else
            {
                LogController.WriteLine(LOG_PREFIX + $"{ commandName } returned " + controllerStatusResponse.ToString(), LogController.LogType.Success);
            }
        }

        private static void ShutDownCommand(CommandInfo commandInfo)
        {
            string commandName = commandInfo.Command.GetClassName();

            LogController.WriteLine(LOG_PREFIX + $"Initialising { commandName } ...", LogController.LogType.Debug);

            StatusReport controllerStatusResponse = commandInfo.Command.ShutDown();

            if (controllerStatusResponse != StatusReport.OK)
            {
                LogController.WriteLine(LOG_PREFIX + $"{ commandName } returned " + controllerStatusResponse.ToString(), LogController.LogType.Error);
            }
            else
            {
                LogController.WriteLine(LOG_PREFIX + $"{ commandName } returned " + controllerStatusResponse.ToString(), LogController.LogType.Success);
            }
        }

        public static void ShutDownCommands()
        {
            LogController.WriteLine(LOG_PREFIX + "Shutdown Commands...");

            foreach (CommandInfo commandInfo in CommandsList.OrderBy(x => x.ShutDownPriority))
            {
                ShutDownCommand(commandInfo);
            }

            LogController.WriteLine(LOG_PREFIX + "Shutdown Commands done");
        }
    }
}
