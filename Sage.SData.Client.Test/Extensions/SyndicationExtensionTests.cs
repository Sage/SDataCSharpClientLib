using System.IO;
using System.Linq;
using System.Xml;
using NUnit.Framework;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Test.Extensions
{
    [TestFixture]
    public class SyndicationExtensionTests
    {
        [Test]
        public void Extension_Namespace_Declared_On_Extension_Element_Test()
        {
            const string xml = @"<entry xmlns=""http://www.w3.org/2005/Atom"">
                                   <opensearch:startIndex xmlns:opensearch=""http://a9.com/-/spec/opensearch/1.1/"">33</opensearch:startIndex>
                                 </entry>";
            var entry = new AtomEntry();

            using (var reader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(reader))
            {
                entry.Load(xmlReader);
            }

            Assert.That(entry.Extensions.OfType<OpenSearchExtension>().Any());
        }

        [Test]
        public void Extension_Namespace_Declared_On_Extension_Element_Without_Prefix_Test()
        {
            const string xml = @"<entry xmlns=""http://www.w3.org/2005/Atom"">
                                   <startIndex xmlns=""http://a9.com/-/spec/opensearch/1.1/"">33</startIndex>
                                 </entry>";
            var entry = new AtomEntry();

            using (var reader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(reader))
            {
                entry.Load(xmlReader);
            }

            Assert.That(entry.Extensions.OfType<OpenSearchExtension>().Any());
        }
    }
}