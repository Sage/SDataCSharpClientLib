using Moq;
using NUnit.Framework;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class SDataResourceSchemaRequestTests : AssertionHelper
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
        public void ResourceSchema_Verify_CanRead()
        {
            var request = new SDataResourceSchemaRequest(_service) {ResourceKind = "employees"};
            _mock.Setup(s => s.ReadSchema(request)).Returns(TestData.Schema);

            var schema = request.Read();
            Expect(schema, Is.Not.Null);
        }
    }
}