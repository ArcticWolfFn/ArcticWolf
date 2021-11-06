using ArcticWolf.DataMiner.Common.Http;
using ArcticWolf.DataMiner.Constants;
using ArcticWolf.DataMiner.Extensions;
using ArcticWolf.DataMiner.Models;
using ArcticWolf.DataMiner.Models.Apis.Nitestats;
using ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels.States;
using ArcticWolf.DataMiner.Models.Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Apis.Nitestats
{
    public class NitestatsApiClient
    {
        private const string LOG_PREFIX = "NiteStatsApi";
        private const string DATA_PARSER_LOG_PREFIX = "NiteStatsApi|DataParser";

        private const string EVENT_FLAGS_DISCORD_BOT = "eventflag-updates";

        private CalendarResponse _cachedLastCalendarResponse = null;

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

            if (_cachedLastCalendarResponse != null)
            {
                // do comparison
                List<Difference> differences = calendarResponse.GetDifferences(_cachedLastCalendarResponse);
                foreach (Difference diff in differences)
                {
                    switch (diff.Type)
                    {
                        case DifferenceType.Added:
                            Log.Error($"(PropertyChanged) Object of type {diff.Type} at {diff.Path} has been added", DATA_PARSER_LOG_PREFIX);
                            break;

                        case DifferenceType.Changed:
                            Log.Error($"(PropertyChanged) {diff.Property} changed from '{diff.OriginalValue}' to '{diff.NewValue}'", DATA_PARSER_LOG_PREFIX);
                            break;

                        case DifferenceType.Removed:
                            Log.Error($"(PropertyChanged) {diff.Property} at {diff.Path} was removed", DATA_PARSER_LOG_PREFIX);
                            break;
                    }
                }

                return;
            }
            else
            {
                // got data for first time

                // process ClientMatchmaking channel
                foreach (State<TkSubState> currentState in calendarResponse.Channels.Tk.States)
                {
                    TkSubState currentSubState = currentState.SubState;

                    //currentSubState.K;
                }
            }
        }

        private void ErrorHandler(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            if (e.ErrorContext.Error.Message.StartsWith("Could not find member "))
            {
                // do something...
                Log.Error($"(NewMemeber) Detected a new member '{e.ErrorContext.Member}'", DATA_PARSER_LOG_PREFIX);
                Log.Error($"(NewMemeber) Path: '{e.ErrorContext.Path}'", DATA_PARSER_LOG_PREFIX);
                Log.Error($"(NewMemeber) Error Message: '{e.ErrorContext.Error.Message}'", DATA_PARSER_LOG_PREFIX);

                // hide the error
                e.ErrorContext.Handled = true;
            }
        }

        public void LoadEventFlagsFromMessages()
        {
            Log.Information("(EventFlagsLoader): Loading data...", LOG_PREFIX);

            string jsonData;
            try
            {
                jsonData = File.ReadAllText(Program.Configuration.EventFlagsDiscordChatHistoryFilePath);
            }
            catch (Exception ex)
            {
                Log.Error("(EventFlagsLoader): An error occured while reading the event flags chat history: " + ex.Message, LOG_PREFIX);
                return;
            }

            if (string.IsNullOrWhiteSpace(jsonData))
            {
                Log.Error("(EventFlagsLoader): An error occured while reading the event flags chat history: File is empty", LOG_PREFIX);
                return;
            }

            ChatHistory chatHistory;
            try
            {
                chatHistory = JsonConvert.DeserializeObject<ChatHistory>(jsonData);
            }
            catch (Exception ex)
            {
                Log.Error("(EventFlagsLoader): An error occured while parsing the event flags chat history: " + ex.Message, LOG_PREFIX);
                return;
            }

            Log.Information($"(EventFlagsLoader): Parsing event flags from '{chatHistory.Guild.Name} - {chatHistory.Channel.Name}'...");
        
            foreach(Message message in chatHistory.Messages)
            {
                Log.Verbose($"(EventFlagsLoader): Analysing message from {message.Author.Name} at {message.Timestamp.ToUniversalTime()}...");

                // validate message
                if (message.Author.Name != EVENT_FLAGS_DISCORD_BOT || !message.Embeds.Any())
                {
                    Log.Verbose($"(EventFlagsLoader): Message invalid! Reason: Wrong author name or no embeds.");
                    continue;
                }

                foreach (Embed embed in message.Embeds)
                {
                    Field eventTypeField = embed.Fields.Find(x => x.Name.Contains("EventType"));
                    Field startsField = embed.Fields.Find(x => x.Name.Contains("Starts"));
                    Field endsField = embed.Fields.Find(x => x.Name.Contains("Ends"));
                    Field statusField = embed.Fields.Find(x => x.Name.Contains("Status"));

                    // validate embed
                    if (eventTypeField == null || startsField == null || endsField == null || statusField == null)
                    {
                        Log.Verbose($"(EventFlagsLoader): Message invalid! Reason: One or more fields are empty.");
                        Log.Verbose($"(EventFlagsLoader): Fields data for previous error: " + JsonConvert.SerializeObject(embed.Fields));
                        continue;
                    }

                    // fix DateTime parsing error due to Discord text styling
                    startsField.Value = Regex.Replace(startsField.Value, "~~.*~~", "");
                    endsField.Value = Regex.Replace(endsField.Value, "~~.*~~", "");

                    DateTime flagStartTimeUtc;
                    DateTime flagEndTimeUtc;

                    try
                    {
                        // appending " Z" tells the parser that this is a utc time
                        flagStartTimeUtc = DateTime.Parse(startsField.Value + " Z", CultureInfo.InvariantCulture).ToUniversalTime();
                        flagEndTimeUtc = DateTime.Parse(endsField.Value + " Z", CultureInfo.InvariantCulture).ToUniversalTime();
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"(EventFlagsLoader): Start or end time is invalid! Error: " + ex.Message);
                        Log.Error($"(EventFlagsLoader): Fields data for previous error: " + JsonConvert.SerializeObject(embed.Fields));
                        continue;
                    }

                    // get season of the flag start time
                    FnSeason flagStartSeason = Fortnite.Seasons.Find(x => x.StartTime < flagStartTimeUtc && x.EndTime > flagStartTimeUtc);

                    // get season of the flag end time
                    FnSeason flagEndSeason = Fortnite.Seasons.Find(x => x.StartTime < flagEndTimeUtc && x.EndTime > flagEndTimeUtc);

                    if (flagEndSeason.SeasonNumber == 0 && flagStartSeason.SeasonNumber == 0)
                    {
                        Log.Verbose("(EventFlagsLoader): Flag starts and ends in unsupported season");
                        continue;
                    }
                    else if (flagEndSeason.SeasonNumber == 0)
                    {
                        Log.Warning("(EventFlagsLoader): Flag ends in unsupported season");
                    }
                    else // flagStartSeason.SeasonNumber == 0
                    {
                        Log.Warning("(EventFlagsLoader): Flag starts in unsupported season");
                    }

                    Log.Information($"(EventFlagsLoader): Flag: {eventTypeField.Value}");
                    Log.Information($"(EventFlagsLoader): Flag starts at {flagStartTimeUtc}");
                    Log.Information($"(EventFlagsLoader): Flag ends at {flagEndTimeUtc}");
                    Log.Information($"(EventFlagsLoader): Flag starts in S{flagStartSeason.SeasonNumber}");
                    Log.Information($"(EventFlagsLoader): Flag ends in S{flagEndSeason.SeasonNumber}");

                    switch (statusField.Value.Trim())
                    {
                        case "Added":
                            Log.Information($"(EventFlagsLoader): Action: Added");
                            break;

                        case "Modified":
                            Log.Information($"(EventFlagsLoader): Action: Modified");
                            break;

                        case "Removed":
                            Log.Information($"(EventFlagsLoader): Action: Removed");

                            // if happened on patch day of new season and already exists, then don't count it to the new season
                            break;

                        default:
                            Log.Error($"(EventFlagsLoader): Flag '{eventTypeField.Value}' has an unknown status ({statusField.Value})");
                            break;
                    }

                    Log.Information($"(EventFlagsLoader): _______________");
                }
            }
        }
    }
}
