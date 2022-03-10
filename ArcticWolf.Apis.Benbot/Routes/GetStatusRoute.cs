using ArcticWolf.Apis.Base;
using ArcticWolf.Apis.Benbot.Models;

namespace ArcticWolf.Apis.Benbot.Routes
{
    public class GetStatusRoute : ApiRouteBase<StatusResponse, BenbotApiClient>
    {
        public override bool SupportsPreviousFnVersions => false;

        protected override string Path => "/api/v1/status";

        protected override string ClassLogPrefix => nameof(GetStatusRoute);

        public GetStatusRoute(BenbotApiClient apiClient, Base.Common.Http.HttpClient httpClient) : base(apiClient, httpClient)
        {
        }
    }
}
