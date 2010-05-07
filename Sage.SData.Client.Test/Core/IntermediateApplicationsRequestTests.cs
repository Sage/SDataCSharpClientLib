using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class IntermediateApplicationsRequestTests : AssertionHelper
    {
        private MockSDataService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new MockSDataService();
        }

        [Test]
        public void IntermediateApplications_Verify_CanContruct()
        {
            var request = new IntermediateApplicationsRequest(_service);
            Expect(request, Is.Not.Null);
        }

        [Test]
        public void IntermediateApplications_Verify_ToString()
        {
            var request = new IntermediateApplicationsRequest(_service);
            var url = request.ToString();
            Expect(url, Is.EqualTo("http://localhost:59213/sdata"));
        }

        [Test]
        public void IntermediateApplications_Verify_CanRead()
        {
            var request = new IntermediateApplicationsRequest(_service);
            var feed = request.Read();
            Expect(feed, Is.Not.Null);
        }
    }
}