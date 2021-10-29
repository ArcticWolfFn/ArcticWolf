using ArcticWolf.DataMiner.Models.Apis.Benbot;
using ArcticWolf.DataMiner.Storage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ArcticWolf.DataMiner.Managers
{
    public static class AesManager
    {
        private const string LOG_PREFIX = "AesManager";

        private static Timer _updateAesTimer = new Timer(10*1000);

        public static void Init()
        {
            AnalyseAesForVersion(Program.Configuration.LastCheckedFnVersion);

            _updateAesTimer.AutoReset = true;
            _updateAesTimer.Elapsed += _updateAesTimer_Elapsed;
            _updateAesTimer.Start();
        }

        private static void _updateAesTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            AnalyseAesForVersion(Program.Configuration.LastCheckedFnVersion);
        }

        public static void AnalyseAesForVersion(decimal version)
        {
            AesResponse aesResponse = Program.BenbotApiClient.GetAesKeys.Get(version.ToString());

            if (aesResponse == null)
            {
                return;
            }

            FnVersion currentVersion = Program.DbContext.FnVersions.Where(x => x.Version == aesResponse.VersionNumber).First();

            if (string.IsNullOrWhiteSpace(currentVersion.MainKey) && !string.IsNullOrWhiteSpace(aesResponse.MainKey))
            {
                currentVersion.MainKey = aesResponse.MainKey;
                Log.Debug($"(Aes): Set MainKey for '{currentVersion.Version:F}' to '{aesResponse.MainKey}'", LOG_PREFIX);
            }

            Program.DbContext.Entry(currentVersion).Collection(x => x.PakFiles).Load();

            foreach (KeyValuePair<string, string> entry in aesResponse.DynamicKeys)
            {
                PakFile pakFile = currentVersion.PakFiles.Where(x => x.File == entry.Key).FirstOrDefault();

                if (pakFile == null)
                {
                    Log.Warning($"(Aes): Pak '{entry.Key}' doesn't exist for v{currentVersion.Version:F}. Creating it...", LOG_PREFIX);

                    PakFile newPakFile = new();
                    newPakFile.File = entry.Key;
                    newPakFile.FnVersion = currentVersion;
                    currentVersion.PakFiles.Add(newPakFile);

                    pakFile = newPakFile;
                }

                if (pakFile.AesKey == entry.Value)
                {
                    Log.Verbose($"(Aes): Skipping key for pak '{entry.Key}' in v{currentVersion.Version:F}. Reason: Key already exists", LOG_PREFIX);
                    continue;
                }

                Log.Information($"(Aes): Detected new key '{entry.Value}' for pak file '{entry.Key}' for v{currentVersion.Version:F}", LOG_PREFIX);
                pakFile.AesKey = entry.Value;
            }
        }
    }
}
