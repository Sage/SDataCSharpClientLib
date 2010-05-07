using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class SDataTemplateResourceRequestTests : AssertionHelper
    {
        private MockSDataService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new MockSDataService();
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
            var entry = request.Read();
            Expect(entry, Is.Not.Null);
        }
    }
}