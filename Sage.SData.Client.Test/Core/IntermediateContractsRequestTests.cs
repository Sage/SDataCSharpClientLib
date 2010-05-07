using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class IntermediateContractsRequestTests : AssertionHelper
    {
        private MockSDataService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new MockSDataService();
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
            var feed = request.Read();
            Expect(feed, Is.Not.Null);
        }
    }
}