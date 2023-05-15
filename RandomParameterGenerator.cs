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

        public static Parameters GetRandomParameters(int timeout)
        {
            
            PlanList allPlans = PlanList.GetAllPlans();

            Region? randomRegion = RandomRegion();
            if (randomRegion != null)
            {
                Plan? randomPlan = RandomPlan(allPlans.GetPlansFromRegion(randomRegion.Slug));
                if (randomPlan != null)
                {
                    Image? randomImage = RandomImage(randomPlan);
                    if (randomImage != null)
                    {
                        return new Parameters(randomRegion.Slug, randomPlan.Slug, randomImage.Slug, timeout);
                    }   
                }
            }
            
            Parameters defaultParameters = new Parameters();
            defaultParameters.SetDefaultParameters();
            return defaultParameters;
        }

        private static Region? RandomRegion()
        {
            if (AvailableRegions.Regions != null)
            {
                int randomIndex = random.Next(AvailableRegions.Regions.Count);
                return AvailableRegions.Regions.ElementAt(randomIndex);
            }
            return null;
        }

        private static Plan? RandomPlan(PlanList regionPlans)
        {
            if (regionPlans.Plans != null)
            {
                int randomIndex = random.Next(regionPlans.Plans.Count);
                return regionPlans.Plans.ElementAt(randomIndex);
            }
            return null;
        }

        private static Image? RandomImage(Plan? plan)
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
