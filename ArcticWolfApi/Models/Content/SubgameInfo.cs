using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class SubgameInfo
    {
        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("color")]
        public string Color { get; set; }

        [JsonProperty("specialMessage")]
        public string SpecialMessage { get; set; }

        [JsonProperty("_type")]
        public string Type => "Subgame Info";

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("subgame")]
        public string Subgame { get; set; }

        [JsonProperty("standardMessageLine2")]
        public string StandardMessageLine2 { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("standardMessageLine1")]
        public string StandardMessageLine1 { get; set; }

        public SubgameInfo(string title, string description, string subgame, string image, string color = null)
        {
            Image = image;
            Title = title;
            Description = description;
            Subgame = subgame;
            Color = color ?? "000000";
        }
    }
}
