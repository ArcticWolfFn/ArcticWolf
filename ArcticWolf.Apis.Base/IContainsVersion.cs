using ArcticWolf.Apis.Base.Parser;
using Newtonsoft.Json;

namespace ArcticWolf.Apis.Base;

public abstract class IContainsVersion
{
    public abstract string Version { get; set; }

    [JsonIgnore]
    public decimal VersionNumber => VersionParser.GetVersionFromFnVersionString(Version);
}