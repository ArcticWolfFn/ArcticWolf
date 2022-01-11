using Newtonsoft.Json;

namespace ArcticWolfApi.Commands.Currency
{
    public class SetMtxPlatform
    {
        [JsonRequired]
        [JsonProperty("newPlatform")]
        public MtxPlatforms NewPlatform { get; set; }
    }
}
