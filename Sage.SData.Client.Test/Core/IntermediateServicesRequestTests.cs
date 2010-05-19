using Moq;
using NUnit.Framework;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class IntermediateServicesRequestTests : AssertionHelper
    {
        private Mock<SDataService> _mock;
        private ISDataService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mock = new Mock<SDataService>(MockBehavior.Strict, "http://localhost:59213/sdata/aw/dynamic/-/", "lee", "abc123");
            _service = _mock.Object;
        }

        [Test]
        public void IntermediateServices_Verify_CanContruct()
        {
            var request = new IntermediateServicesRequest(_service);
            Expect(request, Is.Not.Null);
        }

        [Test]
        public void IntermediateServices_Verify_ToString()
        {
            var request = new IntermediateServicesRequest(_service) {ResourceKind = "employees"};
            var url = request.ToString();
            Expect(url, Is.EqualTo("http://localhost:59213/sdata/aw/dynamic/-/employees/$service"));
        }

        [Test]
        public void IntermediateServices_Verify_CanRead()
        {
            var request = new IntermediateServicesRequest(_service) {ResourceKind = "employees"};
            _mock.Setup(s => s.ReadFeed(request)).Returns(TestData.Feed);

            var feed = request.Read();
            Expect(feed, Is.Not.Null);
        }
    }
}