using System.Collections.Generic;
using System.Xml;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaServiceOperationType : SDataSchemaTopLevelType
    {
        private InvocationMode? _invocationMode;

        public SDataSchemaServiceOperationType()
        {
        }

        public SDataSchemaServiceOperationType(string elementName)
            : base(elementName)
        {
        }

        /// <summary>
        /// The supported invocation modes.
        /// </summary>
        public InvocationMode InvocationMode
        {
            get { return _invocationMode ?? InvocationMode.Sync; }
            set { _invocationMode = value; }
        }

        protected override bool ReadSmeAttribute(XmlAttribute attribute)
        {
            switch (attribute.LocalName)
            {
                case "invocationMode":
                    _invocationMode = EnumEx.Parse<InvocationMode>(attribute.Value, true);
                    return true;
                default:
                    return base.ReadSmeAttribute(attribute);
            }
        }

        protected override void WriteSmeAttributes(ICollection<XmlAttribute> attributes)
        {
            WriteSmeAttribute("role", "serviceOperation", attributes);
            WriteSmeAttribute("invocationMode", _invocationMode, attributes);
            base.WriteSmeAttributes(attributes);
        }
    }
}