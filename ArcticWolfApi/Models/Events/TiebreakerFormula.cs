using Newtonsoft.Json;
using System.Collections.Generic;

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
