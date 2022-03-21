using System.Net;
using ArcticWolf.Apis.Base;
using ArcticWolf.Apis.NiteStats.Routes;

namespace ArcticWolf.Apis.NiteStats
{
    public class NiteStatsApiClient : IApiClient
    {
        public string ServerUrl => "https://api.nitestats.com";
        
        public readonly GetStagingServersRoute GetStagingServers;
        public readonly GetCalendarDataRoute GetCalendarData;
        public readonly GetAccessTokenRoute GetAccessToken;

        public NiteStatsApiClient()
        {
            Log.Information("Initialising...");

            GetStagingServers = new GetStagingServersRoute(this);
            GetCalendarData = new GetCalendarDataRoute(this);
            GetAccessToken = new GetAccessTokenRoute(this);
        }
    }
}
