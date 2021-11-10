using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Events
{
    public class Rank
    {
        [JsonProperty("threshold")]
        public double Threshold { get; set; }

        [JsonProperty("payouts")]
        public List<Payout> Payouts { get; set; }
    }
}
