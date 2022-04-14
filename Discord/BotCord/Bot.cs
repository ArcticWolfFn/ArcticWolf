using BotCord.Controllers;
using BotCord.Managers;
using System;

namespace BotCord
{
    public static class Bot
    {
        public static bool Debug = true;

        public static void Initalize(string[] args)
        {
            Log.Initialize(new System.Collections.Generic.List<LogVisibility> { LogVisibility.Console }, new()
            {
                new() {
                    ClassName = typeof(EventHandlers.MessageReceivedEvent).Name,
                    MinLogLevel = LogLevel.Information
                },
                new()
                {
                    ClassName = typeof(CommandsManager).Name,
                    MinLogLevel = LogLevel.Error
                },
                new()
                {
                    ClassName = typeof(ControllerManager).Name,
                    MinLogLevel = LogLevel.Error
                },
            });

            Log.Information("Booting the bot...");

            ControllerManager.ParseArguments(args);
            ControllerManager.InitControllers();
        }
    }
}
