﻿using BotCord.Commands;
using BotCord.Extensions;
using BotCord.Models.Enums;
using Discord.WebSocket;
using FNitePlusBot.Messages.Staging;
using System.Collections.Generic;
using System.Linq;
using ArcticWolf.Apis.NiteStats.Models.Staging;

namespace FNitePlusBot.Commands.Fortnite.Staging
{
    public class GetStagingStats : CommandBase
    {
        public const string LOG_PREFIX = "GetStagingStatsCommand";

        public override void Handle(SocketMessage msg, string[] msg_args)
        {
            Dictionary<string, uint> versionCounts = new();

            foreach (var serverEntry in Cache.StagingServers)
            {
                Server server = serverEntry.Value;
                if (!versionCounts.ContainsKey(server.Version))
                {
                    versionCounts.Add(server.Version, 1);
                }
                else
                {
                    var versionCount = versionCounts.First(x => x.Key == server.Version);
                    versionCounts.Remove(versionCount.Key);
                    versionCounts.Add(versionCount.Key, versionCount.Value + 1);
                }
                
            }

            msg.Reply(null, false, StatsMessage.GetMessage(versionCounts));
        }

        public override StatusReport Init()
        {
            return StatusReport.OK;
        }

        public override StatusReport ShutDown()
        {
            return StatusReport.OK;
        }
    }
}
