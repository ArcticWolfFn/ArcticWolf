using ArcticWolfApi.Models.Profile;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArcticWolfApi.Models.Fortnite
{
    public class AthenaLockerSlotActiveVariant
    {
        [JsonProperty("variants")]
        public List<ItemVariant> Variants { get; set; }

        public AthenaLockerSlotActiveVariant(List<ItemVariant> variants) => Variants = variants;
    }
}
