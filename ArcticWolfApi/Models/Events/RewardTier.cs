
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Events
{
    public class RewardTier
    {
        [JsonProperty("keyValue")]
        public int KeyValue { get; set; }

        [JsonProperty("pointsEarned")]
        public int PointsEarned { get; set; }

        [JsonProperty("multiplicative")]
        public bool Multiplicative { get; set; }
    }
}
