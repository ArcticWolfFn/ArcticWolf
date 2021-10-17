﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Calendar.States
{
    public class StandaloneStoreState
    {
        [JsonProperty("activePurchaseLimitingEventIds")]
        public string[] ActivePurchaseLimitingEventIds => Array.Empty<string>();

        [JsonProperty("storefront")]
        public object Storefront => new object();

        [JsonProperty("rmtPromotionConfig")]
        public string[] RMTPromotionConfig => Array.Empty<string>();

        [JsonProperty("storeEnd")]
        public DateTime StoreEnd => DateTime.MinValue;
    }
}
