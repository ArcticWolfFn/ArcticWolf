using ArcticWolf.DataMiner.Constants;
using ArcticWolf.DataMiner.Models.Apis.Benbot;
using ArcticWolf.DataMiner.Storage;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ArcticWolf.DataMiner.Managers
{
    public static class OldVersionsManager
    {
        private const string LOG_PREFIX = "OldVersionsManager";

        public static void Init()
        {
            Thread analyseOldVersionThread = new Thread(AnalyseOldVersions);
            analyseOldVersionThread.Start();
        }

        public static void AnalyseOldVersions()
        {
            Log.Information("(EventFlags): Trying to retrive old event flags...");
            Program.NitestatsApiClient.LoadEventFlagsFromMessages();

            Log.Information("(AnalyseVersions): Starting analytics for older versions...");
            foreach (decimal version in Fortnite.Versions)
            {
                AnalyseVersion(version);
            }
        }

        public static void AnalyseVersion(decimal version)
        {
            IEnumerable<FnVersion> foundVersions = Program.DbContext.FnVersions.Where(x => x.Version == version);

            if (!foundVersions.Any())
            {
                AesResponse aesResponse = Program.BenbotApiClient.GetAesKeys.Get(version.ToString());

                if (aesResponse == null)
                {
                    Log.Error($"(AnalyseVersion): Can't get data for v{version:F}", LOG_PREFIX);
                    return;
                }

                FnVersion newVersion = new();
                newVersion.Version = aesResponse.VersionNumber;
                newVersion.VersionString = aesResponse.Version;
                Program.DbContext.FnVersions.Add(newVersion);
                Program.DbContext.SaveChanges();
            }

            AesManager.AnalyseAesForVersion(version);
        }
    }
}
