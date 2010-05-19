using Moq;
using NUnit.Framework;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class SDataTemplateResourceRequestTests : AssertionHelper
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
        public void TemplateResource_Verify_CanConstruct()
        {
            var request = new SDataTemplateResourceRequest(_service);
            Expect(request, Is.Not.Null);
        }

        [Test]
        public void TemplateResource_Verify_ToString()
        {
            var request = new SDataTemplateResourceRequest(_service) {ResourceKind = "employees"};
            var url = request.ToString();
            Expect(url, Is.EqualTo("http://localhost:59213/sdata/aw/dynamic/-/employees/$template"));
        }

        [Test]
        public void TemplateResource_Verify_CanRead()
        {
            var request = new SDataTemplateResourceRequest(_service) {ResourceKind = "employees"};
            _mock.Setup(s => s.ReadEntry(request)).Returns(TestData.Entry);

            var entry = request.Read();
            Expect(entry, Is.Not.Null);
        }
    }
}