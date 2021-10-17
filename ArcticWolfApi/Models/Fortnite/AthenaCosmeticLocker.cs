using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Fortnite
{
    public class AthenaCosmeticLocker
    {
        [JsonProperty("locker_slots_data")]
        public AthenaLockerSlotsData LockerSlotsData { get; set; }

        [JsonProperty("use_count")]
        public int UseCount => 0;

        [JsonProperty("banner_icon_template")]
        public string BannerIcon { get; set; }

        [JsonProperty("banner_color_template")]
        public string BannerColor { get; set; }

        [JsonProperty("locker_name")]
        public string LockerName { get; set; }

        [JsonProperty("item_seen")]
        public bool ItemSeen => false;

        [JsonProperty("favorite")]
        public bool Favorite => false;
    }
}
