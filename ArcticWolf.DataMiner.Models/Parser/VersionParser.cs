using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Parser
{
    public static class VersionParser
    {
        public static decimal GetVersionFromFnVersionString(string version)
        {
            string[] parts = version.Split("-");
            string previousPart = "";

            foreach (string part in parts)
            {
                if (previousPart == "++Fortnite+Release")
                {
                    return decimal.Parse(part, NumberStyles.Any, CultureInfo.InvariantCulture);
                }

                previousPart = part;
            }

            return 0;
        } 
    }
}
