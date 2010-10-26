using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchema
    {
        public static SDataSchema Read(Stream stream)
        {
            return Read(stream, null);
        }

        public static SDataSchema Read(Stream stream, string targetElementName)
        {
            using (var reader = new StreamReader(stream))
            {
                return Read(reader, targetElementName);
            }
        }

        public static SDataSchema Read(TextReader reader)
        {
            return Read(reader, null);
        }

        public static SDataSchema Read(TextReader reader, string targetElementName)
        {
            var xsd = XmlSchema.Read(reader, null);

            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(xsd);

            var resources = new Dictionary<string, SDataResource>();

            foreach (DictionaryEntry entry in xsd.Elements)
            {
                var name = (XmlQualifiedName) entry.Key;

                if (name.Namespace != xsd.TargetNamespace)
                {
                    continue;
                }

                var resource = new SDataResource();
                resource.Load(name, schemaSet);
                resources.Add(name.Name, resource);
            }

            return new SDataSchema
                   {
                       TargetElementName = targetElementName,
                       Namespace = xsd.TargetNamespace,
                       Resources = resources
                   };
        }

        private SDataSchema()
        {
        }

        public string TargetElementName { get; private set; }
        public string Namespace { get; private set; }
        public IDictionary<string, SDataResource> Resources { get; private set; }

        public void Write(Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                Write(writer);
            }
        }

        public void Write(TextWriter writer)
        {
            var resource = Resources.Values.FirstOrDefault();
            var str = MetadataManager.GetSchema(Namespace, resource != null ? resource.Namespaces : null, Resources.Values);
            writer.Write(str);
        }
    }
}