using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Content
{
    public class BattleRoyaleNewsWebsiteMOTD : BattleRoyaleNewsMOTD
    {
        [JsonProperty("websiteURL")]
        public string WebsiteURL { get; set; }

        [JsonProperty("websiteButtonText")]
        public string WebsiteButtonText { get; set; }

        public BattleRoyaleNewsWebsiteMOTD(string title, string body, string image, string tileImage, string website, string websiteText = null)
          : base(title, body, image, tileImage, "Website")
        {
            WebsiteURL = website;
            WebsiteButtonText = websiteText;
        }
    }
}
