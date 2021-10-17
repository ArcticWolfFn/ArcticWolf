using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Commands.Cosmetics
{
    public class SetBattleRoyaleBanner
    {
        [JsonProperty("homebaseBannerColorId")]
        [JsonRequired]
        public string BannerColor { get; set; }

        [JsonProperty("homebaseBannerIconId")]
        [JsonRequired]
        public string BannerIcon { get; set; }
    }
}
