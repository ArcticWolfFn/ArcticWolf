using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class Background
    {
        public Background(params BackgroundData[] backgrounds) => this.Backgrounds = ((IEnumerable<BackgroundData>)backgrounds).Select<BackgroundData, BackgroundData>((Func<BackgroundData, BackgroundData>)(x => new BackgroundData(x.Stage, x.Key))).ToList<BackgroundData>();

        [JsonProperty("backgrounds", Order = -7)]
        public List<BackgroundData> Backgrounds { get; set; }

        [JsonProperty("_type")]
        public string Type => "DynamicBackgroundList";
    }
}
