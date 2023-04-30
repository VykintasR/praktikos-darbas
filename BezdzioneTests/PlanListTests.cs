using Bezdzione.Data;

namespace BezdzioneTests
{

    [TestFixture]
    public class PlanListTests
    {
        private PlanList _planList;

        [SetUp]
        public void Setup()
        {
            _planList = PlanList.GetAllPlans();
        }
        [Test]
        public void GetAllPlans_ReturnsNonNullObject()
        {
            PlanList plans = _planList;
            Assert.IsNotNull(plans);
        }

        [Test]
        [TestCase("eu_nord_1")]
        [TestCase("eu_west_1")]
        [TestCase("us_chicago_1")]
        public void GetPlansFromRegion_ReturnsCorrectPlans(string regionSlug)
        {
            // Arrange
            PlanList allPlans = _planList;

            // Act
            PlanList plansFromRegion = allPlans.GetPlansFromRegion(regionSlug);

            // Assert
            if (plansFromRegion.Plans != null)
            {
                Assert.That(plansFromRegion.Plans.All(plan => plan.Regions != null && plan.Regions.Any(region => region.Slug == regionSlug)));
            }
            else
            {
                Assert.Fail("The Plans collection in plansFromRegion should not be null");
            }
        }

        [Test]
        [TestCase("invalid_region")]
        public void GetPlansFromRegion_ReturnsEmptyListWithInvalidRegion(string regionSlug)
        {
            // Arrange
            PlanList allPlans = _planList;

            // Act
            PlanList plansFromRegion = allPlans.GetPlansFromRegion(regionSlug);

            // Assert
            if (plansFromRegion.Plans != null)
            {
                Assert.That(plansFromRegion.Plans, Is.Empty);
            }
            else
            {
                Assert.Fail("The Plans collection in plansFromRegion should not be null");
            }
        }
        [Test]
        [TestCase(625)]
        public void GetPlan_ReturnsNonNullObject(int planId)
        {
            // Arrange
            PlanList allPlans = _planList;

            // Act
            PlanList plan = allPlans.GetPlan(planId);

            //Assert
            Assert.That(plan, Is.Not.Null);
        }

        [Test]
        [TestCase(625)]
        public void GetPlan_ReturnsOnePlan(int planId)
        {
            // Arrange
            PlanList allPlans = _planList;

            // Act
            PlanList plan = allPlans.GetPlan(planId);

            // Assert
            if (plan.Plans != null)
            {
                Assert.That(plan.Plans, Has.Count.EqualTo(1));
            }
            else
            {
                Assert.Fail("The Plans collection should not be null");
            }
        }

        [Test]
        [TestCase(625)]
        public void GetPlan_ReturnsCorrectPlan(int planId)
        {
            // Arrange
            PlanList allPlans = _planList;

            // Act
            PlanList plan = allPlans.GetPlan(planId);

            // Assert
            if (plan.Plans != null)
            {
                Assert.IsTrue(plan.Plans.All(p => p.Slug != null && p.Slug.Equals(planId)));
            }
            else
            {
                Assert.Fail("The Plans collection should not be null");
            }
        }
    }
}