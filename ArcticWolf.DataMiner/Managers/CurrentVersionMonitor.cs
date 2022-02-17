using ArcticWolf.Storage;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using ArcticWolf.DataMiner.Extensions;
using ArcticWolf.DataMiner.Models.Apis.Nitestats;
using Microsoft.EntityFrameworkCore;

namespace ArcticWolf.DataMiner.Managers
{
    public static class CurrentVersionMonitor
    {
        public const string AES_LOG_PREFIX = "AesAnalyser";

        private static Timer _updateAesTimer;
        private static Timer _updateStagingServersTimer;
        private static Timer _updateStatusTimer;

        public static void Init()
        {
            AnalyseAesForVersion(Program.Configuration.LastCheckedFnVersion);

            _updateAesTimer = new(_updateAesTimer_Elapsed, new AutoResetEvent(true), 0, 1000 * 10);

            _updateStagingServersTimer = new Timer(_updateStagingServersTimer_Elapsed, new AutoResetEvent(true), 0, 1000 * 10);

            _updateStatusTimer = new Timer(_updateStatusTimer_Elapsed, new AutoResetEvent(true), 0, 1000 * 10);

            Program.BenbotApiClient.NewUpdateAvailable += BenbotApiClient_NewUpdateAvailable;
        }

        private static void _updateStagingServersTimer_Elapsed(object state)
        {
            
            
            var response = Program.NitestatsApiClient.GetStagingServers.Request();
            FNitePlusBot.Cache.StagingServers = response;
        }
        
        private static CalendarResponse _cachedLastCalendarResponse = null;

        private static void ProcessFlagData()
        {
            var calendarResponse = Program.NitestatsApiClient.GetCalendarData.Request();

            var dbContext = Program.DbContext;
            
            dbContext.FnEventFlags.Load();

            if (_cachedLastCalendarResponse != null)
            {
                // do comparison
                var differences = calendarResponse.GetDifferences(_cachedLastCalendarResponse);
                foreach (var diff in differences)
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
                foreach (var currentState in calendarResponse.Channels.ClientEvents.States)
                {
                    foreach (var activeEvent in currentState.ActiveEvents)
                    {
                        var foundFlags = dbContext.FnEventFlags.Include(x => x.TimeSpans).Where(x => x.Event == activeEvent.EventType);

                        if (foundFlags.Any())
                        {
                            var flag = foundFlags.First();
                            // flag exists so check if it has been modified or readded

                            Log.Verbose($"Found flag {flag.Event}");
                            Log.Verbose($"Flag has {flag.TimeSpans.Count} time spans");

                            var foundTimeSpans = flag.TimeSpans.Where(x => x.StartTime == activeEvent.ActiveSince || x.EndTime == activeEvent.ActiveUntil);

                            if (foundTimeSpans.Any())
                            {
                                // time span was modified
                                // make sure to modify the time span that will end last
                                FnEventFlagTimeSpan timeSpan = foundTimeSpans.OrderByDescending(x => x.EndTime).First();

                                if (timeSpan.EndTime != activeEvent.ActiveUntil || timeSpan.StartTime != activeEvent.ActiveSince)
                                {
                                    Log.Information($"Flag {activeEvent.EventType}: Modified timespan");
                                    Log.Information($"Flag {activeEvent.EventType}: Start: {timeSpan.StartTime}' -> '{activeEvent.ActiveSince}'");
                                    Log.Information($"Flag {activeEvent.EventType}: End: {timeSpan.EndTime}' -> '{activeEvent.ActiveUntil}'");

                                    FnEventFlagModification modification = new()
                                    {
                                        ModifiedTimeSpan = timeSpan,
                                        NewStartTime = activeEvent.ActiveSince,
                                        NewEndTime = activeEvent.ActiveUntil,
                                        OverriddenStartTime = timeSpan.StartTime,
                                        OverriddenEndTime = timeSpan.EndTime
                                    };
                                    flag.Modifications.Add(modification);

                                    timeSpan.StartTime = activeEvent.ActiveSince;
                                    timeSpan.EndTime = activeEvent.ActiveUntil;
                                    continue;
                                }
                            }

                            // time span already exists
                            if (flag.TimeSpans.Any(x => x.StartTime == activeEvent.ActiveSince && x.EndTime == activeEvent.ActiveUntil))
                            {
                                continue;
                            }

                            // new time span was added
                            FnEventFlagTimeSpan newTimeSpan = new()
                            {
                                StartTime = activeEvent.ActiveSince,
                                EndTime = activeEvent.ActiveUntil
                            };
                            flag.TimeSpans.Add(newTimeSpan);
                            Log.Information($"Flag {activeEvent.EventType}: Added new time span: Start: {newTimeSpan.StartTime} | End: {newTimeSpan.EndTime}");
                        }
                        else
                        {
                            // flag doesn't exist so create it
                            Log.Information($"Adding new flag {activeEvent.EventType}");

                            FnEventFlag flag = new()
                            {
                                Event = activeEvent.EventType
                            };

                            FnEventFlagTimeSpan newTimeSpan = new()
                            {
                                StartTime = activeEvent.ActiveSince,
                                EndTime = activeEvent.ActiveUntil
                            };
                            flag.TimeSpans.Add(newTimeSpan);

                            dbContext.FnEventFlags.Add(flag);
                        }
                    }
                }

                _ = dbContext.SaveChanges();
            }
        }

        private static void _updateStatusTimer_Elapsed(object state)
        {
            Program.BenbotApiClient.GetStatus();
        }

        private static void BenbotApiClient_NewUpdateAvailable(object sender, Events.NewUpdateAvailableEventArgs e)
        {
            Log.Information("It seems that the Benbot API has the latest FN version. Trying to collect data...");

            AnalyseAesForVersion(e.UpdateVersion.Version);
        }

        private static void _updateAesTimer_Elapsed(object state)
        {
            AnalyseAesForVersion(Program.Configuration.LastCheckedFnVersion);
        }

        public static void AnalyseAesForVersion(decimal version)
        {
            var dbContext = Program.DbContext;

            Log.Verbose($"Analysing keys for v{version:F}", AES_LOG_PREFIX);

            var aesResponse = Program.BenbotApiClient.GetAesKeys.Request(version);

            if (aesResponse == null)
            {
                Log.Warning($"Failed to get aes data for v{version:F}", AES_LOG_PREFIX);
                return;
            }

            IEnumerable<FnVersion> foundVersions = dbContext.FnVersions.AsQueryable().Where(x => x.Version == aesResponse.VersionNumber);

            if (!foundVersions.Any())
            {
                Log.Warning("Oupsie. It seems that new AES keys are available for an updated version, which hasn't been detected yet! Aborting AES check...", AES_LOG_PREFIX);
                return;
            }

            var currentVersion = foundVersions.First();

            if (string.IsNullOrWhiteSpace(currentVersion.MainKey) && !string.IsNullOrWhiteSpace(aesResponse.MainKey))
            {
                currentVersion.MainKey = aesResponse.MainKey;
                Log.Information($"Set MainKey for '{currentVersion.Version:F}' to '{aesResponse.MainKey}'", AES_LOG_PREFIX);
            }

            dbContext.Entry(currentVersion).Collection(x => x.PakFiles).Load();

            foreach (var entry in aesResponse.DynamicKeys)
            {
                var pakFile = currentVersion.PakFiles.FirstOrDefault(x => x.File == entry.Key);

                if (pakFile == null)
                {
                    Log.Warning($"Pak '{entry.Key}' doesn't exist for v{currentVersion.Version:F}. Creating it...", AES_LOG_PREFIX);

                    PakFile newPakFile = new()
                    {
                        File = entry.Key,
                        FnVersion = currentVersion
                    };
                    currentVersion.PakFiles.Add(newPakFile);

                    pakFile = newPakFile;
                }

                if (pakFile.AesKey == entry.Value)
                {
                    Log.Verbose($"Skipping key for pak '{entry.Key}' in v{currentVersion.Version:F}. Reason: Key already exists", AES_LOG_PREFIX);
                    continue;
                }

                Log.Information($"Detected new key '{entry.Value}' for pak file '{entry.Key}' for v{currentVersion.Version:F}", AES_LOG_PREFIX);
                pakFile.AesKey = entry.Value;
            }

            dbContext.SaveChanges();
        }
    }
}
