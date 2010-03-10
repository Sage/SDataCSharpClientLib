using System;
using System.Xml;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;

// ReSharper disable InconsistentNaming

namespace Sage.SData.Client.Test
{
    [TestFixture]
    public class Class1 : AssertionHelper
    {
        private static ISDataService GetService()
        {
            //uncomment to return a real service instance
            //return new SDataService("http://localhost:59213/sdata/aw/dynamic/-/", "lee", "")

            return new MockSDataService("http://localhost:59213/sdata/aw/dynamic/-/", "lee", "");
        }

        [Test]
        public void verify_canconstruct_SDataService()
        {
            var a = new SDataService();
            Expect(a, Is.Not.Null);
        }

        [Test]
        public void verify_canconstructwithurl_SDataService()
        {
            var a = new SDataService("http://localhost:59213/sdata/aw/dynamic/-/employees", "lee", "");
            Expect(a, Is.Not.Null);
        }

        [Test]
        public void verity_caninitialize_SDataService()
        {
            var a = new SDataService("http://localhost:59213/sdata/aw/dynamic/-/employees", "lee", "");

            Expect(a.UserName, Is.Not.Null);
            Expect(a.UserName == "lee");

            Expect(a.Password, Is.Not.Null);
            Expect(a.Password == "");

            Expect(a.Protocol, Is.Not.Null);
            Expect(a.Protocol == "http");

            Expect(a.ServerName, Is.Not.Null);
            Expect(a.ServerName == "localhost");

            Expect(a.Port, Is.Not.Null);
            Expect(a.Port == 59213);

            Expect(a.VirtualDirectory, Is.Not.Null);
            Expect(a.VirtualDirectory == "sdata");

            Expect(a.ApplicationName, Is.Not.Null);
            Expect(a.ApplicationName == "aw");

            Expect(a.ContractName, Is.Not.Null);
            Expect(a.ContractName == "dynamic");

            Expect(a.DataSet, Is.Not.Null);
            Expect(a.DataSet == "-");
        }

        [Test]
        public void verify_canreadatomfeed_SDataService()
        {
            var a = GetService();

            var b = new SDataResourceCollectionRequest(a) {ResourceKind = "employees"};

            var feed = a.ReadFeed(b);
            Expect(feed, Is.Not.Null);
        }

        [Test]
        public void verify_canread_SDataResourceSchemaRequest()
        {
            var a = GetService();

            var b = new SDataResourceSchemaRequest(a) {ResourceKind = "employees"};

            var schema = b.Read();

            Expect(schema, Is.Not.Null);
        }

        [Test]
        public void verify_cancontruct_IntermediateApplicationsRequest()
        {
            var a = GetService();

            var b = new IntermediateApplicationsRequest(a);
            Expect(b, Is.Not.Null);
        }

        [Test]
        public void verify_tostring_IntermediateApplicationsRequest()
        {
            var a = GetService();

            var b = new IntermediateApplicationsRequest(a);
            var url = b.ToString();

            Expect(url == "http://localhost:59213/sdata");
        }

        [Test]
        public void verify_canread_IntermediateApplicationsRequest()
        {
            var a = GetService();

            var b = new IntermediateApplicationsRequest(a);
            b.Read();
        }

        [Test]
        public void verify_canconstruct_IntermediateContractsRequest()
        {
            var a = GetService();

            var b = new IntermediateContractsRequest(a);
            Expect(b, Is.Not.Null);
        }

        [Test]
        public void verify_tostring_IntermediateContractsRequest()
        {
            var a = GetService();

            var b = new IntermediateContractsRequest(a) {ApplicationName = "aw"};
            var url = b.ToString();
            Expect(url == "http://localhost:59213/sdata/aw");
        }

        [Test]
        public void verify_canread_IntermediateContractsRequest()
        {
            var a = GetService();

            var b = new IntermediateContractsRequest(a) {ApplicationName = "aw"};
            var feed = b.Read();
            Expect(feed, Is.Not.Null);
        }

        [Test]
        public void verify_canconstruct_IntermediateDataSetsRequest()
        {
            var a = GetService();

            new IntermediateDataSetsRequest(a);
        }

        [Test]
        public void verify_tostring_IntermediateDataSetsRequest()
        {
            var a = GetService();

            var b = new IntermediateDataSetsRequest(a) {ContractName = "dynamic"};

            var url = b.ToString();
            Expect(url == "http://localhost:59213/sdata/aw/dynamic");
        }

        [Test]
        public void verify_canread_IntermediateDataSetsRequest()
        {
            var a = GetService();

            var b = new IntermediateDataSetsRequest(a) {ContractName = "dynamic"};

            var feed = b.Read();
            Expect(feed, Is.Not.Null);
        }

        [Test]
        public void verify_canconstruct_IntermediateResourceCollectionsRequest()
        {
            var a = GetService();

            var b = new IntermediateResourceCollectionsRequest(a);

            Expect(b, Is.Not.Null);
        }

        [Test]
        public void verify_tostring_IntermediateResourceCollectionsRequest()
        {
            var a = GetService();

            var b = new IntermediateResourceCollectionsRequest(a);

            var url = b.ToString();

            Expect(url == "http://localhost:59213/sdata/aw/dynamic/-");
        }

        [Test]
        public void verify_canread_IntermediateResourceCollectionsRequest()
        {
            var a = GetService();

            var b = new IntermediateResourceCollectionsRequest(a);

            var feed = b.Read();

            Expect(feed, Is.Not.Null);
        }

        [Test]
        public void verify_canconstruct_IntermediateServicesRequest()
        {
            var a = GetService();

            var b = new IntermediateServicesRequest(a);

            Expect(b, Is.Not.Null);
        }

        [Test]
        public void verify_tostring_IntermediateServicesRequest()
        {
            var a = GetService();

            var b = new IntermediateServicesRequest(a) {ResourceKind = "employees"};

            var url = b.ToString();
            Expect(url == "http://localhost:59213/sdata/aw/dynamic/-/employees/$service");
        }

        [Test]
        public void verify_canread_IntermediateServicesRequest()
        {
            var a = GetService();

            var b = new IntermediateServicesRequest(a) {ResourceKind = "employees"};

            var feed = b.Read();
            Expect(feed, Is.Not.Null);
        }

        [Test]
        public void verify_canconstruct_SDataResourceCollectionsRequest()
        {
            var a = GetService();

            var b = new SDataResourceCollectionRequest(a);

            Expect(b, Is.Not.Null);
        }

        [Test]
        public void verify_tostringwithpageing_SDataResourceCollectionsRequest()
        {
            var a = GetService();

            var b = new SDataResourceCollectionRequest(a)
                    {
                        ResourceKind = "employees",
                        StartIndex = 1,
                        Count = 100
                    };

            var url = b.ToString();
            Expect(url == "http://localhost:59213/sdata/aw/dynamic/-/employees?startIndex=1&count=100");
        }

        [Test]
        public void verify_tostringwithquery_SDataResourceCollectionsRequest()
        {
            var a = GetService();

            var b = new SDataResourceCollectionRequest(a)
                    {
                        ResourceKind = "employees",
                        QueryValues = {{"where", "gender eq m"}}
                    };

            var url = b.ToString();
            Expect(url == "http://localhost:59213/sdata/aw/dynamic/-/employees?where=gender eq m");
        }

        [Test]
        public void verify_tostringwithquery_multiplevalues_SDataResourceCollectionsRequest()
        {
            var a = GetService();

            var b = new SDataResourceCollectionRequest(a)
                    {
                        ResourceKind = "employees",
                        QueryValues =
                            {
                                {"where", "gender eq m"},
                                {"orderBy", "orderDate DESC"}
                            }
                    };

            var url = b.ToString();
            Expect(url == "http://localhost:59213/sdata/aw/dynamic/-/employees?where=gender eq m&orderBy=orderDate DESC");
        }

        [Test]
        public void verify_canread_SDataResourceCollectionsRequest()
        {
            var a = GetService();

            var b = new SDataResourceCollectionRequest(a) {ResourceKind = "employees"};

            var c = b.Read();

            Expect(c, Is.Not.Null);
        }

        [Test]
        public void verify_canread_SDataResourceCollectionsRequest_Reader()
        {
            var a = GetService();

            var b = new SDataResourceCollectionRequest(a)
                    {
                        ResourceKind = "employees",
                        Count = 10,
                        StartIndex = 1
                    };

            var reader = b.ExecuteReader();

            Expect(reader, Is.Null);
        }

        [Test]
        public void verify_canconstruct_SDataSingleResourceRequest()
        {
            var a = GetService();

            var b = new SDataSingleResourceRequest(a);

            Expect(b, Is.Not.Null);
        }

        [Test]
        public void verify_canconstructwithtemplateEntry_SDataSingleResourceRequest()
        {
            var a = GetService();

            var entry = new AtomEntry();
            var b = new SDataSingleResourceRequest(a, entry);

            Expect(b, Is.Not.Null);
            Expect(b.Entry, Is.Not.Null);
        }

        [Test]
        public void verify_tostringwithresourcekind_SDataSingleResourceRequest()
        {
            var a = GetService();

            var b = new SDataSingleResourceRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "id = '1234'"
                    };

            var result = b.ToString();

            Expect(result == "http://localhost:59213/sdata/aw/dynamic/-/employees(id = '1234')");
        }

        [Test]
        public void verify_tostringwithresourcekindandinclude_SDataSingleResourceRequest()
        {
            var a = GetService();

            var b = new SDataSingleResourceRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "(1)",
                        Include = "contact"
                    };

            var result = b.ToString();

            Expect(result == "http://localhost:59213/sdata/aw/dynamic/-/employees(1)?include=contact");
        }

        [Test]
        public void verify_canprocess_SDataBatchRequest()
        {
            var a = GetService();

            var b = new SDataSingleResourceRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "(1)"
                    };

            var c = new SDataSingleResourceRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "(2)"
                    };

            var entry1 = c.Read();

            var payload = entry1.GetSDataPayload();
            if (payload != null)
            {
                payload.Values["MaritalStatus"] = "Married";
                entry1.SetSDataPayload(payload);
            }

            c.Entry = entry1;

            var d = new SDataSingleResourceRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "(3)"
                    };

            var entry2 = d.Read();
            d.Entry = entry2;

            AtomFeed batchfeed;
            using (var batch = new SDataBatchRequest(a))
            {
                batch.ResourceKind = "employees";
                b.Read();
                c.Update();
                d.Delete();

                batchfeed = batch.Commit();
            }

            Expect(batchfeed, Is.Not.Null);
        }

        [Test]
        public void verify_canread_SDataSingleResourceRequest()
        {
            var a = GetService();

            var b = new SDataSingleResourceRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "(1)"
                    };

            var entry = b.Read();

            Expect(entry, Is.Not.Null);
        }

        [Test]
        public void verify_cancreate_SDataSingleResourceRequest()
        {
            var a = GetService();

            var b = new SDataTemplateResourceRequest(a) {ResourceKind = "employees"};

            var templateentry = b.Read();

            var c = new SDataSingleResourceRequest(a) {ResourceKind = "employees"};

            var payload = templateentry.GetSDataPayload();
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
            templateentry.SetSDataPayload(payload);

            c.Entry = templateentry;
            templateentry.UpdatedOn = DateTime.Now;
            templateentry.PublishedOn = DateTime.Now;

            var newentry = c.Create();

            Expect(newentry, Is.Not.Null);
        }

        [Test]
        public void verify_canupdate_SDataSingleResourceRequest()
        {
            var a = GetService();

            var b = new SDataSingleResourceRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "(1)"
                    };

            var entry = b.Read();

            var payload = entry.GetSDataPayload();
            if (payload != null)
            {
                payload.Values["Title"] = "test update";
                entry.SetSDataPayload(payload);
            }

            b.Entry = entry;
            var updateentry = b.Update();

            Expect(updateentry, Is.Not.Null);
        }

        [Test]
        public void verify_candelete_SDataSingleResourceRequest()
        {
            var a = GetService();

            var b = new SDataSingleResourceRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "(1)"
                    };

            var entry = b.Read();
            b.Entry = entry;

            var result = b.Delete();

            Expect(result);
        }

        [Test]
        public void verify_canconstruct_SDataResourcePropertyRequest()
        {
            var a = GetService();

            var b = new SDataResourcePropertyRequest(a);

            Expect(b, Is.Not.Null);
        }

        [Test]
        public void verify_tostringwithsingleresourceproperty_SDataResourcePropertyRequest()
        {
            var a = GetService();

            var b = new SDataResourcePropertyRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "(1)",
                        ResourceProperties = {"LoginID"}
                    };

            var result = b.ToString();

            Expect(result == "http://localhost:59213/sdata/aw/dynamic/-/employees(1)/LoginID");
        }

        [Test]
        public void verify_tostringwithmultipleresourceproperties_SDataResourcePropertyRequest()
        {
            var a = GetService();

            var b = new SDataResourcePropertyRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "(1)",
                        ResourceProperties = {"Address", "City"}
                    };

            var result = b.ToString();

            Expect(result == "http://localhost:59213/sdata/aw/dynamic/-/employees(1)/Address/City");
        }

        [Test]
        public void verify_canreadfeed_SDataResourcePropertyRequest()
        {
            var a = GetService();

            var b = new SDataResourcePropertyRequest(a)
                    {
                        ResourceKind = "employee",
                        ResourceSelector = "(1)",
                        ResourceProperties = {"Contacts"}
                    };

            var feed = b.ReadFeed();

            Expect(feed, Is.Not.Null);
        }

        [Test]
        public void verify_canreadentry_SDataResourcePropertyRequest()
        {
            var a = GetService();

            var b = new SDataResourcePropertyRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "(1)",
                        ResourceProperties = {"LoginID"}
                    };

            var entry = b.Read();

            Expect(entry, Is.Not.Null);
        }

        [Test]
        public void verify_canreadcreate_SDataResourcePropertyRequest()
        {
            var a = GetService();

            var b = new SDataResourcePropertyRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "(id = '1234')",
                        ResourceProperties = {"Address", "City"}
                    };

            var entry = b.Create();

            Expect(entry, Is.Not.Null);
        }

        [Test]
        public void verify_canupdate_SDataResourcePropertyRequest()
        {
            var a = GetService();

            var b = new SDataResourcePropertyRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "(id = '1234')",
                        ResourceProperties = {"Address", "City"}
                    };

            var entry = b.Update();

            Expect(entry, Is.Not.Null);
        }

        [Test]
        public void verify_candelete_SDataResourcePropertyRequest()
        {
            var a = GetService();

            var b = new SDataResourcePropertyRequest(a)
                    {
                        ResourceKind = "employees",
                        ResourceSelector = "(1)",
                        ResourceProperties = {"Address", "City"}
                    };

            var result = b.Delete();

            Expect(result);
        }

        [Test]
        public void verify_canconstruct_SDataTemplateResourceRequest()
        {
            var a = GetService();

            var b = new SDataTemplateResourceRequest(a);

            Expect(b, Is.Not.Null);
        }

        [Test]
        public void verify_tostring_SDataTemplateResourceRequest()
        {
            var a = GetService();

            var b = new SDataTemplateResourceRequest(a) {ResourceKind = "employees"};

            var result = b.ToString();

            Expect(result == "http://localhost:59213/sdata/aw/dynamic/-/employees/$template");
        }

        [Test]
        public void verify_canread_SDataTemplateResourceRequest()
        {
            var a = GetService();

            var b = new SDataTemplateResourceRequest(a) {ResourceKind = "employees"};

            b.Read();

            Expect(b, Is.Not.Null);
        }

        [Test]
        public void verify_tostring_SDataServiceOperationRequest()
        {
            var a = GetService();

            var b = new SDataServiceOperationRequest(a)
                    {
                        ResourceKind = "employees",
                        OperationName = "getStats"
                    };

            var url = b.ToString();
            Expect(url == "http://localhost:59213/sdata/aw/dynamic/-/employees/$service/getStats");
        }
    }
}