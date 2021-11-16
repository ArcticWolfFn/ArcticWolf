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
            LogController.WriteLine("Booting the bot...");

            ControllerManager.ParseArguments(args);
            ControllerManager.InitControllers();
        }
    }
}
