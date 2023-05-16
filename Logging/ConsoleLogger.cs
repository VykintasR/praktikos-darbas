using Bezdzione.Request;

namespace Bezdzione.Logs
{
    public static class ConsoleLogger
    {
        //Information messages
        public static void Log(string message) => Console.WriteLine($"{message}");
        public static void RequestInfo(Server server) => Console.WriteLine(MessageFormatter.Info(MessageFormatter.RequestInfo(server.Parameters)));
        public static void End() => Console.WriteLine("Press enter key to exit...");
        //Testing messages
        public static void TestingStarted(int testCount, string type) => Console.WriteLine($"Running {testCount} {type} tests.");
        public static void TestingComplete() => Console.WriteLine($"Testing done. Press Enter key to exit....");
        public static void TestStart(int testNumber, RequestParameters parameters) => Console.WriteLine($"Running #{testNumber} Server deployment test with timeout of {parameters.Timeout} minutes...");
        public static void TestFinished() => Console.WriteLine($"Test finished.");
        public static void TestFail(string message) => Console.WriteLine($"Test failed. Reason: {message}");
        //Error messages
        public static void Error(string message) => Console.WriteLine($"ERROR: {message}");
        public static void MissingServerConfiguration() => Console.WriteLine($"Server is null. Deployment failed.");
        public static void Exception(string message) => Console.WriteLine($"An exception was thrown: {message}");
        public static void UnexpectedError(string message) => Console.WriteLine($"An unexpected error occurred: {message}");
        public static void BadFormatConversionError() => Console.WriteLine($"Invalid format. Timeout and testcount must be integer values.");
        public static void DefaultTimeoutsNotSet() => Console.WriteLine($"Default timeout configuration is missing. {TimeoutManager.DEFAULT_TIMEOUT} min timeout will be used.");
        public static void InvalidTestCount() => Console.WriteLine("Invalid value for testcount. testcount must be positive integer value");
        public static void InvalidTimeout() => Console.WriteLine($"Invalid value for timeout. Using default timeout configuration.");
        public static void UnknownOption() => Console.WriteLine($"Unknown option. Valid options are: default, random, category, plan, image, region, timeout, testcount.");
        public static void IncompatibleOptions() => Console.WriteLine($"Either only --random can be specified, or any combination of category, region, plan, image.");
        
    }
}
