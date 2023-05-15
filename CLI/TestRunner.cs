using Bezdzione.Request;
using Bezdzione.Logs;
using BezdzioneTests;

namespace Bezdzione.CLI
{
    public static class TestRunner
    {
        public static async Task RunRandomTest(ServerTests serverTests, int testCount)
        {
            int i = 0;
            while (i < testCount)
            {
                ConsoleLogger.TestStart("random", new Parameters());
                await ExecuteServerDeploymentTest(serverTests);
                ConsoleLogger.TestSuccess();
                i++;
            }
        }

        public static async Task RunDefaultTest(ServerTests serverTests)
        {
            ConsoleLogger.TestStart("default", new Parameters());
            await ExecuteServerDeploymentTest(serverTests);
            ConsoleLogger.TestSuccess();
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
