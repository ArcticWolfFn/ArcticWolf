using ArcticWolf.DataMiner.Apis.Benbot;
using ArcticWolf.DataMiner.Apis.FnDotNet;
using ArcticWolf.DataMiner.Apis.Nitestats;
using ArcticWolf.DataMiner.Managers;
using ArcticWolf.DataMiner.Models;
using ArcticWolf.DataMiner.Storage;
using Config.Net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace ArcticWolf.DataMiner
{
    class Program
    {
        public static DatabaseContext DbContext { get; set; }
        public static IAppConfig Configuration { get; private set; }
        public static BenbotApiClient BenbotApiClient;

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

            DbContext = new DatabaseContext();
            DbContext.Database.Migrate();

            Log.Initalize(new System.Collections.Generic.List<Common.Logging.LogVisibility> 
            { 
                Common.Logging.LogVisibility.Console 
            }, new System.Collections.Generic.Dictionary<string, Common.Logging.LogLevel>
            {
                {"Http", Common.Logging.LogLevel.Information },
                {"AesManager", Common.Logging.LogLevel.Debug },
            }
            );

            BenbotApiClient = new BenbotApiClient();
            new NitestatsApiClient();
            new FnDotNetApiClient();

            AesManager.Init();
            OldVersionsManager.Init();

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
