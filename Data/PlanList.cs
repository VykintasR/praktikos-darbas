using Bezdzione.Request;
using Newtonsoft.Json;
using RestSharp;

namespace Bezdzione.Data
{
    public class PlanList
    {
        public List<Plan>? Plans { get; set; }

        public PlanList()
        {
            Plans = new List<Plan>();
        }

        private PlanList(List<Plan>? plans)
        {
            Plans = plans;
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

        public static PlanList GetAllPlans()
        {
            RestResponse response = HTTPClient.GetPlans();
            string? content = response.Content;

            return content == null ? new PlanList() : new PlanList(content);
        }

        public PlanList GetPlansFromRegion(string? regionSlug)
        {
            return Plans != null
                ? new PlanList(Plans.Where(plan => plan.Regions != null && plan.Regions.Any(region => region.Slug == regionSlug)).ToList())
                : new PlanList();
        }

        public PlanList GetPlanByID(int id)
        {
            return Plans != null
               ? new PlanList(Plans.Where(plan => plan.Id.Equals(id)).ToList())
               : new PlanList();
        }

        public PlanList GetPlanBySlug(string slug)
        {
            return Plans != null
               ? new PlanList(Plans.Where(plan => plan.Slug.Equals(slug)).ToList())
               : new PlanList();
        }

        public PlanList GetPlansWithCategory(string category) 
        {
            return Plans != null
               ? new PlanList(Plans.Where(plan => plan.Category != null && plan.Category.Equals(category)).ToList())
               : new PlanList();
        }

        public PlanList GetPlansWithImage(string imageSlug)
        {
            return Plans != null
               ? new PlanList(Plans.Where(plan => plan.Images != null && plan.Images.Any(image => image.Slug == imageSlug)).ToList())
               : new PlanList();
        }

        public PlanList FilterPlansByOption(PlanList plans, string option, string value)
        {
            switch (option)
            {
                case "region":
                    return plans.GetPlansFromRegion(value);
                case "category":
                    return plans.GetPlansWithCategory(value);
                case "image":
                    return plans.GetPlansWithImage(value);
                case "plan":
                    return plans.GetPlanBySlug(value);
                default:
                    return plans;
            }
        }

    }
}
