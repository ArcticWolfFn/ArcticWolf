using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Lightswitch
{
    public class LightswitchStatus
    {
        [JsonProperty("banned")]
        public bool Banned;
        [JsonProperty("maintenanceUrl")]
        public string MaintenanceUrl;
        [JsonProperty("overrideCatalogIds")]
        public string[] OverrideCatalogIds = new string[1]
        {
      "a7f138b2e51945ffbfdacc1af0541053"
        };

        [JsonProperty("serviceInstanceId")]
        public string ServiceInstanceId { get; set; }

        [JsonProperty("status")]
        public string Status => "UP";

        [JsonProperty("message")]
        public string Message => "Fortnite is UP";

        [JsonProperty("allowedActions")]
        public string[] AllowedActions => Array.Empty<string>();

        [JsonProperty("launcherInfoDTO")]
        public LauncherInfo LauncherInfoDTO => new LauncherInfo();

        public LightswitchStatus(string serviceId) => this.ServiceInstanceId = serviceId;
    }
}
