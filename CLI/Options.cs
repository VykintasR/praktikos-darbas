using CommandLine;
using Bezdzione.Logs;

namespace Bezdzione.CLI
{
    public class Options
    {
        [Option("region", HelpText = "Filter by region")]
        public string? Region { get; set; } = null;

        [Option("plan", HelpText = "Filter by plan")]
        public string? Plan { get; set; } = null;

        [Option("image", HelpText = "Filter by OS template")]
        public string? Image { get; set; } = null;

        [Option("category", HelpText = "Filter by category")]
        public string? Category { get; set; } = null;

        [Option("random", HelpText = "Perform a random Server deployment test.")]
        public bool Random { get; set; }

        [Option("timeout", HelpText = "Timeout in minutes for running tests.")]
        public int? Timeout { get; set; } = null;

        [Option("testcount", HelpText = "Number of tests to run with given options.")]
        public int Testcount{ get; set; } = 1;
        public List<(string, string)> InputOptions { get; set; } = new List<(string, string)>();

        public static Options SetOptions(string[] args)
        {
            var options = new Options();
            var parser = new Parser(with => with.HelpWriter = null);
            var result = parser.ParseArguments<Options>(args);

            bool hasRandom = false;
            bool hasOtherOptions = false;

            result.WithParsed(o =>
            {
                hasRandom = o.Random;
                options.Timeout = o.Timeout;
                options.Testcount = o.Testcount;
                hasOtherOptions = ParseFilterOptions(o, options);
            })
            .WithNotParsed(HandleParsingErrors);

            if (hasRandom && hasOtherOptions)
            {
                ConsoleLogger.IncompatibleOptions();
                Environment.Exit(1);
            }
            if (options.Timeout < TimeoutManager.MINIMUM_TIMEOUT)
            {
                ConsoleLogger.InvalidTimeout();
                Environment.Exit(1);
            }
            if (options.Testcount <= 0)
            {
                ConsoleLogger.InvalidTestCount();
                Environment.Exit(1);
            }

            options.Random = hasRandom;

            return options;
        }

        private static bool ParseFilterOptions(Options o, Options options)
        {
            bool hasOtherOptions = !(string.IsNullOrEmpty(o.Region) && string.IsNullOrEmpty(o.Plan) && string.IsNullOrEmpty(o.Image) && string.IsNullOrEmpty(o.Category));
            if (hasOtherOptions)
            {
                options.InputOptions = new List<(string, string)>();
                if (!string.IsNullOrEmpty(o.Region))
                {
                    options.Region = o.Region;
                    options.InputOptions.Add(("region", o.Region));
                }
                if (!string.IsNullOrEmpty(o.Category))
                {
                    options.Category = o.Category;
                    options.InputOptions.Add(("category", o.Category));
                }
                if (!string.IsNullOrEmpty(o.Plan))
                {
                    options.Plan = o.Plan;
                    options.InputOptions.Add(("plan", o.Plan));
                }
                if (!string.IsNullOrEmpty(o.Image))
                {
                    options.Image = o.Image;
                    options.InputOptions.Add(("image", o.Image));
                }
                
            }

            return hasOtherOptions;
        }

        private static void HandleParsingErrors(IEnumerable<Error> errors)
        {
            foreach (var error in errors)
            {
                HandleError(error);
            }
            Environment.Exit(1);
        }

        private static void HandleError(Error error)
        {
            switch (error)
            {
                case BadFormatConversionError:
                    ConsoleLogger.BadFormatConversionError();
                    break;
                case UnknownOptionError:
                    ConsoleLogger.UnknownOption();
                    break;
                default:
                    ConsoleLogger.UnexpectedError(error.ToString() ?? "error without message while parsing CLI options.");
                    break;
            }
        }
    }
}
