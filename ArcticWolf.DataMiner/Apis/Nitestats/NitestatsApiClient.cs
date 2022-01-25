
using ArcticWolf.DataMiner.Common.Http;
using ArcticWolf.DataMiner.Common.Json;
using ArcticWolf.DataMiner.Extensions;
using ArcticWolf.DataMiner.Models;
using ArcticWolf.DataMiner.Models.Apis.Nitestats;
using ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels;
using ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels.States;
using ArcticWolf.DataMiner.Models.Apis.Nitestats.Staging;
using ArcticWolf.DataMiner.Models.Discord;
using ArcticWolf.Storage;
using ArcticWolf.Storage.Constants;
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
using System.Threading;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Apis.Nitestats
{
    public class NitestatsApiClient
    {
        private const string LOG_PREFIX = "NiteStatsApi";
        private const string DATA_PARSER_LOG_PREFIX = "NiteStatsApi|DataParser";
        private const string CALENDAR_LOG_PREFIX = "NiteStatsApi|Calendar";

        private const string EVENT_FLAGS_DISCORD_BOT = "eventflag-updates";

        private CalendarResponse _cachedLastCalendarResponse = null;

        public NitestatsApiClient()
        {
            Log.Information("Initalizing...", LOG_PREFIX);
            GetCalendarData();
        }

        private void GetCalendarData()
        {
            DatabaseContext dbContext = Program.DbContext;

            HttpResponse response = new HttpClient().Request("https://api.nitestats.com/v1/epic/modes");

            if (!response.Success)
            {
                Log.Error("Request to retrieve calendar data was not successful!", CALENDAR_LOG_PREFIX);
                return;
            }

            CalendarResponse calendarResponse = JsonDeserializer.Deserialize<CalendarResponse>(response.Content);

            dbContext.FnEventFlags.Load();

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

                // process ClientEvents channel
                foreach (State<ClientEventsSubState> currentState in calendarResponse.Channels.ClientEvents.States)
                {
                    foreach (Event activeEvent in currentState.ActiveEvents)
                    {
                        IQueryable<FnEventFlag> foundFlags = dbContext.FnEventFlags.Include(x => x.TimeSpans).Where(x => x.Event == activeEvent.EventType);

                        if (foundFlags.Count() > 0)
                        {
                            FnEventFlag flag = foundFlags.First();
                            // flag exists so check if it has been modified or readded

                            Log.Verbose($"Found flag {flag.Event}", CALENDAR_LOG_PREFIX);
                            Log.Verbose($"Flag has {flag.TimeSpans.Count} time spans", CALENDAR_LOG_PREFIX);

                            IEnumerable<FnEventFlagTimeSpan> foundTimeSpans = flag.TimeSpans.Where(x => x.StartTime == activeEvent.ActiveSince || x.EndTime == activeEvent.ActiveUntil);

                            if (foundTimeSpans.Count() > 0)
                            {
                                // time span was modified
                                // make sure to modify the time span that will end last
                                FnEventFlagTimeSpan timeSpan = foundTimeSpans.OrderByDescending(x => x.EndTime).First();

                                if (timeSpan.EndTime != activeEvent.ActiveUntil || timeSpan.StartTime != activeEvent.ActiveSince)
                                {
                                    Log.Information($"Flag {activeEvent.EventType}: Modified timespan", CALENDAR_LOG_PREFIX);
                                    Log.Information($"Flag {activeEvent.EventType}: Start: {timeSpan.StartTime}' -> '{activeEvent.ActiveSince}'", CALENDAR_LOG_PREFIX);
                                    Log.Information($"Flag {activeEvent.EventType}: End: {timeSpan.EndTime}' -> '{activeEvent.ActiveUntil}'", CALENDAR_LOG_PREFIX);

                                    FnEventFlagModification modification = new();
                                    modification.ModifiedTimeSpan = timeSpan;
                                    modification.NewStartTime = activeEvent.ActiveSince;
                                    modification.NewEndTime = activeEvent.ActiveUntil;
                                    modification.OverriddenStartTime = timeSpan.StartTime;
                                    modification.OverriddenEndTime = timeSpan.EndTime;
                                    flag.Modifications.Add(modification);

                                    timeSpan.StartTime = activeEvent.ActiveSince;
                                    timeSpan.EndTime = activeEvent.ActiveUntil;
                                    continue;
                                }
                            }

                            // time span already exists
                            if (flag.TimeSpans.Where(x => x.StartTime == activeEvent.ActiveSince && x.EndTime == activeEvent.ActiveUntil).Any())
                            {
                                continue;
                            }

                            // new time span was added
                            FnEventFlagTimeSpan newTimeSpan = new();
                            newTimeSpan.StartTime = activeEvent.ActiveSince;
                            newTimeSpan.EndTime = activeEvent.ActiveUntil;
                            flag.TimeSpans.Add(newTimeSpan);
                            Log.Information($"Flag {activeEvent.EventType}: Added new time span: Start: {newTimeSpan.StartTime} | End: {newTimeSpan.EndTime}", CALENDAR_LOG_PREFIX);
                        }
                        else
                        {
                            // flag doesn't exist so create it
                            Log.Information($"Adding new flag {activeEvent.EventType}", CALENDAR_LOG_PREFIX);

                            FnEventFlag flag = new();
                            flag.Event = activeEvent.EventType;
                            
                            FnEventFlagTimeSpan newTimeSpan = new();
                            newTimeSpan.StartTime = activeEvent.ActiveSince;
                            newTimeSpan.EndTime = activeEvent.ActiveUntil;
                            flag.TimeSpans.Add(newTimeSpan);

                            dbContext.FnEventFlags.Add(flag);
                        }
                    }
                }

                _ = dbContext.SaveChanges();
            }
        }

        public void LoadEventFlagsFromMessages()
        {
            DatabaseContext dbContext = Program.DbContext;

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
                chatHistory = JsonDeserializer.Deserialize<ChatHistory>(jsonData);
            }
            catch (Exception ex)
            {
                Log.Error("(EventFlagsLoader): An error occured while parsing the event flags chat history: " + ex.Message, LOG_PREFIX);
                return;
            }

            Log.Information($"(EventFlagsLoader): Parsing event flags from '{chatHistory.Guild.Name} - {chatHistory.Channel.Name}'...", LOG_PREFIX);

            dbContext.FnEventFlags.Load();

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

                    if (eventTypeField.Value == "EventFlag.Anniversary.EnableEnemyVariants")
                    {

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

                    if (dbContext.FnEventFlags.Local.Any(x => x.Event == eventTypeField.Value))
                    {
                        eventFlag = dbContext.FnEventFlags.Local.First(x => x.Event == eventTypeField.Value);
                        if (!dbContext.Entry(eventFlag).Collection(x => x.TimeSpans).IsLoaded)
                        {
                            dbContext.Entry(eventFlag).Collection(x => x.TimeSpans).Load();
                        }
                        if (!dbContext.Entry(eventFlag).Collection(x => x.Modifications).IsLoaded)
                        {
                            dbContext.Entry(eventFlag).Collection(x => x.Modifications).Load();
                        }
                    }
                    else
                    {
                        eventFlag = new();
                        eventFlag.Event = eventTypeField.Value;
                        _ = dbContext.FnEventFlags.Add(eventFlag);
                    }



                    // get season of the flag start time
                    FnSeason flagStartSeason = Fortnite.Seasons.Find(x => x.StartTime <= flagStartTimeUtc && x.EndTime >= flagStartTimeUtc);

                    // get season of the flag end time
                    FnSeason flagEndSeason = Fortnite.Seasons.Find(x => x.StartTime <= flagEndTimeUtc && x.EndTime >= flagEndTimeUtc);

                    if (flagEndSeason.SeasonNumber == 0 && flagStartSeason.SeasonNumber == 0)
                    {
                        Log.Verbose("(EventFlagsLoader): Flag starts and ends in unsupported season", LOG_PREFIX);
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
                                Log.Debug($"(EventFlagsLoader): _______________", LOG_PREFIX);
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
                            Log.Debug($"(EventFlagsLoader): _______________", LOG_PREFIX);

                            break;

                        case "Modified":

                            if (overriddenFlagStartTimeUtc == default && overriddenFlagEndTimeUtc == default)
                            {
                                Log.Fatal($"(EventFlagsLoader): Well... That shouldn't happen. The overridden times have default values even though the flag has been modified.", LOG_PREFIX);
                                Log.Error($"(EventFlagsLoader): Fields data for previous error: " + JsonConvert.SerializeObject(embed.Fields), LOG_PREFIX);
                                Log.Error($"(EventFlagsLoader): Flag update from " + message.Timestamp, LOG_PREFIX);
                                Log.Debug($"(EventFlagsLoader): _______________", LOG_PREFIX);
                                break;
                            }

                            // check if modification already exists
                            if (eventFlag.Modifications.Any(x => x.OverriddenStartTime == overriddenFlagStartTimeUtc && x.OverriddenEndTime == overriddenFlagEndTimeUtc
                            && x.NewStartTime == flagStartTimeUtc && x.NewEndTime == flagEndTimeUtc))
                            {
                                Log.Debug($"(EventFlagsLoader): _______________", LOG_PREFIX);
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
                                    Log.Debug($"(EventFlagsLoader): _______________", LOG_PREFIX);
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
                            Log.Debug($"(EventFlagsLoader): _______________", LOG_PREFIX);

                            break;

                        case "Removed":
                            Log.Verbose($"(EventFlagsLoader): Action: Removed", LOG_PREFIX);
                            Log.Verbose($"(EventFlagsLoader): Flag: {eventTypeField.Value}", LOG_PREFIX);
                            Log.Verbose($"(EventFlagsLoader): Flag starts at {flagStartTimeUtc}", LOG_PREFIX);
                            Log.Verbose($"(EventFlagsLoader): Flag ends at {flagEndTimeUtc}", LOG_PREFIX);
                            Log.Verbose($"(EventFlagsLoader): Flag starts in S{flagStartSeason.SeasonNumber}", LOG_PREFIX);
                            Log.Verbose($"(EventFlagsLoader): Flag ends in S{flagEndSeason.SeasonNumber}", LOG_PREFIX);
                            Log.Verbose($"(EventFlagsLoader): Used in: {flagSeasonsString}", LOG_PREFIX);
                            Log.Verbose($"(EventFlagsLoader): _______________", LOG_PREFIX);

                            // Add removed flag if it hasn't been added yet
                            if (!eventFlag.TimeSpans.Any(x => x.StartTime == flagStartTimeUtc && x.EndTime == flagEndTimeUtc))
                            {
                                timeSpan = new();
                                timeSpan.StartTime = flagStartTimeUtc;
                                timeSpan.EndTime = flagEndTimeUtc;
                                eventFlag.TimeSpans.Add(timeSpan);
                            }

                            // if happened on patch day of new season and already exists, then don't count it to the new season
                            break;

                        default:
                            Log.Error($"(EventFlagsLoader): Flag '{eventTypeField.Value}' has an unknown status ({statusField.Value})", LOG_PREFIX);
                            Log.Debug($"(EventFlagsLoader): _______________", LOG_PREFIX);
                            break;
                    }
                }
            }

            int modifiedItemsCount = dbContext.ChangeTracker.Entries().Count(x => x.State == EntityState.Modified && x.Entity.GetType().Name == nameof(FnEventFlag));
            int addedItemsCount = dbContext.ChangeTracker.Entries().Count(x => x.State == EntityState.Added && x.Entity.GetType().Name == nameof(FnEventFlag));

            Log.Information($"(EventFlagsLoader): Added {addedItemsCount} items and modified {modifiedItemsCount} items.");

            int modifiedTSItemsCount = dbContext.ChangeTracker.Entries().Count(x => x.State == EntityState.Modified && x.Entity.GetType().Name == nameof(FnEventFlagTimeSpan));
            int addedTSItemsCount = dbContext.ChangeTracker.Entries().Count(x => x.State == EntityState.Added && x.Entity.GetType().Name == nameof(FnEventFlagTimeSpan));

            Log.Information($"(EventFlagsLoader): Added {addedTSItemsCount} TS items and modified {modifiedTSItemsCount} TS items.");

            dbContext.SaveChanges();

            // ToDo: do validation: No duplicate time spans for each flag
        }

        public void LoadHotFixesFromMessages()
        {
            DatabaseContext dbContext = Program.DbContext;

            Log.Information("(HotfixLoader): Loading data...", LOG_PREFIX);

            string jsonData;
            try
            {
                jsonData = File.ReadAllText(Program.Configuration.HotfixDiscordChatHistoryFilePath);
            }
            catch (Exception ex)
            {
                Log.Error("(HotfixLoader): An error occured while reading the hotfix chat history: " + ex.Message, LOG_PREFIX);
                return;
            }

            if (string.IsNullOrWhiteSpace(jsonData))
            {
                Log.Error("(HotfixLoader): An error occured while reading the hotfix chat history: File is empty", LOG_PREFIX);
                return;
            }

            ChatHistory chatHistory;
            try
            {
                chatHistory = JsonDeserializer.Deserialize<ChatHistory>(jsonData);
            }
            catch (Exception ex)
            {
                Log.Error("(HotfixLoader): An error occured while parsing the hotfix chat history: " + ex.Message, LOG_PREFIX);
                return;
            }

            Log.Information($"(HotfixLoader): Parsing hotfix data from '{chatHistory.Guild.Name} - {chatHistory.Channel.Name}'...", LOG_PREFIX);

            foreach (Message msg in chatHistory.Messages)
            {
                if (msg.Attachments.Count < 1)
                {
                    continue;
                }

                foreach (Attachment attachment in msg.Attachments)
                {
                    Log.Debug($"(HotfixLoader): Loading file '{attachment.Url}'");

                    string hotFixFileContent = "";
                    try
                    {
                        hotFixFileContent = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Program.Configuration.HotfixDiscordChatHistoryFilePath), attachment.Url));
                    }
                    catch (Exception ex)
                    {
                        Log.Error("(HotfixLoader): An error occured while parsing a hotfix file: " + ex.Message, LOG_PREFIX);
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(hotFixFileContent))
                    {
                        Log.Verbose("(HotfixLoader): Skipping file, because it's empty");
                        continue;
                    }

                    // Category Regex
                    MatchCollection categoryMatches = Regex.Matches(hotFixFileContent, @"\[.*\]\s");

                    foreach (Match categoryMatch in categoryMatches)
                    {
                        Match nextMatch = categoryMatch.NextMatch();
                        string catergoryName = categoryMatch.Value.Replace('\r', ' ').Replace('\n', ' ').Replace('[', ' ').Replace(']', ' ').Trim();

                        int categoryEndIndex = nextMatch.Index;
                        if (categoryEndIndex == 0)
                        {
                            categoryEndIndex = hotFixFileContent.Length;
                            Log.Debug("(HotfixLoader): This is the last category of this hotfix file.");
                        }

                        Log.Debug($"Found hotfix category '{catergoryName}', starting at index {categoryMatch.Index} and ending at index {categoryEndIndex}");

                        int categoryStartIndex = categoryMatch.Index + categoryMatch.Length;

                        string categoryContent = hotFixFileContent.Substring(categoryStartIndex, categoryEndIndex - categoryStartIndex);

                        MatchCollection variableMatches = Regex.Matches(hotFixFileContent, @".*=.*(\r\n|\r|\n|$)");

                        foreach(Match variableMatch in variableMatches)
                        {
                            string variableContent = variableMatch.Value.Replace('\r', ' ').Replace('\n', ' ').Trim();

                            // remove the diff prefix
                            if (variableContent.StartsWith("+ ") || variableContent.StartsWith("- "))
                            {
                                variableContent = variableContent[2..];
                            }

                            Match variableNameMatch = Regex.Matches(variableContent, @"([^=])*=").First();
                            string variableName = variableNameMatch.Value.Replace("=", "").Replace("+", "").Replace("-", "");

                            Log.Debug($"(HotfixLoader): Found variable: '{variableName}'");

                            string variableValue = variableContent.Replace(variableNameMatch.Value, "");

                            Log.Debug($"(HotfixLoader): Found variable value: '{variableValue}'");

                            if (variableValue.StartsWith("(") && variableValue.EndsWith(")"))
                            {
                                // Variable has multi params

                                // remove first and last parenthesis
                                variableValue = variableValue[1..];
                                variableValue = variableValue.Remove(variableValue.Length - 1);

                                MatchCollection variableParamMatches = Regex.Matches(variableValue, @"\w*=(\((\(.*\))*?\)|\(.*\)|\"".*?\""|\w*)");

                                foreach (Match paramMatch in variableParamMatches)
                                {
                                    string paramName = Regex.Match(paramMatch.Value, @"\w*=").Value;
                                    paramName = paramName.Remove(paramName.Length - 1); // remove '=' sign

                                    string paramValue = Regex.Match(paramMatch.Value, @"=.*").Value[1..]; // remove '=' sign

                                    Log.Debug($"(HotfixLoader): Found variable param '{paramName}' with value of '{paramValue}'");
                                }
                            }
                            else // Variable Value
                            {
                                Log.Debug($"(HotfixLoader): Found variable value: '{variableValue}'");
                            }
                        }
                    }

                    Thread.Sleep(3000);
                }
            }
        }

        public Dictionary<string, Server> GetStagingServers()
        {
            HttpResponse response = new HttpClient().Request("https://api.nitestats.com/v1/epic/staging/fortnite");

            if (!response.Success)
            {
                Log.Error("Request to retrieve staging data was not successful!", CALENDAR_LOG_PREFIX);
                return new Dictionary<string, Server>();
            }

            return JsonDeserializer.Deserialize<Dictionary<string, Server>>(response.Content);
        }
    }
}
