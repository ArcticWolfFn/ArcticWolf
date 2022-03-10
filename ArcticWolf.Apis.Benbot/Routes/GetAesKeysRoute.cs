using ArcticWolf.Apis.Base;
using ArcticWolf.Apis.Benbot.Models;

namespace ArcticWolf.Apis.Benbot.Routes
{
    public class GetAesKeysRoute : ApiRouteBase<AesResponse, BenbotApiClient>
    {
        public override bool SupportsPreviousFnVersions => true;
        protected override string Path => "/api/v1/aes";
        protected override string ClassLogPrefix => nameof(GetAesKeysRoute);

        public GetAesKeysRoute(BenbotApiClient apiClient, Base.Common.Http.HttpClient httpClient) : base(apiClient, httpClient)
        {
        }
    }
}
