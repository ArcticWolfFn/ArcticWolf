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
        private readonly string LOG_PREFIX = $"[{nameof(DiscordController)}] ";

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
                LogController.WriteLine(LOG_PREFIX + "The 'DiscordBotToken' in the config file is empty. It is required to start the Discord bot.", LogController.LogType.Error);
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
            LogController.WriteLine(LOG_PREFIX + "Init of discord-dependent controllers...");

            foreach (ControllerInfo controllerInfo in ControllerManager.ControllersList.Where(x => x.CanOnlyRunIfConnectedToDiscord)
                .OrderByDescending(x => x.InitPriority))
            {
                ControllerManager.InitalizeController(controllerInfo, isInitByDiscordController: true);
            }

            LogController.WriteLine(LOG_PREFIX + "Init of discord-dependent controllers done!");

        }

        private void ShutDownDiscordDependentControllers()
        {
            LogController.WriteLine(LOG_PREFIX + "Shutdown of discord-dependent controllers...");

            foreach (ControllerInfo controllerInfo in ControllerManager.ControllersList.Where(x => x.CanOnlyRunIfConnectedToDiscord)
                .OrderByDescending(x => x.ShutDownPriority))
            {
                ControllerManager.ShutDownController(controllerInfo);
            }

            LogController.WriteLine(LOG_PREFIX + "Shutdown of discord-dependent controllers done!");
        }

        private Task DSclient_Log(Discord.LogMessage arg)
        {
            LogController.WriteLine("[Discord] " + arg.Message);
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
