using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.Apis.NiteStats.Models.Calendar.Channels.States
{
    public class TkSubState
    {
        [JsonProperty("k")]
        public List<string> K { get; set; }
    }
}
