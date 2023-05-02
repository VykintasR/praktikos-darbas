using BezdzioneTests;

namespace Bezdzione
{
    public class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Vykdomas testas....");
            await Task.Run(async () =>
            {
                ServerTests serverTests = new ServerTests();
                await serverTests.TestDefaultServerDeployment();
            });
            Console.WriteLine("Testas įvykdytas.");
            Console.WriteLine("Press Enter key to exit....");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}