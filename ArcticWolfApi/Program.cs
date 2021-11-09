using ArcticWolf.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcticWolfApi
{
    public class Program
    {
        public static readonly Dictionary<string, object> CosmeticLoadout = new Dictionary<string, object>()
    {
      {
        "character",
        (object) ""
      },
      {
        "backpack",
        (object) ""
      },
      {
        "pickaxe",
        (object) "AthenaPickaxe:defaultpickaxe"
      },
      {
        "glider",
        (object) "AthenaGlider:defaultglider"
      },
      {
        "skydivecontrail",
        (object) ""
      },
      {
        "loadingscreen",
        (object) ""
      },
      {
        "musicpack",
        (object) ""
      },
      {
        "dance",
        (object) new string[6]
        {
          "AthenaDance:eid_dancemoves",
          "",
          "",
          "",
          "",
          ""
        }
      },
      {
        "itemwrap",
        (object) new string[7]{ "", "", "", "", "", "", "" }
      }
    };
        public static readonly string[] WhitelistedCosmetics = new string[6] // itemType:id
        {
      "AthenaPickaxe:defaultpickaxe",
      "AthenaGlider:defaultglider",
      "AthenaDance:eid_dancemoves",
      "AthenaDance:eid_wir",
      "AthenaDance:eid_boogiedown",
      "AthenaGlider:glider_id_243_myth"
        };

        public static string ClientId { get; set; }

        public static string Id { get; set; }

        public static string DisplayName { get; set; }

        public static int AthenaRvn { get; set; }

        public static int CommonCoreRvn { get; set; }

        public static DatabaseContext Database { get; private set; }

        public static void Main(string[] args)
        {
            Database = new(@"C:\Users\Administrator\source\repos\ArcticWolf\ArcticWolf.DataMiner\bin\Debug\net5.0\db.sqlite");

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args);
            builder.ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseUrls("https://*:44366");
                webBuilder.UseStartup<Startup>();
            });
            return builder;
        }

    }
}
