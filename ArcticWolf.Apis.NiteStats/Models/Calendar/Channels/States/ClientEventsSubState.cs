using ArcticWolf.Apis.NiteStats.Models.Calendar.Channels.States.Models;
using Newtonsoft.Json;

namespace ArcticWolf.Apis.NiteStats.Models.Calendar.Channels.States
{
    public class ClientEventsSubState
    {
        [JsonProperty("activeStorefronts")]
        public List<object> ActiveStorefronts { get; set; }

        [JsonProperty("eventNamedWeights")]
        public EventNamedWeights EventNamedWeights { get; set; }

        [JsonProperty("activeEvents")]
        public List<EventDetailed> ActiveEvents { get; set; }

        [JsonProperty("seasonNumber")]
        public int SeasonNumber { get; set; }

        [JsonProperty("seasonTemplateId")]
        public string SeasonTemplateId { get; set; }

        [JsonProperty("matchXpBonusPoints")]
        public int MatchXpBonusPoints { get; set; }

        [JsonProperty("eventPunchCardTemplateId")]
        public string EventPunchCardTemplateId { get; set; }

        [JsonProperty("seasonBegin")]
        public DateTime SeasonBegin { get; set; }

        [JsonProperty("seasonEnd")]
        public DateTime SeasonEnd { get; set; }

        [JsonProperty("seasonDisplayedEnd")]
        public DateTime SeasonDisplayedEnd { get; set; }

        [JsonProperty("weeklyStoreEnd")]
        public DateTime WeeklyStoreEnd { get; set; }

        [JsonProperty("stwEventStoreEnd")]
        public DateTime StwEventStoreEnd { get; set; }

        [JsonProperty("stwWeeklyStoreEnd")]
        public DateTime StwWeeklyStoreEnd { get; set; }

        [JsonProperty("sectionStoreEnds")]
        public Dictionary<string, DateTime> SectionStoreEnds { get; set; }

        [JsonProperty("rmtPromotion")]
        public string RmtPromotion { get; set; }

        [JsonProperty("dailyStoreEnd")]
        public DateTime DailyStoreEnd { get; set; }
    }
}
