using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test
{
    [TestFixture]
    public class EndPointsCollectionRequestTests : AssertionHelper
    {
        private bool bUseTestSerivce = true;

        [Test]
        public void verify_endpointcollection_url()
        {
            ISDataService a;
            if (bUseTestSerivce)
                a = new SDataServiceTest("http://localhost:59213/sdata/aw/dynamic/-/", "lee", "");
            else
                a = new SDataService("http://localhost:59213/sdata/aw/dynamic/-/", "lee", "");
            a.Initialize();

            var b = new SDataEndPointCollectionRequest(a);

            string url = b.ToString();
            Expect(url == "http://localhost:59213/sdata/$system/registry/endpoints");
        }

        [Test]
        public void verify_canread_endpoints_collection()
        {
            ISDataService a;
            if (bUseTestSerivce)
                a = new SDataServiceTest("http://localhost:59213/sdata/aw/dynamic/-/", "lee", "");
            else
                a = new SDataService("http://localhost:59213/sdata/aw/dynamic/-/", "lee", "");
            a.Initialize();

            var b = new SDataEndPointCollectionRequest(a);

            b.Read();

            Expect(b, Is.Not.Null);
        }
    }
}