using ArcticWolfApi.Models.Content;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [Route("[controller]/api/pages/fortnite-game")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        public ActionResult<Pages> GetContentPages()
        {
            int seasonNumber = this.Request.GetSeasonNumber();
            Decimal buildVersion = this.Request.GetBuildVersion();
            string str;
            switch (seasonNumber)
            {
                case 10:
                    str = "seasonx";
                    break;
                case 15:
                    str = "season15xmas";
                    break;
                default:
                    str = string.Format("season{0}", (object)seasonNumber);
                    break;
            }
            string stage = str;
            if (buildVersion >= 14.40M && buildVersion < 14.50M)
                stage = "halloween2020";
            string tileImage = "https://cdn.discordapp.com/attachments/864975167490490419/867830756760223774/rift.png";
            if (seasonNumber >= 5)
                tileImage = "https://cdn.discordapp.com/attachments/797250357485895730/797473690324828170/1024-512.png";
            return (ActionResult<Pages>)new Pages()
            {
                BattleRoyaleNews = new BattleRoyaleNewsEntry(new object[1]
              {
          (object) new BattleRoyaleNewsMOTD("Arctic Wolf", "Welcome to Arctic Wolf!", "https://cdn.discordapp.com/attachments/797250357485895730/797250571845107722/background.png", tileImage)
              }),
                EmergencyNotice = new EmergencyNoticeEntry(new PagesMessage[1]
              {
          new PagesMessage("Arctic Wolf", "")
              }),
                SubgameInfo = new SubgameInfoEntry()
                {
                    BattleRoyale = new SubgameInfo("Battle Royale", "100 Player PvP", "battleroyale", "", "000000"),
                    SaveTheWorld = new SubgameInfo("savetheworld", "Cooperative PvE Adventure", "savetheworld", "", "7615E9FF"),
                    Creative = new SubgameInfo("", "Your Islands. Your Friends. Your Rules.", "creative", "", "13BDA1FF")
                },
                SubgameSelect = new SubgameSelectEntry()
                {
                    BattleRoyale = new PagesMessage("100 Player PvP", "100 Player PvP Battle Royale.\n\nPvE Progress does not affect Battle Royale.", ""),
                    SaveTheWorld = new PagesMessage("Co-op PvE", "Cooperative PvE storm-fighting adventure!", ""),
                    Creative = new PagesMessage("NEW - Build Your Own Island!", "Create your own unique games and play with your friends!", "")
                },
                DynamicBackgrounds = new DynamicBackground()
                {
                    Backgrounds = new Background(new BackgroundData[2]
                {
            new BackgroundData(stage, "lobby"),
            new BackgroundData(stage, "vault")
                })
                }
            };
        }
    }
}
