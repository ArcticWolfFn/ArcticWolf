using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Common.Json
{
    public static class JsonDeserializer
    {
        private const string LOG_PREFIX = "JsonDeserializer";

        /// <summary>
        ///     Converts the given json data into the specified <typeparamref name="Type"/> and
        ///     reports missing properties, if <paramref name="enableReport"/> is <see langword="true"/>
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="json"></param>
        /// <param name="enableReport"></param>
        /// <returns></returns>
        public static Type Deserialize<Type>(string json, bool enableReport = true)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error,
                Error = ErrorHandler,
            };

            return JsonConvert.DeserializeObject<Type>(json);
        }

        private static void ErrorHandler(object sender, ErrorEventArgs e)
        {
            if (e.ErrorContext.Error.Message.StartsWith("Could not find member "))
            {
                // do something...
                Log.Error($"(NewMemeber) Detected a new member '{e.ErrorContext.Member}'", LOG_PREFIX);
                Log.Error($"(NewMemeber) Path: '{e.ErrorContext.Path}'", LOG_PREFIX);
                Log.Error($"(NewMemeber) Error Message: '{e.ErrorContext.Error.Message}'", LOG_PREFIX);

                // hide the error
                e.ErrorContext.Handled = true;
            }
        }
    }
}
