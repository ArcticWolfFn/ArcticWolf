using ArcticWolfApi.Models.Profile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Commands.Cosmetics
{
    public class SetCosmeticLockerSlot
    {
        [JsonRequired]
        [JsonProperty("lockerItem")]
        public string LockerItem { get; set; }

        [JsonRequired]
        [JsonProperty("category")]
        public CosmeticLockerItemCategories Category { get; set; }

        [JsonProperty("itemToSlot")]
        public string ItemToSlot { get; set; }

        [JsonProperty("slotIndex")]
        public int SlotIndex { get; set; }

        [JsonProperty("variantUpdates")]
        public List<ItemVariant> VariantUpdates { get; set; }
    }
}
