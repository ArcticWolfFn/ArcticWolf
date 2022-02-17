using System.Collections.Generic;
using ArcticWolf.DataMiner.Models.Apis.Nitestats.Staging;
using ArcticWolf.Storage;

namespace ArcticWolf.DataMiner.Apis.Nitestats.Routes;

public class GetStagingServersRoute : ApiRouteBase<Dictionary<string, Server>, NitestatsApiClient>
{
    public override bool SupportsPreviousFnVersions => false;
    
    protected override string Path => "v1/epic/staging/fortnite";

    protected override string ClassLogPrefix => nameof(GetStagingServersRoute);

    public GetStagingServersRoute(NitestatsApiClient apiClient) : base(apiClient)
    {
    }
}