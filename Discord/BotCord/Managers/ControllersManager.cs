using BotCord.Controllers;
using BotCord.Extensions;
using BotCord.Messages.Logging;
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
    /// This class manages the initialization and shutdown of all controllers.
    /// </summary>
    public static class ControllerManager
    {
        public static readonly string LOG_PREFIX = $"[{ nameof(ControllerManager) }] ";

        /// <summary>
        /// Stores all controllers of this application for managing them.
        /// Register all controllers of this application here!
        /// </summary>
        public static readonly List<ControllerInfo> ControllersList = new()
        {
            new ControllerInfo(controller: new LogController(), initPriority: 2, shutDownOnInitFail: true, shutDownPriority: 0),
            new ControllerInfo(controller: new ConfigController(), initPriority: 2, shutDownOnInitFail: true, shutDownPriority: 1),
            new ControllerInfo(controller: new DiscordController(), initPriority: 1, shutDownOnInitFail: true, shutDownPriority: 2),
        };

        /// <summary>
        /// Initializes all controllers of the application, which are registered in this function. If a critical error occurs, the application will be closed.
        /// </summary>
        public static void InitControllers()
        {
            LogController.WriteLine(LOG_PREFIX + "Initialising controllers...", LogController.LogType.Info);

            foreach (ControllerInfo cInfoItem in ControllersList.OrderByDescending(x => x.InitPriority))
            {
                InitalizeController(cInfoItem);
            }

            Console.CancelKeyPress += Console_CancelKeyPress;

            LogController.WriteLine(LOG_PREFIX + "The initialization of the controllers has been completed!", LogController.LogType.Info);
        }

        public static void InitalizeController(ControllerInfo controllerInfo, bool isInitByDiscordController = false)
        {
            // If controller requires an active connection to Discord, the controller will automatically 
            // initalized by the DiscordController as it manages the connection to Discord.
            if (controllerInfo.CanOnlyRunIfConnectedToDiscord && !isInitByDiscordController)
            {
                return;
            }

            string controllerName = controllerInfo.Controller.GetClassName();

            LogController.WriteLine(LOG_PREFIX + $"Initialising { controllerName } ...", LogController.LogType.Debug);

            StatusReport controllerStatusResponse = controllerInfo.Controller.Init();

            if (controllerStatusResponse != StatusReport.OK)
            {
                LogController.WriteLine(LOG_PREFIX + $"{ controllerName } returned " + controllerStatusResponse.ToString(), LogController.LogType.Error);

                if (controllerInfo.ShutDownOnInitFail)
                {
                    ShutDownControllers();
                    Environment.Exit(-1);
                }
            }
            else
            {
                LogController.WriteLine(LOG_PREFIX + $"{ controllerName } returned " + controllerStatusResponse.ToString(), LogController.LogType.Success);
            }
        }

        public static void ShutDownController(ControllerInfo controllerInfo)
        {
            string controllerName = controllerInfo.Controller.GetClassName();

            LogController.WriteLine(LOG_PREFIX + $"Shutting down { controllerName }...", LogController.LogType.Debug);

            StatusReport controllerStatusResponse = controllerInfo.Controller.ShutDown();

            LogController.WriteLine(LOG_PREFIX + $"{ controllerName } returned " + controllerStatusResponse.ToString(), LogController.LogType.Debug);
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            ShutDownControllers("Console initiated shut down");
        }

        /// <summary>
        /// Shutdowns all controllers of the application, which are registered in this function.
        /// </summary>
        public static void ShutDownControllers(string cause = "Unknown")
        {
            if (DiscordController.DiscordClient != null)
            {
                new ShutDownMessage().SendToLogChannel(cause);
            }
            else
            {
            }

            LogController.WriteLine(LOG_PREFIX + "Shutting down the controllers...", LogController.LogType.Info);

            foreach (ControllerInfo controllerInfo in ControllersList.OrderByDescending(x => x.ShutDownPriority))
            {
                ShutDownController(controllerInfo);
            }

            Console.WriteLine(LOG_PREFIX + "The controller shutdown has been completed!");
            Environment.Exit(0);
        }

        /// <summary>
        /// Parses the app boot arguments and changes the app configuration.
        /// </summary>
        /// <param name="args">App Boot ChatCommand Line Arguments</param>
        public static void ParseArguments(string[] args)
        {
            foreach (String arg in args)
            {
                switch (arg)
                {
                    case "-debug":
                        Bot.Debug = true;
                        LogController.WriteLine(LOG_PREFIX + "Debug mode activated", LogController.LogType.Debug);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
