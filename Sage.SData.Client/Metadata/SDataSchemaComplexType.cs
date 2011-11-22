using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaComplexType : SDataSchemaType
    {
        private SDataSchemaTypeReference _baseType;
        private KeyedObjectCollection<SDataSchemaProperty> _properties;

        public SDataSchemaComplexType()
        {
        }

        public SDataSchemaComplexType(string baseName)
            : base(baseName, "type")
        {
        }

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

        public override IEnumerable<SDataSchemaObject> Children
        {
            get { return (BaseType != null ? new[] {BaseType} : base.Children).Concat(Properties.Cast<SDataSchemaObject>()); }
        }

        public KeyedObjectCollection<SDataSchemaProperty> Properties
        {
            get { return _properties ?? (_properties = new KeyedObjectCollection<SDataSchemaProperty>(this, prop => prop.Name)); }
        }

        public KeyedEnumerable<string, SDataSchemaValueProperty> ValueProperties
        {
            get { return new KeyedEnumerable<string, SDataSchemaValueProperty>(Properties.OfType<SDataSchemaValueProperty>(), prop => prop.Name); }
        }

        public KeyedEnumerable<string, SDataSchemaRelationshipProperty> RelationshipProperties
        {
            get { return new KeyedEnumerable<string, SDataSchemaRelationshipProperty>(Properties.OfType<SDataSchemaRelationshipProperty>(), prop => prop.Name); }
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
                    if (type.ContentModel.Content == null)
                    {
                        throw new InvalidOperationException(string.Format("Missing content on complex type content model '{0}'", type.Name));
                    }

                    var extension = type.ContentModel.Content as XmlSchemaComplexContentExtension;

                    if (extension == null)
                    {
                        throw new InvalidOperationException(string.Format("Unexpected content type '{0}' on complex type content model '{1}'", type.ContentModel.Content.GetType(), type.Name));
                    }

                    AnyAttribute = extension.AnyAttribute;
                    BaseType = extension.BaseTypeName;
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
                        throw new InvalidOperationException(string.Format("Unexpected particle type '{0}' on complex type '{1}'", particle.GetType(), type.Name));
                    }

                    foreach (var item in all.Items)
                    {
                        var element = item as XmlSchemaElement;

                        if (element == null)
                        {
                            throw new InvalidOperationException(string.Format("Unexpected all item type '{0}' on complex type '{1}'", item.GetType(), type.Name));
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