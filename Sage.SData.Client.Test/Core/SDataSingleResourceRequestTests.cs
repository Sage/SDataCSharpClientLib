using System;
using System.Xml;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class SDataSingleResourceRequestTests : AssertionHelper
    {
        private MockSDataService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new MockSDataService();
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

            request2.Entry = request2.Read();
            request3.Entry = request3.Read();

            var payload2 = request2.Entry.GetSDataPayload();
            payload2.Values["MaritalStatus"] = "Married";

            AtomFeed batchfeed;

            using (var batch = new SDataBatchRequest(_service))
            {
                batch.ResourceKind = "employees";
                request1.Read();
                request2.Update();
                request3.Delete();

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

            var entry = request.Read();
            var payload = entry.GetSDataPayload();
            payload.Values["Title"] = "test update";
            request.Entry = entry;

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

            request.Entry = request.Read();
            var result = request.Delete();
            Expect(result);
        }
    }
}