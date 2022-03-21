using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.Apis.NiteStats.Models.Staging
{
    public class ServerModule
    {
        [JsonProperty("cln")]
        public string Cln { get; set; }

        [JsonProperty("build")]
        public string Build { get; set; }

        [JsonProperty("buildDate")]
        public DateTime BuildDate { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }
    }
}
