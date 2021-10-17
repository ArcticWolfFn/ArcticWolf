using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class DynamicBackground : BasePagesEntry
    {
        public DynamicBackground()
          : base("dynamicbackgrounds")
        {
        }

        [JsonProperty("backgrounds", Order = -7)]
        public Background Backgrounds { get; set; }
    }
}
