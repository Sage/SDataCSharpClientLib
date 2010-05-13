using System;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Test.Extensions
{
    [TestFixture]
    public class SDataPayloadTests
    {
        [Test]
        public void Typical()
        {
            var xml = @"<salesOrder sdata:key=""43660""
                                    xmlns=""http://schemas.sage.com/myContract""
                                    xmlns:sdata=""http://schemas.sage.com/sdata/2008/1""
                                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
                          <orderDate>2001-07-01</orderDate>
                          <shipDate xsi:nil=""true"" />
                          <contact sdata:key=""216"" 
                                   sdata:uri=""http://www.example.com/sdata/myApp/myContract/-/contacts('216')"" 
                                   sdata:lookup=""http://www.example.com/sdata/myApp/myContract/-/contacts""/>
                          <orderLines sdata:uri=""http://www.example.com/sdata/myApp/myContract/-/salesOrderLines?where=salesOrderID%20eq%2043660""/>
                        </salesOrder>";
            var payload = LoadPayload(xml);

            Assert.That(payload.ResourceName, Is.EqualTo("salesOrder"));
            Assert.That(payload.Namespace, Is.EqualTo("http://schemas.sage.com/myContract"));
            Assert.That(payload.Key, Is.EqualTo("43660"));
            Assert.That(payload.Values.Count, Is.EqualTo(4));

            object value;
            payload.Values.TryGetValue("orderDate", out value);
            Assert.That(value, Is.EqualTo("2001-07-01"));

            payload.Values.TryGetValue("shipDate", out value);
            Assert.That(value, Is.Null);

            payload.Values.TryGetValue("contact", out value);
            Assert.That(value, Is.InstanceOfType(typeof (SDataPayload)));
            var obj = (SDataPayload) value;
            Assert.That(obj.Key, Is.EqualTo("216"));
            Assert.That(obj.Uri, Is.EqualTo(new Uri("http://www.example.com/sdata/myApp/myContract/-/contacts('216')")));
            Assert.That(obj.Lookup, Is.EqualTo("http://www.example.com/sdata/myApp/myContract/-/contacts"));
            CollectionAssert.IsEmpty(obj.Values);

            payload.Values.TryGetValue("orderLines", out value);
            Assert.That(value, Is.InstanceOfType(typeof (SDataPayloadCollection)));
            var col = (SDataPayloadCollection) value;
            Assert.That(col.Uri, Is.EqualTo(new Uri("http://www.example.com/sdata/myApp/myContract/-/salesOrderLines?where=salesOrderID%20eq%2043660")));
            CollectionAssert.IsEmpty(col);
        }

        [Test]
        public void Object_Property_Without_Attributes()
        {
            var xml = @"<salesOrder>
                          <contact>
                            <firstName>John</firstName>
                            <lastName>Smith</lastName>
                          </contact>
                        </salesOrder>";
            var payload = LoadPayload(xml);

            Assert.That(payload.ResourceName, Is.EqualTo("salesOrder"));
            Assert.That(payload.Values.Count, Is.EqualTo(1));

            object value;
            payload.Values.TryGetValue("contact", out value);
            Assert.That(value, Is.InstanceOfType(typeof (SDataPayload)));
            var obj = (SDataPayload) value;
            Assert.That(obj.Values.Count, Is.EqualTo(2));

            obj.Values.TryGetValue("firstName", out value);
            Assert.That(value, Is.EqualTo("John"));

            obj.Values.TryGetValue("lastName", out value);
            Assert.That(value, Is.EqualTo("Smith"));
        }

        [Test]
        public void Empty_Collection_Property_Without_Attributes()
        {
            var xml = @"<salesOrder>
                          <orderLines />
                        </salesOrder>";
            var payload = LoadPayload(xml);

            Assert.That(payload.ResourceName, Is.EqualTo("salesOrder"));
            Assert.That(payload.Values.Count, Is.EqualTo(1));

            object value;
            payload.Values.TryGetValue("orderLines", out value);
            Assert.That(value, Is.InstanceOfType(typeof (SDataPayloadCollection)));
            var col = (SDataPayloadCollection) value;
            CollectionAssert.IsEmpty(col);
        }

        [Test]
        public void Collection_Property_Without_Attributes()
        {
            var xml = @"<salesOrder xmlns:sdata=""http://schemas.sage.com/sdata/2008/1"">
                          <orderLines>
                            <salesOrderLine sdata:key=""43660-1"" />
                            <salesOrderLine sdata:key=""43660-2"" />
                          </orderLines>
                        </salesOrder>";
            var payload = LoadPayload(xml);

            Assert.That(payload.ResourceName, Is.EqualTo("salesOrder"));
            Assert.That(payload.Values.Count, Is.EqualTo(1));

            object value;
            payload.Values.TryGetValue("orderLines", out value);
            Assert.That(value, Is.InstanceOfType(typeof (SDataPayloadCollection)));
            var col = (SDataPayloadCollection) value;
            Assert.That(col.Count, Is.EqualTo(2));

            var item = col[0];
            Assert.That(item, Is.InstanceOfType(typeof (SDataPayload)));
            Assert.That(item.ResourceName, Is.EqualTo("salesOrderLine"));
            Assert.That(item.Key, Is.EqualTo("43660-1"));
            CollectionAssert.IsEmpty(item.Values);

            item = col[1];
            Assert.That(item, Is.InstanceOfType(typeof (SDataPayload)));
            Assert.That(item.ResourceName, Is.EqualTo("salesOrderLine"));
            Assert.That(item.Key, Is.EqualTo("43660-2"));
            CollectionAssert.IsEmpty(item.Values);
        }

        private static SDataPayload LoadPayload(string xml)
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
    }
}