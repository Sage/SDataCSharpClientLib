using NUnit.Framework;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class SDataServiceTests : AssertionHelper
    {
        [Test]
        public void Service_Verify_CanConstruct()
        {
            var service = new SDataService();
            Expect(service, Is.Not.Null);
        }

        [Test]
        public void Service_Verify_CanConstructWithUrl()
        {
            var service = new SDataService("http://localhost:59213/sdata/aw/dynamic/-/employees", "lee", "abc123");
            Expect(service, Is.Not.Null);
        }

        [Test]
        public void Service_Verity_CanInitialize()
        {
            var service = new SDataService("http://localhost:59213/sdata/aw/dynamic/-/employees", "lee", "abc123");

            Expect(service.UserName, Is.Not.Null);
            Expect(service.UserName, Is.EqualTo("lee"));

            Expect(service.Password, Is.Not.Null);
            Expect(service.Password, Is.EqualTo("abc123"));

            Expect(service.Protocol, Is.Not.Null);
            Expect(service.Protocol, Is.EqualTo("http"));

            Expect(service.ServerName, Is.Not.Null);
            Expect(service.ServerName, Is.EqualTo("localhost"));

            Expect(service.Port, Is.Not.Null);
            Expect(service.Port, Is.EqualTo(59213));

            Expect(service.VirtualDirectory, Is.Not.Null);
            Expect(service.VirtualDirectory, Is.EqualTo("sdata"));

            Expect(service.ApplicationName, Is.Not.Null);
            Expect(service.ApplicationName, Is.EqualTo("aw"));

            Expect(service.ContractName, Is.Not.Null);
            Expect(service.ContractName, Is.EqualTo("dynamic"));

            Expect(service.DataSet, Is.Not.Null);
            Expect(service.DataSet, Is.EqualTo("-"));
        }
    }
}