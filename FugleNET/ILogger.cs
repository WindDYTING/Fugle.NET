namespace FugleNET
{
    public interface ILogger
    {
        bool IsEnabled(LoggingLevel level);

        void Log(string message, LoggingLevel level);

        LoggingLevel MinimumLevel { get; set; }
    }

    public enum LoggingLevel
    {
        Debug,
        Info,
        Warn,
        Error
    }
}
