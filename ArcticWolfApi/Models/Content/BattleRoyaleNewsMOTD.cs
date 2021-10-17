using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class BattleRoyaleNewsMOTD : PagesMOTD
    {
        public BattleRoyaleNewsMOTD(
          string title,
          string body,
          string image,
          string tileImage,
          string type = "Text")
          : base(title, body, type)
        {
            this.Image = image;
            this.TileImage = tileImage;
        }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("tileImage")]
        public string TileImage { get; set; }
    }
}
