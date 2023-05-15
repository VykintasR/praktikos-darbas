using Bezdzione.Request;
using Bezdzione.Logs;
using BezdzioneTests;

namespace Bezdzione.CLI
{
    public static class TestRunner
    {
        public static async Task RunRandomTest(ServerTests serverTests, int testCount, int? timeout)
        {
            int i = 0;
            while (i < testCount)
            {
                ConsoleLogger.TestStart("random", RandomParameterGenerator.GetRandomParameters(timeout));
                await ExecuteServerDeploymentTest(serverTests);
                ConsoleLogger.TestCompleted();
                i++;
            }
        }

        public static async Task RunDefaultTest(ServerTests serverTests, int? timeout)
        {
            ConsoleLogger.TestStart("default", new Parameters(timeout));
            await ExecuteServerDeploymentTest(serverTests);
            ConsoleLogger.TestCompleted();
        }

        private static async Task ExecuteServerDeploymentTest(ServerTests serverTests)
        {
            try
            {
                await Task.Run(serverTests.TestServerDeployment);
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
