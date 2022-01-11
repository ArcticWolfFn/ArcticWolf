using Newtonsoft.Json;
using System;

namespace ArcticWolfApi.Models.Calendar
{
    public class ChannelEvent
    {
        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("activeUntil")]
        public DateTime ActiveUntil => DateTime.MaxValue;

        [JsonProperty("activeSince")]
        public DateTime ActiveSince => DateTime.UtcNow;

        public ChannelEvent(string eventType) => EventType = eventType;
    }
}
