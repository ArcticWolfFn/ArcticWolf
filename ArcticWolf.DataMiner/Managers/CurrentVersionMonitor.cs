using ArcticWolf.DataMiner.Models.Apis.Benbot;
using ArcticWolf.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ArcticWolf.DataMiner.Managers
{
    public static class CurrentVersionMonitor
    {
        private const string LOG_PREFIX = "CurrentVersionMonitor";
        public const string AES_LOG_PREFIX = "AesAnalyser";

        private static Timer _updateAesTimer = new(10 * 1000);
        private static Timer _updateStatusTimer = new(10 * 1000);

        public static void Init()
        {
            AnalyseAesForVersion(Program.Configuration.LastCheckedFnVersion);

            _updateAesTimer.AutoReset = true;
            _updateAesTimer.Elapsed += _updateAesTimer_Elapsed;
            _updateAesTimer.Start();

            _updateAesTimer.AutoReset = true;
            _updateAesTimer.Elapsed += _updateAesTimer_Elapsed;
            _updateAesTimer.Start();

            Program.BenbotApiClient.NewUpdateAvailable += BenbotApiClient_NewUpdateAvailable;
        }

        private static void BenbotApiClient_NewUpdateAvailable(object sender, Events.NewUpdateAvailableEventArgs e)
        {
            Log.Information("It seems that the Benbot API has the latest FN version. Trying to collect data...", LOG_PREFIX);

            AnalyseAesForVersion(e.UpdateVersion.Version);
        }

        private static void _updateAesTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            AnalyseAesForVersion(Program.Configuration.LastCheckedFnVersion);
        }

        public static void AnalyseAesForVersion(decimal version)
        {
            Log.Debug($"(Aes): Analsing keys for v{version:F}", AES_LOG_PREFIX);

            AesResponse aesResponse = Program.BenbotApiClient.GetAesKeys.Get(version.ToString());

            if (aesResponse == null)
            {
                Log.Warning($"(Aes): Failed to get aes data for v{version:F}", AES_LOG_PREFIX);
                return;
            }

            IEnumerable<FnVersion> foundVersions = Program.DbContext.FnVersions.AsQueryable().Where(x => x.Version == aesResponse.VersionNumber);

            if (!foundVersions.Any())
            {
                Log.Warning("Oupsie. It seems that new AES keys are available for an updated version, which hasn't been detected yet! Aborting AES check...", AES_LOG_PREFIX);
                return;
            }

            FnVersion currentVersion = foundVersions.First();

            if (string.IsNullOrWhiteSpace(currentVersion.MainKey) && !string.IsNullOrWhiteSpace(aesResponse.MainKey))
            {
                currentVersion.MainKey = aesResponse.MainKey;
                Log.Information($"(Aes): Set MainKey for '{currentVersion.Version:F}' to '{aesResponse.MainKey}'", AES_LOG_PREFIX);
            }

            Program.DbContext.Entry(currentVersion).Collection(x => x.PakFiles).Load();

            foreach (KeyValuePair<string, string> entry in aesResponse.DynamicKeys)
            {
                PakFile pakFile = currentVersion.PakFiles.Where(x => x.File == entry.Key).FirstOrDefault();

                if (pakFile == null)
                {
                    Log.Warning($"(Aes): Pak '{entry.Key}' doesn't exist for v{currentVersion.Version:F}. Creating it...", AES_LOG_PREFIX);

                    PakFile newPakFile = new();
                    newPakFile.File = entry.Key;
                    newPakFile.FnVersion = currentVersion;
                    currentVersion.PakFiles.Add(newPakFile);

                    pakFile = newPakFile;
                }

                if (pakFile.AesKey == entry.Value)
                {
                    Log.Verbose($"(Aes): Skipping key for pak '{entry.Key}' in v{currentVersion.Version:F}. Reason: Key already exists", AES_LOG_PREFIX);
                    continue;
                }

                Log.Information($"(Aes): Detected new key '{entry.Value}' for pak file '{entry.Key}' for v{currentVersion.Version:F}", AES_LOG_PREFIX);
                pakFile.AesKey = entry.Value;
            }
        }
    }
}
