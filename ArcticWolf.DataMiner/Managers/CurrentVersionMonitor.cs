using ArcticWolf.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using ArcticWolf.Apis.BenBot.Models;
using ArcticWolf.Apis.NiteStats.Models;
using ArcticWolf.DataMiner.Events;
using ArcticWolf.DataMiner.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ArcticWolf.DataMiner.Managers
{
    public static class CurrentVersionMonitor
    {
        public const string AES_LOG_PREFIX = "AesAnalyser";

        private static Timer _updateAesTimer;
        private static Timer _updateStagingServersTimer;
        private static Timer _updateStatusTimer;
        
        /// <summary>
        /// True if the cdn version doesn't match the benbot version after an update
        /// </summary>
        private static bool _isBenBotUpdatePending = false;

        private static bool _isNewUpdateAvailable = false;
        
        public delegate void NewUpdateAvailableEventHandler(object sender, NewUpdateAvailableEventArgs e);

        public static event NewUpdateAvailableEventHandler NewUpdateAvailable;

        public static void Init()
        {
            AnalyseAesForVersion(Program.Configuration.LastCheckedFnVersion);
            
            _updateAesTimer = new Timer(1000 * 10);
            _updateAesTimer.Elapsed += _updateAesTimer_Elapsed;

            _updateStagingServersTimer = new Timer(1000 * 10);
            _updateStagingServersTimer.Elapsed += _updateStagingServersTimer_Elapsed;

            _updateStatusTimer = new Timer(1000 * 10);
            _updateStatusTimer.Elapsed += _updateStatusTimer_Elapsed;

            _updateAesTimer.Start();
            _updateStagingServersTimer.Start();
            _updateStatusTimer.Start();

            NewUpdateAvailable += BenBotApiClient_NewUpdateAvailable;
        }

        private static void _updateStagingServersTimer_Elapsed(object sender , ElapsedEventArgs args)
        {
            var response = Program.NiteStatsApiClient.GetStagingServers.Request();
            FNitePlusBot.Cache.StagingServers = response;
        }
        
        private static CalendarResponse _cachedLastCalendarResponse = null;

        private static void ProcessFlagData()
        {
            var calendarResponse = Program.NiteStatsApiClient.GetCalendarData.Request();

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

        private static void _updateStatusTimer_Elapsed(object sender , ElapsedEventArgs args)
        {
            var dbContext = Program.DbContext;

            var statusResponse = Program.BenbotApiClient.GetStatus.Request();

            if (statusResponse == null)
            {
                Log.Error("Status response was null.");
                return;
            }

            if (Program.Configuration.LastCheckedFnVersion != statusResponse.CurrentCdnVersionNumber || !dbContext.FnVersions.Any(x => x.Version == statusResponse.CurrentFortniteVersionNumber))
            {
                if (Program.Configuration.LastCheckedFnVersion == 0)
                {
                    Log.Warning("It seems like ArcticWolf DataMiner is running for the first time. Starting full checks for the current Fn version...", "Status");
                }

                Log.Information($"Detected a new Fn version: {Program.Configuration.LastCheckedFnVersion:F} -> {statusResponse.CurrentCdnVersionNumber:F}");

                if (!dbContext.FnVersions.Any(x => x.Version == statusResponse.CurrentCdnVersionNumber))
                {
                    FnVersion newVersion = new()
                    {
                        Version = statusResponse.CurrentCdnVersionNumber,
                        VersionString = statusResponse.CurrentCdnVersion
                    };
                    dbContext.FnVersions.Add(newVersion);
                    dbContext.SaveChanges();

                    _isNewUpdateAvailable = true;
                }
                else
                {
                    Log.Warning("The Fn version was found in the database. This might happen if the database was already used in another instance of the DataMiner.");
                }

                if (statusResponse.CurrentFortniteVersion != statusResponse.CurrentCdnVersion)
                {
                    _isBenBotUpdatePending = true;
                }
                else
                {
                    AnalyseStatusResponse(statusResponse, dbContext);
                    _isBenBotUpdatePending = false;

                    if (_isNewUpdateAvailable)
                    {
                        // can be null if nobody subscribed to the event
                        NewUpdateAvailable?.Invoke(null, new NewUpdateAvailableEventArgs()
                        {
                            UpdateVersion = dbContext.FnVersions.First(x => x.Version == statusResponse.CurrentCdnVersionNumber),
                        });
                        _isNewUpdateAvailable = false;
                    }
                }

                Program.Configuration.LastCheckedFnVersion = statusResponse.CurrentCdnVersionNumber;
            }

            if (_isBenBotUpdatePending)
            {
                if (statusResponse.CurrentFortniteVersion == statusResponse.CurrentCdnVersion)
                {
                    AnalyseStatusResponse(statusResponse, dbContext);
                    _isBenBotUpdatePending = false;

                    if (_isNewUpdateAvailable)
                    {
                        NewUpdateAvailable?.Invoke(null, new NewUpdateAvailableEventArgs()
                        {
                            UpdateVersion = dbContext.FnVersions.First(x =>
                                x.Version == statusResponse.CurrentCdnVersionNumber),
                        });
                        _isNewUpdateAvailable = false;
                    }
                }
            }
            //
            // return statusResponse;
        }

        private static void BenBotApiClient_NewUpdateAvailable(object sender, Events.NewUpdateAvailableEventArgs e)
        {
            Log.Information("It seems that the BenBot API has the latest FN version. Trying to collect data...");

            AnalyseAesForVersion(e.UpdateVersion.Version);
        }

        private static void _updateAesTimer_Elapsed(object sender , ElapsedEventArgs args)
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

        private static void AnalyseStatusResponse(StatusResponse response, DatabaseContext dbContext)
        {
            var currentVersion = dbContext.FnVersions.Include(x => x.PakFiles).First(x => x.Version == response.CurrentFortniteVersionNumber);

            // find already existing db pak files for current version
            var dbPakFiles = currentVersion.PakFiles;

            foreach (var foundPakFile in response.AllPakFiles)
            {
                if (dbPakFiles.Any(x => x.File == foundPakFile))
                {
                    Log.Debug($"(Pak): Skipping pak file '{foundPakFile}'. Reason: Already exists", "Analyser");
                    continue;
                }

                Log.Debug($"(Pak): Adding pak file '{foundPakFile}' to '{currentVersion.Version:F}'...", "Analyser");

                PakFile newPakFile = new()
                {
                    File = foundPakFile,
                    FnVersion = currentVersion
                };

                dbContext.PakFiles.Add(newPakFile);
            }
        }
    }
}
