using Newtonsoft.Json;;

namespace ArcticWolfApi.Models.Lightswitch
{
    public class LauncherInfo
    {
        [JsonProperty("appName")]
        public string AppName => "Fortnite";

        [JsonProperty("catalogItemId")]
        public string CatalogItemId => "4fe75bbc5a674f4f9b356b5c90567da5";

        [JsonProperty("namespace")]
        public string Namespace => "fn";
    }
}
