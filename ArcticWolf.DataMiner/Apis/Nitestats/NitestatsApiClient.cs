
using ArcticWolf.DataMiner.Common.Http;
using ArcticWolf.DataMiner.Common.Json;
using ArcticWolf.DataMiner.Extensions;
using ArcticWolf.DataMiner.Models.Apis.Nitestats;
using ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels;
using ArcticWolf.DataMiner.Models.Apis.Nitestats.Calendar.Channels.States;
using ArcticWolf.DataMiner.Models.Apis.Nitestats.Staging;
using ArcticWolf.DataMiner.Models.Discord;
using ArcticWolf.Storage;
using ArcticWolf.Storage.Constants;
using Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace ArcticWolf.DataMiner.Apis.Nitestats
{
    public class NitestatsApiClient
    {
        private const string EVENT_FLAGS_DISCORD_BOT = "eventflag-updates";

        private CalendarResponse _cachedLastCalendarResponse = null;

        public NitestatsApiClient()
        {
            Log.Information("Initalizing...");
            GetCalendarData();
        }

        [LogPrefix("Calendar")]
        private void GetCalendarData()
        {
            DatabaseContext dbContext = Program.DbContext;

            HttpResponse response = new HttpClient().Request("https://api.nitestats.com/v1/epic/modes");

            if (!response.Success)
            {
                Log.Error("Request to retrieve calendar data was not successful!");
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
                            Log.Error($"(PropertyChanged) Object of type {diff.Type} at {diff.Path} has been added");
                            break;

                        case DifferenceType.Changed:
                            Log.Error($"(PropertyChanged) {diff.Property} changed from '{diff.OriginalValue}' to '{diff.NewValue}'");
                            break;

                        case DifferenceType.Removed:
                            Log.Error($"(PropertyChanged) {diff.Property} at {diff.Path} was removed");
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

                            Log.Verbose($"Found flag {flag.Event}");
                            Log.Verbose($"Flag has {flag.TimeSpans.Count} time spans");

                            IEnumerable<FnEventFlagTimeSpan> foundTimeSpans = flag.TimeSpans.Where(x => x.StartTime == activeEvent.ActiveSince || x.EndTime == activeEvent.ActiveUntil);

                            if (foundTimeSpans.Count() > 0)
                            {
                                // time span was modified
                                // make sure to modify the time span that will end last
                                FnEventFlagTimeSpan timeSpan = foundTimeSpans.OrderByDescending(x => x.EndTime).First();

                                if (timeSpan.EndTime != activeEvent.ActiveUntil || timeSpan.StartTime != activeEvent.ActiveSince)
                                {
                                    Log.Information($"Flag {activeEvent.EventType}: Modified timespan");
                                    Log.Information($"Flag {activeEvent.EventType}: Start: {timeSpan.StartTime}' -> '{activeEvent.ActiveSince}'");
                                    Log.Information($"Flag {activeEvent.EventType}: End: {timeSpan.EndTime}' -> '{activeEvent.ActiveUntil}'");

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
                            Log.Information($"Flag {activeEvent.EventType}: Added new time span: Start: {newTimeSpan.StartTime} | End: {newTimeSpan.EndTime}");
                        }
                        else
                        {
                            // flag doesn't exist so create it
                            Log.Information($"Adding new flag {activeEvent.EventType}");

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

        /// <summary>
        /// Generates an epic games client credentials token, not linked to an account. All tokens are cached and regenerated every 10 minutes.
        /// </summary>
        public AccessTokenResponse GetAccessToken()
        {
            HttpResponse response = new HttpClient().Request("https://api.nitestats.com/v1/epic/bearer");

            if (!response.Success)
            {
                Log.Error("Request to retrieve access token was not successful!");
                return new AccessTokenResponse();
            }

            AccessTokenResponse accessTokenResponse = JsonDeserializer.Deserialize<AccessTokenResponse>(response.Content);

            return accessTokenResponse;
        }

        public void LoadEventFlagsFromMessages()
        {
            DatabaseContext dbContext = Program.DbContext;

            Log.Information("Loading data...", "EventFlagsLoader");

            string jsonData;
            try
            {
                jsonData = File.ReadAllText(Program.Configuration.EventFlagsDiscordChatHistoryFilePath);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while reading the event flags chat history: " + ex.Message, "EventFlagsLoader");
                return;
            }

            if (string.IsNullOrWhiteSpace(jsonData))
            {
                Log.Error("An error occured while reading the event flags chat history: File is empty", "EventFlagsLoader");
                return;
            }

            ChatHistory chatHistory;
            try
            {
                chatHistory = JsonDeserializer.Deserialize<ChatHistory>(jsonData);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while parsing the event flags chat history: " + ex.Message, "EventFlagsLoader");
                return;
            }

            Log.Information($"Parsing event flags from '{chatHistory.Guild.Name} - {chatHistory.Channel.Name}'...", "EventFlagsLoader");

            dbContext.FnEventFlags.Load();

            foreach (Message message in chatHistory.Messages)
            {
                Log.Verbose($"Analysing message from {message.Author.Name} at {message.Timestamp.ToUniversalTime()}...", "EventFlagsLoader");

                // validate message
                if (message.Author.Name != EVENT_FLAGS_DISCORD_BOT || !message.Embeds.Any())
                {
                    Log.Verbose($"Message invalid! Reason: Wrong author name or no embeds.", "EventFlagsLoader");
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
                        Log.Verbose($"Message invalid! Reason: One or more fields are empty.", "EventFlagsLoader");
                        Log.Verbose($"Fields data for previous error: " + JsonConvert.SerializeObject(embed.Fields), "EventFlagsLoader");
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
                        Log.Error($"Start or end time is invalid! Error: " + ex.Message, "EventFlagsLoader");
                        Log.Error($"Fields data for previous error: " + JsonConvert.SerializeObject(embed.Fields), "EventFlagsLoader");
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
                    FnSeason flagStartSeason = flagStartTimeUtc.GetSeason();

                    // get season of the flag end time
                    FnSeason flagEndSeason = flagEndTimeUtc.GetSeason();

                    if (flagEndSeason.SeasonNumber == 0 && flagStartSeason.SeasonNumber == 0)
                    {
                        Log.Verbose("Flag starts and ends in unsupported season", "EventFlagsLoader");
                    }
                    else if (flagEndSeason.SeasonNumber == 0)
                    {
                        Log.Verbose("Flag ends in unsupported season", "EventFlagsLoader");
                    }
                    else // flagStartSeason.SeasonNumber == 0
                    {
                        Log.Verbose("Flag starts in unsupported season", "EventFlagsLoader");
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
                                Log.Verbose("TimeSpan already exists", "EventFlagsLoader");
                                Log.Debug($"_______________", "EventFlagsLoader");
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

                            Log.Debug($"Action: Added", "EventFlagsLoader");
                            Log.Debug($"Flag: {eventTypeField.Value}", "EventFlagsLoader");
                            Log.Debug($"Flag starts at {flagStartTimeUtc}", "EventFlagsLoader");
                            Log.Debug($"Flag ends at {flagEndTimeUtc}", "EventFlagsLoader");
                            Log.Debug($"Flag starts in S{flagStartSeason.SeasonNumber}", "EventFlagsLoader");
                            Log.Debug($"Flag ends in S{flagEndSeason.SeasonNumber}", "EventFlagsLoader");
                            Log.Debug($"Used in: {flagSeasonsString}", "EventFlagsLoader");
                            Log.Debug($"_______________", "EventFlagsLoader");

                            break;

                        case "Modified":

                            if (overriddenFlagStartTimeUtc == default && overriddenFlagEndTimeUtc == default)
                            {
                                Log.Fatal($"Well... That shouldn't happen. The overridden times have default values even though the flag has been modified.", "EventFlagsLoader");
                                Log.Error($"Fields data for previous error: " + JsonConvert.SerializeObject(embed.Fields), "EventFlagsLoader");
                                Log.Error($"Flag update from " + message.Timestamp, "EventFlagsLoader");
                                Log.Debug($"_______________", "EventFlagsLoader");
                                break;
                            }

                            // check if modification already exists
                            if (eventFlag.Modifications.Any(x => x.OverriddenStartTime == overriddenFlagStartTimeUtc && x.OverriddenEndTime == overriddenFlagEndTimeUtc
                            && x.NewStartTime == flagStartTimeUtc && x.NewEndTime == flagEndTimeUtc))
                            {
                                Log.Debug($"_______________", "EventFlagsLoader");
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
                                    Log.Verbose("TimeSpan already exists", "EventFlagsLoader");
                                    Log.Debug($"_______________", "EventFlagsLoader");
                                    break;
                                }

                                timeSpan = new();
                                timeSpan.StartTime = flagStartTimeUtc;
                                timeSpan.EndTime = flagEndTimeUtc;
                                eventFlag.TimeSpans.Add(timeSpan);
                            }

                            Log.Debug($"Action: Modified", "EventFlagsLoader");
                            Log.Debug($"Flag: {eventTypeField.Value}", "EventFlagsLoader");
                            Log.Debug($"Flag starts at {flagStartTimeUtc}", "EventFlagsLoader");
                            Log.Debug($"Flag ends at {flagEndTimeUtc}", "EventFlagsLoader");
                            Log.Debug($"Flag starts in S{flagStartSeason.SeasonNumber}", "EventFlagsLoader");
                            Log.Debug($"Flag ends in S{flagEndSeason.SeasonNumber}", "EventFlagsLoader");
                            Log.Debug($"Used in: {flagSeasonsString}", "EventFlagsLoader");
                            Log.Debug($"_______________", "EventFlagsLoader");

                            break;

                        case "Removed":
                            Log.Verbose($"Action: Removed", "EventFlagsLoader");
                            Log.Verbose($"Flag: {eventTypeField.Value}", "EventFlagsLoader");
                            Log.Verbose($"Flag starts at {flagStartTimeUtc}", "EventFlagsLoader");
                            Log.Verbose($"Flag ends at {flagEndTimeUtc}", "EventFlagsLoader");
                            Log.Verbose($"Flag starts in S{flagStartSeason.SeasonNumber}", "EventFlagsLoader");
                            Log.Verbose($"Flag ends in S{flagEndSeason.SeasonNumber}", "EventFlagsLoader");
                            Log.Verbose($"Used in: {flagSeasonsString}", "EventFlagsLoader");
                            Log.Verbose($"_______________", "EventFlagsLoader");

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
                            Log.Error($"Flag '{eventTypeField.Value}' has an unknown status ({statusField.Value})", "EventFlagsLoader");
                            Log.Debug($"_______________", "EventFlagsLoader");
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

            Log.Information("Loading data...", "HotfixLoader");

            string jsonData;
            try
            {
                jsonData = File.ReadAllText(Program.Configuration.HotfixDiscordChatHistoryFilePath);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while reading the hotfix chat history: " + ex.Message, "HotfixLoader");
                return;
            }

            if (string.IsNullOrWhiteSpace(jsonData))
            {
                Log.Error("An error occured while reading the hotfix chat history: File is empty", "HotfixLoader");
                return;
            }

            ChatHistory chatHistory;
            try
            {
                chatHistory = JsonDeserializer.Deserialize<ChatHistory>(jsonData);
            }
            catch (Exception ex)
            {
                Log.Error("An error occured while parsing the hotfix chat history: " + ex.Message, "HotfixLoader");
                return;
            }

            Log.Information($"Parsing hotfix data from '{chatHistory.Guild.Name} - {chatHistory.Channel.Name}'...", "HotfixLoader");

            foreach (Message msg in chatHistory.Messages)
            {
                if (msg.Attachments.Count < 1)
                {
                    continue;
                }

                foreach (Attachment attachment in msg.Attachments)
                {
                    Log.Debug($"Loading file '{attachment.Url}'", "HotfixLoader");

                    string hotFixFileContent = "";
                    try
                    {
                        hotFixFileContent = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Program.Configuration.HotfixDiscordChatHistoryFilePath), attachment.Url));
                    }
                    catch (Exception ex)
                    {
                        Log.Error("An error occured while parsing a hotfix file: " + ex.Message, "HotfixLoader");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(hotFixFileContent))
                    {
                        Log.Verbose("Skipping file, because it's empty", "HotfixLoader");
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
                            Log.Debug("This is the last category of this hotfix file.", "HotfixLoader");
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

                            Log.Debug($"Found variable: '{variableName}'", "HotfixLoader");

                            string variableValue = variableContent.Replace(variableNameMatch.Value, "");

                            Log.Debug($"Found variable value: '{variableValue}'", "HotfixLoader");

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

                                    Log.Debug($"Found variable param '{paramName}' with value of '{paramValue}'", "HotfixLoader");
                                }
                            }
                            else // Variable Value
                            {
                                Log.Debug($"Found variable value: '{variableValue}'", "HotfixLoader");
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
                Log.Error("Request to retrieve staging data was not successful!", "Calendar");
                return new Dictionary<string, Server>();
            }

            return JsonDeserializer.Deserialize<Dictionary<string, Server>>(response.Content);
        }
    }
}
