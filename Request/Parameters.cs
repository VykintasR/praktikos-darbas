using Bezdzione.Data;
using Newtonsoft.Json;

namespace Bezdzione.Request
{
    public class Parameters
    {
        [JsonProperty("plan")]
        public string? PlanSlug { get; private set; }
        [JsonProperty("image")]
        public string? ImageSlug { get; private set; }
        [JsonProperty("region")]
        public string? RegionSlug { get; private set; }
        [JsonIgnore]
        public string? Category { get; private set; }
        [JsonIgnore]
        public int Timeout { get; private set; }

        public Parameters(int? userTimeout)
        {
            SetDefaultParameters(userTimeout);
        }

        public Parameters(string? regionSlug, string? planSlug, string imageSlug, int timeout)
        {
            SetRegion(regionSlug);
            SetPlan(planSlug);
            SetImage(imageSlug);
            SetTimeout(timeout);
        }

        public void SetDefaultParameters(int? userTimeout)
        {
            Timeout = TimeoutManager.GetDefaultTimeout();
            RegionList AvailableRegions = RegionList.GetAllRegions();
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
                    SetCategory(firstPlan.Category);
                    SetTimeout(userTimeout);

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

        public void SetTimeout(int? userTimeout)
        {
            Timeout = Category == null
                ? userTimeout == null ? TimeoutManager.GetDefaultTimeout() : userTimeout.Value
                : userTimeout == null ? TimeoutManager.GetTimeout(userTimeout, Category) : userTimeout.Value;
        }

        private void SetCategory(string? category)
        {
           Category = category;
        }
    }
}
