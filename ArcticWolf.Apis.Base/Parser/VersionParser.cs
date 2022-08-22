using ArcticWolf.Apis.Base.Exceptions;
using System.Globalization;

namespace ArcticWolf.Apis.Base.Parser;

public static class VersionParser
{
    public static decimal GetVersionFromFnVersionString(string version)
    {
        if (version == null) throw new InvalidParameterException($"Parameter '{nameof(version)}' cannot be null");

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