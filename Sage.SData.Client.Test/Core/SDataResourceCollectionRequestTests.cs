using Moq;
using NUnit.Framework;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class SDataResourceCollectionRequestTests : AssertionHelper
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
        public void ResourceCollection_Verify_CanConstruct()
        {
            var request = new SDataResourceCollectionRequest(_service);
            Expect(request, Is.Not.Null);
        }

        [Test]
        public void ResourceCollection_Verify_ToStringWithPaging()
        {
            var request = new SDataResourceCollectionRequest(_service)
                          {
                              ResourceKind = "employees",
                              StartIndex = 1,
                              Count = 100
                          };
            var url = request.ToString();
            Expect(url, Is.EqualTo("http://localhost:59213/sdata/aw/dynamic/-/employees?startIndex=1&count=100"));
        }

        [Test]
        public void ResourceCollection_Verify_ToStringWithQuery()
        {
            var request = new SDataResourceCollectionRequest(_service)
                          {
                              ResourceKind = "employees",
                              QueryValues = {{"where", "gender eq m"}}
                          };
            var url = request.ToString();
            Expect(url, Is.EqualTo("http://localhost:59213/sdata/aw/dynamic/-/employees?where=gender eq m"));
        }

        [Test]
        public void ResourceCollection_Verify_ToStringWithQuery_MultipleValues()
        {
            var request = new SDataResourceCollectionRequest(_service)
                          {
                              ResourceKind = "employees",
                              QueryValues =
                                  {
                                      {"where", "gender eq m"},
                                      {"orderBy", "orderDate DESC"}
                                  }
                          };
            var url = request.ToString();
            Expect(url, Is.EqualTo("http://localhost:59213/sdata/aw/dynamic/-/employees?where=gender eq m&orderBy=orderDate DESC"));
        }

        [Test]
        public void ResourceCollection_Verify_CanRead()
        {
            var request = new SDataResourceCollectionRequest(_service) {ResourceKind = "employees"};
            _mock.Setup(s => s.ReadFeed(request)).Returns(TestData.Feed);

            var feed = request.Read();
            Expect(feed, Is.Not.Null);
        }
    }
}