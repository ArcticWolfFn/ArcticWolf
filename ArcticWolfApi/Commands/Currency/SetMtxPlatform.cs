using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Commands.Currency
{
    public class SetMtxPlatform
    {
        [JsonRequired]
        [JsonProperty("newPlatform")]
        public MtxPlatforms NewPlatform { get; set; }
    }
}
