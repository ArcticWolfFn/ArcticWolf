using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models.Discord
{
    public class Reaction
    {
        [JsonProperty("emoji")]
        public Emoji Emoji { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
