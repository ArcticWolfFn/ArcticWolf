using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class BattleRoyaleNewsWebsiteMOTD : BattleRoyaleNewsMOTD
    {
        public BattleRoyaleNewsWebsiteMOTD(
          string title,
          string body,
          string image,
          string tileImage,
          string website,
          string websiteText = null)
          : base(title, body, image, tileImage, "Website")
        {
            this.WebsiteURL = website;
            this.WebsiteButtonText = websiteText;
        }

        [JsonProperty("websiteURL")]
        public string WebsiteURL { get; set; }

        [JsonProperty("websiteButtonText")]
        public string WebsiteButtonText { get; set; }
    }
}
