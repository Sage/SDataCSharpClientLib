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

            return source.SelectChildren(XPathNodeType.Element)
                .Cast<XPathNavigator>()
                .GroupBy(item => item.LocalName)
                .All(group => LoadItem(group.Key, group, manager));
        }

        private bool LoadItem(string name, IEnumerable<XPathNavigator> items, XmlNamespaceManager manager)
        {
            object value;

            if (items.Count() > 1)
            {
                var collection = new SDataPayloadCollection();

                if (!collection.Load(items, manager))
                {
                    return false;
                }

                value = collection;
            }
            else
            {
                var item = items.First();

                switch (InferItemType(item))
                {
                    case ItemType.Property:
                    {
                        string nilValue;

                        if (item.TryGetAttribute("nil", Framework.Common.XSI.Namespace, out nilValue) && XmlConvert.ToBoolean(nilValue))
                        {
                            value = null;
                        }
                        else
                        {
                            value = item.Value;
                        }

                        break;
                    }
                    case ItemType.Object:
                    {
                        var obj = new SDataPayload();

                        if (!obj.Load(item, manager))
                        {
                            return false;
                        }

                        value = obj;
                        break;
                    }
                    case ItemType.PayloadCollection:
                    {
                        var collection = new SDataPayloadCollection();

                        if (!collection.Load(item, manager))
                        {
                            return false;
                        }

                        value = collection;
                        break;
                    }
                    case ItemType.SimpleCollection:
                    {
                        var collection = new SDataSimpleCollection();

                        if (!collection.Load(item))
                            return false;

                        value = collection;
                        break;
                    }
                    default:
                        return false;
                }
            }

            Values.Add(name, value);
            return true;
        }

        internal static ItemType InferItemType(XPathNavigator item)
        {
            string value;

            if (item.TryGetAttribute("nil", Framework.Common.XSI.Namespace, out value) && XmlConvert.ToBoolean(value))
            {
                return ItemType.Property;
            }

            if (item.HasAttribute("key", Framework.Common.SData.Namespace) ||
                item.HasAttribute("uuid", Framework.Common.SData.Namespace) ||
                item.HasAttribute("lookup", Framework.Common.SData.Namespace) ||
                item.HasAttribute("descriptor", Framework.Common.SData.Namespace))
            {
                return ItemType.Object;
            }

            if (item.HasAttribute("url", Framework.Common.SData.Namespace) ||
                item.HasAttribute("uri", Framework.Common.SData.Namespace) ||
                item.HasAttribute("deleteMissing", Framework.Common.SData.Namespace))
            {
                return ItemType.PayloadCollection;
            }

            if (item.IsEmptyElement)
            {
                // workaround: Older versions of the SIF generate payload collections as empty namespace-less elements 
                return string.IsNullOrEmpty(item.NamespaceURI) ? ItemType.PayloadCollection : ItemType.Property;
            }

            var children = item.SelectChildren(XPathNodeType.Element).Cast<XPathNavigator>();
            var childCount = children.Count();

            if (childCount == 0)
            {
                return ItemType.Property;
            }

            if (childCount > 1 && children.Select(child => child.LocalName).Distinct().Count() == 1)
            {
                if (children.All(child => InferItemType(child) == ItemType.Object))
                {
                    return ItemType.PayloadCollection;
                }
                if (children.All(child => InferItemType(child) == ItemType.Property))
                {
                    return ItemType.SimpleCollection;
                }
            }

            return ItemType.Object;
        }

        internal enum ItemType
        {
            Property,
            Object,
            PayloadCollection,
            SimpleCollection
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
            if (string.IsNullOrEmpty(name))
            {
                name = ResourceName;
            }

            if (string.IsNullOrEmpty(ns))
            {
                ns = Namespace;
            }

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
            else if (value is SDataSimpleCollection)
            {
                var children = (SDataSimpleCollection) value;
                children.WriteTo(name, ns, writer);
            }
            else
            {
                writer.WriteElementString(name, Namespace, ValueToString(value));
            }
        }

        internal static string ValueToString(object value)
        {
            if (value is byte)
            {
                return XmlConvert.ToString((byte) value);
            }
            if (value is sbyte)
            {
                return XmlConvert.ToString((sbyte) value);
            }
            if (value is short)
            {
                return XmlConvert.ToString((short) value);
            }
            if (value is ushort)
            {
                return XmlConvert.ToString((ushort) value);
            }
            if (value is int)
            {
                return XmlConvert.ToString((int) value);
            }
            if (value is uint)
            {
                return XmlConvert.ToString((uint) value);
            }
            if (value is long)
            {
                return XmlConvert.ToString((long) value);
            }
            if (value is ulong)
            {
                return XmlConvert.ToString((ulong) value);
            }

            if (value is bool)
            {
                return XmlConvert.ToString((bool) value);
            }
            if (value is char)
            {
                return XmlConvert.ToString((char) value);
            }
            if (value is float)
            {
                return XmlConvert.ToString((float) value);
            }
            if (value is double)
            {
                return XmlConvert.ToString((double) value);
            }
            if (value is decimal)
            {
                return XmlConvert.ToString((decimal) value);
            }

            if (value is Guid)
            {
                return XmlConvert.ToString((Guid) value);
            }
            if (value is DateTime)
            {
                return XmlConvert.ToString((DateTime) value, XmlDateTimeSerializationMode.RoundtripKind);
            }
            if (value is DateTimeOffset)
            {
                return XmlConvert.ToString((DateTimeOffset) value);
            }
            if (value is TimeSpan)
            {
                return XmlConvert.ToString((TimeSpan) value);
            }

            return value.ToString();
        }
    }
}