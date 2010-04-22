using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class SDataResourceSchemaRequestTests : AssertionHelper
    {
        private MockSDataService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new MockSDataService();
        }

        [Test]
        public void ResourceSchema_Verify_CanRead()
        {
            var request = new SDataResourceSchemaRequest(_service) {ResourceKind = "employees"};
            var schema = request.Read();
            Expect(schema, Is.Not.Null);
        }
    }
}