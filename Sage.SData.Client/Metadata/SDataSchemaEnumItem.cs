using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaEnumItem : SDataSchemaItem
    {
        public string Value { get; set; }

        protected internal override void Read(XmlSchemaObject obj)
        {
            var facet = (XmlSchemaEnumerationFacet) obj;
            Value = facet.Value;
            base.Read(obj);
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
            var facet = (XmlSchemaEnumerationFacet) obj;
            facet.Value = Value;
            base.Write(obj);
        }
    }
}