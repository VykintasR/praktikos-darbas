using Bezdzione.Data;
using RestSharp;

namespace Bezdzione
{
    public class Program
    {
        public static void Main()
        {
            Server defaultServer = new Server();

            RestResponse response = defaultServer.Request();

            Console.WriteLine(response.StatusCode);

        }
    }
}