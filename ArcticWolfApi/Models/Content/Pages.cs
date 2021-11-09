using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class Pages : BasePagesEntry
    {
        [JsonProperty("battleroyalenews", NullValueHandling = NullValueHandling.Ignore)]
        public BattleRoyaleNewsEntry BattleRoyaleNews { get; set; }

        [JsonProperty("battleroyalenewsv2", NullValueHandling = NullValueHandling.Ignore)]
        public BattleRoyaleNewsEntry BattleRoyaleNewsV2 => this.BattleRoyaleNews;

        [JsonProperty("emergencynotice", NullValueHandling = NullValueHandling.Ignore)]
        public EmergencyNoticeEntry EmergencyNotice { get; set; }

        [JsonProperty("emergencynoticev2", NullValueHandling = NullValueHandling.Ignore)]
        public EmergencyNoticeEntry EmergencyNoticeV2 => this.EmergencyNotice;

        [JsonProperty("subgameinfo", NullValueHandling = NullValueHandling.Ignore)]
        public SubgameInfoEntry SubgameInfo { get; set; }

        [JsonProperty("subgameselectdata", NullValueHandling = NullValueHandling.Ignore)]
        public SubgameSelectEntry SubgameSelect { get; set; }

        [JsonProperty("dynamicbackgrounds", NullValueHandling = NullValueHandling.Ignore)]
        public DynamicBackground DynamicBackgrounds { get; set; }

        [JsonProperty("playlistinformation", NullValueHandling = NullValueHandling.Ignore)]
        public PlaylistInformationEntry PlaylistInformation { get; set; }

        [JsonProperty("tournamentinformation", NullValueHandling = NullValueHandling.Ignore)]
        public TournamentInformationEntry TournamentInformation { get; set; }

        public Pages()
          : base("Fortnite Game")
        {
        }
    }
}
