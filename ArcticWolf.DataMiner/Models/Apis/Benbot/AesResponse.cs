using ArcticWolf.DataMiner.Parser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models.Apis.Benbot
{
    public class AesResponse
    {
        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonIgnore]
        public decimal VersionNumber
        {
            get
            {
                if (_versionNumber != 0)
                {
                    return _versionNumber;
                }

                _versionNumber = VersionParser.GetVersionFromFnVersionString(Version);

                return _versionNumber;
            }
        }
        private decimal _versionNumber = 0;

        [JsonProperty("mainKey")]
        public string MainKey { get; set; }

        [JsonProperty("dynamicKeys")]
        public Dictionary<string, string> DynamicKeys { get; set; }
    }
}
