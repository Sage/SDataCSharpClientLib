using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Core;

namespace Sage.SData.Client.Test.Core
{
    [TestFixture]
    public class AtomFeedReaderTests : AssertionHelper
    {
        private MockSDataService _service;

        [TestFixtureSetUp]
        public void Setup()
        {
            _service = new MockSDataService();
        }

        [Test]
        public void AtomFeedReader_Verify_CanRead()
        {
            var request = new SDataResourceCollectionRequest(_service)
                          {
                              ResourceKind = "employees",
                              Count = 10,
                              StartIndex = 1
                          };
            var reader = request.ExecuteReader();
            Expect(reader, Is.Not.Null);
        }

        [Test]
        public void AtomFeedReader_EnumeratorMatchesIndexer()
        {
            var request = new SDataResourceCollectionRequest(_service);
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
            var reader = request.ExecuteReader();
            var enumerator = reader.GetEnumerator();

            do
            {
                enumerator.MoveNext();
                Expect(reader.Current, Is.EqualTo(enumerator.Current));
            } while (reader.MoveNext());

            Expect(!enumerator.MoveNext());
        }
    }
}