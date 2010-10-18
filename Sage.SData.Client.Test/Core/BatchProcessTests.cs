using NUnit.Framework;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class BatchProcessTests
    {
        [Test]
        public void BatchProcess_AddItemWithoutRequest()
        {
            var item = new SDataBatchRequestItem();
            var added = BatchProcess.Instance.AddToBatch(item);
            Assert.That(added, Is.False);
        }

        [Test]
        public void BatchProcess_RequestRemovedOnDispose()
        {
            var service = new SDataService("http://localhost:59213/sdata/aw/dynamic/-/");

            using (var request = new SDataBatchRequest(service))
            {
                Assert.That(BatchProcess.Instance.Requests, Contains.Item(request));
            }

            Assert.That(BatchProcess.Instance.Requests, Is.Empty);
        }

        [Test]
        public void BatchProcess_AddItemWithRequest()
        {
            var service = new SDataService("http://localhost:59213/sdata/aw/dynamic/-/");

            using (var request = new SDataBatchRequest(service) {ResourceKind = "employees"})
            {
                var item = new SDataBatchRequestItem
                           {
                               Url = "http://localhost:59213/sdata/aw/dynamic/-/employees"
                           };
                var added = BatchProcess.Instance.AddToBatch(item);
                Assert.That(added, Is.True);
                Assert.That(request.Items, Contains.Item(item));
            }
        }

        [Test]
        public void BatchProcess_AddItemWithUnsuitableRequest()
        {
            var service = new SDataService("http://localhost:59213/sdata/aw/dynamic/-/");

            using (var request = new SDataBatchRequest(service) {ResourceKind = "employees"})
            {
                var item = new SDataBatchRequestItem
                           {
                               Url = "http://localhost:59213/sdata/aw/dynamic/-/contacts"
                           };
                var added = BatchProcess.Instance.AddToBatch(item);
                Assert.That(added, Is.False);
                Assert.That(request.Items, Is.Empty);
            }
        }
    }
}