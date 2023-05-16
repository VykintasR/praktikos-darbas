﻿using RestSharp;
using Bezdzione.Logs;
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

        public int Deploy()
        {
            string parameterInfo = $"region: {Parameters.RegionSlug}, plan: {Parameters.PlanSlug}, OS image: {Parameters.ImageSlug}";
            Id = HTTPClient.DeployServer(Parameters);
            FileLogger.Log(Id == 0 ? MessageFormatter.Error($"Failed to deploy Server with {parameterInfo}.") : MessageFormatter.Info(MessageFormatter.RequestInfo(Parameters)));
            return Id;
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
                FileLogger.Log(MessageFormatter.Error("Failed to retrieve Server info."));
            }
        }

        public RestResponse Delete()
        {
            FileLogger.Log(MessageFormatter.Info($"Sending request to delete Server {Name}."));
            return HTTPClient.DeleteServer(Id);
        }
    }
}
