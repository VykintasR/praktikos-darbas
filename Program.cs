using Bezdzione.Data;
using Bezdzione.CLI;
using BezdzioneTests;
using Bezdzione.Logs;
using Bezdzione.Request;
using System.Threading;

namespace Bezdzione
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            ServerTests serverTests = new ServerTests();
            Options options = Options.SetOptions(args);

            if (options.Random)
            {
                serverTests.SetUp(new Server(RandomParameterGenerator.GetRandomParameters(options.Timeout)));
                await TestRunner.RunRandomTest(serverTests, 5);
            }
            else if (options.InputOrder.Count > 0)
            {
                // all plans
                PlanList plans = PlanList.GetAllPlans();
                Console.WriteLine(plans.Plans.Count);

                foreach ((string, string) filterParameter in options.InputOrder)
                {
                    string option = filterParameter.Item1;
                    string value = filterParameter.Item2;
                    Console.WriteLine(option + " " + value);
                    plans = PlanList.FilterPlansByOptions(plans, option, value);
                }

                Console.WriteLine(plans.Plans.Count);
                foreach(Plan plan in plans.Plans)
                {
                    plan.ShowInfo();
                }
            }
            else
            {
                serverTests.SetUp(new Server(options.Timeout));
                await TestRunner.RunDefaultTest(serverTests);
            }
            ConsoleLogger.TestingComplete();
            Console.ReadLine();
            Environment.Exit(0);
        }   
    }
}