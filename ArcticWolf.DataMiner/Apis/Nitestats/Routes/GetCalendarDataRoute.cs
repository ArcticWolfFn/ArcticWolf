using System.Collections.Generic;
using ArcticWolf.DataMiner.Models.Apis.Nitestats;

namespace ArcticWolf.DataMiner.Apis.Nitestats.Routes;

public class GetCalendarDataRoute : IApiRoute<CalendarResponse, NitestatsApiClient>
{
    public bool SupportsPreviousFnVersions => false;
    
    public string Path => "/v1/epic/modes";
    
    public NitestatsApiClient ParentApiClient { get; }
    
    public string ClassLogPrefix => nameof(GetCalendarDataRoute);

    public GetCalendarDataRoute(NitestatsApiClient apiClient)
    {
        ParentApiClient = apiClient;
    }
}