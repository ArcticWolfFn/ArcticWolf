using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Profile.Changes
{
    public class McpChange
    {
        [JsonProperty("changeType", Order = -2)]
        public string ChangeType { get; set; }

        public McpChange(string changeType) => this.ChangeType = changeType;
    }
}
