using Moq;
using NUnit.Framework;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class SDataResourcePropertyRequestTests : AssertionHelper
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
            _mock.Setup(s => s.ReadFeed(request)).Returns(TestData.Feed);

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
            _mock.Setup(s => s.ReadEntry(request)).Returns(TestData.Entry);

            var entry = request.Read();
            Expect(entry, Is.Not.Null);
        }

        [Test]
        public void ResourceProperty_Verify_CanCreate()
        {
            var request = new SDataResourcePropertyRequest(_service)
                          {
                              ResourceKind = "employees",
                              ResourceSelector = "id eq '1234'",
                              ResourceProperties = {"Address", "City"},
                              Entry = new AtomEntry()
                          };
            _mock.Setup(s => s.CreateEntry(request, request.Entry)).Returns(TestData.Entry);

            var entry = request.Create();
            Expect(entry, Is.Not.Null);
        }

        [Test]
        public void ResourceProperty_Verify_CanUpdate()
        {
            var request = new SDataResourcePropertyRequest(_service)
                          {
                              ResourceKind = "employees",
                              ResourceSelector = "id eq '1234'",
                              ResourceProperties = {"Address", "City"},
                              Entry = new AtomEntry()
                          };
            _mock.Setup(s => s.UpdateEntry(request, request.Entry)).Returns(TestData.Entry);

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
            _mock.Setup(s => s.DeleteEntry(request, request.Entry)).Returns(true);

            var result = request.Delete();
            Expect(result);
        }
    }
}