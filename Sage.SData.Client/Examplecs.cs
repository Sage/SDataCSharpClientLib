using System;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using System.Xml.Schema;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;


namespace SDataAPIPSuedo
{
    class Program
    {
        static void Main(string[] args)
        {

            SDataService service = new SDataService();

            // set user name to authenticate with
            service.UserName = "lee";
            // set password to authenticate with
            service.Password = "";

            service.Protocol = "HTTP";
            service.ServerName = "sdata.acme.com";
            service.ApplicationName = "sageApp";
            service.VirtualDirectory = "sdata";

            service.Initialize();

            AtomFeed feed = null;
            AtomEntry entry = null;
            
            #region CREATE an Entry
            // read the template for accounts
            SDataTemplateResourceRequest tru1 = new SDataTemplateResourceRequest(service);
            tru1.ContractName = "test";
            // empty Data set defaults to -
            tru1.DataSet = string.Empty;
            tru1.ResourceKind = "accounts";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/accounts/$template 

            // read the entry from the server
            entry = service.Read(tru1) as AtomEntry;
            // set the payload for the entry big fat TODO
            SDataSingleResourceRequest sru1 = new SDataSingleResourceRequest(service);
            sru1.ContractName = "test";
            // empty Data set defaults to -
            sru1.DataSet = string.Empty;
            sru1.ResourceKind = "accounts";

            AtomEntry newEntry = service.Create(sru1, entry) as AtomEntry;
            #endregion

            

            

            #region CREATE a BATCH Operaton (Synchronous)
            // create the BatchURL
            SDataBatchRequest sbu = new SDataBatchRequest(service);
            sbu.ContractName = "test";
            // default dataset = -
            sbu.DataSet = string.Empty;
            sbu.ResourceKind = "products";
            // the configuration above generates http://sdata.acme.com/sageApp/test/-/products/$batch 
            
            using(BatchProcess batch = new BatchProcess())
            {


                // read the template for accounts
                SDataTemplateResourceRequest templateResourceRequest = new SDataTemplateResourceRequest(service);
                templateResourceRequest.ContractName = "test";
                // empty Data set defaults to -
                templateResourceRequest.DataSet = string.Empty;
                templateResourceRequest.ResourceKind = "accounts";
                // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/accounts/$template 

                // read the entry from the server
                AtomEntry templateEntry = service.Read(templateResourceRequest) as AtomEntry;
                
                SDataSingleResourceRequest insertRequest = new SDataSingleResourceRequest(service);
                insertRequest.ContractName = "test";
                // empty Data set defaults to -
                insertRequest.DataSet = string.Empty;
                insertRequest.ResourceKind = "accounts";
                /*
                 *  Do some stuff with the ntry
                 */

                service.Create(insertRequest, templateEntry);
              

                // build, submit and get
                AtomFeed batchffed  = batch.Commit();
            }

            #endregion

            #region CREATE a BATCH Operation (Asynchronous)
            // create the BatchURL
            sbu = new SDataBatchRequest(service);
            sbu.ContractName = "test";
            // default dataset = -
            sbu.DataSet = string.Empty;
            sbu.ResourceKind = "products";

            // the configuration above generates http://sdata.acme.com/sageApp/test/-/products/$batch 

            using (BatchProcess batch = new BatchProcess())
            {
                // read the template for accounts
                SDataTemplateResourceRequest templateResourceRequest = new SDataTemplateResourceRequest(service);
                templateResourceRequest.ContractName = "test";
                // empty Data set defaults to -
                templateResourceRequest.DataSet = string.Empty;
                templateResourceRequest.ResourceKind = "accounts";
                // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/accounts/$template 

                // read the entry from the server
                AtomEntry templateEntry = service.Read(templateResourceRequest) as AtomEntry;

                SDataSingleResourceRequest insertRequest = new SDataSingleResourceRequest(service);
                insertRequest.ContractName = "test";
                // empty Data set defaults to -
                insertRequest.DataSet = string.Empty;
                insertRequest.ResourceKind = "accounts";
                /*
                 *  Do some stuff with the ntry
                 */

                string trackingID = System.Guid.NewGuid().ToString();
                service.CreateAsync(sbu, trackingID);


                // build, submit and get
                AtomFeed batchffed = batch.Commit();
                // wait around until the response is ready
                while (request.Response == null)
                {
                    Thread.Sleep(request.PollingMilliseconds);
                    // check with the server again
                    request.Refresh();
                }
                feed = request.Response as AtomFeed;
            }

            #endregion

            #region READ a Resource Collection Feed
            // Read a Resource Collection Feed
            SDataResourceCollectionRequest rcu = new SDataResourceCollectionRequest(service);
            rcu.ContractName = "test";
            // default for dataset is -
            rcu.DataSet = "prod";
            rcu.ResourceKind = "accounts";
            
            // pageing
            rcu.StartIndex = 21;
            rcu.ItemsPerPage = 10;

            // query
            rcu.QueryValues.Add("where", "accountid='123456789abc'");
            rcu.QueryValues.Add("orderby", "'account'");

            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/prod/accounts?startIndex=21&count=10 
            // Read the feed from the server
            feed = service.Read(rcu) as AtomFeed;
            #endregion

            #region READ a Single Resource Entry
            // Read a Single Resource Entry
            SDataSingleResourceRequest sru = new SDataSingleResourceRequest(service);
            sru.ContractName = "test";
            // default for dataset is -
            sru.DataSet = string.Empty;
            sru.ResourceKind = "accounts";
            sru.ResourceSelector = "('A001')";
            // the above configuration generates  http://sdata.acme.com/sdata/sageApp/test/-/accounts('A001') 

            // read the entry from the server
            entry = service.Read(sru) as AtomEntry;
            #endregion

            #region READ a Resource Property
            SDataResourcePropertyRequest rpu = new SDataResourcePropertyRequest(service);
            rpu.ContractName = "test";
            rpu.ResourceKind = "accounts";
            rpu.ResourceSelector = "('A001')";
            rpu.ResourceProperties.Add(0, "postalAddress");
            rpu.ResourceProperties.Add(1, "country");
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/accounts('A001')/postalAddress/country

            // read the entry from the server
            entry = service.Read(rpu) as AtomEntry;


            // now reconfigure and read property as a feed
            rpu.ResourceProperties.Clear();
            rpu.ResourceProperties.Add(0, "salesOrders('0023'");
            rpu.ResourceProperties.Add(1, "orderLines");
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/accounts('A001')/salesOrders('0023')/orderLines

            // read the feed from the server
            feed = service.Read(rpu) as AtomFeed;
            #endregion

            #region READ a Template Resource
            SDataTemplateResourceRequest tru = new SDataTemplateResourceRequest(service);
            tru.ContractName = "test";
            // default dataset = -
            tru.DataSet = string.Empty;
            tru.ResourceKind = "accounts";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/accounts/$template 

            // read the entry from the server
            entry = service.Read(tru) as AtomEntry;
            #endregion

            #region READ a Resource Schema
            SDataResourceSchemaRequest rsu = new SDataResourceSchemaRequest(service);
            rsu.ContractName = "test";
            // default dataset = -
            rsu.DataSet = string.Empty;
            rsu.ResourceKind = string.Empty;
            rsu.Version = string.Empty;
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/$schema
 
            // read the feed from the server
            XmlSchema xsd  = service.Read(rsu);

            // now reconfigurate and set resource kind and version
            rsu.ResourceKind = "accounts";
            rsu.Version = "5";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/accounts/$schema?version=5

            // read the entry from the server
            xsd = service.Read(rsu);           
            #endregion

            #region READ System Resources or Services
            SDataSystemRequest su = new SDataSystemRequest(service);
            
            // the above configuration generates http://sdata.acme.com/sdata/$system
            // read the feed from the server
            feed = service.Read(su) as AtomFeed;
            #endregion

            #region READ Intermediate URLS
            #region READ Enumeration of Applications
            IntermediateApplicationsRequest iau = new IntermediateApplicationsRequest(service);
            
            // the above configuration generates http://sdata.acme.com/sdata

            // read the feed from the server
            feed = service.Read(iau) as AtomFeed;
            #endregion

            #region READ Enumeration of DataSets
            IntermediateDataSetsRequest idu= new IntermediateDataSetsRequest(service);
            // the above configuration generates http://sdata.acme.com/sdata/sageApp

            // read the feed from the server
            feed = service.Read(idu) as AtomFeed;
            #endregion

            #region READ Enumeration of Contracts
            IntermediateContractsRequest icu = new IntermediateContractsRequest(service);
            
            // the above configuration generates http://sdata.acme.com/sdata/sageApp

            // read the feed from the server
            feed = service.Read(icu) as AtomFeed;
            #endregion

            #region READ Enumeration of Resource Collections
            IntermediateResourceCollectionsRequest ircu = new IntermediateResourceCollectionsRequest(service);
            ircu.ContractName = "test";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test

            // read the feed from the server
            feed = service.Read(ircu) as AtomFeed;
            #endregion

            #region READ Enumeration of Services
            IntermediateServicesRequest isu = new IntermediateServicesRequest(service);
            
            isu.ResourceKind = string.Empty;
            isu.ContractName = "test";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/$service

            // read the feed from the server
            feed = service.Read(isu) as AtomFeed;

            // reconfigure and set the resource kind
            isu.ResourceKind = "accounts";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/accounts/$service
            // read the feed from the server
            feed = service.Read(isu) as AtomFeed;
            #endregion
            #endregion

            #region Update an Entry
            // Read a Single Resource Entry
            SDataSingleResourceRequest sru2 = new SDataSingleResourceRequest(service);
            sru2.ContractName = "test";
            sru2.ResourceKind = "accounts";
            sru2.ResourceSelector = "('A001')";
            // the above configuration generates  http://sdata.acme.com/sdata/sageApp/test/accounts('A001') 

            // TODO: Make changes to the entry payload
            newEntry.Payload.Elements();
            // update the server
            service.Update(sru2, newEntry);
            #endregion

            #region DELETE an Entry
            service.Delete(sru2);
            #endregion

        }

        #region CREATE a Service Operation (Synchronous)
        AtomFeed CreateServiceRequest(SDataService service)
        {
            SDataServiceOperationRequest request = new SDataServiceOperationRequest(service);
            request.ContractName = "test";
            // default dataset = -
            request.DataSet = string.Empty;
            request.ResourceKind = "products";
            request.OperationName = "computePrice";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/products/$service/computePrice

            // now reconfigure and and generate for globally for the entire contract
            request.ResourceKind = string.Empty;
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/$service/computePrice

            // read the feed from the server
            AtomFeed feed = request.Create();
            return feed;

        }
        #endregion

        #region CREATE a Service Operation (Asynchronous)
        AtomFeed CreateServiceOperationAsync(SDataService service)
        {
            SDataServiceOperationRequest request = new SDataServiceOperationRequest(service);
            request.ContractName = "test";
            // default dataset = -
            request.DataSet = string.Empty;
            request.ResourceKind = "products";
            request.OperationName = "computePrice";
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/products/$service/computePrice

            // now reconfigure and and generate for globally for the entire contract
            request.ResourceKind = string.Empty;
            // the above configuration generates http://sdata.acme.com/sdata/sageApp/test/-/$service/computePrice

            // great a UUID for the transaction
            string uuid = System.Guid.NewGuid().ToString();

            // read the feed from the server
            AsyncRequest asyncRequest = request.CreateAsync(uuid);

            // wait around until the response is ready
            while (asyncRequest.Response == null)
            {
                Thread.Sleep(asyncRequest.PollingMilliseconds);
                asyncRequest.Refresh();
            }
            AtomFeed feed = request.Response;
            return feed;
        }

        #endregion
    }


   

}
