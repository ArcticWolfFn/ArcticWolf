using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ArcticWolf.Apis.Base.Common.Json;
using ArcticWolf.DataMiner.Models.Discord;
using ArcticWolf.Storage;
using ArcticWolf.Storage.Constants;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ArcticWolf.DataMiner.Managers;

public class EventFlagsManager
{
    private const string EventFlagsDiscordBot = "eventflag-updates";

    public static void LoadEventFlagsFromMessages()
    {
        var dbContext = Program.DbContext;

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

        if (chatHistory == null)
        {
            Log.Warning("No chat history available.", "EventFlagsLoader");
            return;
        }

        Log.Information($"Parsing event flags from '{chatHistory.Guild.Name} - {chatHistory.Channel.Name}'...",
            "EventFlagsLoader");

        dbContext.FnEventFlags.Load();

        foreach (var message in chatHistory.Messages)
        {
            Log.Verbose($"Analysing message from {message.Author.Name} at {message.Timestamp.ToUniversalTime()}...",
                "EventFlagsLoader");

            // validate message
            if (message.Author.Name != EventFlagsDiscordBot || !message.Embeds.Any())
            {
                Log.Verbose($"Message invalid! Reason: Wrong author name or no embeds.", "EventFlagsLoader");
                continue;
            }

            foreach (var embed in message.Embeds)
            {
                var eventTypeField = embed.Fields.Find(x => x.Name.Contains("EventType"));
                var startsField = embed.Fields.Find(x => x.Name.Contains("Starts"));
                var endsField = embed.Fields.Find(x => x.Name.Contains("Ends"));
                var statusField = embed.Fields.Find(x => x.Name.Contains("Status"));

                // validate embed
                if (eventTypeField == null || startsField == null || endsField == null || statusField == null)
                {
                    Log.Verbose($"Message invalid! Reason: One or more fields are empty.", "EventFlagsLoader");
                    Log.Verbose($"Fields data for previous error: " + JsonConvert.SerializeObject(embed.Fields),
                        "EventFlagsLoader");
                    continue;
                }

                // get the overridden times
                var overriddenStartsField = Regex.Match(startsField.Value, "~~.*~~").Value;
                var overriddenEndsField = Regex.Match(endsField.Value, "~~.*~~").Value;

                // fix DateTime parsing error due to Discord text styling
                startsField.Value = Regex.Replace(startsField.Value, "~~.*~~", "");
                endsField.Value = Regex.Replace(endsField.Value, "~~.*~~", "");

                // just remove useless chars
                eventTypeField.Value = eventTypeField.Value.Trim();
                statusField.Value = statusField.Value.Trim();

                DateTime overriddenFlagStartTimeUtc = new();
                DateTime overriddenFlagEndTimeUtc = new();
                DateTime flagStartTimeUtc;
                DateTime flagEndTimeUtc;

                try
                {
                    // appending " Z" tells the parser that this is a utc time
                    flagStartTimeUtc = DateTime.Parse(startsField.Value + " Z", CultureInfo.InvariantCulture)
                        .ToUniversalTime();
                    flagEndTimeUtc = DateTime.Parse(endsField.Value + " Z", CultureInfo.InvariantCulture)
                        .ToUniversalTime();

                    if (!string.IsNullOrWhiteSpace(overriddenStartsField))
                    {
                        overriddenFlagStartTimeUtc = DateTime
                            .Parse(endsField.Value + " Z", CultureInfo.InvariantCulture).ToUniversalTime();
                    }

                    if (!string.IsNullOrWhiteSpace(overriddenEndsField))
                    {
                        overriddenFlagEndTimeUtc = DateTime.Parse(endsField.Value + " Z", CultureInfo.InvariantCulture)
                            .ToUniversalTime();
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Start or end time is invalid! Error: " + ex.Message, "EventFlagsLoader");
                    Log.Error($"Fields data for previous error: " + JsonConvert.SerializeObject(embed.Fields),
                        "EventFlagsLoader");
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
                    eventFlag = new()
                    {
                        Event = eventTypeField.Value
                    };
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

                var flagSeasons = Fortnite.Seasons.Where(x =>
                    // starts in season
                    (x.StartTime <= flagStartTimeUtc && x.EndTime >= flagStartTimeUtc) ||

                    // ends in season
                    (x.StartTime <= flagEndTimeUtc && x.EndTime >= flagEndTimeUtc) ||

                    // during season
                    (x.StartTime >= flagStartTimeUtc && x.EndTime <= flagStartTimeUtc)
                );

                var flagSeasonsString =
                    flagSeasons.Aggregate("", (current, season) => current + $"S{season.SeasonNumber}, ");

                switch (statusField.Value.Trim())
                {
                    case "Added":
                        if (eventFlag.TimeSpans.Any(x =>
                                x.StartTime == flagStartTimeUtc && x.EndTime == flagEndTimeUtc))
                        {
                            Log.Verbose("TimeSpan already exists", "EventFlagsLoader");
                            Log.Debug($"_______________", "EventFlagsLoader");
                            break;
                        }

                        // too many time spans (adding time spans, which already have been modified)
                        if (eventFlag.TimeSpans.Any(x =>
                                x.StartTime == flagStartTimeUtc && x.EndTime == flagEndTimeUtc))
                        {
                            break;
                        }

                        FnEventFlagTimeSpan timeSpan = new()
                        {
                            StartTime = flagStartTimeUtc,
                            EndTime = flagEndTimeUtc
                        };
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
                            Log.Fatal(
                                $"Well... That shouldn't happen. The overridden times have default values even though the flag has been modified.",
                                "EventFlagsLoader");
                            Log.Error($"Fields data for previous error: " + JsonConvert.SerializeObject(embed.Fields),
                                "EventFlagsLoader");
                            Log.Error($"Flag update from " + message.Timestamp, "EventFlagsLoader");
                            Log.Debug($"_______________", "EventFlagsLoader");
                            break;
                        }

                        // check if modification already exists
                        if (eventFlag.Modifications.Any(x => x.OverriddenStartTime == overriddenFlagStartTimeUtc &&
                                                             x.OverriddenEndTime == overriddenFlagEndTimeUtc
                                                             && x.NewStartTime == flagStartTimeUtc &&
                                                             x.NewEndTime == flagEndTimeUtc))
                        {
                            Log.Debug($"_______________", "EventFlagsLoader");
                            break;
                        }

                        if (eventFlag.TimeSpans.Any(x =>
                                x.StartTime == overriddenFlagStartTimeUtc && x.EndTime == overriddenFlagEndTimeUtc))
                        {
                            timeSpan = eventFlag.TimeSpans.First(x =>
                                x.StartTime == overriddenFlagStartTimeUtc && x.EndTime == overriddenFlagEndTimeUtc);

                            FnEventFlagModification modification = new()
                            {
                                ModifiedTimeSpan = timeSpan,
                                OverriddenStartTime = overriddenFlagStartTimeUtc,
                                OverriddenEndTime = overriddenFlagEndTimeUtc,
                                NewStartTime = flagStartTimeUtc,
                                NewEndTime = flagEndTimeUtc
                            };
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
                            if (eventFlag.TimeSpans.Any(x =>
                                    x.StartTime == flagStartTimeUtc && x.EndTime == flagEndTimeUtc))
                            {
                                Log.Verbose("TimeSpan already exists", "EventFlagsLoader");
                                Log.Debug($"_______________", "EventFlagsLoader");
                                break;
                            }

                            timeSpan = new()
                            {
                                StartTime = flagStartTimeUtc,
                                EndTime = flagEndTimeUtc
                            };
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
                        if (!eventFlag.TimeSpans.Any(
                                x => x.StartTime == flagStartTimeUtc && x.EndTime == flagEndTimeUtc))
                        {
                            timeSpan = new()
                            {
                                StartTime = flagStartTimeUtc,
                                EndTime = flagEndTimeUtc
                            };
                            eventFlag.TimeSpans.Add(timeSpan);
                        }

                        // if happened on patch day of new season and already exists, then don't count it to the new season
                        break;

                    default:
                        Log.Error($"Flag '{eventTypeField.Value}' has an unknown status ({statusField.Value})",
                            "EventFlagsLoader");
                        Log.Debug($"_______________", "EventFlagsLoader");
                        break;
                }
            }
        }

        var modifiedItemsCount = dbContext.ChangeTracker.Entries().Count(x =>
            x.State == EntityState.Modified && x.Entity.GetType().Name == nameof(FnEventFlag));
        var addedItemsCount = dbContext.ChangeTracker.Entries().Count(x =>
            x.State == EntityState.Added && x.Entity.GetType().Name == nameof(FnEventFlag));

        Log.Information($"(EventFlagsLoader): Added {addedItemsCount} items and modified {modifiedItemsCount} items.");

        var modifiedTsItemsCount = dbContext.ChangeTracker.Entries().Count(x =>
            x.State == EntityState.Modified && x.Entity.GetType().Name == nameof(FnEventFlagTimeSpan));
        var addedTsItemsCount = dbContext.ChangeTracker.Entries().Count(x =>
            x.State == EntityState.Added && x.Entity.GetType().Name == nameof(FnEventFlagTimeSpan));

        Log.Information(
            $"(EventFlagsLoader): Added {addedTsItemsCount} TS items and modified {modifiedTsItemsCount} TS items.");

        dbContext.SaveChanges();

        // ToDo: do validation: No duplicate time spans for each flag
    }
}