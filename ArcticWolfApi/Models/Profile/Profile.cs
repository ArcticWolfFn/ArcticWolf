﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ArcticWolfApi.Models.Profile
{
    public class Profile
    {
        [JsonProperty("_id")]
        public string _Id { get; set; }

        [JsonProperty("created")]
        public DateTime Created => DateTime.Now;

        [JsonProperty("updated")]
        public DateTime Updated => DateTime.Now;

        [JsonProperty("rvn")]
        public int Rvn { get; set; }

        [JsonProperty("wipeNumber")]
        public int WipeNumber => 1;

        [JsonProperty("accountId")]
        public string Id { get; set; }

        [JsonProperty("profileId")]
        public string ProfileId { get; set; }

        [JsonProperty("version")]
        public string Version => "arctic_wolf_dev_v1";

        [JsonProperty("items")]
        public Dictionary<string, object> Items { get; set; }

        [JsonProperty("stats")]
        public ProfileStats Stats { get; set; }
    }
}
