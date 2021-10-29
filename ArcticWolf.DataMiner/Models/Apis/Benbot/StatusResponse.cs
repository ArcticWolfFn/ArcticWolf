using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models.Apis.Benbot
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
        public double CurrentFortniteVersionNumber { get; set; }

        [JsonProperty("currentCdnVersion")]
        public string CurrentCdnVersion { get; set; }

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
