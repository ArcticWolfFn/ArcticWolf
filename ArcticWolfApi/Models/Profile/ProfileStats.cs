using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Profile
{
    public class ProfileStats
    {
        [JsonProperty("attributes")]
        public object Attributes { get; set; }

        [JsonProperty("commandRevision")]
        public int CommandRevision { get; set; } = -1;
    }
}
