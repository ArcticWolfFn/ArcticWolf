using ArcticWolf.Apis.Base;
using ArcticWolf.Apis.NiteStats.Models;

namespace ArcticWolf.Apis.NiteStats.Routes
{
    /// <summary>
    /// Generates an epic games client credentials token, not linked to an account. All tokens are cached and regenerated every 10 minutes.
    /// </summary>
    public class GetAccessTokenRoute : ApiRouteBase<AccessTokenResponse, NiteStatsApiClient>
    {
        public override bool SupportsPreviousFnVersions => false;
        protected override string Path => "/v1/epic/bearer";
        protected override string ClassLogPrefix => nameof(GetAccessTokenRoute);

        public GetAccessTokenRoute(NiteStatsApiClient apiClient) : base(apiClient)
        {
        }
    }
}