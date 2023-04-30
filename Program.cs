using Bezdzione.Data;
namespace Bezdzione
{
    public class Program
    {
        public static void Main()
        {
            PlanList AllPlans = PlanList.GetAllPlans();
            PlanList RegionPlans = AllPlans.GetPlansFromRegion("us_chicago_1");
            PlanList CategoryPlans = AllPlans.GetPlansWithCategory("Shared resources");
            PlanList ImagePlans = AllPlans.GetPlansWithImage("self_install");
            Console.WriteLine(AllPlans.Plans.Count);
            Console.WriteLine(RegionPlans.Plans.Count);
            Console.WriteLine(CategoryPlans.Plans.Count);
            Console.WriteLine(ImagePlans.Plans.Count);
            if (CategoryPlans != null)
            {
                foreach (Plan plan in CategoryPlans.Plans)
                {
                    plan.ShowInfo();
                }
            }
        }
    }
}