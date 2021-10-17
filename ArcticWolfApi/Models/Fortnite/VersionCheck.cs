using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Fortnite
{
    public class VersionCheck
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        public VersionCheck(string type) => this.Type = type;
    }
}
