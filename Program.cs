using Bezdzione.Data;
using Bezdzione.CLI;
using BezdzioneTests;
using Bezdzione.Logs;

namespace Bezdzione
{
    public class Program
    {
        static async Task Main(string[] args)
        {

            Options options = Options.SetOptions(args);

            //cached plans
            PlanList allPlans = PlanList.GetAllPlans();
            ServerTests serverTests = new ServerTests(allPlans);

            if (options.Random)
            {
                await TestRunner.SetUpRandomTest(serverTests, allPlans, options.Testcount, options.Timeout);
            }
            else if (options.InputOptions.Count > 0)
            {
                PlanList filteredPlans = FilterPlans(allPlans, options);
                if (filteredPlans.Plans != null && filteredPlans.Plans.Count == 0)
                {
                    ExceptionHandler.Handle(new Exception($"No plans were found with these options."));
                    ConsoleLogger.End();
                    Console.ReadLine();
                    Environment.Exit(1);
                }
                else
                {;
                    await TestRunner.SetUpFilteredRandomTest(serverTests, FilterPlans(allPlans, options), options);
                }   
            }
            else
            {
                await TestRunner.SetUpDefaultTest(serverTests, allPlans, options.Testcount, options.Timeout);
            }
            Console.ReadLine();
            Environment.Exit(0);
        }
        
        private static PlanList FilterPlans(PlanList listToFilter, Options options)
        {
            foreach((string, string) filterParameter in options.InputOptions)
                {
                string option = filterParameter.Item1;
                string value = filterParameter.Item2;
                listToFilter =  listToFilter.FilterPlansByOption(listToFilter, option, value);
            }
            return listToFilter;
        }
    }
}