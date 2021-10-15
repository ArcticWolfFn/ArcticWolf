using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi.Exceptions
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BaseException : Exception
    {
        public int Status = 400;

        public BaseException(int numericCode, string message, params string[] variables)
          : base(string.Format(message, (object[])variables))
        {
            this.Variables = variables;
            this.NumericCode = numericCode;
        }

        [JsonProperty("errorCode")]
        public string Code => this.GetType().FullName.Replace(".Backend.Models", "").ToLower();

        [JsonProperty("errorMessage")]
        public override string Message => base.Message;

        [JsonProperty("messageVars")]
        public string[] Variables { get; private set; }

        [JsonProperty("numericErrorCode")]
        public int NumericCode { get; private set; }

        [JsonProperty("originatingService")]
        public string OriginatingService => "rift";

        [JsonProperty("intent")]
        public string Intent => "prod";
    }
}
