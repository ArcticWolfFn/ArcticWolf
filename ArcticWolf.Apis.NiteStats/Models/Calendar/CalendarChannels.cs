using Newtonsoft.Json;
using ArcticWolf.Apis.NiteStats.Models.Calendar.Channels;
using ArcticWolf.Apis.NiteStats.Models.Calendar.Channels.States;

namespace ArcticWolf.Apis.NiteStats.Models.Calendar
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
