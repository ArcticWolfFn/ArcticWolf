using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Profile
{
    public class ProfileItem
    {
        [JsonProperty("templateId")]
        public string TemplateId { get; set; }

        [JsonProperty("attributes")]
        public object Attributes { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        public ProfileItem(string templateId, object attributes = null, int quantity = 1)
        {
            TemplateId = templateId;
            Attributes = attributes ?? new ItemAttributes();
            Quantity = quantity;
        }
    }
}
