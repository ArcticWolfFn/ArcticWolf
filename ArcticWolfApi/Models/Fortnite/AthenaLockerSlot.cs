using ArcticWolfApi.Models.Profile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Fortnite
{
    public class AthenaLockerSlot
    {
        [JsonProperty("items")]
        public string[] Items { get; set; }

        [JsonProperty("activeVariants", NullValueHandling = NullValueHandling.Ignore)]
        public List<AthenaLockerSlotActiveVariant> ActiveVariants { get; set; }

        public AthenaLockerSlot(List<ItemVariant> variants = null, params string[] items)
        {
            this.Items = items;
            if (variants == null)
                return;
            this.ActiveVariants = new List<AthenaLockerSlotActiveVariant>()
      {
        new AthenaLockerSlotActiveVariant(variants)
      };
        }
    }
}
