using Bezdzione.Data;

namespace BezdzioneTests
{

    [TestFixture]
    public class PlanListTests
    {
        private static PlanList _planList = new();
        
        [SetUp]
        public void Setup()
        {
            _planList = PlanList.GetAllPlans();
        }

        private static IEnumerable<string?> GetValidRegionSlugs()
        {
            RegionList regionList = RegionList.GetAllRegions();
            return regionList.Regions != null ? regionList.Regions.Select(r => r.Slug) : Enumerable.Empty<string?>();
        }

        [Test]
        public void GetAllPlans_ReturnsNonNullObject()
        {
            PlanList plans = _planList;
            Assert.That(plans, Is.Not.Null);
        }

        [Test]
        [TestCaseSource(nameof(GetValidRegionSlugs))]
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
                Assert.Fail("The Plans collection in plansWithCategory should not be null");
            }
        }

        [Test]
        [TestCase(625)]
        public void GetPlan_ReturnsNonNullObject(int planId)
        {
            // Arrange
            PlanList allPlans = _planList;

            // Act
            PlanList plan = allPlans.GetPlanByID(planId);

            // Assert
            Assert.That(plan, Is.Not.Null, $"Failed for plan id {planId}");
        }

        [Test]
        [TestCase(625)]
        public void GetPlan_ReturnsOnePlan(int planId)
        {
            // Arrange
            PlanList allPlans = _planList;

            // Act
            PlanList plan = allPlans.GetPlanByID(planId);

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
            PlanList plan = allPlans.GetPlanByID(planId);

            // Assert
            if (plan.Plans != null)
            {
                Assert.IsTrue(plan.Plans.All(p => p.Id.Equals(planId)));
            }
            else
            {
                Assert.Fail("The Plans collection should not be null");
            }
        }

        [Test]
        [TestCase("lightweight")]
        public void GetPlansWithCategory_ReturnsCorrectPlans(string category)
        {
            // Arrange
            PlanList allPlans = _planList;

            // Act
            PlanList plansWithCategory = allPlans.GetPlansWithCategory(category);

            // Assert
            if (plansWithCategory.Plans != null)
            {
               
                Assert.That(plansWithCategory.Plans.All(plan => plan.Category != null && plan.Category.Equals(category)));
            }
            else
            {
                Assert.Fail("The Plans collection in plansWithCategory should not be null");
            }
        }

        [Test]
        [TestCase("invalid_category")]
        public void GetPlansWithCategory_ReturnsEmptyListWithInvalidCategory(string category)
        {
            // Arrange
            PlanList allPlans = _planList;

            // Act
            PlanList plansWithCategory = allPlans.GetPlansWithCategory(category);

            // Assert
            if (plansWithCategory.Plans != null)
            {
                Assert.That(plansWithCategory.Plans, Is.Empty);
            }
            else
            {
                Assert.Fail("The Plans collection in plansWithCategory should not be null");
            }
        }

        [Test]
        [TestCase("self_install")]
        public void GetPlansWithImage_ReturnsCorrectPlans(string imageSlug)
        {
            // Arrange
            PlanList allPlans = _planList;

            // Act
            PlanList plansWithImage = allPlans.GetPlansWithCategory(imageSlug);

            // Assert
            if (plansWithImage.Plans != null)
            {

                Assert.That(plansWithImage.Plans.All(plan => plan.Category != null && plan.Category.Equals(imageSlug)));
            }
            else
            {
                Assert.Fail("The Plans collection in plansWithImage should not be null");
            }
        }

        [Test]
        [TestCase("invalid_image")]
        public void GetPlansWithImage_ReturnsEmptyListWithInvalidImage(string imageSlug)
        {
            // Arrange
            PlanList allPlans = _planList;

            // Act
            PlanList plansWithImage = allPlans.GetPlansWithCategory(imageSlug);

            // Assert
            if (plansWithImage.Plans != null)
            {
                Assert.That(plansWithImage.Plans, Is.Empty);
            }
            else
            {
                Assert.Fail("The Plans collection in plansWithImage should not be null");
            }
        }
    }
}