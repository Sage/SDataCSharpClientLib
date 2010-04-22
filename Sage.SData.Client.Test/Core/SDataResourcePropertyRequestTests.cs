using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class SDataResourcePropertyRequestTests : AssertionHelper
    {
        private MockSDataService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new MockSDataService();
        }

        [Test]
        public void ResourceProperty_Verify_CanConstruct()
        {
            var request = new SDataResourcePropertyRequest(_service);
            Expect(request, Is.Not.Null);
        }

        [Test]
        public void ResourceProperty_Verify_ToStringWithSingleResourceProperty()
        {
            var request = new SDataResourcePropertyRequest(_service)
                          {
                              ResourceKind = "employees",
                              ResourceSelector = "1",
                              ResourceProperties = {"LoginID"}
                          };

            var url = request.ToString();
            Expect(url, Is.EqualTo("http://localhost:59213/sdata/aw/dynamic/-/employees(1)/LoginID"));
        }

        [Test]
        public void ResourceProperty_Verify_ToStringWithMultipleResourceProperties()
        {
            var b = new SDataResourcePropertyRequest(_service)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "1",
                        ResourceProperties = {"Address", "City"}
                    };

            var url = b.ToString();

            Expect(url, Is.EqualTo("http://localhost:59213/sdata/aw/dynamic/-/employees(1)/Address/City"));
        }

        [Test]
        public void ResourceProperty_Verify_CanReadFeed()
        {
            var request = new SDataResourcePropertyRequest(_service)
                          {
                              ResourceKind = "employee",
                              ResourceSelector = "1",
                              ResourceProperties = {"Contacts"}
                          };

            var feed = request.ReadFeed();
            Expect(feed, Is.Not.Null);
        }

        [Test]
        public void ResourceProperty_Verify_CanReadEntry()
        {
            var request = new SDataResourcePropertyRequest(_service)
                          {
                              ResourceKind = "employees",
                              ResourceSelector = "1",
                              ResourceProperties = {"LoginID"}
                          };

            var entry = request.Read();
            Expect(entry, Is.Not.Null);
        }

        [Test]
        public void ResourceProperty_Verify_CanCreate()
        {
            var request = new SDataResourcePropertyRequest(_service)
                          {
                              ResourceKind = "employees",
                              ResourceSelector = "id = '1234'",
                              ResourceProperties = {"Address", "City"},
                              Entry = new AtomEntry()
                          };

            var entry = request.Create();
            Expect(entry, Is.Not.Null);
        }

        [Test]
        public void ResourceProperty_Verify_CanUpdate()
        {
            var request = new SDataResourcePropertyRequest(_service)
                          {
                              ResourceKind = "employees",
                              ResourceSelector = "id = '1234'",
                              ResourceProperties = {"Address", "City"},
                              Entry = new AtomEntry()
                          };

            var entry = request.Update();
            Expect(entry, Is.Not.Null);
        }

        [Test]
        public void ResourceProperty_Verify_CanDelete()
        {
            var request = new SDataResourcePropertyRequest(_service)
                          {
                              ResourceKind = "employees",
                              ResourceSelector = "1",
                              ResourceProperties = {"Address", "City"}
                          };

            var result = request.Delete();
            Expect(result);
        }
    }
}