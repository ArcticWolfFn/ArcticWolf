using ArcticWolf.Apis.Base;
using ArcticWolf.Apis.BenBot.Models;

namespace ArcticWolf.Apis.BenBot.Routes
{
    public class GetStatusRoute : ApiRouteBase<StatusResponse, BenBotApiClient>
    {
        public override bool SupportsPreviousFnVersions => false;

        protected override string Path => "/api/v1/status";

        protected override string ClassLogPrefix => nameof(GetStatusRoute);

        public GetStatusRoute(BenBotApiClient apiClient, Base.Common.Http.HttpClient httpClient) : base(apiClient, httpClient)
        {
        }
    }
}
