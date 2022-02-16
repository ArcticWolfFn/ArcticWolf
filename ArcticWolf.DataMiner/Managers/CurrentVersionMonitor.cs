using ArcticWolf.Storage;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

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
            var response = Program.NitestatsApiClient.GetStagingServers();
            FNitePlusBot.Cache.StagingServers = response;
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

            var aesResponse = Program.BenbotApiClient.GetAesKeys.Get(version.ToString(CultureInfo.InvariantCulture));

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

            FnVersion currentVersion = foundVersions.First();

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
