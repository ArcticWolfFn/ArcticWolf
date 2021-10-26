using ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models.Apis.Nitestats
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
