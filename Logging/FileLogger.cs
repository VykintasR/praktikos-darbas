namespace Bezdzione.Logs
{
    public static class FileLogger
    {
        private static readonly string LogDirectoryPath =
            Path.Combine(Environment.CurrentDirectory, "Logs");

        private static readonly string LogFilePath =
            Path.Combine(LogDirectoryPath, "logfile.txt");

        public static void Log(string message)
        {
            EnsureLogDirectoryExists();
            EnsureLogFileExists();

            File.AppendAllText(LogFilePath, message);
        }

        private static void EnsureLogDirectoryExists()
        {
            if (!Directory.Exists(LogDirectoryPath))
            {
                Directory.CreateDirectory(LogDirectoryPath);
            }
        }

        private static void EnsureLogFileExists()
        {
            if (!File.Exists(LogFilePath))
            {
                File.Create(LogFilePath).Close();
            }
        }
    }
}
