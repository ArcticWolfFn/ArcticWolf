﻿using ArcticWolf.DataMiner.Common.Logging;
using ArcticWolf.DataMiner.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArcticWolf.DataMiner
{
    public static class Log
    {
        private static List<LogVisibility> _logVisibilities { get; set; }
        private static Dictionary<string, LogLevel> _minLogLevels { get; set; }
        private static bool _initalized = false;
        private static readonly object _consoleLock = new object();

        public static void Initalize(List<LogVisibility> logVisibilities, Dictionary<string, LogLevel> minLogLevels)
        {
            if (_initalized)
            {
                return;
            }

            _logVisibilities = logVisibilities;
            _minLogLevels = minLogLevels;

            _initalized = true;

            LogBasicInformation();
        }

        private static void LogBasicInformation()
        {
            if (_logVisibilities.Contains(LogVisibility.Console))
            {
                string logo = @"
   ###    ########   ######  ######## ####  ######  ##      ##  #######  ##       ######## 
  ## ##   ##     ## ##    ##    ##     ##  ##    ## ##  ##  ## ##     ## ##       ##       
 ##   ##  ##     ## ##          ##     ##  ##       ##  ##  ## ##     ## ##       ##       
##     ## ########  ##          ##     ##  ##       ##  ##  ## ##     ## ##       ######   
######### ##   ##   ##          ##     ##  ##       ##  ##  ## ##     ## ##       ##       
##     ## ##    ##  ##    ##    ##     ##  ##    ## ##  ##  ## ##     ## ##       ##       
##     ## ##     ##  ######     ##    ####  ######   ###  ###   #######  ######## ##       
";
                Console.WriteLine(logo);
                Console.WriteLine("ArcticWolf DataMiner v1.0.0");
                Console.WriteLine("© 2021 Joschua Haß (DeveloperJoschi#3193)");
                Console.WriteLine();
            }
        }

        private static void Write(LogLevel logLevel, string message, string prefix = null)
        {
            if (prefix != null && _minLogLevels.ContainsKey(prefix))
            {
                if (_minLogLevels.GetValueOrDefault(prefix) > logLevel)
                {
                    return;
                }
            }

            string seperator = " ";
            string formatedDateTime = $"[{DateTime.Now.ToString()}]";
            string formatedLogLevel = $"[{logLevel.ToString()}]";

            string formatedMessage = "";

            if (prefix != null)
            {
                formatedMessage += $"[{prefix}] ";
            }

            formatedMessage += message + Environment.NewLine;

            if (_logVisibilities.Contains(LogVisibility.Console))
            {
                lock(_consoleLock)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(formatedDateTime + seperator);
                    Console.ForegroundColor = logLevel.ToConsoleColor();
                    Console.Write(formatedLogLevel + seperator);
                    Console.ResetColor();
                    Console.Write(formatedMessage);
                }
            }
        }

        public static void Debug(string message, string prefix = null)
        {
            Write(LogLevel.Debug, message, prefix);
        }

        public static void Information(string message, string prefix = null)
        {
            Write(LogLevel.Information, message, prefix);
        }

        public static void Warning(string message, string prefix = null)
        {
            Write(LogLevel.Warning, message, prefix);
        }

        public static void Error(string message, string prefix = null)
        {
            Write(LogLevel.Error, message, prefix);
        }

        public static void Fatal(string message, string prefix = null)
        {
            Write(LogLevel.Fatal, message, prefix);
        }
    }
}
