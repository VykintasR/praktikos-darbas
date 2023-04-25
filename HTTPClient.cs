using Bezdzione.Constants;
using RestSharp;

namespace Bezdzione
{
    internal static class HTTPClient
    {

        internal static RestResponse SendHTTPRequest(string endpointURL, Method method)
        {
            RestClient client = new RestClient(API_URLS.BASE_API_URL);
            RestRequest request = new RestRequest(endpointURL,method);
            request.AddHeader("Authorization", "Bearer " + Configuration.GetSetting("API_KEY"));

            return client.Execute(request);
        }
    }
}
