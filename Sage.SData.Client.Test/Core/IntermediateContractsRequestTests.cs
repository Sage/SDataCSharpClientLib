using Moq;
using NUnit.Framework;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class IntermediateContractsRequestTests : AssertionHelper
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
        public void IntermediateContracts_Verify_CanConstruct()
        {
            var request = new IntermediateContractsRequest(_service);
            Expect(request, Is.Not.Null);
        }

        [Test]
        public void IntermediateContracts_Verify_ToString()
        {
            var request = new IntermediateContractsRequest(_service);
            var url = request.ToString();
            Expect(url, Is.EqualTo("http://localhost:59213/sdata/aw"));
        }

        [Test]
        public void IntermediateContracts_Verify_CanRead()
        {
            var request = new IntermediateContractsRequest(_service);
            _mock.Setup(s => s.ReadFeed(request)).Returns(TestData.Feed);

            var feed = request.Read();
            Expect(feed, Is.Not.Null);
        }
    }
}