using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Profile.Changes
{
    public class McpFullProfileUpdate : McpChange
    {
        public McpFullProfileUpdate(Profile profile)
          : base("fullProfileUpdate")
        {
            this.Profile = profile;
        }

        [JsonProperty("profile")]
        public Profile Profile { get; set; }
    }
}
