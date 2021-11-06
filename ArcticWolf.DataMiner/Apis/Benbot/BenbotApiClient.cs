using ArcticWolf.DataMiner.Apis.Benbot.Routes;
using ArcticWolf.DataMiner.Common.Http;
using ArcticWolf.DataMiner.Models.Apis.Benbot;
using ArcticWolf.DataMiner.Storage;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner.Apis.Benbot
{
    public class BenbotApiClient
    {
        private const string LOG_PREFIX = "BenbotApi";
        private const string STATUS_LOG_PREFIX = "BenbotApi|Status";
        private const string ANALYSER_LOG_PREFIX = "BenbotApi|Analyser";

        /// <summary>
        /// True if the cdn version doesn't match the benbot version after an update
        /// </summary>
        private bool isBenbotUpdatePending = false;

        private HttpClient _client;
        private static readonly WebHeaderCollection defaultHeaders = new()
        {
            { HttpRequestHeader.ContentType, "application/json" }
        };

        // Routes
        public GetAesKeys GetAesKeys;

        public BenbotApiClient()
        {
            Log.Information("Initialising...", LOG_PREFIX);
            _client = new HttpClient(defaultHeaders);

            GetAesKeys = new(_client);

            GetStatus();
        }

        public void GetStatus()
        {
            HttpResponse response = _client.Request("https://benbot.app/api/v1/status");

            if (!response.Success)
            {
                Log.Error("Request to retrieve status data was not successful!", LOG_PREFIX);
                return;
            }

            StatusResponse statusResponse = JsonConvert.DeserializeObject<StatusResponse>(response.Content);

            if (Program.Configuration.LastCheckedFnVersion != statusResponse.CurrentCdnVersionNumber || !Program.DbContext.FnVersions.Where(x => x.Version == statusResponse.CurrentFortniteVersionNumber).Any())
            {
                if (Program.Configuration.LastCheckedFnVersion == 0)
                {
                    Log.Warning("It seems like ArcticWolf DataMiner is runnning for the first time. Starting full checks for the current Fn version...", STATUS_LOG_PREFIX);
                }

                Log.Information($"Detected a new Fn version: {$"{Program.Configuration.LastCheckedFnVersion:F}"} -> {$"{statusResponse.CurrentCdnVersionNumber:F}"}");
                
                if (!Program.DbContext.FnVersions.Where(x => x.Version == statusResponse.CurrentCdnVersionNumber).Any())
                {
                    FnVersion newVersion = new();
                    newVersion.Version = statusResponse.CurrentCdnVersionNumber;
                    newVersion.VersionString = statusResponse.CurrentCdnVersion;
                    Program.DbContext.FnVersions.Add(newVersion);
                    Program.DbContext.SaveChanges();
                }
                else
                {
                    Log.Warning("The Fn version was found in the database. This might happen if the database was already used in another instance of the DataMiner.");
                }

                if (statusResponse.CurrentFortniteVersion != statusResponse.CurrentCdnVersion)
                {
                    isBenbotUpdatePending = true;
                }
                else
                {
                    AnalyseStatusResponse(statusResponse);
                    isBenbotUpdatePending = false;
                }

                Program.Configuration.LastCheckedFnVersion = statusResponse.CurrentCdnVersionNumber;
            }

            if (isBenbotUpdatePending)
            {
                if (statusResponse.CurrentFortniteVersion == statusResponse.CurrentCdnVersion)
                {
                    AnalyseStatusResponse(statusResponse);
                    isBenbotUpdatePending = false;
                }
            }

            Log.Information("Current FN CDN version is " + statusResponse.CurrentCdnVersion, LOG_PREFIX);
            Log.Information("Current FN version is " + statusResponse.CurrentCdnVersionNumber, LOG_PREFIX);
        }

        public static void AnalyseStatusResponse(StatusResponse response)
        {
            FnVersion currentVersion = Program.DbContext.FnVersions.Where(x => x.Version == response.CurrentFortniteVersionNumber).Include(x => x.PakFiles).First();

            // find already existing db pak files for current version
            ICollection<PakFile> dbPakFiles = currentVersion.PakFiles;

            foreach (string foundPakFile in response.AllPakFiles)
            {
                if (dbPakFiles.Where(x => x.File == foundPakFile).Any())
                {
                    Log.Debug($"(Pak): Skipping pak file '{foundPakFile}'. Reason: Already exists", ANALYSER_LOG_PREFIX);
                    continue;
                }

                Log.Debug($"(Pak): Adding pak file '{foundPakFile}' to '{currentVersion.Version:F}'...", ANALYSER_LOG_PREFIX);

                PakFile newPakFile = new();
                newPakFile.File = foundPakFile;
                newPakFile.FnVersion = currentVersion;

                Program.DbContext.PakFiles.Add(newPakFile);
            }
        }
    }
}
