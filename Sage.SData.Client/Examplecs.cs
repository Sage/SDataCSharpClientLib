using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;

#pragma warning disable 168
#pragma warning disable 219
// ReSharper disable JoinDeclarationAndInitializer
// ReSharper disable RedundantAssignment
// ReSharper disable UseObjectOrCollectionInitializer

namespace Examples
{
    internal class Program
    {
        private static void Main()
        {
            var service = new SDataService();

            // set user name to authenticate with
            service.UserName = "lee";
            // set password to authenticate with
            service.Password = "";

            service.Protocol = "HTTP";
            service.ServerName = "sdata.acme.com";
            service.ApplicationName = "sageApp";
            service.VirtualDirectory = "sdata";

            AtomFeed feed;
            AtomEntry entry;
            SDataPayload payload;

            #region CREATE an Entry

            // read the template for accounts
            var tru1 = new SDataTemplateResourceRequest(service);
            tru1.ContractName = "test";
            tru1.ResourceKind = "accounts";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/accounts/$template 

            // read the entry from the server
            entry = service.ReadEntry(tru1);

            // TODO: Make changes to the entry payload
            payload = entry.GetSDataPayload();

            var sru1 = new SDataSingleResourceRequest(service);
            sru1.ContractName = "test";
            sru1.ResourceKind = "accounts";

            var newEntry = service.CreateEntry(sru1, entry);

            #endregion

            #region CREATE a BATCH Operaton (Synchronous)

            // create the BatchURL
            var sbu = new SDataBatchRequest(service);
            sbu.ContractName = "test";
            sbu.ResourceKind = "products";
            // the configuration above generates http://sdata.acme.com/sageApp/test/-/products/$batch 

            using (var batch = new SDataBatchRequest(service))
            {
                // read the template for accounts
                var templateResourceRequest = new SDataTemplateResourceRequest(service);
                templateResourceRequest.ContractName = "test";
                templateResourceRequest.ResourceKind = "accounts";
                // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/accounts/$template 

                // read the entry from the server
                var templateEntry = service.ReadEntry(templateResourceRequest);

                var insertRequest = new SDataSingleResourceRequest(service);
                insertRequest.ContractName = "test";
                insertRequest.ResourceKind = "accounts";

                // do some stuff with the entry

                service.CreateEntry(insertRequest, templateEntry);

                // build, submit and get
                var result = batch.Commit();
            }

            #endregion

            #region CREATE a BATCH Operation (Asynchronous)

            // create the BatchURL
            sbu = new SDataBatchRequest(service);
            sbu.ContractName = "test";
            sbu.ResourceKind = "products";

            // the configuration above generates http://sdata.acme.com/sageApp/test/-/products/$batch 

            using (var batch = new SDataBatchRequest(service))
            {
                // read the template for accounts
                var templateResourceRequest = new SDataTemplateResourceRequest(service);
                templateResourceRequest.ContractName = "test";
                templateResourceRequest.ResourceKind = "accounts";
                // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/accounts/$template 

                // read the entry from the server
                var templateEntry = service.ReadEntry(templateResourceRequest);

                var insertRequest = new SDataSingleResourceRequest(service);
                insertRequest.ContractName = "test";
                insertRequest.ResourceKind = "accounts";

                // do some stuff with the entry

                var request = batch.CreateAsync();
                ISyndicationResource result;

                // wait around until the response is ready
                do
                {
                    var progress = request.Progress;
                } while ((result = request.Refresh()) == null);

                feed = result as AtomFeed;
            }

            #endregion

            #region READ a Resource Collection Feed

            // Read a Resource Collection Feed
            var rcu = new SDataResourceCollectionRequest(service);
            rcu.ContractName = "test";
            rcu.DataSet = "prod";
            rcu.ResourceKind = "accounts";

            // pageing
            rcu.StartIndex = 21;
            rcu.Count = 10;

            // query
            rcu.QueryValues.Add("where", "accountid='123456789abc'");
            rcu.QueryValues.Add("orderby", "'account'");

            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/prod/accounts?startIndex=21&count=10 
            // Read the feed from the server
            feed = service.ReadFeed(rcu);

            #endregion

            #region READ a Single Resource Entry

            // Read a Single Resource Entry
            var sru = new SDataSingleResourceRequest(service);
            sru.ContractName = "test";
            sru.ResourceKind = "accounts";
            sru.ResourceSelector = "'A001'";
            // the above configuration generates  http://sdata.acme.com/sdata/sageApp/test/-/accounts('A001') 

            // read the entry from the server
            entry = service.ReadEntry(sru);

            #endregion

            #region READ a Resource Property

            var rpu = new SDataResourcePropertyRequest(service);
            rpu.ContractName = "test";
            rpu.ResourceKind = "accounts";
            rpu.ResourceSelector = "'A001'";
            rpu.ResourceProperties.Add("postalAddress");
            rpu.ResourceProperties.Add("country");
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/accounts('A001')/postalAddress/country

            // read the entry from the server
            entry = service.ReadEntry(rpu);

            // now reconfigure and read property as a feed
            rpu.ResourceProperties.Add("salesOrders('0023')");
            rpu.ResourceProperties.Add("orderLines");
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/accounts('A001')/salesOrders('0023')/orderLines

            // read the feed from the server
            service.ReadFeed(rpu);

            #endregion

            #region READ a Template Resource

            var tru = new SDataTemplateResourceRequest(service);
            tru.ContractName = "test";
            tru.ResourceKind = "accounts";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/accounts/$template 

            // read the entry from the server
            entry = service.ReadEntry(tru);

            #endregion

            #region READ a Resource Schema

            var rsu = new SDataResourceSchemaRequest(service);
            rsu.ContractName = "test";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/$schema

            // read the feed from the server
            var schema = service.ReadSchema(rsu);

            // now reconfigurate and set resource kind and version
            rsu.ResourceKind = "accounts";
            rsu.Version = "5";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/accounts/$schema?version=5

            // read the entry from the server
            schema = service.ReadSchema(rsu);

            #endregion

            #region READ System Resources or Services

            var su = new SDataSystemRequest(service);

            // the above configuration generates http://sdata.acme.com/sdata/$system
            // read the feed from the server
            service.ReadFeed(su);

            #endregion

            #region READ Intermediate URLS

            #region READ Enumeration of Applications

            var iau = new IntermediateApplicationsRequest(service);

            // the above configuration generates http://sdata.acme.com/sdata

            // read the feed from the server
            service.ReadFeed(iau);

            #endregion

            #region READ Enumeration of DataSets

            var idu = new IntermediateDataSetsRequest(service);
            // the above configuration generates http://sdata.acme.com/sdata/sageApp

            // read the feed from the server
            feed = service.ReadFeed(idu);

            #endregion

            #region READ Enumeration of Contracts

            var icu = new IntermediateContractsRequest(service);

            // the above configuration generates http://sdata.acme.com/sdata/sageApp

            // read the feed from the server
            feed = service.ReadFeed(icu);

            #endregion

            #region READ Enumeration of Resource Collections

            var ircu = new IntermediateResourceCollectionsRequest(service);
            ircu.ContractName = "test";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test

            // read the feed from the server
            feed = service.ReadFeed(ircu);

            #endregion

            #region READ Enumeration of Services

            var isu = new IntermediateServicesRequest(service);
            isu.ContractName = "test";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/$service

            // read the feed from the server
            service.ReadFeed(isu);

            // reconfigure and set the resource kind
            isu.ResourceKind = "accounts";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/accounts/$service
            // read the feed from the server
            service.ReadFeed(isu);

            #endregion

            #endregion

            #region Update an Entry

            // Read a Single Resource Entry
            var sru2 = new SDataSingleResourceRequest(service);
            sru2.ContractName = "test";
            sru2.ResourceKind = "accounts";
            sru2.ResourceSelector = "'A001'";
            // the above configuration generates  http://sdata.acme.com/sdata/sageApp/test/accounts('A001') 

            // TODO: Make changes to the entry payload
            payload = newEntry.GetSDataPayload();
            // update the server
            service.UpdateEntry(sru2, newEntry);

            #endregion

            #region DELETE an Entry

            service.DeleteEntry(sru2, newEntry);

            #endregion
        }

        #region CREATE a Service Operation (Synchronous)

        private AtomEntry CreateServiceRequest(SDataService service)
        {
            var request = new SDataServiceOperationRequest(service);
            request.ContractName = "test";
            request.ResourceKind = "products";
            request.OperationName = "computePrice";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/products/$service/computePrice

            // now reconfigure and generate for globally for the entire contract
            request.ResourceKind = string.Empty;
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/$service/computePrice

            // read the feed from the server
            return request.Create();
        }

        #endregion

        #region CREATE a Service Operation (Asynchronous)

        private AtomEntry CreateServiceOperationAsync(SDataService service)
        {
            var request = new SDataServiceOperationRequest(service);
            request.ApplicationName = "sageApp";
            request.ContractName = "test";
            request.OperationName = "computePrice";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/$service/computePrice

            // read the entry from the server
            var asyncRequest = request.CreateAsync();
            ISyndicationResource result;

            // wait around until the response is ready
            do
            {
                var progress = asyncRequest.Progress;
                // report progress to the user
            } while ((result = asyncRequest.Refresh()) == null);

            return result as AtomEntry;
        }

        #endregion
    }
}