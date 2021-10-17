using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Calendar
{
    public class ChannelState
    {
        [JsonProperty("validFrom")]
        public DateTime ValidFrom { get; set; } = DateTime.UtcNow;

        [JsonProperty("activeEvents")]
        public List<ChannelEvent> ActiveEvents { get; set; }

        [JsonProperty("state")]
        public object State { get; set; }
    }
}
