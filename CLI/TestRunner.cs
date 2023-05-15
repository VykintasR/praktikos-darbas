
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
                try
                {
                    int timeout = int.Parse(Configuration.GetSetting("DEFAULT_BAREMETAL_TIMEOUT"));
                    ConsoleLogger.TestStart("random", RandomParameterGenerator.GetRandomParameters(timeout));
                    serverTests.SetUp(new Server(RandomParameterGenerator.GetRandomParameters(timeout)));
                    await Task.Run(() => serverTests.TestServerDeployment(timeout));
                    ConsoleLogger.TestSuccess();
                }
                catch (Exception ex)
                {
                    ExceptionHandler.Handle(ex);
                }
                i++;
            }
        }

        public static async Task RunDefaultTest(ServerTests serverTests)
        {
            try
            {
                int timeout = int.Parse(Configuration.GetSetting("DEFAULT_VIRTUAL_TIMEOUT"));
                ConsoleLogger.TestStart("default", new Parameters());
                serverTests.SetUp(new Server());
                await Task.Run(() => serverTests.TestServerDeployment(timeout));
                ConsoleLogger.TestSuccess();
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
