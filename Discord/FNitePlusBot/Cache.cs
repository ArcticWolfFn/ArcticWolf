using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNitePlusBot
{
    public static class Cache
    {
        public static Dictionary<string, ArcticWolf.DataMiner.Models.Apis.Nitestats.Staging.Server> StagingServers = new();
    }
}
