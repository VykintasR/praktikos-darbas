using Bezdzione.Logs;
namespace Bezdzione
{
    public static class TimeoutManager
    {
        private static readonly int DEFAULT_TIMEOUT;
        private static readonly int DEFAULT_VIRTUAL_TIMEOUT;
        private static readonly int DEFAULT_BAREMETAL_TIMEOUT;

        static TimeoutManager()
        {
            try
            {
                DEFAULT_TIMEOUT = int.Parse(Configuration.GetSetting("DEFAULT_TIMEOUT"));
                DEFAULT_VIRTUAL_TIMEOUT = int.Parse(Configuration.GetSetting("DEFAULT_VIRTUAL_TIMEOUT"));
                DEFAULT_BAREMETAL_TIMEOUT = int.Parse(Configuration.GetSetting("DEFAULT_BAREMETAL_TIMEOUT"));
            }
            catch
            {
                ConsoleLogger.InvalidTimeout();
                DEFAULT_TIMEOUT = 10;
                DEFAULT_VIRTUAL_TIMEOUT = 15;
                DEFAULT_BAREMETAL_TIMEOUT = 3;
            }
        }

        public static int GetDefaultTimeout() => DEFAULT_TIMEOUT;
        public static int GetTimeoutForCategory(int? userTimeout, string category)
        {

            if (userTimeout <= 0)
            {
                throw new ArgumentException("Invalid timeout. Timeout must be a positive  integer value.");
            }

            int timeout;

            switch (category)
            {
                case "Shared resources":
                case "Dedicated resources":
                    timeout = userTimeout ?? DEFAULT_VIRTUAL_TIMEOUT;
                    break;
                case "lightweight":
                case "middleweight":
                case "heavyweight":
                    timeout = userTimeout ?? DEFAULT_BAREMETAL_TIMEOUT;
                    break;
                default:
                    throw new ArgumentException("Invalid category.");
            }

            return timeout;
        }
    }
}
