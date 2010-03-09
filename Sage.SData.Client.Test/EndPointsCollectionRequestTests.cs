using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Core;

// ReSharper disable InconsistentNaming

namespace Sage.SData.Client.Test
{
    [TestFixture]
    public class EndPointsCollectionRequestTests : AssertionHelper
    {
        private static ISDataService GetService()
        {
            //uncomment to return a real service instance
            //return new SDataService("http://localhost:59213/sdata/aw/dynamic/-/", "lee", "")

            return new MockSDataService("http://localhost:59213/sdata/aw/dynamic/-/", "lee", "");
        }

        [Test]
        public void verify_endpointcollection_url()
        {
            var a = GetService();

            var b = new SDataEndPointCollectionRequest(a);

            string url = b.ToString();
            Expect(url == "http://localhost:59213/sdata/$system/registry/endpoints");
        }

        [Test]
        public void verify_canread_endpoints_collection()
        {
            var a = GetService();

            var b = new SDataEndPointCollectionRequest(a);

            b.Read();

            Expect(b, Is.Not.Null);
        }
    }
}