using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Events
{
    public class TiebreakerFormula
    {
        [JsonProperty("basePointsBits")]
        public int BasePointsBits { get; set; }

        [JsonProperty("components")]
        public List<Component> Components { get; set; }
    }
}
