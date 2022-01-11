using Newtonsoft.Json;

namespace ArcticWolfApi.Models.Profile.Changes
{
    public class McpItemAttrChanged : McpChange
    {
        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("attributeName")]
        public string AttributeName { get; set; }

        [JsonProperty("attributeValue")]
        public object AttributeValue { get; set; }

        public McpItemAttrChanged(string itemId, string attributeName, object attributeValue) : base("itemAttrChanged")
        {
            ItemId = itemId;
            AttributeName = attributeName;
            AttributeValue = attributeValue;
        }
    }
}
