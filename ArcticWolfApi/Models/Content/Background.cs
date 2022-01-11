using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace ArcticWolfApi.Models.Content
{
    public class Background
    {
        [JsonProperty("backgrounds", Order = -7)]
        public List<BackgroundData> Backgrounds { get; set; }

        [JsonProperty("_type")]
        public string Type => "DynamicBackgroundList";

        public Background(params BackgroundData[] backgrounds)
        {
            Backgrounds = ((IEnumerable<BackgroundData>)backgrounds).Select(x => new BackgroundData(x.Stage, x.Key)).ToList();
        }
    }
}
