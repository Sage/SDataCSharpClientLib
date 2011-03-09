using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.XPath;

namespace Sage.SData.Client.Extensions
{
    internal static class XPathNavigatorExtensions
    {
        public static bool HasAttribute(this XPathNavigator navigator, string localName, string namespaceUri)
        {
            var result = navigator.MoveToAttribute(localName, namespaceUri);

            if (result)
            {
                navigator.MoveToParent();
            }

            return result;
        }

        public static bool TryGetAttribute(this XPathNavigator navigator, string localName, string namespaceUri, out string value)
        {
            var result = navigator.MoveToAttribute(localName, namespaceUri);

            if (result)
            {
                value = navigator.Value;
                navigator.MoveToParent();
            }
            else
            {
                value = null;
            }

            return result;
        }

        public static IEnumerable<KeyValuePair<string, string>> GetAllNamespaces(this XPathNavigator source)
        {
            return source.GetNamespacesInScope(XmlNamespaceScope.ExcludeXml)
                .Concat(source.Select("descendant::*").Cast<XPathNavigator>()
                            .SelectMany(descendant => descendant.GetNamespacesInScope(XmlNamespaceScope.Local)));
        }
    }
}