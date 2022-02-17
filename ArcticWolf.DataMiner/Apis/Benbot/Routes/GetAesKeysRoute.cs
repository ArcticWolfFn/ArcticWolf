using ArcticWolf.DataMiner.Common.Http;
using ArcticWolf.DataMiner.Common.Json;
using ArcticWolf.DataMiner.Models.Apis.Benbot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Apis.Benbot.Routes
{
    public class GetAesKeysRoute : ApiRouteBase<AesResponse, BenbotApiClient>
    {
        public override bool SupportsPreviousFnVersions => true;
        protected override string Path => "/api/v1/aes";
        protected override string ClassLogPrefix => nameof(GetAesKeysRoute);

        public GetAesKeysRoute(BenbotApiClient apiClient, HttpClient httpClient) : base(apiClient, httpClient)
        {
        }
    }
}
