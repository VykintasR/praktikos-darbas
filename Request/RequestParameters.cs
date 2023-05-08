using Bezdzione.Data;
using Newtonsoft.Json;

namespace Bezdzione.Request
{
    public class RequestParameters
    {
        [JsonIgnore]
        RegionList AvailableRegions;
        [JsonProperty("plan")]
        public string? PlanSlug { get; private set; }
        [JsonProperty("image")]
        public string? ImageSlug { get; private set; }
        [JsonProperty("region")]
        public string? RegionSlug { get; private set; }

        public RequestParameters()
        {
            AvailableRegions = RegionList.GetAllRegions();
        }

        public RequestParameters(string? regionSlug, string? planSlug, string imageSlug)
        {
            AvailableRegions = RegionList.GetAllRegions();
            SetRegion(regionSlug);
            SetPlan(planSlug);
            SetImage(imageSlug);
        }

        public void SetDefaultParameters()
        {
            if (AvailableRegions.Regions != null)
            {
                SetRegion(AvailableRegions.Regions.ElementAt(0).Slug);
            }

            if (RegionSlug != null)
            {
                PlanList regionPlans = PlanList.GetAllPlans().GetPlansFromRegion(RegionSlug);

                if (regionPlans.Plans != null)
                {
                    Plan firstPlan = regionPlans.Plans.ElementAt(0);
                    SetPlan(firstPlan.Slug);

                    if (firstPlan.Images != null && firstPlan.Images.ElementAt(1) != null)
                    {
                        SetImage(firstPlan.Images.ElementAt(0).Slug);
                    }
                }
            }
        }

        public void SetRegion(string? regionSlug)
        {
            RegionSlug = regionSlug;
        }
        public void SetPlan(string? planSlug)
        {
            PlanSlug = planSlug;
        }
        public void SetImage(string? imageSlug)
        {
            ImageSlug = imageSlug;
        }
    }
}
