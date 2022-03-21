using ArcticWolf.Apis.Base;
using ArcticWolf.Apis.NiteStats.Models;

namespace ArcticWolf.Apis.NiteStats.Routes
{
    public class GetCalendarDataRoute : ApiRouteBase<CalendarResponse, NiteStatsApiClient>
    {
        public override bool SupportsPreviousFnVersions => false;
    
        protected override string Path => "/v1/epic/modes";

        protected override string ClassLogPrefix => nameof(GetCalendarDataRoute);

        public GetCalendarDataRoute(NiteStatsApiClient apiClient) : base(apiClient)
        {
        }
    }   
}