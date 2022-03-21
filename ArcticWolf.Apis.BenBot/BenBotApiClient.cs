using ArcticWolf.Apis.Base;
using System.Net;
using ArcticWolf.Apis.BenBot.Routes;

namespace ArcticWolf.Apis.BenBot
{
    public class BenBotApiClient : IApiClient
    {
        public string ServerUrl => "https://benbot.app";

        private readonly Base.Common.Http.HttpClient _client;
        private static readonly WebHeaderCollection defaultHeaders = new()
        {
            { HttpRequestHeader.ContentType, "application/json" }
        };

        // Routes
        public readonly GetAesKeysRoute GetAesKeys;
        public readonly GetStatusRoute GetStatus;

        public BenBotApiClient()
        {
            Log.Information("Initialising...");

            _client = new(defaultHeaders);

            GetAesKeys = new(this, _client);
            GetStatus = new(this, _client);
        }
    }
}
