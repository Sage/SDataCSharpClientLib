using System.IO;
using NUnit.Framework;
using Sage.SData.Client.Metadata;

namespace Sage.SData.Client.Test.Metadata
{
    [TestFixture]
    public class SDataSchemaTests
    {
        [Test]
        public void Properties_Without_Types_Specified_Are_Supported_Test()
        {
            const string xsd = @"
<xs:schema targetNamespace=""http://schemas.sage.com/crmErp/2008""
           xmlns=""http://schemas.sage.com/crmErp/2008""
           xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:complexType name=""tradingAccount--type"">
    <xs:all>
      <xs:element name=""active"" />
    </xs:all>
  </xs:complexType>
</xs:schema>";
            SDataSchema schema;

            using (var reader = new StringReader(xsd))
            {
                schema = SDataSchema.Read(reader);
            }

            Assert.That(schema, Is.Not.Null);
            var resource = schema.ComplexTypes["tradingAccount--type"];
            Assert.That(resource, Is.Not.Null);
            Assert.That(resource.Properties.Count, Is.EqualTo(1));
            var property = resource.Properties[0];
            Assert.That(property, Is.Not.Null);
            Assert.That(property.Name, Is.EqualTo("active"));
            Assert.That(property.Type.QualifiedName.IsEmpty, Is.True);
        }

        [Test]
        public void Element_Then_Complex_Type_Then_List_Type_Test()
        {
            const string xsd = @"
<xs:schema targetNamespace=""http://schemas.sage.com/crmErp/2008""
           xmlns=""http://schemas.sage.com/crmErp/2008""
           xmlns:xs=""http://www.w3.org/2001/XMLSchema""
           xmlns:sme=""http://schemas.sage.com/sdata/sme/2007"">
  <xs:element name=""tradingAccount"" type=""tradingAccount--type"" sme:role=""resourceKind"" />
  <xs:complexType name=""tradingAccount--type"">
    <xs:all />
  </xs:complexType>
  <xs:complexType name=""tradingAccount--list"">
    <xs:sequence>
      <xs:element type=""tradingAccount--type"" />
    </xs:sequence>
  </xs:complexType>
</xs:schema>";
            SDataSchema schema;

            using (var reader = new StringReader(xsd))
            {
                schema = SDataSchema.Read(reader);
            }

            var resource = schema.ResourceTypes["tradingAccount"];
            Assert.That(resource, Is.Not.Null);
            Assert.That(resource.Name, Is.EqualTo("tradingAccount--type"));
            Assert.That(resource.ListName, Is.EqualTo("tradingAccount--list"));
        }

        [Test]
        public void Element_Then_List_Type_Then_Complex_Type_Test()
        {
            const string xsd = @"
<xs:schema targetNamespace=""http://schemas.sage.com/crmErp/2008""
           xmlns=""http://schemas.sage.com/crmErp/2008""
           xmlns:xs=""http://www.w3.org/2001/XMLSchema""
           xmlns:sme=""http://schemas.sage.com/sdata/sme/2007"">
  <xs:element name=""tradingAccount"" type=""tradingAccount--type"" sme:role=""resourceKind"" />
  <xs:complexType name=""tradingAccount--list"">
    <xs:sequence>
      <xs:element type=""tradingAccount--type"" />
    </xs:sequence>
  </xs:complexType>
  <xs:complexType name=""tradingAccount--type"">
    <xs:all />
  </xs:complexType>
</xs:schema>";
            SDataSchema schema;

            using (var reader = new StringReader(xsd))
            {
                schema = SDataSchema.Read(reader);
            }

            var resource = schema.ResourceTypes["tradingAccount"];
            Assert.That(resource, Is.Not.Null);
            Assert.That(resource.Name, Is.EqualTo("tradingAccount--type"));
            Assert.That(resource.ListName, Is.EqualTo("tradingAccount--list"));
        }
    }
}