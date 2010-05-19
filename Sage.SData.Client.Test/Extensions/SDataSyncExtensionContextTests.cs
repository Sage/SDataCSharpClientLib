using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Extensions;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Test.Extensions
{
    [TestFixture]
    public class SDataSyncExtensionContextTests
    {
        [Test]
        public void Typical_Feed()
        {
            var xml = @"<feed xmlns=""http://www.w3.org/2005/Atom""
                              xmlns:sync=""http://schemas.sage.com/sdata/sync/2008/1"">
                          <sync:syncMode>catchUp</sync:syncMode>
                          <sync:digest>
                            <sync:origin>http://www.example.com/sdata/myApp1/myContract/-/accounts</sync:origin>
                            <sync:digestEntry>
                              <sync:endpoint>http://www.example.com/sdata/myApp1/myContract/-/accounts</sync:endpoint>
                              <sync:tick>6</sync:tick>
                              <sync:stamp>2008-10-30T17:23:08Z</sync:stamp>
                              <sync:conflictPriority>2</sync:conflictPriority>
                            </sync:digestEntry>
                            <sync:digestEntry>
                              <sync:endpoint>http://www.example.com/sdata/myApp2/myContract/-/accounts</sync:endpoint>
                              <sync:tick>10</sync:tick>
                              <sync:stamp>2008-10-30T12:16:51Z</sync:stamp>
                              <sync:conflictPriority>1</sync:conflictPriority>
                            </sync:digestEntry>
                          </sync:digest>
                        </feed>";
            var feed = new AtomFeed();
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                feed.Load(stream);
            }

            var syncMode = feed.GetSDataSyncMode();
            Assert.That(syncMode, Is.EqualTo(SyncMode.CatchUp));

            var digest = feed.GetSDataSyncDigest();
            Assert.That(digest, Is.Not.Null);
            Assert.That(digest.Origin, Is.EqualTo("http://www.example.com/sdata/myApp1/myContract/-/accounts"));
            Assert.That(digest.Entries.Length, Is.EqualTo(2));

            var entry = digest.Entries[0];
            Assert.That(entry.EndPoint, Is.EqualTo("http://www.example.com/sdata/myApp1/myContract/-/accounts"));
            Assert.That(entry.Tick, Is.EqualTo(6L));
            Assert.That(entry.Stamp, Is.EqualTo(new DateTime(2008, 10, 30, 17, 23, 08)));
            Assert.That(entry.ConflictPriority, Is.EqualTo(2));

            entry = digest.Entries[1];
            Assert.That(entry.EndPoint, Is.EqualTo("http://www.example.com/sdata/myApp2/myContract/-/accounts"));
            Assert.That(entry.Tick, Is.EqualTo(10L));
            Assert.That(entry.Stamp, Is.EqualTo(new DateTime(2008, 10, 30, 12, 16, 51)));
            Assert.That(entry.ConflictPriority, Is.EqualTo(1));
        }

        [Test]
        public void Typical_Entry()
        {
            var xml = @"<entry xmlns=""http://www.w3.org/2005/Atom""
                               xmlns:sync=""http://schemas.sage.com/sdata/sync/2008/1"">
                          <sync:syncState>
                            <sync:endpoint>http://www.example.com/sdata/myApp1/myContract/-/accounts</sync:endpoint>
                            <sync:tick>5</sync:tick>
                            <sync:stamp>2008-10-30T14:55:43Z</sync:stamp>
                          </sync:syncState>
                        </entry>";
            var entry = new AtomEntry();
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                entry.Load(stream);
            }

            var syncState = entry.GetSDataSyncState();
            Assert.That(syncState.EndPoint, Is.EqualTo("http://www.example.com/sdata/myApp1/myContract/-/accounts"));
            Assert.That(syncState.Tick, Is.EqualTo(5L));
            Assert.That(syncState.Stamp, Is.EqualTo(new DateTime(2008, 10, 30, 14, 55, 43)));
        }
    }
}