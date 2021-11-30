using BotCord.Managers;
using BotCord.Messages.Logging;
using BotCord.Models;
using BotCord.Models.Enums;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Controllers
{
    /// Controls the interaction with Discord
    /// </summary>
    public class DiscordController : ControllerBase
    {
        // Logging
        private readonly string LOG_PREFIX = nameof(DiscordController);

        // Discord
        public static DiscordSocketClient DiscordClient;

        /// <summary>
        /// Initializes the controller. Only called by the MainController.
        /// </summary>
        /// <returns>Controller Initialization Status</returns>
        public override StatusReport Init()
        {
            if (string.IsNullOrWhiteSpace(ConfigController.Config.DiscordBotToken))
            {
                Log.Error("The 'DiscordBotToken' in the config file is empty. It is required to start the Discord bot.", LOG_PREFIX);
                return StatusReport.FatalError;
            }

            DiscordClient = new DiscordSocketClient();

            DiscordClient.Log += DSclient_Log;
            DiscordClient.MessageReceived += EventHandlers.MessageReceivedEvent.Handle;
            DiscordClient.Ready += DSclient_Ready;
            DiscordClient.Disconnected += DSclient_Disconnected;

            DiscordClient.LoginAsync(Discord.TokenType.Bot, ConfigController.Config.DiscordBotToken);
            DiscordClient.StartAsync();

            InitDone = true;

            return StatusReport.OK;
        }

        private Task DSclient_Disconnected(Exception arg)
        {
            ShutDownDiscordDependentControllers();

            CommandsManager.ShutDownCommands();

            return Task.CompletedTask;
        }

        private Task DSclient_Ready()
        {
            InitDiscordDependentControllers();

            CommandsManager.InitCommands();

            new StartedMessage().SendToLogChannel();

            return Task.CompletedTask;
        }

        private void InitDiscordDependentControllers()
        {
            Log.Information("Init of discord-dependent controllers...", LOG_PREFIX);

            foreach (ControllerInfo controllerInfo in ControllerManager.ControllersList.Where(x => x.CanOnlyRunIfConnectedToDiscord)
                .OrderByDescending(x => x.InitPriority))
            {
                ControllerManager.InitalizeController(controllerInfo, isInitByDiscordController: true);
            }

            Log.Information("Init of discord-dependent controllers done!", LOG_PREFIX);

        }

        private void ShutDownDiscordDependentControllers()
        {
            Log.Information("Shutdown of discord-dependent controllers...", LOG_PREFIX);

            foreach (ControllerInfo controllerInfo in ControllerManager.ControllersList.Where(x => x.CanOnlyRunIfConnectedToDiscord)
                .OrderByDescending(x => x.ShutDownPriority))
            {
                ControllerManager.ShutDownController(controllerInfo);
            }

            Log.Information("Shutdown of discord-dependent controllers done!", LOG_PREFIX);
        }

        private Task DSclient_Log(Discord.LogMessage arg)
        {
            Log.Information("[Discord] " + arg.Message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Shutdowns the controller. Only called by the MainController.
        /// </summary>
        /// <returns>Controller Shutdown Status</returns>
        public override StatusReport ShutDown()
        {
            if (!InitDone)
            {
                return StatusReport.InitNotDone;
            }

            DiscordClient.StopAsync();
            DiscordClient.LogoutAsync();
            DiscordClient.Dispose();

            return StatusReport.OK;
        }
    }
}
