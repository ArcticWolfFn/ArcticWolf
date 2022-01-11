using ArcticWolfApi.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ArcticWolfApi
{
    public static class Tools
    {
        public static string CreateRandomHexString()
        {
            Random random = new();
            string empty = string.Empty;

            for (int index = 0; index < 4; ++index)
            {
                int num = random.Next(0, int.MaxValue);
                empty += num.ToString("X8");
            }

            return empty.ToLower();
        }

        public static int GetSeasonNumber(this HttpRequest request)
        {
            return Convert.ToInt32(request.GetBuildVersion());
        }

        public static decimal GetBuildVersion(this HttpRequest request)
        {
            return decimal.Parse(request.GetBuildVersionString(), NumberStyles.Any, CultureInfo.InvariantCulture);
        }

        public static string GetBuildVersionString(this HttpRequest request)
        {
            string header = request.Headers["User-Agent"];
            if (!string.IsNullOrEmpty(header))
            {
                if (header.Contains("Fortnite"))
                {
                    try
                    {
                        string s = header.Split("-")[1];
                        return s == "Next" || s == "Cert" || s.Contains("+++Fortnite+Release") ? "2.0" : s;
                    }
                    catch
                    {
                        return "1.0";
                    }
                }
            }

            return "1.0";
        }

        public static int GetCLNumber(this HttpRequest request)
        {
            string header = request.Headers["User-Agent"];

            if (!string.IsNullOrEmpty(header))
            {
                if (header.Contains("Fortnite"))
                {
                    try
                    {
                        string str = header.Remove(0, header.IndexOf("CL-")).Replace("CL-", "");
                        return int.TryParse(str, out int result) ? result : int.Parse(new string(str.TakeWhile(new Func<char, bool>(char.IsDigit)).ToArray()));
                    }
                    catch
                    {
                        return 0;
                    }
                }
            }
            return 0;
        }

        public static IApplicationBuilder UseEpicStatusErrors(this IApplicationBuilder app)
        {
            app.UseStatusCodePages(async context =>
            {
                string str = "";
                context.HttpContext.Response.Headers["Content-Type"] = "application/json; charset=utf-8";
                string text;

                switch ((HttpStatusCode)context.HttpContext.Response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        text = JsonConvert.SerializeObject((object)new NotFoundException());
                        break;

                    case HttpStatusCode.MethodNotAllowed:
                        text = JsonConvert.SerializeObject((object)new MethodNotAllowedException());
                        break;

                    default:
                        text = str;
                        break;
                }

                await context.HttpContext.Response.WriteAsync(text);
            });

            return app;
        }

        public static DateTime TrimDate(this DateTime date) => new(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, date.Kind);
    }
}
