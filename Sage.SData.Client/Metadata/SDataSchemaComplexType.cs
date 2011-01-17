using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaComplexType : SDataSchemaType
    {
        private SDataSchemaTypeReference _baseType;
        private SDataSchemaKeyedObjectCollection<SDataSchemaProperty> _properties;

        public SDataSchemaTypeReference BaseType
        {
            get { return _baseType; }
            set
            {
                if (_baseType != value)
                {
                    _baseType = value;
                    value.Parent = this;
                }
            }
        }

        public XmlSchemaAnyAttribute AnyAttribute { get; set; }
        public string ListName { get; set; }
        public string ListItemName { get; set; }
        public XmlSchemaAnyAttribute ListAnyAttribute { get; set; }

        public override IEnumerable<SDataSchemaObject> Children
        {
            get { return (BaseType != null ? new[] {BaseType} : base.Children).Concat(Properties.Cast<SDataSchemaObject>()); }
        }

        public XmlQualifiedName ListQualifiedName
        {
            get { return new XmlQualifiedName(ListName, Schema != null ? Schema.TargetNamespace : null); }
        }

        public SDataSchemaKeyedObjectCollection<SDataSchemaProperty> Properties
        {
            get { return _properties ?? (_properties = new SDataSchemaKeyedObjectCollection<SDataSchemaProperty>(this, prop => prop.Name)); }
        }

        public SDataSchemaKeyedEnumerable<SDataSchemaValueProperty> ValueProperties
        {
            get { return new SDataSchemaKeyedEnumerable<SDataSchemaValueProperty>(Properties.OfType<SDataSchemaValueProperty>(), prop => prop.Name); }
        }

        public SDataSchemaKeyedEnumerable<SDataSchemaRelationshipProperty> RelationshipProperties
        {
            get { return new SDataSchemaKeyedEnumerable<SDataSchemaRelationshipProperty>(Properties.OfType<SDataSchemaRelationshipProperty>(), prop => prop.Name); }
        }

        protected internal override void Read(XmlSchemaObject obj)
        {
            if (obj is XmlSchemaComplexType)
            {
                var type = (XmlSchemaComplexType) obj;
                AnyAttribute = type.AnyAttribute;
                var particle = type.Particle;

                if (particle == null && type.ContentModel != null)
                {
                    var extension = type.ContentModel.Content as XmlSchemaComplexContentExtension;

                    if (extension == null)
                    {
                        throw new NotSupportedException();
                    }

                    AnyAttribute = extension.AnyAttribute;
                    BaseType = new SDataSchemaTypeReference(extension.BaseTypeName);
                    particle = extension.Particle;
                }
                else
                {
                    AnyAttribute = type.AnyAttribute;
                }

                if (particle != null)
                {
                    var all = particle as XmlSchemaAll;

                    if (all == null)
                    {
                        throw new NotSupportedException();
                    }

                    foreach (var item in all.Items)
                    {
                        var element = item as XmlSchemaElement;

                        if (element == null)
                        {
                            throw new NotSupportedException();
                        }

                        SDataSchemaProperty prop;

                        if (element.UnhandledAttributes != null && element.UnhandledAttributes.Any(attr => attr.NamespaceURI == SmeNamespaceUri && attr.LocalName == "relationship"))
                        {
                            prop = new SDataSchemaRelationshipProperty();
                        }
                        else
                        {
                            prop = new SDataSchemaValueProperty();
                        }

                        prop.Read(element);
                        Properties.Add(prop);
                    }
                }
            }

            base.Read(obj);
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
            if (obj is XmlSchemaComplexType)
            {
                var type = (XmlSchemaComplexType) obj;
                type.Name = Name;
                var all = new XmlSchemaAll();

                foreach (var prop in Properties)
                {
                    var element = new XmlSchemaElement();
                    prop.Write(element);
                    all.Items.Add(element);
                }

                if (BaseType != null)
                {
                    type.ContentModel = new XmlSchemaComplexContent
                                        {
                                            Content = new XmlSchemaComplexContentExtension
                                                      {
                                                          BaseTypeName = BaseType.QualifiedName,
                                                          AnyAttribute = AnyAttribute,
                                                          Particle = all
                                                      }
                                        };
                }
                else
                {
                    type.AnyAttribute = AnyAttribute;
                    type.Particle = all;
                }
            }

            base.Write(obj);
        }
    }
}