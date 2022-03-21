using ArcticWolf.Storage;
using ArcticWolf.Storage.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ArcticWolf.Apis.BenBot.Models;

namespace ArcticWolf.DataMiner.Managers
{
    public static class OldVersionsManager
    {
        public static void Init()
        {
            Thread analyseOldVersionThread = new(AnalyseOldVersions);
            analyseOldVersionThread.Start();
        }

        private static void AnalyseOldVersions()
        {
            if (!string.IsNullOrWhiteSpace(Program.Configuration.EventFlagsDiscordChatHistoryFilePath))
            {
                Log.Information("Trying to retrieve old event flags...");
                EventFlagsManager.LoadEventFlagsFromMessages();
            }

            Log.Information("Trying to retrieve hotfix data...");
            HotFixManager.LoadHotFixesFromMessages();

            Log.Information("Starting analytics for older versions...");
            foreach (var version in Fortnite.Versions)
            {
                AnalyseVersion(version);
            }

            Log.Information("Finished analysing older versions!");
        }

        private static void AnalyseVersion(decimal version)
        {
            var dbContext = Program.DbContext;

            IEnumerable<FnVersion> foundVersions = dbContext.FnVersions.AsQueryable().Where(x => x.Version == version);

            if (!foundVersions.Any())
            {
                var aesResponse = Program.BenbotApiClient.GetAesKeys.Request(version);

                if (aesResponse == null)
                {
                    Log.Error($"Can't get data for v{version:F}", "AnalyseVersion");
                    return;
                }

                FnVersion newVersion = new()
                {
                    Version = aesResponse.VersionNumber,
                    VersionString = aesResponse.Version
                };
                _ = dbContext.FnVersions.Add(newVersion);
                _ = dbContext.SaveChanges();
            }

            CurrentVersionMonitor.AnalyseAesForVersion(version);
        }
    }
}
