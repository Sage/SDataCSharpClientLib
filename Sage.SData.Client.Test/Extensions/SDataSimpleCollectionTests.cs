using System;
using System.Xml.XPath;
using NUnit.Framework;
using Sage.SData.Client.Extensions;
using System.Linq;

namespace Sage.SData.Client.Test.Extensions
{
    [TestFixture]
    public class SDataSimpleCollectionTests
    {
        #region deserialization tests

        private const string TestCase1 = @"
      <validationRule 
            sdata:key=""fc9bd0aee4d0445395f69dbc3070b6a1"" 
            sdata:uri=""http://localhost:8001/sdata/$app/metadata/-/validationRules('fc9bd0aee4d0445395f69dbc3070b6a1')"" 
            sdata:lookup=""http://localhost:8001/sdata/$app/metadata/-/validationRules"" 
            xmlns=""http://schemas.sage.com/gobiplatform/2010""
            xmlns:sdata=""http://schemas.sage.com/sdata/2008/1""
            xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
        <name>AccountTypeValidation</name>
        <displayName>Account Type Validation</displayName>
        <entity sdata:key=""Account"" sdata:uri=""http://localhost:8001/sdata/$app/metadata/-/entities('Account')"" sdata:lookup=""http://localhost:8001/sdata/$app/metadata/-/entities"" />
        <isPropertyLevel>true</isPropertyLevel>
        <propertyName>Type</propertyName>
        <customMessageTemplate>false</customMessageTemplate>
        <messageTemplate xsi:nil=""true"" />
        <validatorType>
          <listValidator>
            <items>
              <item>Customer</item>
              <item>Prospect</item>
              <item>Lead</item>
              <item>None</item>
            </items>
            <operator>In</operator>
          </listValidator>
        </validatorType>
      </validationRule>
";

        [Test]
        public void correctly_identifies_simple_collection()
        {
            var payload = SDataPayloadUtility.LoadPayload(TestCase1);

            var validatorType = (SDataPayloadCollection)payload.Values["validatorType"];
            var listValidator = validatorType[0];
            Assert.That(listValidator.Values["items"], Is.InstanceOf<SDataSimpleCollection>());
        }

        [Test]
        public void correct_number_of_items_in_simple_collection()
        {
            var payload = SDataPayloadUtility.LoadPayload(TestCase1);

            var validatorType = (SDataPayloadCollection)payload.Values["validatorType"];
            var listValidator = validatorType[0];
            Assume.That(listValidator.Values["items"], Is.InstanceOf<SDataSimpleCollection>());

            var items = (SDataSimpleCollection)listValidator.Values["items"];
            Assert.That(items, Has.Count.EqualTo(4));
        }

        [Test]
        public void correct_values_in_simple_collection()
        {
            var payload = SDataPayloadUtility.LoadPayload(TestCase1);

            var validatorType = (SDataPayloadCollection)payload.Values["validatorType"];
            var listValidator = validatorType[0];
            Assume.That(listValidator.Values["items"], Is.InstanceOf<SDataSimpleCollection>());

            var items = (SDataSimpleCollection)listValidator.Values["items"];
            var res = items.Intersect(ItemValues).Count();
            Assert.That(res, Is.EqualTo(4));
        }

        #endregion

        #region serialization tests

        private readonly SDataPayload SerializationTestCase1 = new SDataPayload
        {
            ResourceName = "validationRule",
            Namespace = "",
            Values =
            {
                { 
                    "validatorType", 
                    new SDataPayloadCollection
                        {
                            new SDataPayload
                                {
                                    ResourceName = "listValidator",
                                    Values =
                                    {
                                        {"items", new SDataSimpleCollection("item") {"Customer", "Prospect", "Lead", "None"}}
                                    }
                                }
                        } 
                    }
            }
        };

        private readonly string[] ItemValues = new[] { "Customer", "Prospect", "Lead", "None" };

        [Test]
        public void throws_exception_when_ItemElementName_is_not_set()
        {
            var payload = new SDataPayload
                              {
                                  ResourceName = "validationRule",
                                  Namespace = "",
                                  Values =
                                      {
                                          { "validatorType", new SDataSimpleCollection() {"item"} }
                                      }
                              };

            Assert.Throws(typeof (InvalidOperationException),
                          delegate { 
                              var res = SDataPayloadUtility.WritePayload(payload);
                          });
        }

        [Test]
        public void does_not_throw_exception_when_ItemElementName_is_set()
        {
            var res = SDataPayloadUtility.WritePayload(SerializationTestCase1);
            Assert.That(res, Is.InstanceOf<XPathNavigator>());
        }

        [Test]
        public void correctly_serializes_array_element_name()
        {
            var res = SDataPayloadUtility.WritePayload(SerializationTestCase1);
            var items = res.SelectSingleNode("//items");
            Assert.That(items, Is.Not.Null);
        }

        [Test]
        public void correct_number_of_items_serialized_in_array()
        {
            var res = SDataPayloadUtility.WritePayload(SerializationTestCase1);
            var items = res.Select("//items/item");
            Assert.That(items.Count, Is.EqualTo(4));
        }

        [Test]
        public void correct_values_serialized_in_array()
        {
            var res = SDataPayloadUtility.WritePayload(SerializationTestCase1);
            var itemIter = res.Select("//items/item");
            var count = itemIter.Cast<XPathNavigator>().Select(x => x.Value).Intersect(ItemValues).Count();
            Assert.That(count, Is.EqualTo(4));
        }

        private readonly SDataPayload SerializationTestCase2 = new SDataPayload
        {
            ResourceName = "validationRule",
            Namespace = "",
            Values =
            {
                { 
                    "validatorType", 
                    new SDataPayloadCollection
                        {
                            new SDataPayload
                                {
                                    ResourceName = "listValidator",
                                    Values =
                                    {
                                        {"items", new SDataSimpleCollection("item")}
                                    }
                                }
                        } 
                    }
            }
        };

        [Test]
        public void correctly_serializes_empty_array()
        {
            var res = SDataPayloadUtility.WritePayload(SerializationTestCase2);
            var items = res.SelectSingleNode("//items");
            
            Assert.That(items, Is.Not.Null);
            Assert.That(items.HasChildren, Is.False);
        }

        #endregion

    }
}