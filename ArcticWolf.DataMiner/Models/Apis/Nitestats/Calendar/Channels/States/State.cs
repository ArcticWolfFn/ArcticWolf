﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels.States
{
    public class State<SubStateType>
    {
        [JsonProperty("validFrom")]
        public DateTime ValidFrom { get; set; }

        /// <summary>
        ///     Type is unknown yet.
        /// </summary>
        [JsonProperty("activeEvents")]
        public List<object> ActiveEvents { get; set; }

        [JsonProperty("state")]
        public SubStateType SubState { get; set; }
    }
}
