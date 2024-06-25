namespace FugleNET
{
    public static class LogExtensions
    {
        public static void Debug(this ILogger logger, string msg) => logger.Log(msg, LoggingLevel.Debug);

        public static void Info(this ILogger logger, string msg) => logger.Log(msg, LoggingLevel.Info);
        
        public static void Warn(this ILogger logger, string msg) => logger.Log(msg, LoggingLevel.Warn);
        
        public static void Error(this ILogger logger, string msg) => logger.Log(msg, LoggingLevel.Error);
    }
}
