using Newtonsoft.Json;
using System.Collections.Generic;

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
