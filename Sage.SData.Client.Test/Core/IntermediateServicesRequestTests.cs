using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class IntermediateServicesRequestTests : AssertionHelper
    {
        private MockSDataService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new MockSDataService();
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
            var feed = request.Read();
            Expect(feed, Is.Not.Null);
        }
    }
}