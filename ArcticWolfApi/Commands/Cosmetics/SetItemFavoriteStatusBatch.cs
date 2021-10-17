using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Commands.Cosmetics
{
    public class SetItemFavoriteStatusBatch
    {
        [JsonRequired]
        [JsonProperty("itemIds")]
        public List<string> ItemIds { get; set; }

        [JsonRequired]
        [JsonProperty("itemFavStatus")]
        public List<bool> ItemFavStatus { get; set; }
    }
}
