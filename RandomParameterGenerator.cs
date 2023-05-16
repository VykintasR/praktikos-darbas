using Bezdzione.Data;
using Bezdzione.Request;

namespace Bezdzione
{
    public static class RandomParameterGenerator
    {
        static Random random;
        private static RegionList AvailableRegions;
        static RandomParameterGenerator() 
        { 
            random = new Random();
            AvailableRegions = RegionList.GetAllRegions();
        }

        public static RequestParameters GetRandomParameters(int? userTimeout, PlanList allPlans)
        {
            Region? randomRegion = RandomRegion();
            if (randomRegion != null)
            {
                Plan? randomPlan = RandomPlan(allPlans.GetPlansFromRegion(randomRegion.Slug));
                if (randomPlan != null)
                {
                    int timeout = randomPlan.Category != null ? TimeoutManager.GetTimeout(userTimeout, randomPlan.Category) : TimeoutManager.DEFAULT_TIMEOUT;
                    Image? randomImage = RandomImage(randomPlan);
                    if (randomImage != null)
                    {
                        return new RequestParameters(randomRegion.Slug, randomPlan.Slug, randomImage.Slug, timeout);
                    }   
                }
            }
            
            RequestParameters defaultParameters = new RequestParameters(userTimeout, allPlans);
            defaultParameters.SetDefaultParameters(userTimeout, allPlans);
            return defaultParameters;
        }

        public static Region? RandomRegion()
        {
            if (AvailableRegions.Regions != null)
            {
                int randomIndex = random.Next(AvailableRegions.Regions.Count);
                return AvailableRegions.Regions.ElementAt(randomIndex);
            }
            return null;
        }
        public static Region? RandomRegionFromPlan(Plan plan)
        {
            if (plan.Regions != null)
            {
                int randomIndex = random.Next(plan.Regions.Count);
                return plan.Regions.ElementAt(randomIndex);
            }
            return null;
        }

        public static Plan? RandomPlan(PlanList regionPlans)
        {
            if (regionPlans.Plans != null)
            {
                int randomIndex = random.Next(regionPlans.Plans.Count);
                return regionPlans.Plans.ElementAt(randomIndex);
            }
            return null;
        }

        public static Image? RandomImage(Plan? plan)
        {
            if (plan != null && plan.Images != null)
            {
                int randomIndex = random.Next(plan.Images.Count);
                return plan.Images.ElementAt(randomIndex);
            }
            return null;
        }
    }
}
