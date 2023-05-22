using RestSharp;
using Bezdzione.Logging;
using Newtonsoft.Json;
using Bezdzione.Data;

namespace Bezdzione.Request
{
    public class Server
    {
        public int Id { get; private set; }
        public string? Name { get; private set; }
        public string? Region { get; private set; }
        public string? Plan { get; private set; }
        public string? Image { get; private set; }
        public RequestParameters Parameters { get; private set; }
        public string? State { get; private set; }

        public Server(PlanList allPlans)
        {
            Parameters = new RequestParameters(null, allPlans);
            Parameters.SetDefaultParameters(null, allPlans);
        }
        public Server(RequestParameters parameters)
        {
            Parameters = parameters;
        }

        public dynamic? Deploy(Result testResult)
        {
            string parameterInfo = $"region: {Parameters.RegionSlug}, plan: {Parameters.PlanSlug}, OS image: {Parameters.ImageSlug}";
            RestResponse response = HTTPClient.DeployServer(Parameters);

            testResult.HTTPStatusCode = (int)response.StatusCode;
            if (!response.IsSuccessStatusCode)
            {
                testResult.DeploymentMessage = response.ErrorMessage ?? "Unknown error when trying to deploy the server.";
            }

            var responseJson = response.Content;
            if (responseJson != null)
            {
                dynamic? responseObj = JsonConvert.DeserializeObject(responseJson);

                if (responseObj != null)
                {
                    Id = responseObj.id;
                    FileLogger.Log(MessageFormatter.Info(MessageFormatter.RequestInfo(Parameters)));
                    return responseObj;
                }
                else
                {
                    FileLogger.Log(MessageFormatter.Error($"Failed to deploy Server with {parameterInfo}."));
                    return null;
                }
            }
            return null;
        }

        public string GetState()
        {
            return HTTPClient.GetServerState(Id);
        }

        public dynamic? UpdateAndGetInfo()
        {
            dynamic? responseObj = HTTPClient.GetServerInfo(Id);
            if (responseObj != null)
            {
                Name = responseObj.hostname;
                Plan = responseObj.name;
                Region = responseObj.region.slug;
                Image = responseObj.image;
                FileLogger.Log(MessageFormatter.ResponseInfo(responseObj));
                return responseObj;
            }
            else
            {
                FileLogger.Log(MessageFormatter.Error("Failed to retrieve Server info."));
            }
            return responseObj;
        }

        public RestResponse Delete()
        {
            FileLogger.Log(MessageFormatter.Info($"Sending request to delete Server {Name}."));
            return HTTPClient.DeleteServer(Id);
        }
    }
}
