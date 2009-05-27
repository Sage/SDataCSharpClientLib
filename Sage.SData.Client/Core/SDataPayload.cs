using System.Xml.XPath;
using Sage.SData.Client.Atom;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Wrapper class for SData payloads
    /// </summary>
    public class SDataPayload
    {
        private XPathNavigator _payload;
        private AtomEntry _parent;

        /// <summary>
        /// Navigator for the payload
        /// </summary>
        public XPathNavigator Payload
        {
            get { return _payload; }
            set { _payload = value;}
        }

        /// <summary>
        /// Parent AtomEntry for the payload
        /// </summary>
        public AtomEntry Parent
        {
            get { return _parent; }
            set{ _parent = value;}
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="payload">payload item</param>
        /// <param name="parent">parent entry</param>
        public SDataPayload(XPathNavigator payload, AtomEntry parent)
        {
            Payload = payload;
            Parent = parent;
        }

    }
}