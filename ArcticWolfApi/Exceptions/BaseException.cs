using Newtonsoft.Json;
using System;

namespace ArcticWolfApi.Exceptions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BaseException : Exception
    {
        public int Status = 400;

        [JsonProperty("errorCode")]
        public string Code => this.GetType().FullName.Replace(".Models", "").ToLower();

        [JsonProperty("errorMessage")]
        public override string Message => base.Message;

        [JsonProperty("messageVars")]
        public string[] Variables { get; private set; }

        [JsonProperty("numericErrorCode")]
        public int NumericCode { get; private set; }

        [JsonProperty("originatingService")]
        public string OriginatingService => "arcticwolf";

        [JsonProperty("intent")]
        public string Intent => "prod";

        public BaseException(int numericCode, string message, params string[] variables)
          : base(string.Format(message, (object[])variables))
        {
            Variables = variables;
            NumericCode = numericCode;
        }
    }
}
