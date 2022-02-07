using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArcticWolfApi.Models.Fortnite
{
    public class AthenaLockerSlotsData
    {
        [JsonProperty("slots")]
        public Dictionary<string, AthenaLockerSlot> Slots { get; set; }
    }
}
