using ArcticWolfApi.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolfApi.Controllers
{
    [ApiController]
    public class MatchmakingController : ControllerBase
    {
        [HttpGet("/fortnite/api/game/v2/matchmakingservice/ticket/player/{playerId}")]
        public ActionResult<TicketResponse> Get(string playerId, [FromQuery(Name = "bucketId")] string bucketId, [FromQuery(Name = "accountId")] string accountId)
        {
            ParsedBckt parsedBckt = new ParsedBckt();

            try
            {
                string[] splitted = bucketId.Split(":");
                parsedBckt.NetCL = splitted[0];
                parsedBckt.HotfixVersion = int.Parse(splitted[1]);
                parsedBckt.Region = splitted[2];
                parsedBckt.Playlist = splitted[3];
            } catch (Exception)
            {
                throw new UnhandledErrorException("Invalid bucketId");
            }

            Response.Cookies.Append("NetCL", parsedBckt.NetCL);

            TicketResponsePayload data = new TicketResponsePayload
            {
                playerId = accountId
            };
            data.partyPlayerIds.Add(accountId);
            data.bucketId = $"FN:Live:{parsedBckt.NetCL}:{parsedBckt.HotfixVersion}:{parsedBckt.Region}:{parsedBckt.Playlist}:PC:public:1";
            data.attributes.Add(new KeyValuePair<string, string>("player.userAgent", Request.Headers["User-Agent"].ToString()));

            foreach (KeyValuePair<string, StringValues> item in Request.Query)
            {
                if (item.Key == "player.subregions" && item.Value.Contains(","))
                {
                    KeyValuePair<string, string> foundItem = data.attributes.Find(x => x.Value == "player.preferredSubregion");
                    data.attributes.Remove(foundItem);
                    data.attributes.Add(new KeyValuePair<string, string>("player.preferredSubregion", item.Value.ToString().Split(",")[0]));
                }

                data.attributes.Add(new KeyValuePair<string, string>(item.Key, item.Value));
            }

            string payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data)));

            TicketResponse response = new()
            {
                payload = ""
            };

            return (ActionResult<TicketResponse>)response;
        }

        [HttpGet("/fortnite/api/matchmaking/session/{sessionId}")]
        public ActionResult<VerifySessionResponse> VerifyMatchmakingSession(string sessionId)
        {
            VerifySessionResponse response = new()
            {
                id = sessionId,
                buildUniqueId = Request.Cookies.First(x => x.Key == "NetCL").Value
            };

            return (ActionResult<VerifySessionResponse>)response;
        }

        [HttpGet("/fortnite/api/game/v2/matchmaking/account/{accountId}/session/{sessionId}")]
        public ActionResult<MatchmakingAccountSessionResponse> GetMatchmakingAccountSession(string accountId, string sessionId)
        {
            MatchmakingAccountSessionResponse response = new();
            response.accountId = accountId;
            response.sessionId = sessionId;

            return response;
        }



        [HttpPost("/fortnite/api/matchmaking/session/{sessionId}/join")]
        public ActionResult JoinMatchmakingSession(string sessionId)
        {
            return this.NoContent();
        }
    }

    public class MatchmakingAccountSessionResponse
    {
        public string accountId = "";
        public string sessionId = "";
        public string key = "none";
    }

    public class TicketResponse
    {
        public string serviceUrl = "ws://matchmaking-fn.herokuapp.com/";
        public string ticketType = "mms-player";
        public string payload;
        public string signature = "";
    }

    public class ParsedBckt
    {
        public string NetCL;
        public string Region;
        public string Playlist;
        public int HotfixVersion = -1;
    }

    public class TicketResponsePayload
    {
        public string playerId;
        public List<string> partyPlayerIds = new List<string>();
        public string bucketId;
        public List<KeyValuePair<string, string>> attributes = new List<KeyValuePair<string, string>>();
        public DateTime expireAt;
        public string nonce;

        public TicketResponsePayload()
        {
            attributes.Add(new KeyValuePair<string, string>("player.preferredSubregion", "None"));
            attributes.Add(new KeyValuePair<string, string>("player.option.spectator", "false"));
            attributes.Add(new KeyValuePair<string, string>("player.inputTypes", ""));
            attributes.Add(new KeyValuePair<string, string>("playlist.revision", "1"));
            attributes.Add(new KeyValuePair<string, string>("player.teamFormat", "fun"));

            expireAt = new DateTime().AddHours(1);
            nonce = randomString(32);
        }

        public string randomString(int length)
        {
            List<char> result = new List<char>();
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            int charactersLength = characters.Length;
            for (int i = 0; i < length; i++)
            {
                result.Add(characters[new Random().Next(0, charactersLength)]);
            }
            return string.Join("", result.ToArray());
        }
    }

    public class VerifySessionResponse
    {
        public string id { get; set; }
        public string ownerId { get; set; } = "ArcticWolf";
        public string ownerName { get; set; } = "ArcticWolf";
        public string serverName { get; set; } = "ArcticWolf";
        public string serverAddress { get; set; } = "127.0.0.1";
        public int serverPort { get; set; } = 7777;
        public int totalPlayers { get; set; } = 0;
        public int maxPublicPlayers { get; set; } = 0;
        public int openPublicPlayers { get; set; } = 0;
        public int maxPrivatePlayers { get; set; } = 0;
        public int openPrivatePlayers { get; set; } = 0;
        public object attributes { get; set; } = new object();
        public List<object> publicPlayers { get; set; } = new List<object>();
        public List<object> privatePlayers { get; set; } = new List<object>();
        public bool allowJoinInProgress { get; set; } = false;
        public bool shouldAdvertise { get; set; } = false;
        public bool isDedicated { get; set; } = false;
        public bool usesStats { get; set; } = false;
        public bool allowInvites { get; set; } = false;
        public bool usesPresence { get; set; } = false;
        public bool allowJoinViaPresence { get; set; } = true;
        public bool allowJoinViaPresenceFriendsOnly { get; set; } = false;
        public string buildUniqueId { get; set; } = "00000000";
        public DateTime lastUpdated { get; set; } = DateTime.UtcNow;
        public bool started { get; set; } = false;
    }
}
