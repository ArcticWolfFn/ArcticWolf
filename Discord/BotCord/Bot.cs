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
            Log.Initalize(new System.Collections.Generic.List<LogVisibility> { LogVisibility.Console }, new System.Collections.Generic.Dictionary<string, LogLevel>());
            Log.Information("Booting the bot...");

            ControllerManager.ParseArguments(args);
            ControllerManager.InitControllers();
        }
    }
}
