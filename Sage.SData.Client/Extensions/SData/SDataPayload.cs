using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Extensions
{
    [TypeDescriptionProvider(typeof (SDataPayloadTypeDescriptionProvider))]
    public class SDataPayload
    {
        private IDictionary<string, object> _values;

        public string ResourceName { get; set; }
        public string Namespace { get; set; }
        public string Key { get; set; }
        public Uri Uri { get; set; }
        public Guid? Uuid { get; set; }
        public string Descriptor { get; set; }
        public string Lookup { get; set; }
        public bool? IsDeleted { get; set; }

        public IDictionary<string, object> Values
        {
            get { return _values ?? (_values = new Dictionary<string, object>()); }
        }

        public bool Load(XPathNavigator source, XmlNamespaceManager manager)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");

            //------------------------------------------------------------
            //	Attempt to extract syndication information
            //------------------------------------------------------------
            ResourceName = source.LocalName;
            Namespace = source.NamespaceURI;

            string value;
            Key = source.TryGetAttribute("key", Framework.Common.SData.Namespace, out value) ? value : null;
            Uri = source.TryGetAttribute("uri", Framework.Common.SData.Namespace, out value) && !string.IsNullOrEmpty(value) ? new Uri(value) : null;
            Uuid = source.TryGetAttribute("uuid", Framework.Common.SData.Namespace, out value) && !string.IsNullOrEmpty(value) ? new Guid(value) : (Guid?) null;
            Descriptor = source.TryGetAttribute("descriptor", Framework.Common.SData.Namespace, out value) ? value : null;
            Lookup = source.TryGetAttribute("lookup", Framework.Common.SData.Namespace, out value) ? value : null;
            IsDeleted = source.TryGetAttribute("isDeleted", Framework.Common.SData.Namespace, out value) && !string.IsNullOrEmpty(value) ? XmlConvert.ToBoolean(value) : (bool?) null;

            return source.SelectChildren(XPathNodeType.Element).Cast<XPathNavigator>().All(item => LoadItem(item, manager));
        }

        private bool LoadItem(XPathNavigator source, XmlNamespaceManager manager)
        {
            object item;

            switch (InferItemType(source))
            {
                case ItemType.Property:
                {
                    string value;

                    if (source.TryGetAttribute("nil", Framework.Common.XSI.Namespace, out value) && XmlConvert.ToBoolean(value))
                    {
                        item = null;
                    }
                    else
                    {
                        item = source.Value;
                    }
                }
                    break;
                case ItemType.Object:
                {
                    var obj = new SDataPayload();

                    if (!obj.Load(source, manager))
                    {
                        return false;
                    }

                    item = obj;
                }
                    break;
                case ItemType.Collection:
                {
                    var collection = new SDataPayloadCollection();

                    if (!collection.Load(source, manager))
                    {
                        return false;
                    }

                    item = collection;
                }
                    break;
                default:
                    return false;
            }

            Values[source.LocalName] = item;
            return true;
        }

        private static ItemType InferItemType(XPathNavigator source)
        {
            string value;

            if (source.TryGetAttribute("nil", Framework.Common.XSI.Namespace, out value) && XmlConvert.ToBoolean(value))
            {
                return ItemType.Property;
            }

            if (source.HasAttribute("key", Framework.Common.SData.Namespace) ||
                source.HasAttribute("uuid", Framework.Common.SData.Namespace) ||
                source.HasAttribute("lookup", Framework.Common.SData.Namespace) ||
                source.HasAttribute("descriptor", Framework.Common.SData.Namespace))
            {
                return ItemType.Object;
            }

            if (source.HasAttribute("url", Framework.Common.SData.Namespace) ||
                source.HasAttribute("deleteMissing", Framework.Common.SData.Namespace) ||
                source.IsEmptyElement)
            {
                return ItemType.Collection;
            }

            var children = source.SelectChildren(XPathNodeType.Element).Cast<XPathNavigator>();
            var childCount = children.Count();

            if (childCount <= 0)
            {
                return ItemType.Property;
            }

            if (children.Select(child => child.LocalName).Distinct().Count() == childCount &&
                children.Any(item => InferItemType(item) != ItemType.Object))
            {
                return ItemType.Object;
            }

            return ItemType.Collection;
        }

        private enum ItemType
        {
            Property,
            Object,
            Collection
        }

        public void WriteTo(XmlWriter writer, string xmlNamespace)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(writer, "writer");

            //------------------------------------------------------------
            //	Write XML representation of the current instance
            //------------------------------------------------------------
            writer.WriteStartElement("payload", xmlNamespace);
            WriteTo(ResourceName, Namespace, writer, xmlNamespace);
            writer.WriteEndElement();
        }

        internal void WriteTo(string name, string ns, XmlWriter writer, string xmlNamespace)
        {
            writer.WriteStartElement(name, ns);

            if (Key != null) writer.WriteAttributeString("key", xmlNamespace, Key);
            if (Uri != null) writer.WriteAttributeString("uri", xmlNamespace, Uri.ToString().Replace(" ", "%20"));
            if (Uuid != null) writer.WriteAttributeString("uuid", xmlNamespace, Uuid.ToString());
            if (Descriptor != null) writer.WriteAttributeString("descriptor", xmlNamespace, Descriptor);
            if (Lookup != null) writer.WriteAttributeString("lookup", xmlNamespace, Lookup);
            if (IsDeleted != null) writer.WriteAttributeString("isDeleted", xmlNamespace, XmlConvert.ToString(IsDeleted.Value));

            foreach (var pair in Values)
            {
                WriteItemTo(pair.Key, ns, pair.Value, writer, xmlNamespace);
            }

            writer.WriteEndElement();
        }

        private void WriteItemTo(string name, string ns, object value, XmlWriter writer, string xmlNamespace)
        {
            if (value == null)
            {
                writer.WriteStartElement(name, Namespace);
                writer.WriteAttributeString("nil", Framework.Common.XSI.Namespace, XmlConvert.ToString(true));
                writer.WriteEndElement();
            }
            else if (value is SDataPayload)
            {
                var child = (SDataPayload) value;
                child.WriteTo(name, ns, writer, xmlNamespace);
            }
            else if (value is SDataPayloadCollection)
            {
                var children = (SDataPayloadCollection) value;
                children.WriteTo(name, ns, writer, xmlNamespace);
            }
            else
            {
                writer.WriteElementString(name, Namespace, value.ToString());
            }
        }
    }
}