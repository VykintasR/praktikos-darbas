using Bezdzione.Data;
using Bezdzione.Request;

namespace Bezdzione
{
    public class RandomParameterGenerator
    {
        Random random;
        private RegionList AvailableRegions;
        public RandomParameterGenerator() 
        { 
            random = new Random();
            AvailableRegions = RegionList.GetAllRegions();
        }

        public RequestParameters GetRandomParameters(int timeout)
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
                        return new RequestParameters(randomRegion.Slug, randomPlan.Slug, randomImage.Slug, timeout);
                    }   
                }
            }
            
            RequestParameters defaultParameters = new RequestParameters();
            defaultParameters.SetDefaultParameters();
            return defaultParameters;
        }

        private Region? RandomRegion()
        {
            if (AvailableRegions.Regions != null)
            {
                int randomIndex = random.Next(AvailableRegions.Regions.Count);
                return AvailableRegions.Regions.ElementAt(randomIndex);
            }
            return null;
        }

        private Plan? RandomPlan(PlanList regionPlans)
        {
            if (regionPlans.Plans != null)
            {
                int randomIndex = random.Next(regionPlans.Plans.Count);
                return regionPlans.Plans.ElementAt(randomIndex);
            }
            return null;
        }

        private Image? RandomImage(Plan? plan)
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
