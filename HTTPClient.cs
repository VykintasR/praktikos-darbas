using Bezdzione.Constants;
using Newtonsoft.Json;
using RestSharp;

namespace Bezdzione
{
    public static class HTTPClient
    {
        public static RestClient client = new RestClient(API_URLS.BASE_API_URL);
        public static RestResponse SendHTTPRequest(string endpointURL, Method method, Server? server = null)
        {
            RestRequest request = new RestRequest(endpointURL,method);
            request.AddHeader("Authorization", "Bearer " + Configuration.GetSetting("API_KEY"));

            if(method == Method.Post)
            {
                string json = JsonConvert.SerializeObject(server);
                request.AddJsonBody(json);
            }
       
            return client.Execute(request);
        }
    }
}
