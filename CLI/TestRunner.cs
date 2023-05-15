

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
                    Console.WriteLine($"Running random server deployment test with timeout of {timeout} minutes...");
                    await Task.Run(() => serverTests.TestRandomServerDeployment(timeout));
                    Console.WriteLine("Test completed successfully.");
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
                Console.WriteLine($"Running default server deployment test with timeout of {timeout} minutes...");
                await Task.Run(() => serverTests.TestDefaultServerDeployment(timeout));
                Console.WriteLine("Test completed successfully.");
            }
            catch (Exception ex)
            {
                ExceptionHandler.Handle(ex);
            }
        }
    }
}
