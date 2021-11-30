using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models.Apis.Nitestats.Staging
{
    public class Server
    {
        [JsonProperty("app")]
        public string App { get; set; }

        [JsonProperty("serverDate")]
        public string ServerDate { get; set; }

        [JsonProperty("overridePropertiesVersion")]
        public string OverridePropertiesVersion { get; set; }

        [JsonProperty("cln")]
        public string Cln { get; set; }

        [JsonProperty("build")]
        public string Build { get; set; }

        [JsonProperty("moduleName")]
        public string ModuleName { get; set; }

        [JsonProperty("buildDate")]
        public string BuildDate { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("modules")]
        public Dictionary<string, ServerModule> Modules { get; set; }

        [JsonProperty("nameID")]
        public string NameID { get; set; }
    }
}
