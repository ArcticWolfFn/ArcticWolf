using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Calendar
{
    public class TimelineChannel
    {
        [JsonProperty("states")]
        public List<ChannelState> States { get; set; }

        [JsonProperty("cacheExpire")]
        public DateTime CacheExpire => DateTime.UtcNow.AddMinutes(15.0);

        public TimelineChannel(params ChannelState[] states) => this.States = ((IEnumerable<ChannelState>)states).ToList<ChannelState>();
    }
}
