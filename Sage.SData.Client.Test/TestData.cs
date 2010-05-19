using System.IO;
using System.Text;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Metadata;
using Sage.SData.Client.Test.Properties;

namespace Sage.SData.Client.Test
{
    public static class TestData
    {
        private static AtomFeed _feed;
        private static AtomEntry _entry;
        private static SDataSchema _schema;

        public static AtomFeed Feed
        {
            get
            {
                if (_feed == null)
                {
                    _feed = new AtomFeed();

                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.TestFeed)))
                    {
                        _feed.Load(stream);
                    }
                }

                return _feed;
            }
        }

        public static AtomEntry Entry
        {
            get
            {
                if (_entry == null)
                {
                    _entry = new AtomEntry();

                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.TestEntry)))
                    {
                        _entry.Load(stream);
                    }
                }

                return _entry;
            }
        }

        public static SDataSchema Schema
        {
            get
            {
                if (_schema == null)
                {
                    using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(Resources.TestSchema)))
                    {
                        _schema = SDataSchema.Read(stream);
                    }
                }

                return _schema;
            }
        }
    }
}