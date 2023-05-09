using Bezdzione.Constants;
using Newtonsoft.Json;
using RestSharp;

namespace Bezdzione.Request
{
    public static class HTTPClient
    {
        private static RestClient client = new RestClient(API_URLS.BASE_API_URL);
        private static RestResponse SendHTTPRequest(string endpointURL, Method method, RequestParameters? parameters = null)
        {
            RestRequest request = new(endpointURL, method);
            request.AddHeader("Authorization", "Bearer " + Configuration.GetSetting("API_KEY"));

            if (method == Method.Post)
            {
                string json = JsonConvert.SerializeObject(parameters);
                request.AddJsonBody(json);
            }

            return client.Execute(request);
        }

        public static RestResponse GetPlans()
        {
            return SendHTTPRequest(API_URLS.GetPlans, Method.Get);
        }

        public static RestResponse GetRegions()
        {
            return SendHTTPRequest(API_URLS.GetRegions, Method.Get);
        }

        public static RestResponse GetPlanImages(string? Slug)
        {
            return SendHTTPRequest(string.Format(API_URLS.GetPlanImages, Slug), Method.Get);
        }

        public static int DeployServer(RequestParameters parameters)
        {
            RestResponse response = SendHTTPRequest(API_URLS.RequestServer, Method.Post, parameters);
            if (!response.IsSuccessful && response.Content != null)
            {
                Console.WriteLine(response.Content);
            }
                    

            
            var responseJson = response.Content;
            if (responseJson != null)
            {
                dynamic? responseObj = JsonConvert.DeserializeObject(responseJson);

                if (responseObj != null)
                {
                    return responseObj.id;
                }
            }
            return 0;
        }

        public static string GetServerState(int id)
        {
            RestResponse response = SendHTTPRequest(string.Format(API_URLS.RetrieveServerInfo, id), Method.Get);
            var responseJson = response.Content;
            if (responseJson != null)
            {
                dynamic? responseObj = JsonConvert.DeserializeObject(responseJson);

                return responseObj != null ? (string)responseObj.state : "";
            }
            return "";
        }

        public static RestResponse GetServerInfo(int id)
        {
            return SendHTTPRequest(string.Format(API_URLS.RetrieveServerInfo, id), Method.Get);
        }

        public static RestResponse DeleteServer(int id)
        {
            return SendHTTPRequest(string.Format(API_URLS.DeleteServer, id), Method.Delete);
        }

    }
}
