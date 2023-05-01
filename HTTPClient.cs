using Bezdzione.Constants;
using RestSharp;

namespace Bezdzione
{
    public static class HTTPClient
    {

        public static RestResponse SendHTTPRequest(string endpointURL, Method method)
        {
            RestClient client = new RestClient(API_URLS.BASE_API_URL);
            RestRequest request = new RestRequest(endpointURL,method);
            request.AddHeader("Authorization", "Bearer " + Configuration.GetSetting("API_KEY"));

            return client.Execute(request);
        }
    }
}
