using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Content
{
    public class PagesMessageBase
    {
        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("hidden")]
        public bool IsHidden => false;

        [JsonProperty("_type")]
        public string Type => "CommonUI Simple Message Base";

        [JsonProperty("messagetype")]
        public string MessageType => "normal";

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("adspace", NullValueHandling = NullValueHandling.Ignore)]
        public string Adspace { get; set; }

        [JsonProperty("spotlight")]
        public bool IsSpotlight => false;

        public PagesMessageBase(string title, string body, string image = null, string adspace = null)
        {
            Title = title;
            Body = body;
            Image = image;
            Adspace = adspace;
        }
    }
}
