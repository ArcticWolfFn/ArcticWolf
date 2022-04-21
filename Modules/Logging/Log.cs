using Logging.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Logging;
using System.Linq;

// ReSharper disable CheckNamespace

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
    private static List<LogVisibility> LogVisibilities { get; set; }
    
    private static List<MinLogLevelOption> MinLogLevels { get; set; }
    
    private static bool _initialized;
    
    private static readonly object ConsoleLock = new object();

    public static void Initialize(List<LogVisibility> logVisibilities, List<MinLogLevelOption> minLogLevels)
    {
        if (_initialized)
        {
            LogVisibilities.AddRange(logVisibilities);
            MinLogLevels.AddRange(minLogLevels);
            return;
        }

        Console.OutputEncoding = Encoding.Unicode;

        LogVisibilities = logVisibilities;
        MinLogLevels = minLogLevels;

        _initialized = true;

        LogBasicInformation();
    }

    private static void LogBasicInformation()
    {
        if (!LogVisibilities.Contains(LogVisibility.Console)) return;
        const string logo = @"
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

    private static void Write(LogLevel logLevel, string message, string prefix = null, string classPrefix = null)
    {
        var frame = new StackFrame(2);
        var method = frame.GetMethod();

        var methodDefaultLogPrefix = "";
        var classDefaultLogPrefix = method?.DeclaringType?.Name;
        if (method != null)
        {
            foreach (var attr in method.GetCustomAttributes(false))
            {
                if (attr is LogPrefixAttribute logPrefixAttr)
                {
                    methodDefaultLogPrefix = logPrefixAttr.Prefix;
                }
            }

            if (method.DeclaringType != null)
            {
                foreach (var attr in method.DeclaringType.GetCustomAttributes(false))
                {
                    if (attr is LogPrefixAttribute logPrefixAttr)
                    {
                        classDefaultLogPrefix = logPrefixAttr.Prefix;
                    }
                }
                
                // first check if there is a custom log level setting for the specified function,
                // then default to the class setting
                if (MinLogLevels.Any(x => x.ClassName == method.DeclaringType.Name && 
                                                          x.MethodName == (prefix ?? method.Name)))
                {
                    if (MinLogLevels.First(x => x.ClassName == method.DeclaringType.Name && 
                                                                x.MethodName == (prefix ?? method.Name)).MinLogLevel > logLevel)
                    {
                        return;
                    }
                }
                else if (MinLogLevels.Any(x => x.ClassName == method.DeclaringType.Name && 
                                                               string.IsNullOrWhiteSpace(x.MethodName)))
                {
                    if (MinLogLevels.First(x => x.ClassName == method.DeclaringType.Name && 
                                                                string.IsNullOrWhiteSpace(x.MethodName)).MinLogLevel > logLevel)
                    {
                        return;
                    }
                }
            }
        }

        const string separator = " ";
        var formattedDateTime = $"[{DateTime.Now}]";
        var formattedLogLevel = $"[{logLevel}]";

        var formattedMessage = $"[{classDefaultLogPrefix}] ";
        if (classPrefix != null)
        {
            formattedMessage = classPrefix;
        }

        if (prefix != null)
        {
            formattedMessage += $"({prefix}): ";
        } 
        else if (methodDefaultLogPrefix != "")
        {
            formattedMessage += $"({methodDefaultLogPrefix}): ";
        }

        formattedMessage += message + Environment.NewLine;

        if (!LogVisibilities.Contains(LogVisibility.Console)) return;
        lock (ConsoleLock)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(formattedDateTime + separator);
            Console.ForegroundColor = logLevel.ToConsoleColor();
            Console.Write(formattedLogLevel + separator);
            Console.ResetColor();
            Console.Write(formattedMessage);
        }
    }

    public static void Verbose(string message, string customMethodPrefix = null, string customClassPrefix = null)
    {
        Write(LogLevel.Verbose, message, customMethodPrefix, customClassPrefix);
    }

    public static void Debug(string message, string customMethodPrefix = null, string customClassPrefix = null)
    {
        Write(LogLevel.Debug, message, customMethodPrefix, customClassPrefix);
    }

    public static void Information(string message, string customMethodPrefix = null, string customClassPrefix = null)
    {
        Write(LogLevel.Information, message, customMethodPrefix, customClassPrefix);
    }

    public static void Warning(string message, string customMethodPrefix = null, string customClassPrefix = null)
    {
        Write(LogLevel.Warning, message, customMethodPrefix, customClassPrefix);
    }

    public static void Error(string message, string customMethodPrefix = null, string customClassPrefix = null)
    {
        Write(LogLevel.Error, message, customMethodPrefix, customClassPrefix);
    }

    public static void Fatal(string message, string customMethodPrefix = null, string customClassPrefix = null)
    {
        Write(LogLevel.Fatal, message, customMethodPrefix, customClassPrefix);
    }
}