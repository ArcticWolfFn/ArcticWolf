using ArcticWolf.Apis.Base;
using ArcticWolf.Apis.Benbot.Routes;
using System.Net;

namespace ArcticWolf.Apis.Benbot
{
    public class BenbotApiClient : IApiClient
    {
        /// <summary>
        /// True if the cdn version doesn't match the benbot version after an update
        /// </summary>
        private bool isBenbotUpdatePending = false;

        private bool isNewUpdateAvailable = false;

        public string ServerUrl => "https://benbot.app";

        private readonly Base.Common.Http.HttpClient _client;
        private static readonly WebHeaderCollection defaultHeaders = new()
        {
            { HttpRequestHeader.ContentType, "application/json" }
        };

        // Routes
        public GetAesKeysRoute GetAesKeys;
        public GetStatusRoute GetStatus;

        //public delegate void NewUpdateAvailableEventHandler(object sender, NewUpdateAvailableEventArgs e);

        //public event NewUpdateAvailableEventHandler NewUpdateAvailable;

        public BenbotApiClient()
        {
            Log.Information("Initialising...");

            _client = new(defaultHeaders);

            GetAesKeys = new(this, _client);
            GetStatus = new(this, _client);
        }

        //public StatusResponse ProcessStatus()
        //{
        //    DatabaseContext dbContext = Program.DbContext;

        //    StatusResponse statusResponse = new();

        //    if (Program.Configuration.LastCheckedFnVersion != statusResponse.CurrentCdnVersionNumber || !dbContext.FnVersions.Any(x => x.Version == statusResponse.CurrentFortniteVersionNumber))
        //    {
        //        if (Program.Configuration.LastCheckedFnVersion == 0)
        //        {
        //            Log.Warning("It seems like ArcticWolf DataMiner is runnning for the first time. Starting full checks for the current Fn version...", "Status");
        //        }

        //        Log.Information($"Detected a new Fn version: {$"{Program.Configuration.LastCheckedFnVersion:F}"} -> {$"{statusResponse.CurrentCdnVersionNumber:F}"}");

        //        if (!dbContext.FnVersions.Any(x => x.Version == statusResponse.CurrentCdnVersionNumber))
        //        {
        //            FnVersion newVersion = new();
        //            newVersion.Version = statusResponse.CurrentCdnVersionNumber;
        //            newVersion.VersionString = statusResponse.CurrentCdnVersion;
        //            dbContext.FnVersions.Add(newVersion);
        //            dbContext.SaveChanges();

        //            isNewUpdateAvailable = true;
        //        }
        //        else
        //        {
        //            Log.Warning("The Fn version was found in the database. This might happen if the database was already used in another instance of the DataMiner.");
        //        }

        //        if (statusResponse.CurrentFortniteVersion != statusResponse.CurrentCdnVersion)
        //        {
        //            isBenbotUpdatePending = true;
        //        }
        //        else
        //        {
        //            AnalyseStatusResponse(statusResponse, dbContext);
        //            isBenbotUpdatePending = false;

        //            if (isNewUpdateAvailable)
        //            {
        //                // can be null if nobody subscribed to the event
        //                NewUpdateAvailable?.Invoke(this, new NewUpdateAvailableEventArgs()
        //                {
        //                    UpdateVersion = dbContext.FnVersions.First(x => x.Version == statusResponse.CurrentCdnVersionNumber),
        //                });
        //                isNewUpdateAvailable = false;
        //            }
        //        }

        //        Program.Configuration.LastCheckedFnVersion = statusResponse.CurrentCdnVersionNumber;
        //    }

        //    if (isBenbotUpdatePending)
        //    {
        //        if (statusResponse.CurrentFortniteVersion == statusResponse.CurrentCdnVersion)
        //        {
        //            AnalyseStatusResponse(statusResponse, dbContext);
        //            isBenbotUpdatePending = false;

        //            if (isNewUpdateAvailable)
        //            {
        //                NewUpdateAvailable(this, new NewUpdateAvailableEventArgs()
        //                {
        //                    UpdateVersion = dbContext.FnVersions.First(x => x.Version == statusResponse.CurrentCdnVersionNumber),
        //                });
        //                isNewUpdateAvailable = false;
        //            }
        //        }
        //    }

        //    return statusResponse;
        //}

        //private static void AnalyseStatusResponse(StatusResponse response, DatabaseContext dbContext)
        //{
        //    FnVersion currentVersion = dbContext.FnVersions.Include(x => x.PakFiles).First(x => x.Version == response.CurrentFortniteVersionNumber);

        //    // find already existing db pak files for current version
        //    ICollection<PakFile> dbPakFiles = currentVersion.PakFiles;

        //    foreach (string foundPakFile in response.AllPakFiles)
        //    {
        //        if (dbPakFiles.Where(x => x.File == foundPakFile).Any())
        //        {
        //            Log.Debug($"(Pak): Skipping pak file '{foundPakFile}'. Reason: Already exists", "Analyser");
        //            continue;
        //        }

        //        Log.Debug($"(Pak): Adding pak file '{foundPakFile}' to '{currentVersion.Version:F}'...", "Analyser");

        //        PakFile newPakFile = new();
        //        newPakFile.File = foundPakFile;
        //        newPakFile.FnVersion = currentVersion;

        //        dbContext.PakFiles.Add(newPakFile);
        //    }
        //}
    }
}
