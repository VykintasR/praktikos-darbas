using Bezdzione.Data;
using Bezdzione.Constants;
using RestSharp;
using Newtonsoft.Json;
using Bezdzione.Logs;

namespace Bezdzione
{
    public class Server
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public string? Name { get; set; }
        [JsonIgnore]
        public string? CreatedRegion { get; set; }
        [JsonIgnore]    
        public string? CreatedPlan { get; set;}
        [JsonIgnore]
        public string? CreatedImage { get; set;}
        [JsonProperty("plan")]
        public string? RequestPlanSlug { get; set; }
        [JsonProperty("image")]
        public string? RequestImageSlug { get; set; }
        [JsonProperty("region")]
        public string? RequestRegionSlug { get; set; }
      
        public Server() 
        {
            RegionList allRegions = RegionList.GetAllRegions();
            if(allRegions.Regions != null)
            {
                RequestRegionSlug = allRegions.Regions.ElementAt(0).Slug;
            }

            if (RequestRegionSlug != null)
            {
                PlanList regionPlans = PlanList.GetAllPlans().GetPlansFromRegion(RequestRegionSlug);

                if (regionPlans.Plans != null)
                {
                    Plan firstPlan = regionPlans.Plans.ElementAt(0);
                    RequestPlanSlug = firstPlan.Slug;

                    if (firstPlan.Images != null && firstPlan.Images.ElementAt(1) != null)
                    {
                        RequestImageSlug = firstPlan.Images.ElementAt(0).Slug;
                    }
                }
            }
        }

        public RestResponse DeployServer()
        {
            Logger.LogInfo(MessageFormatter.FormatServerRequest(this));
            RestResponse response = HTTPClient.SendHTTPRequest(API_URLS.RequestServer, Method.Post, this);
            var responseJson = response.Content;
            if (responseJson != null)
            {
                dynamic? responseObj = JsonConvert.DeserializeObject(responseJson);

                if (responseObj != null)
                {
                    this.Id = responseObj.id;
                    this.Name = responseObj.hostname;
                    this.CreatedPlan = responseObj.name;
                    this.CreatedRegion = responseObj.region.slug;
                    this.CreatedImage = responseObj.image;
                }
            }
            return response;
        }

        public string GetServerState()
        {
            RestResponse response = HTTPClient.SendHTTPRequest(string.Format(API_URLS.RetrieveServerInfo, this.Id), Method.Get);
            var responseJson = response.Content;
            if (responseJson != null)
            {
                dynamic? responseObj = JsonConvert.DeserializeObject(responseJson);

                return responseObj != null ? (string)responseObj.state : "";
            }
            return "";
        }

        public string CheckImageAfterActivation()
        {
            RestResponse response = HTTPClient.SendHTTPRequest(string.Format(API_URLS.RetrieveServerInfo, this.Id), Method.Get);
            var responseJson = response.Content;
            if (responseJson != null)
            {
                dynamic? responseObj = JsonConvert.DeserializeObject(responseJson);

                return responseObj != null ? (string)responseObj.image : "";
            }
            return "";
        }

        public RestResponse DeleteServer(Server server)
        {
            return  HTTPClient.SendHTTPRequest(string.Format(API_URLS.DeleteServer, server.Id), Method.Delete);
        }
    }
}
