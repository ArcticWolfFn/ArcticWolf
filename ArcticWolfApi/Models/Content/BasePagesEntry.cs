using Newtonsoft.Json;
using System;

namespace ArcticWolfApi.Models.Content
{
    public class BasePagesEntry
    {
        [JsonProperty("_title", Order = -8)]
        public string Title { get; set; }

        [JsonProperty("_activeDate", Order = -5)]
        public DateTime ActiveDate => DateTime.UtcNow.AddMonths(-12);

        [JsonProperty("lastModified", Order = -4)]
        public DateTime LastModified { get; set; }

        [JsonProperty("_locale", Order = -3)]
        public string Locale => "en-US";

        public BasePagesEntry(string title, DateTime? lastModified = null)
        {
            Title = title;
            LastModified = lastModified ?? DateTime.UtcNow;
        }
    }
}
