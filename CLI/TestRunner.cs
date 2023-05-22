using Bezdzione.Request;
using Bezdzione.Logging;
using Bezdzione.Data;
using BezdzioneTests;
using System.Reflection.Metadata.Ecma335;

namespace Bezdzione.CLI
{
    public static class TestRunner
    {
        public static async Task SetUpRandomTest(ServerTests serverTests, PlanList allPlans, int testCount, int? timeout)
        {
            ConsoleLogger.TestingStarted(testCount, "random");
            for (int i = 0; i < testCount; i++)
            {
                serverTests.SetUp(new Server(RandomParameterGenerator.GetRandomParameters(timeout, allPlans)));
                ConsoleLogger.TestStart(i+1, serverTests.GetServerParameters());
                await ExecuteServerDeploymentTest(serverTests);
                ConsoleLogger.TestFinished();
            }
            ConsoleLogger.TestingComplete();
        }

        public static async Task SetUpDefaultTest(ServerTests serverTests, PlanList allPlans, int testCount, int? timeout)
        {
            ConsoleLogger.TestingStarted(testCount, "default");
            serverTests.SetUp(new Server(new RequestParameters(timeout, allPlans)));
            for (int i = 0; i < testCount; i++)
            {
                ConsoleLogger.TestStart(i+1, new RequestParameters(timeout, allPlans));
                await ExecuteServerDeploymentTest(serverTests);
                ConsoleLogger.TestFinished();
            }
            ConsoleLogger.TestingComplete();
        }

        public static async Task SetUpFilteredRandomTest(ServerTests serverTests, PlanList filteredPlans, Options options)
        {
            ConsoleLogger.TestingStarted(options.Testcount, "random");
            for (int i = 0; i < options.Testcount; i++)
            {
                serverTests.SetUp(new Server(RequestParameters.ParamatetersFromOptions(filteredPlans, options)));
                ConsoleLogger.TestStart(i + 1, serverTests.GetServerParameters());
                await ExecuteServerDeploymentTest(serverTests);
                ConsoleLogger.TestFinished();
            }
            ConsoleLogger.TestingComplete();
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
