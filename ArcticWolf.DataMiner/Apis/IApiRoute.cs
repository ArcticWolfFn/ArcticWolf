using ArcticWolf.DataMiner.Common.Http;
using ArcticWolf.DataMiner.Common.Json;
using ArcticWolf.Storage;

namespace ArcticWolf.DataMiner.Apis
{
    public interface IApiRoute<out TReturnType, out TApiClient> where TReturnType : new() where TApiClient : IApiClient
    {
        public bool SupportsPreviousFnVersions { get; }

        public string Path { get; }

        public TApiClient ParentApiClient { get; }

        public string ClassLogPrefix { get; }

        public virtual TReturnType Request(FnSeason season)
        {
            var requestUrl = ParentApiClient.ServerUrl + Path;
            if (SupportsPreviousFnVersions)
            {
                // ToDo: make this custom
                requestUrl += "?version=" + season.SeasonNumber;
            }
            
            var response = new HttpClient().Request(requestUrl);

            if (response.Success) return JsonDeserializer.Deserialize<TReturnType>(response.Content);
            
            Log.Error("Request to retrieve data was not successful: " + response.Content, null, ClassLogPrefix);
            
            return new TReturnType();
        }
    }
}
