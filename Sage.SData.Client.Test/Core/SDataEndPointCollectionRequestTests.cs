using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class SDataEndPointCollectionRequestTests : AssertionHelper
    {
        private MockSDataService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new MockSDataService();
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
            var feed = request.Read();
            Expect(feed, Is.Not.Null);
        }
    }
}