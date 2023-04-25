using Microsoft.Extensions.Configuration;

namespace Bezdzione
{
    internal static class Configuration
    {

        private static readonly IConfiguration _configuration;

        static Configuration()
        {
            _configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                                          .Build();
        }

        public static string GetSetting(string key, string defaultValue = "")
        {
            return _configuration[key] ?? defaultValue;
        }
    }
}
