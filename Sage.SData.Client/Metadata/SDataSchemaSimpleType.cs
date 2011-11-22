using System.Collections.Generic;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaSimpleType : SDataSchemaValueType
    {
        private IList<XmlSchemaFacet> _facets;

        public SDataSchemaSimpleType()
        {
        }

        public SDataSchemaSimpleType(string baseName)
            : base(baseName, "type")
        {
        }

        public IList<XmlSchemaFacet> Facets
        {
            get { return _facets ?? (_facets = new List<XmlSchemaFacet>()); }
        }

        protected internal override void Read(XmlSchemaObject obj)
        {
            var simpleType = (XmlSchemaSimpleType) obj;
            var restriction = (XmlSchemaSimpleTypeRestriction) simpleType.Content;

            foreach (XmlSchemaFacet facet in restriction.Facets)
            {
                Facets.Add(facet);
            }

            base.Read(obj);
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
            var simpleType = (XmlSchemaSimpleType) obj;
            var restriction = new XmlSchemaSimpleTypeRestriction();

            foreach (var facet in Facets)
            {
                restriction.Facets.Add(facet);
            }

            simpleType.Content = restriction;
            base.Write(obj);
        }
    }
}