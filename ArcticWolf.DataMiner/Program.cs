using ArcticWolf.DataMiner.Apis.Benbot;
using ArcticWolf.DataMiner.Apis.FnDotNet;
using ArcticWolf.DataMiner.Apis.Nitestats;
using ArcticWolf.DataMiner.Common.Http;
using ArcticWolf.DataMiner.Managers;
using ArcticWolf.DataMiner.Models;
using ArcticWolf.Storage;
using Config.Net;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace ArcticWolf.DataMiner
{
    class Program
    {
        public static DatabaseContext DbContext
        {
            get
            {
                DatabaseContext DbContext = new DatabaseContext(Program.Configuration.DatabasePath);
                DbContext.Database.Migrate();
                return DbContext;
            }
        }
        public static IAppConfig Configuration { get; private set; }
        public static BenbotApiClient BenbotApiClient { get; private set; }
        public static NitestatsApiClient NitestatsApiClient { get; private set; }
        public static FnDotNetApiClient FnDotNetApiClient { get; private set; }

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            Console.CancelKeyPress += Console_CancelKeyPress;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            if (!File.Exists("app.ini"))
            {
                File.Create("app.ini").Dispose();
            }

            Configuration = new ConfigurationBuilder<IAppConfig>().UseIniFile("app.ini").Build();

            // DbContext.FnEventFlags.Include(x => x.TimeSpans).Include(x => x.Modifications);

            Log.Initalize(new List<LogVisibility>
            {
                LogVisibility.Console 
            }, new ()
            {
                new()
                {
                    ClassName = nameof(HttpClient),
                    MinLogLevel = LogLevel.Information
                },
                new()
                {
                    ClassName = nameof(Apis.Nitestats.NitestatsApiClient),
                    MinLogLevel = LogLevel.Information
                },
                new()
                {
                    ClassName = nameof(Apis.Nitestats.NitestatsApiClient),
                    MethodName = nameof(Apis.Nitestats.NitestatsApiClient.LoadHotFixesFromMessages),
                    MinLogLevel = LogLevel.Verbose
                },
                new()
                {
                    ClassName = nameof(CurrentVersionMonitor),
                    MethodName = "AesAnalyser",
                    MinLogLevel = LogLevel.Debug
                },
                new()
                {
                    ClassName = nameof(CurrentVersionMonitor),
                    MinLogLevel = LogLevel.Debug
                },
            }
            );

            BenbotApiClient = new BenbotApiClient();
            NitestatsApiClient = new NitestatsApiClient();
            FnDotNetApiClient = new FnDotNetApiClient();

            CurrentVersionMonitor.Init();
            OldVersionsManager.Init();

            DiscordManager.Init();

            /*FnEventFlag flag = DbContext.FnEventFlags.Find("EventFlag.Anniversary.EnableEnemyVariants");
            DbContext.Entry(flag).Collection(x => x.TimeSpans).Load();
            Log.Error(JsonConvert.SerializeObject(flag));*/

            while (true)
            {

            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Log.Information("Exiting");
            DbContext.SaveChanges();
        }
    }
}
