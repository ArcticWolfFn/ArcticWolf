using ArcticWolfApi.Models.Profile;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Commands.Cosmetics
{
    public class EquipBattleRoyaleCustomization
    {
        [JsonRequired]
        [JsonProperty("slotName")]
        public CosmeticLockerItemCategories SlotName { get; set; }

        [JsonRequired]
        [JsonProperty("itemToSlot")]
        public string ItemToSlot { get; set; }

        [JsonProperty("indexWithinSlot")]
        public int IndexWithinSlot { get; set; }

        [JsonProperty("variantUpdates")]
        public List<ItemVariant> VariantUpdates { get; set; }
    }
}
