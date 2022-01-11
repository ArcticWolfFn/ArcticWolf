using Newtonsoft.Json;

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
