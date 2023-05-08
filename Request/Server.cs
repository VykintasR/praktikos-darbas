using RestSharp;
using Bezdzione.Logs;
using Newtonsoft.Json;

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

        public Server()
        {
            Parameters = new RequestParameters();
            Parameters.SetDefaultParameters();
        }

        public Server(RequestParameters parameters)
        {
            Parameters = parameters;
        }

        public void Deploy()
        {
            Id = HTTPClient.DeployServer(Parameters);
            FileLogger.Log(Id == 0 ? MessageFormatter.Error("Failed to deploy server.") : MessageFormatter.Info(MessageFormatter.RequestInfo(Parameters)));
        }

        public string GetState()
        {
            return HTTPClient.GetServerState(Id);
        }

        public void UpdateInfo()
        {
            RestResponse response = HTTPClient.GetServerInfo(Id);
            var responseJson = response.Content;
            var responseObj = responseJson != null ? JsonConvert.DeserializeObject(responseJson) : (dynamic?)null;
            if (responseObj != null)
            {
                Name = responseObj.hostname;
                Plan = responseObj.name;
                Region = responseObj.region.slug;
                Image = responseObj.image;
                FileLogger.Log(MessageFormatter.ResponseInfo(responseObj, response.StatusCode));
            }
            else
            {
                FileLogger.Log(MessageFormatter.Error("Failed to retrieve server info."));
            }
        }

        public RestResponse Delete()
        {
            FileLogger.Log(MessageFormatter.Info($"Sending request to delete server {Name}."));
            return HTTPClient.DeleteServer(Id);
        }
    }
}
