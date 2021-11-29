using ArcticWolf.DataMiner.Apis.Benbot.Routes;
using ArcticWolf.DataMiner.Common.Http;
using ArcticWolf.DataMiner.Common.Json;
using ArcticWolf.DataMiner.Events;
using ArcticWolf.DataMiner.Models.Apis.Benbot;
using ArcticWolf.Storage;
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

        private bool isNewUpdateAvailable = false;

        private HttpClient _client;
        private static readonly WebHeaderCollection defaultHeaders = new()
        {
            { HttpRequestHeader.ContentType, "application/json" }
        };

        // Routes
        public GetAesKeys GetAesKeys;

        public delegate void NewUpdateAvailableEventHandler(object sender, NewUpdateAvailableEventArgs e);

        public event NewUpdateAvailableEventHandler NewUpdateAvailable;

        public BenbotApiClient()
        {
            Log.Information("Initialising...", LOG_PREFIX);
            _client = new HttpClient(defaultHeaders);

            GetAesKeys = new(_client);

            /*StatusResponse statusResponse = GetStatus();

            if (statusResponse != null)
            {
                Log.Information("Current FN CDN version is " + statusResponse.CurrentCdnVersion, LOG_PREFIX);
                Log.Information("Current FN version is " + statusResponse.CurrentCdnVersionNumber, LOG_PREFIX);
            }*/
        }

        public StatusResponse GetStatus()
        {
            DatabaseContext dbContext = Program.DbContext;

            HttpResponse response = _client.Request("https://benbot.app/api/v1/status");

            if (!response.Success)
            {
                Log.Error("Request to retrieve status data was not successful!", LOG_PREFIX);
                return null;
            }

            StatusResponse statusResponse = JsonDeserializer.Deserialize<StatusResponse>(response.Content);

            if (Program.Configuration.LastCheckedFnVersion != statusResponse.CurrentCdnVersionNumber || !dbContext.FnVersions.Any(x => x.Version == statusResponse.CurrentFortniteVersionNumber))
            {
                if (Program.Configuration.LastCheckedFnVersion == 0)
                {
                    Log.Warning("It seems like ArcticWolf DataMiner is runnning for the first time. Starting full checks for the current Fn version...", STATUS_LOG_PREFIX);
                }

                Log.Information($"Detected a new Fn version: {$"{Program.Configuration.LastCheckedFnVersion:F}"} -> {$"{statusResponse.CurrentCdnVersionNumber:F}"}");

                if (!dbContext.FnVersions.Any(x => x.Version == statusResponse.CurrentCdnVersionNumber))
                {
                    FnVersion newVersion = new();
                    newVersion.Version = statusResponse.CurrentCdnVersionNumber;
                    newVersion.VersionString = statusResponse.CurrentCdnVersion;
                    dbContext.FnVersions.Add(newVersion);
                    dbContext.SaveChanges();

                    isNewUpdateAvailable = true;
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
                    AnalyseStatusResponse(statusResponse, dbContext);
                    isBenbotUpdatePending = false;

                    if (isNewUpdateAvailable)
                    {
                        // can be null if nobody subscribed to the event
                        NewUpdateAvailable?.Invoke(this, new NewUpdateAvailableEventArgs()
                        {
                            UpdateVersion = dbContext.FnVersions.First(x => x.Version == statusResponse.CurrentCdnVersionNumber),
                        });
                        isNewUpdateAvailable = false;
                    }
                }

                Program.Configuration.LastCheckedFnVersion = statusResponse.CurrentCdnVersionNumber;
            }

            if (isBenbotUpdatePending)
            {
                if (statusResponse.CurrentFortniteVersion == statusResponse.CurrentCdnVersion)
                {
                    AnalyseStatusResponse(statusResponse, dbContext);
                    isBenbotUpdatePending = false;

                    if (isNewUpdateAvailable)
                    {
                        NewUpdateAvailable(this, new NewUpdateAvailableEventArgs()
                        {
                            UpdateVersion = dbContext.FnVersions.First(x => x.Version == statusResponse.CurrentCdnVersionNumber),
                        });
                        isNewUpdateAvailable = false;
                    }
                }
            }

            return statusResponse;
        }

        private static void AnalyseStatusResponse(StatusResponse response, DatabaseContext dbContext)
        {
            FnVersion currentVersion = dbContext.FnVersions.Include(x => x.PakFiles).First(x => x.Version == response.CurrentFortniteVersionNumber);

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

                dbContext.PakFiles.Add(newPakFile);
            }
        }
    }
}
