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

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:8000");
                    webBuilder.UseStartup<Startup>();
                });
    }
}
