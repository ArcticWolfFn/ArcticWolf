using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Profile.Changes
{
    public class McpStatModified : McpChange
    {
        [JsonProperty("name")]
        public string Stat { get; set; }

        [JsonProperty("value")]
        public object Value { get; set; }

        public McpStatModified(string stat, object value)
          : base("statModified")
        {
            this.Stat = stat;
            this.Value = value;
        }
    }
}
