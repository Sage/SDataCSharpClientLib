using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;
using Sage.SData.Client.Test.Properties;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class AtomFeedReaderTests : AssertionHelper
    {
        private Mock<SDataService> _mock;
        private ISDataService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mock = new Mock<SDataService>(MockBehavior.Strict, "http://localhost:59213/sdata/aw/dynamic/-/", "lee", "abc123");
            _service = _mock.Object;
        }

        [Test]
        public void AtomFeedReader_Verify_CanRead()
        {
            var request = new SDataResourceCollectionRequest(_service);
            _mock.Setup(s => s.ReadFeed(request)).Returns(TestData.Feed);

            var reader = request.ExecuteReader();
            Expect(reader, Is.Not.Null);
        }

        [Test]
        public void AtomFeedReader_EnumeratorMatchesIndexer()
        {
            var request = new SDataResourceCollectionRequest(_service);
            _mock.Setup(s => s.ReadFeed(request)).Returns(TestData.Feed);

            var reader = request.ExecuteReader();
            var i = 0;

            foreach (var entry in reader)
            {
                Expect(entry, Is.EqualTo(reader[i]));
                i++;
            }
        }

        [Test]
        public void AtomFeedReader_IndexerMatchesCurrent()
        {
            var request = new SDataResourceCollectionRequest(_service);
            _mock.Setup(s => s.ReadFeed(request)).Returns(TestData.Feed);

            var reader = request.ExecuteReader();

            for (var i = 0; i < reader.Count; i++)
            {
                Expect(i, Is.EqualTo(reader.CurrentIndex));
                Expect(reader[i], Is.EqualTo(reader.Current));
                reader.MoveNext();
            }

            Expect(!reader.MoveNext());
        }

        [Test]
        public void AtomFeedReader_CurrentMatchesEnumerator()
        {
            var request = new SDataResourceCollectionRequest(_service);
            _mock.Setup(s => s.ReadFeed(request)).Returns(TestData.Feed);

            var reader = request.ExecuteReader();
            var enumerator = reader.GetEnumerator();

            do
            {
                enumerator.MoveNext();
                Expect(reader.Current, Is.EqualTo(enumerator.Current));
            } while (reader.MoveNext());

            Expect(!enumerator.MoveNext());
        }

        [Test]
        public void AtomFeedReader_MultiPageEnumerator()
        {
            var page1 = new AtomFeed();
            var page2 = new AtomFeed();

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.TestFeed)))
            {
                page1.Load(stream);
                stream.Seek(0, SeekOrigin.Begin);
                page2.Load(stream);
            }

            page1.SetOpenSearchStartIndex(1);
            page1.SetOpenSearchItemsPerPage(1);
            ((IList<AtomEntry>) page1.Entries).RemoveAt(1);

            page2.SetOpenSearchStartIndex(2);
            page2.SetOpenSearchItemsPerPage(1);
            ((IList<AtomEntry>) page2.Entries).RemoveAt(0);

            var pages = new Stack<AtomFeed>(new[] {page2, page1});

            var request = new SDataResourceCollectionRequest(_service);
            _mock.Setup(s => s.ReadFeed(request)).Returns(pages.Pop).AtMost(2);

            var reader = request.ExecuteReader();
            reader.ToList();
        }
    }
}