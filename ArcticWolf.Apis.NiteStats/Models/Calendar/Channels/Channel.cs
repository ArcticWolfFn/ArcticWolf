using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.Apis.NiteStats.Models.Calendar.Channels
{
    public class Channel<StateType>
    {
        [JsonProperty("cacheExpire")]
        public DateTime CacheExpire { get; set; }

        [JsonProperty("states")]
        public List<StateType> States { get; set; }
    }
}
