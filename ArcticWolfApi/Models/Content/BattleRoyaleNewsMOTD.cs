using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Content
{
    public class BattleRoyaleNewsMOTD : PagesMOTD
    {
        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("tileImage")]
        public string TileImage { get; set; }

        public BattleRoyaleNewsMOTD(string title, string body, string image, string tileImage, string type = "Text") : base(title, body, type)
        {
            Image = image;
            TileImage = tileImage;
        }
    }
}
