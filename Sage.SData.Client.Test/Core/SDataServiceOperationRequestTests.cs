using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class SDataServiceOperationRequestTests : AssertionHelper
    {
        private MockSDataService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new MockSDataService();
        }

        [Test]
        public void ServiceOperation_Verify_ToString()
        {
            var request = new SDataServiceOperationRequest(_service)
                          {
                              ResourceKind = "employees",
                              OperationName = "getStats"
                          };
            var url = request.ToString();
            Expect(url, Is.EqualTo("http://localhost:59213/sdata/aw/dynamic/-/employees/$service/getStats"));
        }
    }
}