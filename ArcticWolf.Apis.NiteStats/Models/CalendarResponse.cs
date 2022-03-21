using Newtonsoft.Json;
using ArcticWolf.Apis.NiteStats.Models.Calendar;

namespace ArcticWolf.Apis.NiteStats.Models
{
    public class CalendarResponse
    {
        [JsonProperty("channels")]
        public CalendarChannels Channels { get; set; }

        [JsonProperty("cacheIntervalMins")]
        public double CacheIntervalMins { get; set; }

        [JsonProperty("currentTime")]
        public DateTime CurrentTime { get; set; }
    }
}
