using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public McpItemAttrChanged(string itemId, string attributeName, object attributeValue)
          : base("itemAttrChanged")
        {
            this.ItemId = itemId;
            this.AttributeName = attributeName;
            this.AttributeValue = attributeValue;
        }
    }
}
