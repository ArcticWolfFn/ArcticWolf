﻿using ArcticWolf.DataMiner.Common.Http;
using ArcticWolf.DataMiner.Extensions;
using ArcticWolf.DataMiner.Models.Apis.Nitestats;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Apis.Nitestats
{
    public class NitestatsApiClient
    {
        private const string LOG_PREFIX = "NiteStatsApi";
        private const string PARSER_LOG_PREFIX = "NiteStatsApi|DataParser";

        public NitestatsApiClient()
        {
            Log.Information("Initalizing...", LOG_PREFIX);
            GetCalendarData();
        }

        private void GetCalendarData()
        {
            HttpResponse response = new HttpClient().Request("https://api.nitestats.com/v1/epic/modes");

            if (!response.Success)
            {
                Log.Error("Request to retrieve calendar data was not successful!", LOG_PREFIX);
                return;
            }

            CalendarResponse calendarResponse = JsonConvert.DeserializeObject<CalendarResponse>(response.Content, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Error,
                Error = ErrorHandler,
            });

            CalendarResponse fakeResponse = JsonConvert.DeserializeObject<CalendarResponse>(JsonConvert.SerializeObject(calendarResponse));
            fakeResponse.CacheIntervalMins = 12;
            fakeResponse.Channels.ClientEvents.States.First().SubState.SeasonNumber = 1;
            fakeResponse.Channels.ClientEvents.States.First().SubState.ActiveEvents.Add(new Models.Apis.Nitestats.Calendar.Channels.Event() { DevName = "CustomEvent" });

            List<Difference> differences = calendarResponse.GetDifferences(fakeResponse);

            foreach(Difference diff in differences)
            {
                switch (diff.Type)
                {
                    case DifferenceType.Added:
                        Log.Error($"(PropertyChanged) Object of type {diff.Type.ToString()} at {diff.Path} has been added", PARSER_LOG_PREFIX);
                        break;

                    case DifferenceType.Changed:
                        Log.Error($"(PropertyChanged) {diff.Property} changed from '{diff.OriginalValue}' to '{diff.NewValue}'", PARSER_LOG_PREFIX);
                        break;

                    case DifferenceType.Removed:
                        Log.Error($"(PropertyChanged) {diff.Property} at {diff.Path} was removed", PARSER_LOG_PREFIX);
                        break;
                }
            }
        }

        void ErrorHandler(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            if (e.ErrorContext.Error.Message.StartsWith("Could not find member "))
            {
                // do something...
                Log.Error($"(NewMemeber) Detected a new member '{e.ErrorContext.Member}'", PARSER_LOG_PREFIX);
                Log.Error($"(NewMemeber) Path: '{e.ErrorContext.Path}'", PARSER_LOG_PREFIX);
                Log.Error($"(NewMemeber) Error Message: '{e.ErrorContext.Error.Message}'", PARSER_LOG_PREFIX);

                // hide the error
                e.ErrorContext.Handled = true;
            }
        }
    }
}
