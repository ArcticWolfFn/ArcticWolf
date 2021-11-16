using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BotCord.Extensions
{
    public static class ObjectExtension
    {
        public static string GetClassName(this object obj)
        {
            return obj.GetType().Name;
        }
    }
}
