using Bezdzione.Data;
using BezdzioneTests;

namespace Bezdzione
{
    public class Program
    {
        public static async Task Main()
        {
            int i = 0;
            while (i < 5)
            {
                try
                {
                    Console.WriteLine("Running random server deployment test...");
                    await Task.Run(async () =>
                    {
                        ServerTests serverTests = new ServerTests();
                        await serverTests.TestRandomServerDeployment();
                    });
                    Console.WriteLine("Test completed successfully.");
                }
                catch (AssertionException ex)
                {
                    Console.WriteLine("Test failed: " + ex.Message);
                    continue; // Continue to the next test
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An unexpected error occurred: " + ex.Message);
                }
                i++;
            }
            Console.WriteLine("Testavimas baigtas.");
            Console.WriteLine("Press Enter key to exit....");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}