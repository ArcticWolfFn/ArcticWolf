using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models.Discord
{
    public class DateRange
    {
        [JsonProperty("after")]
        public object After { get; set; }

        [JsonProperty("before")]
        public object Before { get; set; }
    }
}
