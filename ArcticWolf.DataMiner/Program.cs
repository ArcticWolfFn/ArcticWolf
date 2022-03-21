using ArcticWolf.DataMiner.Managers;
using ArcticWolf.DataMiner.Models;
using ArcticWolf.Storage;
using Config.Net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using ArcticWolf.Apis.Base.Common.Http;
using ArcticWolf.Apis.BenBot;
using ArcticWolf.Apis.NiteStats;

namespace ArcticWolf.DataMiner
{
    class Program
    {
        public static DatabaseContext DbContext
        {
            get
            {
                var dbContext = new DatabaseContext(Configuration.DatabasePath);
                dbContext.Database.Migrate();
                return dbContext;
            }
        }
        public static IAppConfig Configuration { get; private set; }
        public static BenBotApiClient BenbotApiClient { get; private set; }
        public static NiteStatsApiClient NiteStatsApiClient { get; private set; }

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
                    ClassName = nameof(Apis.NiteStats.NiteStatsApiClient),
                    MinLogLevel = LogLevel.Information
                },
                new()
                {
                    ClassName = nameof(HotFixManager),
                    MethodName = nameof(HotFixManager.LoadHotFixesFromMessages),
                    MinLogLevel = LogLevel.Verbose
                },
                new()
                {
                    ClassName = nameof(CurrentVersionMonitor),
                    MethodName = CurrentVersionMonitor.AES_LOG_PREFIX,
                    MinLogLevel = LogLevel.Verbose
                },
                new()
                {
                    ClassName = nameof(CurrentVersionMonitor),
                    MinLogLevel = LogLevel.Verbose
                },
            }
            );

            NiteStatsApiClient = new NiteStatsApiClient();
            BenbotApiClient = new BenBotApiClient();

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
