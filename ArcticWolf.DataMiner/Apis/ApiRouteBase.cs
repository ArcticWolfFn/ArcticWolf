using ArcticWolf.DataMiner.Common.Http;
using ArcticWolf.DataMiner.Common.Json;
using ArcticWolf.Storage;

namespace ArcticWolf.DataMiner.Apis
{
    public abstract class ApiRouteBase<TReturnType, TApiClient> where TReturnType : new() where TApiClient : IApiClient
    {
        public abstract bool SupportsPreviousFnVersions { get; }

        protected abstract string Path { get; }

        public TApiClient ParentApiClient { get; }

        protected abstract string ClassLogPrefix { get; }

        public ApiRouteBase(TApiClient apiClient)
        {
            ParentApiClient = apiClient;
        }

        public virtual TReturnType Request(FnSeason? season = null)
        {
            var requestUrl = ParentApiClient.ServerUrl + Path;
            if (SupportsPreviousFnVersions && season != null)
            {
                // ToDo: make this custom
                requestUrl += "?version=" + season?.SeasonNumber;
            }
            
            var response = new HttpClient().Request(requestUrl);

            if (response.Success) return JsonDeserializer.Deserialize<TReturnType>(response.Content);
            
            Log.Error("Request to retrieve data was not successful: " + response.Content, null, ClassLogPrefix);
            
            return new TReturnType();
        }
    }
}
