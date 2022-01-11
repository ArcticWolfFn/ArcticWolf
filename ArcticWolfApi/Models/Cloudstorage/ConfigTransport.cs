namespace ArcticWolfApi.Models.Cloudstorage
{
    public class ConfigTransport
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public string AppName => "Fortnite";

        public bool IsEnabled { get; set; }

        public bool IsRequired { get; set; }

        public bool IsPrimary { get; set; }

        public int TimeoutSeconds => 30;

        public int Priority { get; set; }

        public ConfigTransport(string name, string type, bool isEnabled, int priority)
        {
            Name = name;
            Type = type;
            IsEnabled = isEnabled;
            IsRequired = isEnabled;
            IsPrimary = isEnabled;
            Priority = priority;
        }
    }
}
