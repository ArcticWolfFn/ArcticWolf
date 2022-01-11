using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ArcticWolfApi.Models.Storefront
{
    public class Catalog
    {
        [JsonProperty("refreshIntervalHrs")]
        public int RefreshIntervalHrs => 24;

        [JsonProperty("dailyPurchaseHrs")]
        public int DailyPurchaseHrs => 24;

        [JsonProperty("expiration")]
        public DateTime Expiration => DateTime.MaxValue;

        [JsonProperty("storefronts")]
        public List<Storefront> Storefronts { get; set; }

        public Catalog() => Storefronts = new List<Storefront>()
        {
            new("BRDailyStorefront"),
            new("BRWeeklyStorefront")
        };
    }
}
