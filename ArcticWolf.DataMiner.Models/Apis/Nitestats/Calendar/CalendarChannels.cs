using ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels;
using ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels.States;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar
{
    public class CalendarChannels
    {
        [JsonProperty("standalone-store")]
        public Channel<State<StandaloneStoreSubState>> StandaloneStore { get; set; }

        [JsonProperty("client-matchmaking")]
        public Channel<State<ClientMatchmakingSubState>> ClientMatchmaking { get; set; }

        [JsonProperty("tk")]
        public Channel<State<TkSubState>> Tk { get; set; }

        [JsonProperty("featured-islands")]
        public Channel<State<FeaturedIslandsSubSate>> FeaturedIslands { get; set; }

        [JsonProperty("community-votes")]
        public Channel<State<CommunityVotesSubState>> CommunityVotes { get; set; }

        [JsonProperty("client-events")]
        public Channel<State<ClientEventsSubState>> ClientEvents { get; set; }
    }
}
