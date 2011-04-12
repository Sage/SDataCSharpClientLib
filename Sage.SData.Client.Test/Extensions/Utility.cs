using System.IO;
using System.Xml;
using System.Xml.XPath;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Test.Extensions
{
    public static class Utility
    {
        public static SDataPayload LoadPayload(string xml)
        {
            var payload = new SDataPayload();

            using (var strReader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(strReader))
            {
                var doc = new XPathDocument(xmlReader);
                var source = doc.CreateNavigator();
                var manager = new XmlNamespaceManager(source.NameTable);
                source.MoveToFirstChild();
                payload.Load(source, manager);
            }

            return payload;
        }

        public static XPathNavigator WritePayload(SDataPayload payload)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream))
                {
                    payload.WriteTo(writer, Client.Framework.Common.SData.Namespace);
                }

                stream.Seek(0, SeekOrigin.Begin);
                return new XPathDocument(stream).CreateNavigator();
            }
        }
    }
}