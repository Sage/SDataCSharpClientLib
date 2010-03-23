using System;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Schema;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Common;
using Sage.SData.Client.Core;
using Sage.SData.Client.Framework;
using Sage.SData.Client.Test.Properties;

namespace Sage.SData.Client.Test
{
    /// <summary>
    /// Service class used for unit test
    /// </summary>
    public class MockSDataService : ISDataService, ISDataRequestSettings
    {
        public MockSDataService()
        {
            Url = "http://localhost:59213/sdata/aw/dynamic/-/";
            UserName = "lee";
            Password = "abc123";

            var uri = new SDataUri(Url);
            Protocol = uri.Scheme;
            ServerName = uri.Host;
            Port = uri.Port;
            VirtualDirectory = uri.Server;
            ApplicationName = uri.Product;
            ContractName = uri.Contract;
            DataSet = uri.CompanyDataset;
        }

        public bool Initialized { get; set; }
        public string Url { get; set; }
        public string Protocol { get; set; }
        public string ServerName { get; set; }
        public int? Port { get; set; }
        public string VirtualDirectory { get; set; }
        public string ApplicationName { get; set; }
        public string ContractName { get; set; }
        public string DataSet { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Timeout { get; set; }
        public CookieContainer Cookies { get; set; }
        public string UserAgent { get; set; }

        public AtomFeed CreateFeed(SDataBaseRequest request, AtomFeed feed)
        {
            var createdFeed = new AtomFeed();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.TestFeed)))
            {
                createdFeed.Load(stream);
            }

            return createdFeed;
        }

        public AtomFeed CreateFeed(SDataBaseRequest request, AtomFeed feed, out string eTag)
        {
            throw new NotImplementedException();
        }

        public AtomEntry CreateEntry(SDataBaseRequest request, AtomEntry entry)
        {
            var createdEntry = new AtomEntry();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.TestEntry)))
            {
                createdEntry.Load(stream);
            }

            return createdEntry;
        }

        public AsyncRequest CreateAsync(SDataBaseRequest request, ISyndicationResource resource)
        {
            const string trackingUrl = "http://www.example.com/sdata/sageApp/test/-/products/$service/computeSimplePrice";
            var tracking = new SDataTracking
                           {
                               Phase = "Initializing",
                               PhaseDetail = "StartingThread",
                               Progress = 0M,
                               ElapsedSeconds = 0,
                               RemainingSeconds = 10,
                               PollingMillis = 500
                           };
            return new AsyncRequest(this, trackingUrl, tracking);
        }

        public bool Delete(string url)
        {
            return true;
        }

        public bool DeleteEntry(SDataBaseRequest request)
        {
            throw new NotImplementedException();
        }

        public bool DeleteEntry(SDataBaseRequest request, AtomEntry entry)
        {
            return true;
        }

        public object Read(string url)
        {
            return "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                   "<tracking xmlns=\"http://schemas.sage.com/sdata/2008/1\">" +
                   "<phase>Initialization</phase>" +
                   "<phaseDetail>Starting thread</phaseDetail>" +
                   "<progress>0.0</progress>" +
                   "<elapsedSeconds>0</elapsedSeconds>" +
                   "<remainingSeconds>7</remainingSeconds>" +
                   "<pollingMillis>500</pollingMillis>" +
                   "</tracking>";
        }

        public AtomFeed ReadFeed(SDataBaseRequest request)
        {
            var feed = new AtomFeed();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.TestFeed)))
            {
                feed.Load(stream);
            }

            return feed;
        }

        public AtomFeed ReadFeed(SDataBaseRequest request, ref string eTag)
        {
            throw new NotImplementedException();
        }

        public AtomEntry ReadEntry(SDataBaseRequest request)
        {
            var entry = new AtomEntry();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.TestEntry)))
            {
                entry.Load(stream);
            }

            return entry;
        }

        public AtomEntry ReadEntry(SDataBaseRequest request, AtomEntry entry)
        {
            throw new NotImplementedException();
        }

        public XmlSchema ReadSchema(SDataResourceSchemaRequest request)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.TestSchema)))
            {
                return XmlSchema.Read(stream, null);
            }
        }

        public AtomEntry UpdateEntry(SDataBaseRequest request, AtomEntry entry)
        {
            var updatedEntry = new AtomEntry();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.TestEntry)))
            {
                updatedEntry.Load(stream);
            }

            return updatedEntry;
        }

        public void Initialize()
        {
        }
    }
}