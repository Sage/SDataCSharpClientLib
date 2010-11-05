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
           xmlns:tns=""http://schemas.sage.com/crmErp/2008"" 
           xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""tradingAccount"" type=""tns:tradingAccount--type"" />
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
            Assert.That(schema.Resources.ContainsKey("tradingAccount"), Is.True);
            var resource = schema.Resources["tradingAccount"];
            Assert.That(resource, Is.Not.Null);
            Assert.That(resource.Properties.Length, Is.EqualTo(1));
            var property = resource.Properties[0];
            Assert.That(property, Is.Not.Null);
            Assert.That(property.Name, Is.EqualTo("active"));
        }
    }
}