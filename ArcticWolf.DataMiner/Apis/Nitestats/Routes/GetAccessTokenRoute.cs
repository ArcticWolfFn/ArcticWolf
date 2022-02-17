using ArcticWolf.DataMiner.Models.Apis.Nitestats;

namespace ArcticWolf.DataMiner.Apis.Nitestats.Routes;

/// <summary>
/// Generates an epic games client credentials token, not linked to an account. All tokens are cached and regenerated every 10 minutes.
/// </summary>
public class GetAccessTokenRoute : ApiRouteBase<AccessTokenResponse, NitestatsApiClient>
{
    public override bool SupportsPreviousFnVersions => false;
    protected override string Path => "/v1/epic/bearer";
    protected override string ClassLogPrefix => nameof(GetAccessTokenRoute);

    public GetAccessTokenRoute(NitestatsApiClient apiClient) : base(apiClient)
    {
    }
}