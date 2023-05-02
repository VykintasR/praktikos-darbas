namespace Bezdzione.Logs
{
    public static class Logger
    {
        public static void LogInfo(string message)
        {
            string formattedMessage = $"INFO: {message}";
            FileLogger.Log(formattedMessage);
        }

        public static void LogError(string message)
        {
            string formattedMessage = $"ERROR: {message}";
            FileLogger.Log(formattedMessage);
        }
    }
}
