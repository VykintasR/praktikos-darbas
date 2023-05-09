using Bezdzione.Data;
using CommandLine;
using BezdzioneTests;

namespace Bezdzione
{
    public class Program
    {
        internal class Options
        {
            [Option("region", HelpText = "Filter by region")]
            public string Region { get; set; } = "";

            [Option("plan", HelpText = "Filter by plan")]
            public string Plan { get; set; } = "";

            [Option("image", HelpText = "Filter by OS template")]
            public string Image { get; set; } = "";

            [Option("category", HelpText = "Filter by category")] 
            public string Category { get; set; } = "";

            [Option("random", HelpText = "Perform a random server deployment test.")]
            public bool Random { get; set; }
            [Option("default", HelpText = "Perform a default server deployment test.")]
            public bool Default { get; set; }

            public List<(string, string)> InputOrder { get; set; } = new List<(string, string)>();

        }

        static async Task Main(string[] args)
        {
            ServerTests serverTests = new ServerTests();
            Options options = ParseOptions(args);

            if (options.Default)
            {
                Console.WriteLine("Running default server deployment test...");
                await Task.Run(serverTests.TestDefaultServerDeployment);
                Console.WriteLine("Test completed successfully.");
            }
            else if (options.Random)
            {
                int i = 0;
                while (i < 30)
                {
                    try
                    {
                        Console.WriteLine("Running random server deployment test...");
                        await Task.Run(serverTests.TestRandomServerDeployment);
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
                   plans = FilterPlans(plans, option, value);
               }

               Console.WriteLine(plans.Plans.Count);
               foreach(Plan plan in plans.Plans)
               {
                   plan.ShowInfo();
               }
           }
           else
           {
               Console.WriteLine("Running default server deployment test...");
               await Task.Run(serverTests.TestDefaultServerDeployment);
               Console.WriteLine("Test completed successfully.");
           }

           /*int i = 0;
           while (i < 30)
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
           Console.WriteLine("Testavimas baigtas.");*/
                Console.WriteLine("Press Enter key to exit....");
            Console.ReadLine();
            Environment.Exit(0);
        }

        private static PlanList FilterPlans(PlanList plans, string option, string value)
        {
            switch(option)
            {
                case "region":
                    return plans.GetPlansFromRegion(value);
                case "category":
                    return plans.GetPlansWithCategory(value);
                case "image":
                    return plans.GetPlansWithImage(value);
                case "plan":
                    return plans.GetPlansBySlug(value);
                default: 
                    return plans;
            }
        }

        private static Options ParseOptions(string[] args)
        {
            var options = new Options();
            var parser = new Parser(with => with.HelpWriter = null);
            var result = parser.ParseArguments<Options>(args);

            bool hasRandom = false;
            bool hasDefault = false;
            bool hasOtherOptions = false;
            bool randomOrDefaultPresent = false;

            result.WithParsed(o =>
            {
                hasRandom = o.Random;
                hasDefault = o.Default;
                randomOrDefaultPresent = hasRandom || hasDefault;
                hasOtherOptions = !(string.IsNullOrEmpty(o.Region) && string.IsNullOrEmpty(o.Plan) && string.IsNullOrEmpty(o.Image) && string.IsNullOrEmpty(o.Category));
                if (hasOtherOptions)
                {
                    options.InputOrder = new List<(string, string)>();
                    if (!string.IsNullOrEmpty(o.Region))
                    {
                        options.Region = o.Region;
                        options.InputOrder.Add(("region", o.Region));
                    }
                    if (!string.IsNullOrEmpty(o.Plan))
                    {
                        options.Plan = o.Plan;
                        options.InputOrder.Add(("plan", o.Plan));
                    }
                    if (!string.IsNullOrEmpty(o.Image))
                    {
                        options.Image = o.Image;
                        options.InputOrder.Add(("image", o.Image));
                    }
                    if (!string.IsNullOrEmpty(o.Category))
                    {
                        options.Category = o.Category;
                        options.InputOrder.Add(("category", o.Category));
                    }
                }
            })
            .WithNotParsed(errors =>
            {
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ToString());
                }
                Environment.Exit(1);
            });

            if (randomOrDefaultPresent && hasOtherOptions)
            {
                Console.WriteLine("Error: Either only one of --random or --default can be specified, or the four other options.");
                Environment.Exit(1);
            }

            options.Random = hasRandom;
            options.Default = hasDefault;

            return options;
        }
    }
}