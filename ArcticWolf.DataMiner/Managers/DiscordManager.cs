﻿using BotCord;
using BotCord.Managers;
using BotCord.Models;
using System;

namespace ArcticWolf.DataMiner.Managers
{
    public static class DiscordManager
    {
        public static void Init()
        {
            CommandsManager.CommandsList.Add(
                new CommandInfo(
                command: new FNitePlusBot.Commands.Fortnite.Staging.GetStagingStats(),
                initPriority: 0,
                shutDownPriority: 0,
                chatCommand: "getserverstats",
                commandDetails: "Returns the latest server statistics"
                ));

            CommandsManager.CommandsList.Add(
                new CommandInfo(
                command: new FNitePlusBot.Commands.Fortnite.Motd.GetMotds(),
                initPriority: 0,
                shutDownPriority: 0,
                chatCommand: "getmotds",
                commandDetails: "Returns the latest Fortnite news"
                ));

            Bot.Initalize(Array.Empty<string>());
        }
    }
}
