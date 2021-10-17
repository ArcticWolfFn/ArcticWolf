using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Storefront
{
    public class Catalog
    {
        public Catalog() => this.Storefronts = new List<Storefront>()
    {
      new Storefront("BRDailyStorefront"),
      new Storefront("BRWeeklyStorefront")
    };

        [JsonProperty("refreshIntervalHrs")]
        public int RefreshIntervalHrs => 24;

        [JsonProperty("dailyPurchaseHrs")]
        public int DailyPurchaseHrs => 24;

        [JsonProperty("expiration")]
        public DateTime Expiration => DateTime.MaxValue;

        [JsonProperty("storefronts")]
        public List<Storefront> Storefronts { get; set; }
    }
}
