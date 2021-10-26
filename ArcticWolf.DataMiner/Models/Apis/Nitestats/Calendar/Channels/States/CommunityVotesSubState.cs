using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels.States
{
    public class CommunityVotesSubState
    {
        [JsonProperty("electionId")]
        public string ElectionId { get; set; }

        [JsonProperty("candidates")]
        public List<object> Candidates { get; set; }

        [JsonProperty("electionEnds")]
        public DateTime ElectionEnds { get; set; }

        [JsonProperty("numWinners")]
        public int NumWinners { get; set; }
    }
}
