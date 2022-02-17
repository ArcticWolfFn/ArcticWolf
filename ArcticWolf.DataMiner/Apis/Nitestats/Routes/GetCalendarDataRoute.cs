using ArcticWolf.DataMiner.Models.Apis.Nitestats;

namespace ArcticWolf.DataMiner.Apis.Nitestats.Routes;

public class GetCalendarDataRoute : ApiRouteBase<CalendarResponse, NitestatsApiClient>
{
    public override bool SupportsPreviousFnVersions => false;
    
    protected override string Path => "/v1/epic/modes";

    protected override string ClassLogPrefix => nameof(GetCalendarDataRoute);

    public GetCalendarDataRoute(NitestatsApiClient apiClient) : base(apiClient)
    {
    }
}