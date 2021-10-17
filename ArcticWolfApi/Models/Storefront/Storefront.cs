using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Storefront
{
    public class Storefront
    {
        public Storefront(string name) => this.Name = name;

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("catalogEntries")]
        public object[] CatalogEntries => Array.Empty<object>();
    }
}
