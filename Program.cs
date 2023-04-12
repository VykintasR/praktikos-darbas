using Newtonsoft.Json;
using RestSharp;
using Microsoft.Extensions.Configuration;

namespace Bezdzione
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration config = builder.Build();

            var client = new RestClient("https://api.cherryservers.com/");
            var request = new RestRequest("v1/regions", Method.Get);
            request.AddHeader("Authorization", "Bearer " + config["API_KEY"]);

            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                List<Region> regions = JsonConvert.DeserializeObject<List<Region>>(response.Content);

                foreach (var r in regions)
                {
                    Console.WriteLine(r.Slug);
                }
                
            }
            else
            {
                Console.WriteLine("Error: " + response.ErrorMessage);
            }

        }
    }
}