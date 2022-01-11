using Newtonsoft.Json;

namespace ArcticWolfApi.Commands.Cosmetics
{
    public class SetCosmeticLockerBanner
    {
        [JsonProperty("lockerItem")]
        [JsonRequired]
        public string LockerItem { get; set; }

        [JsonProperty("bannerColorTemplateName")]
        [JsonRequired]
        public string BannerColorTemplateName { get; set; }

        [JsonProperty("bannerIconTemplateName")]
        [JsonRequired]
        public string BannerIconTemplateName { get; set; }
    }
}
