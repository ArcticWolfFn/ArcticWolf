using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNitePlusBot.Models.API.Motd
{
    public class ContentItem
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("contentId")]
        public string ContentId { get; set; }

        [JsonProperty("tcId")]
        public string TcId { get; set; }

        [JsonProperty("contentSchemaName")]
        public string ContentSchemaName { get; set; }

        [JsonProperty("contentSchemaVersion")]
        public int ContentSchemaVersion { get; set; }

        [JsonProperty("contentFields")]
        public ContentFields ContentFields { get; set; }
    }
}
