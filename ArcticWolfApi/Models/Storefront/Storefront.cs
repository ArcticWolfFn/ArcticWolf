using Newtonsoft.Json;
using System;

namespace ArcticWolfApi.Models.Storefront
{
    public class Storefront
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("catalogEntries")]
        public object[] CatalogEntries => Array.Empty<object>();

        public Storefront(string name) => Name = name;
    }
}
