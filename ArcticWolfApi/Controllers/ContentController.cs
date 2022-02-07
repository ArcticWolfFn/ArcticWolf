using ArcticWolfApi.Models.Content;
using Microsoft.AspNetCore.Mvc;

namespace ArcticWolfApi.Controllers
{
    [Route("[controller]/api")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        [HttpGet("pages/fortnite-game")]
        public ActionResult<Pages> GetContentPages()
        {
            int seasonNumber = Request.GetSeasonNumber();
            decimal buildVersion = Request.GetBuildVersion();

            string str = seasonNumber switch
            {
                10 => "seasonx",
                /*case 15: // not working on 15.00 but on 15.30
                  str = "season15xmas";
                 break;*/
                _ => $"season{seasonNumber}",
            };
            string stage = str;
            if (buildVersion >= 14.40M && buildVersion < 14.50M)
            {
                stage = "halloween2020";
            }

            string tileImage = "https://cdn.discordapp.com/attachments/797250357485895730/797473690324828170/1024-512.png";

            return new Pages()
            {
                BattleRoyaleNews = new BattleRoyaleNewsEntry(new object[1]
                {
                    new BattleRoyaleNewsMOTD("Arctic Wolf", "Welcome to Arctic Wolf!", "https://cdn.discordapp.com/attachments/797250357485895730/797250571845107722/background.png", tileImage)
                }),

                EmergencyNotice = new EmergencyNoticeEntry(new PagesMessage[1]
                {
                    new("Arctic Wolf", "")
                }),

                SubgameInfo = new SubgameInfoEntry()
                {
                    BattleRoyale = new("Battle Royale", "100 Player PvP", "battleroyale", "", "000000"),
                    SaveTheWorld = new("savetheworld", "Cooperative PvE Adventure", "savetheworld", "", "7615E9FF"),
                    Creative = new("", "Your Islands. Your Friends. Your Rules.", "creative", "", "13BDA1FF")
                },

                SubgameSelect = new SubgameSelectEntry()
                {
                    BattleRoyale = new("100 Player PvP", "100 Player PvP Battle Royale.\n\nPvE Progress does not affect Battle Royale.", ""),
                    SaveTheWorld = new("Co-op PvE", "Cooperative PvE storm-fighting adventure!", ""),
                    Creative = new("NEW - Build Your Own Island!", "Create your own unique games and play with your friends!", "")
                },

                DynamicBackgrounds = new DynamicBackground()
                {
                    Backgrounds = new Background(
                        new BackgroundData[2]
                        {
                            new(stage, "lobby"),
                            new(stage, "vault")
                        })
                },

                TournamentInformation = new TournamentInformationEntry()
                {
                    tournament_info = new()
                    {
                        tournaments = new()
                        {
                            new()
                            {
                                BackgroundLeftColor = "0126B7",
                                BackgroundRightColor = "000410",
                                BackgroundTextColor = "000F4A",
                                BaseColor = "FFFFFF",
                                DetailsDescription = "Jump in for a chance to earn a new Outfit and for the top scoring players, a PlayStation 5.For more details and official rules, visit: https://fn.gg/gencup",
                                FlavorDescription = "Jump in for a chance to earn a new Outfit and for the top scoring players, a PlayStation 5!",
                                HighlightColor = "6DFDFF",
                                LoadingScreenImage = "https://cdn2.unrealengine.com/14br-comp-in-game-psgenerationscup-modetile-1024x512-42764868d8a9.jpg",
                                LongFormatTitle = "Fortnite Generations Cup - only on - PS5 | PS4",
                                PinEarnedText = "Winner!",
                                PlaylistTileImage = "https://cdn2.unrealengine.com/14br-comp-in-game-psgenerationscup-modetile-1024x512-42764868d8a9.jpg",
                                PosterBackImage = "https://cdn2.unrealengine.com/14br-comp-in-game-psgenerationscup-poster-back-750x1080-5057166cf6fd.jpg",
                                PosterFadeColor = "000D3C",
                                PosterFrontImage = "https://cdn2.unrealengine.com/14br-comp-in-game-psgenerationscup-poster-front-750x1080-8ceecc97aa29.jpg",
                                PrimaryColor = "FFFFFF",
                                ScheduleInfo = "December 18th",
                                SecondaryColor = "000F4A",
                                ShadowColor = "000F4A",
                                ShortFormatTitle = "Solo Tournament",
                                TitleColor = "FFFFFF",
                                TitleLine1 = "Generations Cup",
                                TitleLine2 = "Only On - PS5 | PS4",
                                TournamentDisplayId = "s15_fncs_week2",
                                Type = "Tournament Display Info",
                            }
                        }
                    }
                }
            };
        }
    }
}
