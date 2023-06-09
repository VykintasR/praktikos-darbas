﻿using Bezdzione.Logging;
namespace Bezdzione
{
    public static class TimeoutManager
    {
        public static readonly int MINIMUM_TIMEOUT = 1;
        public static readonly int DEFAULT_TIMEOUT = 10;
        private static readonly int DEFAULT_VIRTUAL_TIMEOUT;
        private static readonly int DEFAULT_BAREMETAL_TIMEOUT;
        
        static TimeoutManager()
        {
            try
            {
                DEFAULT_VIRTUAL_TIMEOUT = int.Parse(Configuration.GetSetting("DEFAULT_VIRTUAL_TIMEOUT"));
                DEFAULT_BAREMETAL_TIMEOUT = int.Parse(Configuration.GetSetting("DEFAULT_BAREMETAL_TIMEOUT"));
            }
            catch
            {
                ConsoleLogger.DefaultTimeoutsNotSet();;
                DEFAULT_VIRTUAL_TIMEOUT = DEFAULT_TIMEOUT;
                DEFAULT_BAREMETAL_TIMEOUT = DEFAULT_TIMEOUT;
            }
        }

        public static int GetTimeout(int? userTimeout, string category)
        {
            switch (userTimeout)
            {
                case int timeout when timeout < MINIMUM_TIMEOUT:
                    ConsoleLogger.InvalidTimeout();
                    return DEFAULT_TIMEOUT;
                case int timeout when timeout >= MINIMUM_TIMEOUT:
                    return userTimeout.Value;
                default:
                    return SetTimeoutByCategory(category);
            }
        }

        private static int SetTimeoutByCategory(string category)
        {
            switch (category)
            {
                case "Shared resources":
                case "Dedicated resources":
                    return DEFAULT_VIRTUAL_TIMEOUT;
                case "lightweight":
                case "middleweight":
                case "heavyweight":
                   return DEFAULT_BAREMETAL_TIMEOUT;
                default:
                    throw new ArgumentException("Invalid category.");
            }
        }
    }
}
