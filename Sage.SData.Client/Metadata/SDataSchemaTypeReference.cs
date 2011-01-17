using System;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaTypeReference : SDataSchemaObject
    {
        private readonly XmlQualifiedName _name;
        private XmlTypeCode? _code;
        private SDataSchemaType _schemaType;

        public SDataSchemaTypeReference()
        {
        }

        internal SDataSchemaTypeReference(XmlQualifiedName name)
        {
            XmlTypeCode code;

            if (name.Namespace == XmlSchema.Namespace && TryParseCode(name.Name, out code))
            {
                _code = code;
            }
            else
            {
                _name = name;
            }
        }

        public XmlQualifiedName QualifiedName
        {
            get
            {
                return SchemaType != null
                           ? SchemaType.QualifiedName
                           : Code != null
                                 ? new XmlQualifiedName(FormatCode(Code.Value), XmlSchema.Namespace)
                                 : _name;
            }
        }

        public XmlTypeCode? Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    _code = value;
                    _schemaType = null;
                }
            }
        }

        public SDataSchemaType SchemaType
        {
            get { return _schemaType; }
            set
            {
                if (_schemaType != value)
                {
                    _code = null;
                    _schemaType = value;
                }
            }
        }

        protected internal override void Read(XmlSchemaObject obj)
        {
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
        }

        private static bool TryParseCode(string value, out XmlTypeCode code)
        {
            if (value != null)
            {
                try
                {
                    code = (XmlTypeCode) Enum.Parse(typeof (XmlTypeCode), value, true);
                    return true;
                }
                catch (FormatException)
                {
                }
            }

            code = XmlTypeCode.None;
            return false;
        }

        private static string FormatCode(XmlTypeCode code)
        {
            var str = code.ToString();
            return char.ToLower(str[0]) + str.Substring(1);
        }
    }
}