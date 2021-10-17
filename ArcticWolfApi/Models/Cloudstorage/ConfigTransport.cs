using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Models.Cloudstorage
{
    public class ConfigTransport
    {
        public ConfigTransport(string name, string type, bool isEnabled, int priority)
        {
            this.Name = name;
            this.Type = type;
            this.IsEnabled = isEnabled;
            this.IsRequired = isEnabled;
            this.IsPrimary = isEnabled;
            this.Priority = priority;
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public string AppName => "Fortnite";

        public bool IsEnabled { get; set; }

        public bool IsRequired { get; set; }

        public bool IsPrimary { get; set; }

        public int TimeoutSeconds => 30;

        public int Priority { get; set; }
    }
}
