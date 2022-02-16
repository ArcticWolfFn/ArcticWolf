using Logging.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Logging;
using System.Linq;

public struct MinLogLevelOption
{
    /// <summary>
    /// Real Class Name
    /// </summary>
    public string ClassName { get; set; }

    /// <summary>
    /// Real Method Name
    /// </summary>
    public string MethodName { get; set; }

    public LogLevel MinLogLevel { get; set; }
}

public static class Log
{
    private static List<LogVisibility> _logVisibilities { get; set; }
    private static List<MinLogLevelOption> _minLogLevels { get; set; }
    private static bool _initalized = false;
    private static readonly object _consoleLock = new object();

    public static void Initalize(List<LogVisibility> logVisibilities, List<MinLogLevelOption> minLogLevels)
    {
        if (_initalized)
        {
            _logVisibilities.AddRange(logVisibilities);
            _minLogLevels.AddRange(minLogLevels);
            return;
        }

        Console.OutputEncoding = Encoding.Unicode;

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

    private static void Write(LogLevel logLevel, string message, string prefix = null, string classPrefix = null)
    {
        StackFrame frame = new StackFrame(2);
        var method = frame.GetMethod();

        string methodDefaultLogPrefix = "";
        foreach(object attr in method.GetCustomAttributes(false))
        {
            if (attr is LogPrefixAttribute)
            {
                methodDefaultLogPrefix = ((LogPrefixAttribute)attr).Prefix;
            }
        }

        string classDefaultLogPrefix = method.DeclaringType.Name;
        foreach (object attr in method.DeclaringType.GetCustomAttributes(false))
        {
            if (attr is LogPrefixAttribute)
            {
                classDefaultLogPrefix = ((LogPrefixAttribute)attr).Prefix;
            }
        }

        // first check if there is a custom log level setting for the specified function, then default to the class setting
        if (_minLogLevels.Any(x => x.ClassName == method.DeclaringType.Name && x.MethodName == method.Name))
        {
            if (_minLogLevels.First(x => x.ClassName == method.DeclaringType.Name && x.MethodName == method.Name).MinLogLevel > logLevel)
            {
                return;
            }
        }
        else if (_minLogLevels.Any(x => x.ClassName == method.DeclaringType.Name && string.IsNullOrWhiteSpace(x.MethodName)))
        {
            if (_minLogLevels.First(x => x.ClassName == method.DeclaringType.Name && string.IsNullOrWhiteSpace(x.MethodName)).MinLogLevel > logLevel)
            {
                return;
            }
        }

        var seperator = " ";
        var formatedDateTime = $"[{DateTime.Now}]";
        var formatedLogLevel = $"[{logLevel}]";

        var formatedMessage = $"[{classDefaultLogPrefix}] ";
        if (classPrefix != null)
        {
            formatedMessage = classPrefix;
        }

        if (prefix != null)
        {
            formatedMessage += $"({prefix}): ";
        } 
        else if (methodDefaultLogPrefix != "")
        {
            formatedMessage += $"({methodDefaultLogPrefix}): ";
        }

        formatedMessage += message + Environment.NewLine;

        if (_logVisibilities.Contains(LogVisibility.Console))
        {
            lock (_consoleLock)
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

    public static void Verbose(string message, string customMethodprefix = null, string customClassPrefix = null)
    {
        Write(LogLevel.Verbose, message, customMethodprefix, customClassPrefix);
    }

    public static void Debug(string message, string customMethodprefix = null, string customClassPrefix = null)
    {
        Write(LogLevel.Debug, message, customMethodprefix, customClassPrefix);
    }

    public static void Information(string message, string customMethodprefix = null, string customClassPrefix = null)
    {
        Write(LogLevel.Information, message, customMethodprefix, customClassPrefix);
    }

    public static void Warning(string message, string customMethodprefix = null, string customClassPrefix = null)
    {
        Write(LogLevel.Warning, message, customMethodprefix, customClassPrefix);
    }

    public static void Error(string message, string customMethodprefix = null, string customClassPrefix = null)
    {
        Write(LogLevel.Error, message, customMethodprefix, customClassPrefix);
    }

    public static void Fatal(string message, string customMethodprefix = null, string customClassPrefix = null)
    {
        Write(LogLevel.Fatal, message, customMethodprefix, customClassPrefix);
    }
}