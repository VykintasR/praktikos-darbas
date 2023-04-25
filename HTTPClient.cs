using RestSharp;
using Microsoft.Extensions.Configuration;

namespace Bezdzione
{
    internal static class HTTPClient
    {
        const string API_URL = "https://api.cherryservers.com/";
        static ConfigurationBuilder builder = new ConfigurationBuilder();
        static IConfiguration config;

        static HTTPClient() 
        {
            builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            config = builder.Build();
        }

        internal static RestResponse SendHTTPRequest(string endpointURL, Method method)
        {
            RestClient client = new RestClient(API_URL);
            RestRequest request = new RestRequest(endpointURL,method);
            request.AddHeader("Authorization", "Bearer " + config["API_KEY"]);

            return client.Execute(request);
        }
    }
}
