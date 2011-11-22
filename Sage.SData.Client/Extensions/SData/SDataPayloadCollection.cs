using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.XPath;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Extensions
{
    public class SDataPayloadCollection : List<SDataPayload>
    {
        public Uri Uri { get; set; }
        public bool? DeleteMissing { get; set; }
        public bool IsNested { get; set; }
        public string ResourceName { get; set; }

        public SDataPayloadCollection()
        {
            IsNested = true;
        }

        public SDataPayloadCollection(string resourceName)
            : this()
        {
            ResourceName = resourceName;
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
            string value;
            Uri = source.TryGetAttribute("uri", Framework.Common.SData.Namespace, out value) && !string.IsNullOrEmpty(value) ? new Uri(value) : null;
            DeleteMissing = source.TryGetAttribute("deleteMissing", Framework.Common.SData.Namespace, out value) ? XmlConvert.ToBoolean(value) : (bool?) null;
            IsNested = true;

            var items = source.SelectChildren(XPathNodeType.Element).Cast<XPathNavigator>();
            return InternalLoad(items, manager);
        }

        public bool Load(IEnumerable<XPathNavigator> items, XmlNamespaceManager manager)
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(items, "items");

            //------------------------------------------------------------
            //	Attempt to extract syndication information
            //------------------------------------------------------------
            IsNested = false;
            return InternalLoad(items, manager);
        }

        private bool InternalLoad(IEnumerable<XPathNavigator> items, XmlNamespaceManager manager)
        {
            foreach (var item in items)
            {
                var child = new SDataPayload();

                if (!child.Load(item, manager))
                {
                    return false;
                }

                if (SDataPayload.InferItemType(item) == SDataPayload.ItemType.Property)
                {
                    string nilValue;
                    object value;

                    if (item.TryGetAttribute("nil", Framework.Common.XSI.Namespace, out nilValue) && XmlConvert.ToBoolean(nilValue))
                    {
                        value = null;
                    }
                    else
                    {
                        value = item.Value;
                    }

                    child.Values.Add(new KeyValuePair<string, object>(item.LocalName, value));
                    break;
                }

                Add(child);

                if (string.IsNullOrEmpty(ResourceName))
                {
                    ResourceName = child.ResourceName;
                }
            }

            return true;
        }

        public void WriteTo(string name, string ns, XmlWriter writer, string xmlNamespace)
        {
            if (IsNested)
            {
                writer.WriteStartElement(name, ns);

                if (Uri != null) writer.WriteAttributeString("uri", xmlNamespace, Uri.AbsoluteUri);
                if (DeleteMissing != null) writer.WriteAttributeString("deleteMissing", xmlNamespace, XmlConvert.ToString(DeleteMissing.Value));
            }

            foreach (var item in this)
            {
                item.WriteTo(ResourceName, ns, writer, xmlNamespace);
            }

            if (IsNested)
            {
                writer.WriteEndElement();
            }
        }
    }
}