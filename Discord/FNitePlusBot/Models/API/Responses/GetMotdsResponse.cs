using FNitePlusBot.Models.API.Motd;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNitePlusBot.Models.API.Responses
{
    public class GetMotdsResponse
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("contentId")]
        public string ContentId { get; set; }

        [JsonProperty("tcId")]
        public string TcId { get; set; }

        [JsonProperty("contentItems")]
        public List<ContentItem> ContentItems { get; set; }

        public static GetMotdsResponse CreateFromJson(string jsonData)
        {
            return JsonConvert.DeserializeObject<GetMotdsResponse>(jsonData); 
        }
    }
}
