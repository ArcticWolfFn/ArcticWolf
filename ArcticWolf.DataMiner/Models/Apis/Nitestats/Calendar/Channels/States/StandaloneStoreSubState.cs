using ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels.States.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels.States
{
    public class StandaloneStoreSubState
    {
        [JsonProperty("activePurchaseLimitingEventIds")]
        public List<object> ActivePurchaseLimitingEventIds { get; set; }

        [JsonProperty("storefront")]
        public object Storefront { get; set; }

        [JsonProperty("rmtPromotionConfig")]
        public List<object> RmtPromotionConfig { get; set; }

        [JsonProperty("storeEnd")]
        public DateTime StoreEnd { get; set; }
    }
}
