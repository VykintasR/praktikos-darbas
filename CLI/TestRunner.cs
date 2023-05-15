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
                serverTests.SetUp(new Server(RandomParameterGenerator.GetRandomParameters(timeout)));
                ConsoleLogger.TestStart("random", serverTests.GetServerParameters());
                await ExecuteServerDeploymentTest(serverTests);
                ConsoleLogger.TestCompleted();
                i++;
            }
        }

        public static async Task RunDefaultTest(ServerTests serverTests, int? timeout)
        {
            serverTests.SetUp(new Server(new Parameters(timeout)));
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
