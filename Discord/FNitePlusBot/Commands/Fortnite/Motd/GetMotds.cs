using AnimatedGif;
using BotCord.Commands;
using BotCord.Controllers;
using BotCord.Extensions;
using BotCord.Models.Enums;
using Discord.WebSocket;
using FNitePlusBot.Models.API.Motd;
using FNitePlusBot.Models.API.Responses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FNitePlusBot.Commands.Fortnite.Motd
{
    public class GetMotds : CommandBase
    {
        public override void Handle(SocketMessage msg, string[] msg_args)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response;
            try
            {
                response = httpClient.GetAsync("http://localhost:8000/api/motds").Result;
            }
            catch (Exception ex)
            {
                LogController.WriteLine("An error occured while getting the motd data: " + ex.Message, LogController.LogType.Error);
                msg.Reply("An error occured while fetching the motd data (Error message: " + ex.Message + ")");
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                msg.Reply("An error occured while fetching the recent motds (Status code: " + response.StatusCode + ")");
            }

            string motdRawData = response.Content.ReadAsStringAsync().Result;
            GetMotdsResponse motdData = GetMotdsResponse.CreateFromJson(motdRawData);

            WebClient webClient = new WebClient();

            AnimatedGifCreator motdsGif = AnimatedGif.AnimatedGif.Create("motds.gif", 1000);

            foreach (ContentItem item in motdData.ContentItems)
            {
                var orderedItems = item.ContentFields.Image.OrderByDescending(x => x.Width);
                if (orderedItems.Count() > 0)
                {
                    Models.API.Shared.Image motdImageInfo = orderedItems.First();

                    byte[] motdImageData = webClient.DownloadData(motdImageInfo.Url);

                    MemoryStream memoryStream = new(motdImageData);

                    Image motdImage = Image.FromStream(memoryStream);
                    motdsGif.AddFrame(motdImage, delay: -1, quality: GifQuality.Bit8);
                }
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
