using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Events
{
    public class Component
    {
        [JsonProperty("trackedStat")]
        public string TrackedStat { get; set; }

        [JsonProperty("bits")]
        public int Bits { get; set; }

        [JsonProperty("multiplier")]
        public double Multiplier { get; set; }

        [JsonProperty("aggregation")]
        public string Aggregation { get; set; }
    }
}
