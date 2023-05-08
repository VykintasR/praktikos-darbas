namespace Bezdzione.Logs
{
    public static class FileLogger
    {
        private static readonly string LogFilePath =
            Path.Combine(Environment.CurrentDirectory, "Logs", "logfile.txt");

        public static void Log(string message)
        {
            File.AppendAllText(LogFilePath, message);
        }
    }
}
