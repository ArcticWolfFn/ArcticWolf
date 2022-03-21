using System.Collections.Generic;
using ArcticWolf.Apis.Base;
using ArcticWolf.Apis.NiteStats.Models.Staging;

namespace ArcticWolf.Apis.NiteStats.Routes
{
    public class GetStagingServersRoute : ApiRouteBase<Dictionary<string, Server>, NiteStatsApiClient>
    {
        public override bool SupportsPreviousFnVersions => false;
    
        protected override string Path => "/v1/epic/staging/fortnite";

        protected override string ClassLogPrefix => nameof(GetStagingServersRoute);

        public GetStagingServersRoute(NiteStatsApiClient apiClient) : base(apiClient)
        {
        }
    }   
}