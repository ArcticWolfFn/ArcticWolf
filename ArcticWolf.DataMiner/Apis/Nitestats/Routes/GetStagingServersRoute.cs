using System.Collections.Generic;
using ArcticWolf.DataMiner.Models.Apis.Nitestats.Staging;

namespace ArcticWolf.DataMiner.Apis.Nitestats.Routes;

public class GetStagingServersRoute : IApiRoute<Dictionary<string, Server>, NitestatsApiClient>
{
    public bool SupportsPreviousFnVersions => false;
    
    public string Path => "v1/epic/staging/fortnite";
    
    public NitestatsApiClient ParentApiClient { get; }
    
    public string ClassLogPrefix => nameof(GetStagingServersRoute);

    public GetStagingServersRoute(NitestatsApiClient apiClient)
    {
        ParentApiClient = apiClient;
    }
}