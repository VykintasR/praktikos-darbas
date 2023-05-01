using Bezdzione.Data;

namespace BezdzioneTests
{
    [TestFixture]
    public class RegionListTests
    {
        private static RegionList _regionList = new();

        [SetUp]
        public void Setup()
        {
            _regionList = RegionList.GetAllRegions();
        }

        [Test]
        public void GetAllRegions_ReturnsNonNullObject()
        {
            RegionList regions = _regionList;
            Assert.That(regions, Is.Not.Null);
        }
    }
}
