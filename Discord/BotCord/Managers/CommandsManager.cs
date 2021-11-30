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
        public static readonly string LOG_PREFIX = nameof(CommandsManager);

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
            Log.Information("Init Commands...", LOG_PREFIX);

            foreach (CommandInfo commandInfo in CommandsList.OrderBy(x => x.InitPriority))
            {
                InitCommand(commandInfo);
            }

            Log.Information("Init Commands done", LOG_PREFIX);
        }

        private static void InitCommand(CommandInfo commandInfo)
        {
            string commandName = commandInfo.Command.GetClassName();

            Log.Debug($"Initialising { commandName } ...", LOG_PREFIX);

            StatusReport controllerStatusResponse = commandInfo.Command.Init();

            if (controllerStatusResponse != StatusReport.OK)
            {
                Log.Error($"{ commandName } returned " + controllerStatusResponse.ToString(), LOG_PREFIX);
            }
            else
            {
                Log.Information($"{ commandName } returned " + controllerStatusResponse.ToString(), LOG_PREFIX);
            }
        }

        private static void ShutDownCommand(CommandInfo commandInfo)
        {
            string commandName = commandInfo.Command.GetClassName();

            Log.Debug($"Initialising { commandName } ...", LOG_PREFIX);

            StatusReport controllerStatusResponse = commandInfo.Command.ShutDown();

            if (controllerStatusResponse != StatusReport.OK)
            {
                Log.Error($"{ commandName } returned " + controllerStatusResponse.ToString(), LOG_PREFIX);
            }
            else
            {
                Log.Information($"{ commandName } returned " + controllerStatusResponse.ToString(), LOG_PREFIX);
            }
        }

        public static void ShutDownCommands()
        {
            Log.Information("Shutdown Commands...", LOG_PREFIX);

            foreach (CommandInfo commandInfo in CommandsList.OrderBy(x => x.ShutDownPriority))
            {
                ShutDownCommand(commandInfo);
            }

            Log.Information("Shutdown Commands done", LOG_PREFIX);
        }
    }
}
