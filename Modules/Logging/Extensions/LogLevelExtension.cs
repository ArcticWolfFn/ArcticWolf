using System;

namespace Logging.Extensions
{
    public static class LogLevelExtension
    {
        public static ConsoleColor ToConsoleColor(this LogLevel level)
        {
            return level switch
            {
                LogLevel.Verbose => ConsoleColor.DarkGray,
                LogLevel.Debug => ConsoleColor.Gray,
                LogLevel.Information => ConsoleColor.Blue,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Fatal => ConsoleColor.DarkRed,
                _ => ConsoleColor.White,
            };
        } 
    }
}
