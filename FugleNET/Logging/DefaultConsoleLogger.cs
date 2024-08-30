using System;

namespace FugleNET.Logging
{
    public class DefaultConsoleLogger : ILogger
    {
        public LoggingLevel MinimumLevel { get; set; }

        public bool IsEnabled(LoggingLevel level)
        {
            return level >= MinimumLevel;
        }

        public virtual void Log(string message, LoggingLevel level)
        {
            if (!IsEnabled(level)) return;

            var oriTextColor = Console.ForegroundColor;

            Console.ForegroundColor = GetColorFromLevel(level);
            Console.WriteLine($"{DateTime.Now:O} [{level}] {message}");
            Console.ForegroundColor = oriTextColor;
        }

        private ConsoleColor GetColorFromLevel(LoggingLevel level) => level switch
        {
            LoggingLevel.Debug => ConsoleColor.Gray,
            LoggingLevel.Info => ConsoleColor.White,
            LoggingLevel.Warn => ConsoleColor.Yellow,
            LoggingLevel.Error => ConsoleColor.Red,
            _ => throw new ArgumentOutOfRangeException(nameof(level))
        };
    }
}
