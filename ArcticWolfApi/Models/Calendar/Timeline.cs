using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Calendar
{
    public class Timeline
    {
        [JsonProperty("channels")]
        public Dictionary<string, TimelineChannel> Channels { get; set; }

        [JsonProperty("currentTime")]
        public DateTime CurrentTime => DateTime.UtcNow;

        [JsonProperty("cacheIntervalMins")]
        public double CacheIntervalMinutes => 15.0;
    }
}
