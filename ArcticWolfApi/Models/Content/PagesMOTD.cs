﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Content
{
    public class PagesMOTD
    {
        [JsonProperty("entryType")]
        public string EntryType { get; set; }

        [JsonProperty("_type")]
        public string Type => "CommonUI Simple Message MOTD";

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("sortingPriority")]
        public int SortingPriority { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        public PagesMOTD(string title, string body, string type = "Text", int sortingPriority = 0, string id = null)
        {
            this.EntryType = type;
            this.Title = title;
            this.Body = body;
            this.SortingPriority = sortingPriority;
            this.Id = id ?? title;
        }
    }
}
