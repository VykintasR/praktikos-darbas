using Newtonsoft.Json;
using RestSharp;

namespace Bezdzione
{
    public class Program
    {
        public static void Main()
        {
            RestResponse response = HTTPClient.SendHTTPRequest("v1/regions", Method.Get);

            if (response.IsSuccessful)
            {
                if (response.Content != null)
                {
                    dynamic? data = JsonConvert.DeserializeObject(response.Content);
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            var value = item.slug;
                            Console.WriteLine(value);
                        }
                    }
                }             
            }
            else
            {
                Console.WriteLine("Error: " + response.ErrorMessage);
            }

        }
    }
}