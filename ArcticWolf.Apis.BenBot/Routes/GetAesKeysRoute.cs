using ArcticWolf.Apis.Base;
using ArcticWolf.Apis.BenBot.Models;

namespace ArcticWolf.Apis.BenBot.Routes
{
    public class GetAesKeysRoute : ApiRouteBase<AesResponse, BenBotApiClient>
    {
        public override bool SupportsPreviousFnVersions => true;
        protected override string Path => "/api/v1/aes";
        protected override string ClassLogPrefix => nameof(GetAesKeysRoute);

        public GetAesKeysRoute(BenBotApiClient apiClient, Base.Common.Http.HttpClient httpClient) : base(apiClient, httpClient)
        {
        }
    }
}
