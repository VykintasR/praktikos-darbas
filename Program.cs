using Bezdzione.Constants;
using Newtonsoft.Json;
using RestSharp;

namespace Bezdzione
{
    public class Program
    {
        public static void Main()
        {
            //RestResponse response = HTTPClient.SendHTTPRequest(API_URLS.GetRegions", Method.Get);
            RestResponse response = HTTPClient.SendHTTPRequest(API_URLS.GetPlans, Method.Get);

            //Bandymas isvesti planus, ju kategorijas ir galimus regionus
            if (response.IsSuccessful)
            {
                if (response.Content != null)
                {
                    dynamic? data = JsonConvert.DeserializeObject(response.Content);
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            Console.Write(item.slug + ", " + item.category + ", ");

                            foreach(var region in item.available_regions)
                            {
                                Console.Write(region.slug + " ");
                            }
                            Console.WriteLine("");
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