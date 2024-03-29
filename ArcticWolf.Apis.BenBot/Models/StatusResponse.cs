﻿using ArcticWolf.Apis.Base.Parser;
using Newtonsoft.Json;

namespace ArcticWolf.Apis.BenBot.Models
{
    public class StatusResponse
    {
        /// <summary>
        /// All pak files that exist for the current version
        /// </summary>
        [JsonProperty("allPakFiles")]
        public List<string> AllPakFiles { get; set; }

        /// <summary>
        /// All pak files that are currently mounted
        /// </summary>
        [JsonProperty("mountedPaks")]
        public List<string> MountedPaks { get; set; }

        /// <summary>
        /// The current build version of fortnite
        /// </summary>
        [JsonProperty("currentFortniteVersion")]
        public string CurrentFortniteVersion { get; set; }

        [JsonProperty("currentFortniteVersionNumber")]
        public decimal CurrentFortniteVersionNumber { get; set; }

        [JsonProperty("currentCdnVersion")]
        public string CurrentCdnVersion { get; set; }

        [JsonIgnore] 
        public decimal CurrentCdnVersionNumber => VersionParser.GetVersionFromFnVersionString(CurrentCdnVersion);

        /// <summary>
        /// The amount of pak files that exist in the current version
        /// </summary>
        [JsonProperty("totalPakCount")]
        public int TotalPakCount { get; set; }

        /// <summary>
        /// The count of dynamic paks that exist in the current version
        /// </summary>
        [JsonProperty("dynamicPakCount")]
        public int DynamicPakCount { get; set; }
    }
}
