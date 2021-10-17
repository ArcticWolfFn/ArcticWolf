using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Fortnite
{
    public class AthenaLockerSlotsData
    {
        [JsonProperty("slots")]
        public Dictionary<string, AthenaLockerSlot> Slots { get; set; }
    }
}
