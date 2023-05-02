namespace Bezdzione.Logs
{
    public static class FileLogger
    {
        private static readonly string LogFilePath =
            Path.Combine(Environment.CurrentDirectory, "Logs", "logfile.txt");

        public static void Log(string message)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}{Environment.NewLine}";
            File.AppendAllText(LogFilePath, logMessage);
        }
    }
}
