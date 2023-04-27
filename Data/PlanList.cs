using Bezdzione.Constants;
using Newtonsoft.Json;
using RestSharp;

namespace Bezdzione.Data
{
    internal class PlanList
    {
        public List<Plan>? Plans { get; }

        private PlanList()
        {
            Plans = new List<Plan>();
        }
        private PlanList(string jsonContent)
        {
            if (jsonContent != null)
            {
                var deserializedPlans = JsonConvert.DeserializeObject<List<Plan>>(jsonContent);
                Plans = deserializedPlans ?? new List<Plan>();
            }
            else
            {
                Plans = new List<Plan>();
            }
        }

        public static PlanList GetPlans()
        {
            RestResponse response = HTTPClient.SendHTTPRequest(API_URLS.GetPlans, Method.Get);
            string? content = response.Content;

            return content == null ? new PlanList() : new PlanList(content);
        }

        public List<Plan> GetPlansFromRegion(string regionSlug)
        {
            return Plans != null
                ? Plans.Where(plan => plan.Regions != null && plan.Regions.Any(region => region.Slug == regionSlug)).ToList()
                : new List<Plan>();
        }

    }
}
