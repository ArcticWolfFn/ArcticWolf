using ArcticWolfApi.Models.Profile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Fortnite
{
    public class AthenaLockerSlotActiveVariant
    {
        [JsonProperty("variants")]
        public List<ItemVariant> Variants { get; set; }

        public AthenaLockerSlotActiveVariant(List<ItemVariant> variants) => this.Variants = variants;
    }
}
