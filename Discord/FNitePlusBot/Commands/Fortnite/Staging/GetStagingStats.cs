using BotCord.Commands;
using BotCord.Controllers;
using BotCord.Extensions;
using BotCord.Models.Enums;
using Discord.WebSocket;
using FNitePlusBot.Messages.Staging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FNitePlusBot.Commands.Fortnite.Staging
{
    public class GetStagingStats : CommandBase
    {
        public const string LOG_PREFIX = "GetStagingStatsCommand";

        public override void Handle(SocketMessage msg, string[] msg_args)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = new HttpResponseMessage();
            try
            {
                response = client.GetAsync("http://localhost:8000/api/staging/stats").Result;
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while getting the staging data: " + ex.Message, LOG_PREFIX);
                msg.Reply("An error occured while fetching the server statistics (Error message: " + ex.Message + ")");
                return;
            }

            if (response.IsSuccessStatusCode)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var stats = JsonConvert.DeserializeObject<Dictionary<string, int>>(data);
                msg.Reply(null, false, StatsMessage.GetMessage(stats));
            }
            else
            {
                msg.Reply("An error occured while fetching the server statistics (Response code: " + response.StatusCode + ")");
            }
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
