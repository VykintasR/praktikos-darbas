using Bezdzione.Data;
using Bezdzione.Constants;
using RestSharp;
using Newtonsoft.Json;
using NUnit.Framework.Internal;

namespace Bezdzione
{
    public class Server
    {

        [JsonProperty("plan")]
        public string? PlanSlug { get; set; }
        [JsonProperty("image")]
        public string? ImageSlug { get; set; }
        [JsonProperty("region")]
        public string? RegionSlug { get; set; }
      
        public Server() 
        {
            RegionList allRegions = RegionList.GetAllRegions();
            if(allRegions.Regions != null)
            {
                RegionSlug = allRegions.Regions.ElementAt(1).Slug;
            }

            if (RegionSlug != null)
            {
                PlanList regionPlans = PlanList.GetAllPlans().GetPlansFromRegion(RegionSlug);

                if (regionPlans.Plans != null)
                {
                    Plan firstPlan = regionPlans.Plans.ElementAt(1);
                    PlanSlug = firstPlan.Slug;

                    if (firstPlan.Images != null && firstPlan.Images.ElementAt(1) != null)
                    {
                        ImageSlug = firstPlan.Images.ElementAt(1).Slug;
                    }
                }
            }
        }

        public RestResponse Request()
        {
            return HTTPClient.SendHTTPRequest(API_URLS.RequestServer, Method.Post, this);
        }
    }
}
