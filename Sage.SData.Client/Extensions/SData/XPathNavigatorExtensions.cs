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
    }
}