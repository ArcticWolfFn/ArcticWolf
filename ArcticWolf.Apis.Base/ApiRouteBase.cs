using ArcticWolf.Apis.Base.Common.Json;

namespace ArcticWolf.Apis.Base
{
    public abstract class ApiRouteBase<TReturnType, TApiClient> where TReturnType : new() where TApiClient : IApiClient
    {
        public abstract bool SupportsPreviousFnVersions { get; }

        protected abstract string Path { get; }

        public TApiClient ParentApiClient { get; }

        private readonly Common.Http.HttpClient _httpClient;

        protected abstract string ClassLogPrefix { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiClient"></param>
        /// <param name="httpClient">If not null, it will be used for all requests.</param>
        protected ApiRouteBase(TApiClient apiClient, Common.Http.HttpClient httpClient = null)
        {
            ParentApiClient = apiClient;
            _httpClient = httpClient;
        }

        public virtual TReturnType? Request(decimal version = 0)
        {
            var requestUrl = ParentApiClient.ServerUrl + Path;
            if (SupportsPreviousFnVersions && version != 0)
            {
                // ToDo: make this custom
                requestUrl += $"?version={version:F}";
            }

            var response = _httpClient?.Request(requestUrl) ?? new Common.Http.HttpClient().Request(requestUrl);

            if (response.Success) return JsonDeserializer.Deserialize<TReturnType>(response.Content);

            Log.Error("Request to retrieve data was not successful: " + response.Content, null, ClassLogPrefix);

            return new TReturnType();
        }
    }
}
