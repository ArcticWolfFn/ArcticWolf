using ArcticWolfApi.Models.Profile;
using Newtonsoft.Json;
using System.Collections.Generic;

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
            Items = items;
            if (variants == null)
            {
                return;
            }
            ActiveVariants = new List<AthenaLockerSlotActiveVariant>()
            {
                new AthenaLockerSlotActiveVariant(variants)
            };
        }
    }
}
