using System;

namespace Logging
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class LogPrefixAttribute : Attribute
    {
        public string Prefix;

        public LogPrefixAttribute(string name)
        {
            this.Prefix = name;
        }
    }
}
