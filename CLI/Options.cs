using CommandLine;
using Bezdzione.Logs;

namespace Bezdzione.CLI
{
    public class Options
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

        [Option("timeout", HelpText = "Timeout in minutes for running tests.")]
        public int? Timeout { get; set; } = null;

        public List<(string, string)> InputOrder { get; set; } = new List<(string, string)>();

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
                ParseFilterOptions(o, options);
            })
            .WithNotParsed(HandleParsingErrors);

            if (hasRandom && hasOtherOptions)
            {
                ConsoleLogger.IncompatibleOptions();
                Environment.Exit(1);
            }

            options.Random = hasRandom;

            return options;
        }

        private static void ParseFilterOptions(Options o, Options options)
        {
            bool hasOtherOptions = !(string.IsNullOrEmpty(o.Region) && string.IsNullOrEmpty(o.Plan) && string.IsNullOrEmpty(o.Image) && string.IsNullOrEmpty(o.Category));
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
            options.Timeout = o.Timeout;
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
                case BadFormatConversionError _:
                    ConsoleLogger.InvalidTimeout();
                    break;
                case UnknownOptionError _:
                    ConsoleLogger.UnknownOption();
                    break;
                default:
                    ConsoleLogger.UnexpectedError(error.ToString() ?? "error without message while parsing CLI options.");
                    break;
            }
        }
    }
}
