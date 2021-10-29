using ArcticWolf.DataMiner.Apis.Benbot;
using ArcticWolf.DataMiner.Apis.Nitestats;
using System;

namespace ArcticWolf.DataMiner
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Initalize(new System.Collections.Generic.List<Common.Logging.LogVisibility> 
            { 
                Common.Logging.LogVisibility.Console 
            }, new System.Collections.Generic.Dictionary<string, Common.Logging.LogLevel>
            {
                {"Http", Common.Logging.LogLevel.Information }
            }
            );

            new NitestatsApiClient();
            new BenbotApiClient();
        }
    }
}
