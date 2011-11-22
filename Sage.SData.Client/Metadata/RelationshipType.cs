using System.Xml.Serialization;

namespace Sage.SData.Client.Metadata
{
    public enum RelationshipType
    {
        [XmlEnum("parent")] Parent,
        [XmlEnum("child")] Child,
        [XmlEnum("reference")] Reference,
        [XmlEnum("association")] Association
    }
}