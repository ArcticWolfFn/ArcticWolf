using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Events
{
    public class RegionMappings
    {
        [JsonProperty("EU")]
        public string EU { get; set; }
    }
}
