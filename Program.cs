using Bezdzione.Data;
using Bezdzione.CLI;
using BezdzioneTests;

namespace Bezdzione
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            ServerTests serverTests = new ServerTests();
            Options options = Options.SetOptions(args);

            if (options.Default)
            {
                await TestRunner.RunDefaultTest(serverTests);
            }
            else if (options.Random)
            {
                await TestRunner.RunRandomTest(serverTests, 15);
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
                await TestRunner.RunDefaultTest(serverTests);
            }
            Console.WriteLine("Testing done.");
            Console.WriteLine("Press Enter key to exit....");
            Console.ReadLine();
            Environment.Exit(0);
        }   
    }
}