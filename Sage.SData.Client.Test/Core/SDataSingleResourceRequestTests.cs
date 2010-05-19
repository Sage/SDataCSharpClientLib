using System;
using System.Xml;
using Moq;
using NUnit.Framework;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class SDataSingleResourceRequestTests : AssertionHelper
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
        public void SingleResource_Verify_CanConstruct()
        {
            var request = new SDataSingleResourceRequest(_service);
            Expect(request, Is.Not.Null);
        }

        [Test]
        public void SingleResource_Verify_CanConstructWithTemplateEntry()
        {
            var entry = new AtomEntry();
            var request = new SDataSingleResourceRequest(_service, entry);
            Expect(request, Is.Not.Null);
            Expect(request.Entry, Is.Not.Null);
        }

        [Test]
        public void SingleResource_Verify_ToStringWithResourceKind()
        {
            var request = new SDataSingleResourceRequest(_service)
                          {
                              ResourceKind = "employees",
                              ResourceSelector = "id = '1234'"
                          };

            var url = request.ToString();
            Expect(url, Is.EqualTo("http://localhost:59213/sdata/aw/dynamic/-/employees(id = '1234')"));
        }

        [Test]
        public void SingleResource_Verify_ToStringWithResourceKindAndInclude()
        {
            var request = new SDataSingleResourceRequest(_service)
                          {
                              ResourceKind = "employees",
                              ResourceSelector = "1",
                              Include = "contact"
                          };

            var url = request.ToString();
            Expect(url, Is.EqualTo("http://localhost:59213/sdata/aw/dynamic/-/employees(1)?include=contact"));
        }

        [Test]
        public void SingleResource_Verify_CanProcess_SDataBatchRequest()
        {
            var request1 = new SDataSingleResourceRequest(_service)
                           {
                               ResourceKind = "employees",
                               ResourceSelector = "1"
                           };
            var request2 = new SDataSingleResourceRequest(_service)
                           {
                               ResourceKind = "employees",
                               ResourceSelector = "2"
                           };
            var request3 = new SDataSingleResourceRequest(_service)
                           {
                               ResourceKind = "employees",
                               ResourceSelector = "3"
                           };
            _mock.Setup(s => s.ReadEntry(request1)).Returns(TestData.Entry);
            _mock.Setup(s => s.ReadEntry(request2)).Returns(TestData.Entry);
            _mock.Setup(s => s.ReadEntry(request3)).Returns(TestData.Entry);

            request2.Entry = request2.Read();
            request3.Entry = request3.Read();

            _mock.Setup(s => s.UpdateEntry(request2, request2.Entry)).Returns(TestData.Entry);
            _mock.Setup(s => s.DeleteEntry(request3, request3.Entry)).Returns(true);

            var payload2 = request2.Entry.GetSDataPayload();
            payload2.Values["MaritalStatus"] = "Married";

            AtomFeed batchfeed;

            using (var batch = new SDataBatchRequest(_service))
            {
                batch.ResourceKind = "employees";
                request1.Read();
                request2.Update();
                request3.Delete();

                _mock.Setup(s => s.CreateFeed(batch, It.IsAny<AtomFeed>())).Returns(TestData.Feed);
                batchfeed = batch.Commit();
            }

            Expect(batchfeed, Is.Not.Null);
        }

        [Test]
        public void SingleResource_Verify_CanRead()
        {
            var request = new SDataSingleResourceRequest(_service)
                          {
                              ResourceKind = "employees",
                              ResourceSelector = "1"
                          };
            _mock.Setup(s => s.ReadEntry(request)).Returns(TestData.Entry);

            var entry = request.Read();
            Expect(entry, Is.Not.Null);
        }

        [Test]
        public void SingleResource_Verify_CanCreate()
        {
            var request = new SDataSingleResourceRequest(_service) {ResourceKind = "employees"};

            var payload = new SDataPayload();
            payload.Values["Title"] = "create 1";
            payload.Values["NationalIdNumber"] = "44444";
            payload.Values["LoginId"] = "create 4";
            payload.Values["ContactId"] = "9999";
            payload.Values["BirthDate"] = SyndicationDateTimeUtility.ToRfc3339DateTime(new DateTime(1970, 8, 2));
            payload.Values["HireDate"] = SyndicationDateTimeUtility.ToRfc3339DateTime(DateTime.Now);
            payload.Values["ModifiedDate"] = SyndicationDateTimeUtility.ToRfc3339DateTime(DateTime.Now);
            payload.Values["MaritalStatus"] = "Single";
            payload.Values["SalariedFlag"] = XmlConvert.ToString(true);
            payload.Values["CurrentFlag"] = XmlConvert.ToString(true);
            payload.Values["Gender"] = "Male";
            payload.Values["RowGuid"] = Guid.NewGuid().ToString();

            var entry = new AtomEntry
                        {
                            UpdatedOn = DateTime.Now,
                            PublishedOn = DateTime.Now
                        };
            entry.SetSDataPayload(payload);
            request.Entry = entry;
            _mock.Setup(s => s.CreateEntry(request, request.Entry)).Returns(TestData.Entry);

            entry = request.Create();
            Expect(entry, Is.Not.Null);
        }

        [Test]
        public void SingleResource_Verify_CanUpdate()
        {
            var request = new SDataSingleResourceRequest(_service)
                          {
                              ResourceKind = "employees",
                              ResourceSelector = "1"
                          };
            _mock.Setup(s => s.ReadEntry(request)).Returns(TestData.Entry);

            var entry = request.Read();
            var payload = entry.GetSDataPayload();
            payload.Values["Title"] = "test update";
            request.Entry = entry;
            _mock.Setup(s => s.UpdateEntry(request, request.Entry)).Returns(TestData.Entry);

            entry = request.Update();
            Expect(entry, Is.Not.Null);
        }

        [Test]
        public void SingleResource_Verify_CanDelete()
        {
            var request = new SDataSingleResourceRequest(_service)
                          {
                              ResourceKind = "employees",
                              ResourceSelector = "1"
                          };
            _mock.Setup(s => s.ReadEntry(request)).Returns(TestData.Entry);

            request.Entry = request.Read();

            _mock.Setup(s => s.DeleteEntry(request, request.Entry)).Returns(true);

            var result = request.Delete();
            Expect(result);
        }
    }
}