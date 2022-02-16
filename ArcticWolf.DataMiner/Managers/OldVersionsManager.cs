using ArcticWolf.DataMiner.Models.Apis.Benbot;
using ArcticWolf.Storage;
using ArcticWolf.Storage.Constants;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

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
                Program.NitestatsApiClient.LoadEventFlagsFromMessages();
            }

            Log.Information("Trying to retrieve hotfix data...");
            Program.NitestatsApiClient.LoadHotFixesFromMessages();

            Log.Information("Starting analytics for older versions...");
            foreach (decimal version in Fortnite.Versions)
            {
                AnalyseVersion(version);
            }

            Log.Information("Finished analysing older versions!");
        }

        private static void AnalyseVersion(decimal version)
        {
            DatabaseContext dbContext = Program.DbContext;

            IEnumerable<FnVersion> foundVersions = dbContext.FnVersions.AsQueryable().Where(x => x.Version == version);

            if (!foundVersions.Any())
            {
                AesResponse aesResponse = Program.BenbotApiClient.GetAesKeys.Get(version.ToString(CultureInfo.InvariantCulture));

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
