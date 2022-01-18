using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArcticWolf.DataMiner.Models.Discord
{
    public class Attachment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("fileSizeBytes")]
        public int FileSizeBytes { get; set; }
    }
}
