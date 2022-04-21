using System;

namespace Logging
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public abstract class LogPrefixAttribute : Attribute
    {
        public readonly string Prefix;

        protected LogPrefixAttribute(string name)
        {
            Prefix = name;
        }
    }
}
