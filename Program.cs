using Bezdzione.Data;
namespace Bezdzione
{
    public class Program
    {
        public static void Main()
        {
            PlanList AllPlans = PlanList.GetPlans();
            List<Plan> RegionPlans = AllPlans.GetPlansFromRegion("us_chicago_1");

            Console.WriteLine(RegionPlans.Count);
            Console.WriteLine(AllPlans.Plans.Count);
            if (RegionPlans != null)
            {
                foreach (Plan plan in RegionPlans)
                {
                    plan.ShowInfo();
                }
            }       
        }
    }
}