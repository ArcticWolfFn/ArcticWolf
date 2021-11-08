using ArcticWolf.DataMiner.Common.Http;
using ArcticWolf.DataMiner.Constants;
using ArcticWolf.DataMiner.Extensions;
using ArcticWolf.DataMiner.Models;
using ArcticWolf.DataMiner.Models.Apis.Nitestats;
using ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels.States;
using ArcticWolf.DataMiner.Models.Discord;
using ArcticWolf.DataMiner.Storage;
using Microsoft.EntityFrameworkCore;
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

            Log.Information($"(EventFlagsLoader): Parsing event flags from '{chatHistory.Guild.Name} - {chatHistory.Channel.Name}'...", LOG_PREFIX);

            Program.DbContext.FnEventFlags.Load();

            foreach (Message message in chatHistory.Messages)
            {
                Log.Verbose($"(EventFlagsLoader): Analysing message from {message.Author.Name} at {message.Timestamp.ToUniversalTime()}...", LOG_PREFIX);

                // validate message
                if (message.Author.Name != EVENT_FLAGS_DISCORD_BOT || !message.Embeds.Any())
                {
                    Log.Verbose($"(EventFlagsLoader): Message invalid! Reason: Wrong author name or no embeds.", LOG_PREFIX);
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
                        Log.Verbose($"(EventFlagsLoader): Message invalid! Reason: One or more fields are empty.", LOG_PREFIX);
                        Log.Verbose($"(EventFlagsLoader): Fields data for previous error: " + JsonConvert.SerializeObject(embed.Fields), LOG_PREFIX);
                        continue;
                    }

                    // get the overridden datetimes
                    string overriddenStartsField = Regex.Match(startsField.Value, "~~.*~~").Value;
                    string overriddenEndsField = Regex.Match(endsField.Value, "~~.*~~").Value;

                    // fix DateTime parsing error due to Discord text styling
                    startsField.Value = Regex.Replace(startsField.Value, "~~.*~~", "");
                    endsField.Value = Regex.Replace(endsField.Value, "~~.*~~", "");

                    // just remove useless chars
                    eventTypeField.Value = eventTypeField.Value.Trim();
                    statusField.Value = statusField.Value.Trim();

                    DateTime overriddenFlagStartTimeUtc = new();
                    DateTime overriddenFlagEndTimeUtc = new();
                    DateTime flagStartTimeUtc = new();
                    DateTime flagEndTimeUtc = new();

                    try
                    {
                        // appending " Z" tells the parser that this is a utc time
                        flagStartTimeUtc = DateTime.Parse(startsField.Value + " Z", CultureInfo.InvariantCulture).ToUniversalTime();
                        flagEndTimeUtc = DateTime.Parse(endsField.Value + " Z", CultureInfo.InvariantCulture).ToUniversalTime();

                        if (!string.IsNullOrWhiteSpace(overriddenStartsField))
                        {
                            overriddenFlagStartTimeUtc = DateTime.Parse(endsField.Value + " Z", CultureInfo.InvariantCulture).ToUniversalTime();
                        }

                        if (!string.IsNullOrWhiteSpace(overriddenEndsField))
                        {
                            overriddenFlagEndTimeUtc = DateTime.Parse(endsField.Value + " Z", CultureInfo.InvariantCulture).ToUniversalTime();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"(EventFlagsLoader): Start or end time is invalid! Error: " + ex.Message, LOG_PREFIX);
                        Log.Error($"(EventFlagsLoader): Fields data for previous error: " + JsonConvert.SerializeObject(embed.Fields), LOG_PREFIX);
                        continue;
                    }

                    // find or create the event flag
                    FnEventFlag eventFlag;

                    if (Program.DbContext.FnEventFlags.Local.Any(x => x.Event == eventTypeField.Value))
                    {
                        eventFlag = Program.DbContext.FnEventFlags.Local.First(x => x.Event == eventTypeField.Value);
                        Program.DbContext.Entry(eventFlag).Collection(x => x.TimeSpans).Load();
                        Program.DbContext.Entry(eventFlag).Collection(x => x.Modifications).Load();
                    }
                    else
                    {
                        eventFlag = new();
                        eventFlag.Event = eventTypeField.Value;
                        Program.DbContext.FnEventFlags.Add(eventFlag);
                    }



                    // get season of the flag start time
                    FnSeason flagStartSeason = Fortnite.Seasons.Find(x => x.StartTime <= flagStartTimeUtc && x.EndTime >= flagStartTimeUtc);

                    // get season of the flag end time
                    FnSeason flagEndSeason = Fortnite.Seasons.Find(x => x.StartTime <= flagEndTimeUtc && x.EndTime >= flagEndTimeUtc);

                    if (flagEndSeason.SeasonNumber == 0 && flagStartSeason.SeasonNumber == 0)
                    {
                        Log.Verbose("(EventFlagsLoader): Flag starts and ends in unsupported season", LOG_PREFIX);
                        continue;
                    }
                    else if (flagEndSeason.SeasonNumber == 0)
                    {
                        Log.Verbose("(EventFlagsLoader): Flag ends in unsupported season", LOG_PREFIX);
                    }
                    else // flagStartSeason.SeasonNumber == 0
                    {
                        Log.Verbose("(EventFlagsLoader): Flag starts in unsupported season", LOG_PREFIX);
                    }

                    IEnumerable<FnSeason> flagSeasons = Fortnite.Seasons.Where(x =>
                        // starts in season
                        (x.StartTime <= flagStartTimeUtc && x.EndTime >= flagStartTimeUtc) ||

                        // ends in season
                        (x.StartTime <= flagEndTimeUtc && x.EndTime >= flagEndTimeUtc) ||

                        // during season
                        (x.StartTime >= flagStartTimeUtc && x.EndTime <= flagStartTimeUtc)
                        );

                    string flagSeasonsString = "";
                    foreach (FnSeason season in flagSeasons)
                    {
                        flagSeasonsString += $"S{season.SeasonNumber}, ";
                    }

                    switch (statusField.Value.Trim())
                    {
                        case "Added":
                            if (eventFlag.TimeSpans.Any(x => x.StartTime == flagStartTimeUtc && x.EndTime == flagEndTimeUtc))
                            {
                                Log.Verbose("(EventFlagsLoader): TimeSpan already exists", LOG_PREFIX);
                                break;
                            }

                            // too many time spans (adding time spans, which already have been modified)
                            if (eventFlag.TimeSpans.Any(x => x.StartTime == flagStartTimeUtc && x.EndTime == flagEndTimeUtc))
                            {
                                break;
                            }

                            FnEventFlagTimeSpan timeSpan = new();
                            timeSpan.StartTime = flagStartTimeUtc;
                            timeSpan.EndTime = flagEndTimeUtc;
                            eventFlag.TimeSpans.Add(timeSpan);

                            Log.Debug($"(EventFlagsLoader): Action: Added", LOG_PREFIX);
                            Log.Debug($"(EventFlagsLoader): Flag: {eventTypeField.Value}", LOG_PREFIX);
                            Log.Debug($"(EventFlagsLoader): Flag starts at {flagStartTimeUtc}", LOG_PREFIX);
                            Log.Debug($"(EventFlagsLoader): Flag ends at {flagEndTimeUtc}", LOG_PREFIX);
                            Log.Debug($"(EventFlagsLoader): Flag starts in S{flagStartSeason.SeasonNumber}", LOG_PREFIX);
                            Log.Debug($"(EventFlagsLoader): Flag ends in S{flagEndSeason.SeasonNumber}", LOG_PREFIX);
                            Log.Debug($"(EventFlagsLoader): Used in: {flagSeasonsString}", LOG_PREFIX);

                            break;

                        case "Modified":

                            if (overriddenFlagStartTimeUtc != default && overriddenFlagEndTimeUtc != default)
                            {
                                Log.Fatal($"(EventFlagsLoader): Well... That shouldn't happen. The overridden times have default values even though the flag has been modified.", LOG_PREFIX);
                                break;
                            }

                            // check if modification already exists
                            if (eventFlag.Modifications.Any(x => x.OverriddenStartTime == overriddenFlagStartTimeUtc && x.OverriddenEndTime == overriddenFlagEndTimeUtc
                            && x.NewStartTime == flagStartTimeUtc && x.NewEndTime == flagEndTimeUtc))
                            {
                                break;
                            }

                            if (eventFlag.TimeSpans.Any(x => x.StartTime == overriddenFlagStartTimeUtc && x.EndTime == overriddenFlagEndTimeUtc))
                            {
                                timeSpan = eventFlag.TimeSpans.First(x => x.StartTime == overriddenFlagStartTimeUtc && x.EndTime == overriddenFlagEndTimeUtc);

                                FnEventFlagModification modification = new();
                                modification.ModifiedTimeSpan = timeSpan;
                                modification.OverriddenStartTime = overriddenFlagStartTimeUtc;
                                modification.OverriddenEndTime = overriddenFlagEndTimeUtc;
                                modification.NewStartTime = flagStartTimeUtc;
                                modification.NewEndTime = flagEndTimeUtc;
                                eventFlag.Modifications.Add(modification);

                                // modified before active
                                if (message.Timestamp < timeSpan.StartTime)
                                {
                                    timeSpan.StartTime = flagStartTimeUtc;
                                }

                                timeSpan.EndTime = flagEndTimeUtc;
                            }
                            else
                            {
                                if (eventFlag.TimeSpans.Any(x => x.StartTime == flagStartTimeUtc && x.EndTime == flagEndTimeUtc))
                                {
                                    Log.Verbose("(EventFlagsLoader): TimeSpan already exists", LOG_PREFIX);
                                    break;
                                }

                                timeSpan = new();
                                timeSpan.StartTime = flagStartTimeUtc;
                                timeSpan.EndTime = flagEndTimeUtc;
                                eventFlag.TimeSpans.Add(timeSpan);
                            }

                            Log.Debug($"(EventFlagsLoader): Action: Modified", LOG_PREFIX);
                            Log.Debug($"(EventFlagsLoader): Flag: {eventTypeField.Value}", LOG_PREFIX);
                            Log.Debug($"(EventFlagsLoader): Flag starts at {flagStartTimeUtc}", LOG_PREFIX);
                            Log.Debug($"(EventFlagsLoader): Flag ends at {flagEndTimeUtc}", LOG_PREFIX);
                            Log.Debug($"(EventFlagsLoader): Flag starts in S{flagStartSeason.SeasonNumber}", LOG_PREFIX);
                            Log.Debug($"(EventFlagsLoader): Flag ends in S{flagEndSeason.SeasonNumber}", LOG_PREFIX);
                            Log.Debug($"(EventFlagsLoader): Used in: {flagSeasonsString}", LOG_PREFIX);

                            break;

                        case "Removed":
                            Log.Verbose($"(EventFlagsLoader): Action: Removed", LOG_PREFIX);
                            Log.Verbose($"(EventFlagsLoader): Flag: {eventTypeField.Value}", LOG_PREFIX);
                            Log.Verbose($"(EventFlagsLoader): Flag starts at {flagStartTimeUtc}", LOG_PREFIX);
                            Log.Verbose($"(EventFlagsLoader): Flag ends at {flagEndTimeUtc}", LOG_PREFIX);
                            Log.Verbose($"(EventFlagsLoader): Flag starts in S{flagStartSeason.SeasonNumber}", LOG_PREFIX);
                            Log.Verbose($"(EventFlagsLoader): Flag ends in S{flagEndSeason.SeasonNumber}", LOG_PREFIX);
                            Log.Verbose($"(EventFlagsLoader): Used in: {flagSeasonsString}", LOG_PREFIX);

                            // if happened on patch day of new season and already exists, then don't count it to the new season
                            break;

                        default:
                            Log.Error($"(EventFlagsLoader): Flag '{eventTypeField.Value}' has an unknown status ({statusField.Value})", LOG_PREFIX);
                            break;
                    }

                    Log.Debug($"(EventFlagsLoader): _______________", LOG_PREFIX);
                }
            }

            int modifiedItemsCount = Program.DbContext.ChangeTracker.Entries().Count(x => x.State == EntityState.Modified && x.Entity.GetType().Name == nameof(FnEventFlag));
            int addedItemsCount = Program.DbContext.ChangeTracker.Entries().Count(x => x.State == EntityState.Added && x.Entity.GetType().Name == nameof(FnEventFlag));

            Log.Information($"(EventFlagsLoader): Added {addedItemsCount} items and modified {modifiedItemsCount} items.");

            int modifiedTSItemsCount = Program.DbContext.ChangeTracker.Entries().Count(x => x.State == EntityState.Modified && x.Entity.GetType().Name == nameof(FnEventFlagTimeSpan));
            int addedTSItemsCount = Program.DbContext.ChangeTracker.Entries().Count(x => x.State == EntityState.Added && x.Entity.GetType().Name == nameof(FnEventFlagTimeSpan));

            Log.Information($"(EventFlagsLoader): Added {addedTSItemsCount} TS items and modified {modifiedTSItemsCount} TS items.");

            Program.DbContext.SaveChanges();
        }
    }
}
