using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class PagesMessage
    {
        [JsonProperty("_type")]
        public string Type => "CommonUI Simple Message";

        [JsonProperty("message")]
        public PagesMessageBase Message { get; set; }

        public PagesMessage(string title, string body, string image = null, string adspace = null) => this.Message = new PagesMessageBase(title, body, image, adspace);
    }
}
