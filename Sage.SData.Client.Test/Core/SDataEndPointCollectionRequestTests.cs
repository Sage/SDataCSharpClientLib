using Moq;
using NUnit.Framework;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class SDataEndPointCollectionRequestTests : AssertionHelper
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
        public void EndPointCollection_Verify_Url()
        {
            var request = new SDataEndPointCollectionRequest(_service);
            var url = request.ToString();
            Expect(url, Is.EqualTo("http://localhost:59213/sdata/$system/registry/endpoints"));
        }

        [Test]
        public void EndPointCollection_Verify_CanRead()
        {
            var request = new SDataEndPointCollectionRequest(_service);
            _mock.Setup(s => s.ReadFeed(request)).Returns(TestData.Feed);

            var feed = request.Read();
            Expect(feed, Is.Not.Null);
        }
    }
}