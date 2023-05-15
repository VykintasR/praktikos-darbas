using Bezdzione.Request;
using System.Net.NetworkInformation;

namespace Bezdzione.Logs
{
    public static class ConsoleLogger
    {
        public static void Log(string message) => Console.WriteLine($"{message}");
        public static void RequestInfo(Server server) => Console.WriteLine(MessageFormatter.Info(MessageFormatter.RequestInfo(server.Parameters)));
        public static void TestStart(string type, Parameters parameters) => Console.WriteLine($"Running {type} server deployment test with timeout of {parameters.Timeout} minutes...");
        public static void TestSuccess() => Console.WriteLine($"Test completed successfully.");
        public static void UnexpectedError(string message) => Console.WriteLine($"An unexpected error occurred: {message}");
        public static void TestFail(string message) => Console.WriteLine($"Test failed. Reason: {message}");
        public static void InvalidTimeout() => Console.WriteLine($"Invalid value for timeout. Please provide a valid integer value.");
        public static void UnknownOption() => Console.WriteLine($"Unknown option. Valid options are: default, random, category, plan, image, region, and timeout.");
        public static void IncompatibleOptions() => Console.WriteLine($"Either only one of --random or --default can be specified, or the four other options.");
        public static void TestingComplete() => Console.WriteLine($"Testing done. Press Enter key to exit....");
    }
}
