using AnimatedGif;
using BotCord.Commands;
using BotCord.Extensions;
using BotCord.Models.Enums;
using Discord.WebSocket;
using FNitePlusBot.Models.API.Responses;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
namespace FNitePlusBot.Commands.Fortnite.Motd
{
    public class GetMotds : CommandBase
    {
        public const string LOG_PREFIX = "GetMotdsCommand";

        public override void Handle(SocketMessage msg, string[] msgArgs)
        {
            var httpClient = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = httpClient.GetAsync("http://localhost:8000/api/motds").Result;
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while getting the motd data: " + ex.Message, LOG_PREFIX);
                msg.Reply("An error occured while fetching the motd data (Error message: " + ex.Message + ")");
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                msg.Reply("An error occured while fetching the recent motds (Status code: " + response.StatusCode + ")");
            }

            var motdRawData = response.Content.ReadAsStringAsync().Result;
            var motdData = GetMotdsResponse.CreateFromJson(motdRawData);

            var webClient = new WebClient();

            var motdsGif = AnimatedGif.AnimatedGif.Create("motds.gif", 1000);

            foreach (var item in motdData.ContentItems)
            {
                var orderedItems = item.ContentFields.Image.OrderByDescending(x => x.Width);
                if (!orderedItems.Any()) continue;
                var motdImageInfo = orderedItems.First();

                var motdImageData = webClient.DownloadData(motdImageInfo.Url);

                MemoryStream memoryStream = new(motdImageData);

                var motdImage = Image.FromStream(memoryStream);
                motdsGif.AddFrame(motdImage, delay: -1, quality: GifQuality.Bit8);
            }

            motdsGif.Dispose();

            // wait before delete
            _ = msg.ReplyWithFile("motds.gif").Result;

            // cleanup
            File.Delete("motd.gif");
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
