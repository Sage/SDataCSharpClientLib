using System.Xml.Serialization;

namespace Sage.SData.Client.Metadata
{
    public enum ComplianceLevel
    {
        [XmlEnum("may")] May,
        [XmlEnum("should")] Should,
        [XmlEnum("must")] Must
    }
}