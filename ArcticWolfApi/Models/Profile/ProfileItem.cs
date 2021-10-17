using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            this.TemplateId = templateId;
            this.Attributes = attributes ?? (object)new ItemAttributes();
            this.Quantity = quantity;
        }
    }
}
