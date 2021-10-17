using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Cloudstorage
{
    public class Config
    {
        public Config() => this.Transports = new Dictionary<string, ConfigTransport>()
        {
            ["McpProxyTransport"] = new ConfigTransport("McpProxyTransport", "ProxyStreamingFile", false, 10),
            ["McpSignatoryTransport"] = new ConfigTransport("McpSignatoryTransport", "ProxySignatory", false, 20),
            ["DssDirectTransport"] = new ConfigTransport("DssDirectTransport", "DirectDss", false, 30)
        };

        [JsonProperty("lastUpdated")]
        public DateTime LastUpdated => DateTime.UtcNow;

        [JsonProperty("disableV2")]
        public bool DisableV2 => true;

        [JsonProperty("isAuthenticated")]
        public bool IsAuthenticated => true;

        [JsonProperty("transports")]
        public string EnumerateFilesPath => "/api/cloudstorage/system";

        [JsonProperty("enumerateFilesPath")]
        public Dictionary<string, ConfigTransport> Transports { get; set; }
    }
}
