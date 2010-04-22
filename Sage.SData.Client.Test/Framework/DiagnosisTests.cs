using System.IO;
using System.Xml;
using System.Xml.XPath;
using NUnit.Framework;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Test.Framework
{
    [TestFixture]
    public class DiagnosisTests
    {
        /// <summary>
        /// According to section 3.10 of the SData spec, the sdataCode and
        /// applicationCode elements on the diagnosis type are both strings,
        /// not integers. sdataCode should be an enumeration since it has a
        /// predefined set of values.
        /// </summary>
        [Test]
        public void Diagnosis_SDataCode_Should_Be_An_Enum_Test()
        {
            var diagnosis = new Diagnosis();
            diagnosis.SDataCode = DiagnosisCode.ApplicationDiagnosis;
            diagnosis.ApplicationCode = "Application error";
            string xml;

            using (var textWriter = new StringWriter())
            using (var xmlWriter = new XmlTextWriter(textWriter))
            {
                diagnosis.WriteTo(xmlWriter, null);
                xml = textWriter.ToString();
            }

            XPathNavigator nav;

            using (var textReader = new StringReader(xml))
            using (var xmlReader = new XmlTextReader(textReader))
            {
                nav = new XPathDocument(xmlReader).CreateNavigator();
            }

            var node = nav.SelectSingleNode("diagnosis/sdataCode");
            Assert.IsNotNull(node);
            Assert.AreEqual("ApplicationDiagnosis", node.Value);

            node = nav.SelectSingleNode("diagnosis/applicationCode");
            Assert.IsNotNull(node);
            Assert.AreEqual("Application error", node.Value);
        }
    }
}