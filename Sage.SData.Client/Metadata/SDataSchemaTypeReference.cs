using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    [DebuggerDisplay("{QualifiedName}")]
    public class SDataSchemaTypeReference : SDataSchemaObject
    {
        private XmlQualifiedName _name;
        private XmlTypeCode? _code;
        private SDataSchemaType _schemaType;

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
            set
            {
                if (value != _name)
                {
                    XmlTypeCode code;

                    if (value.Namespace == XmlSchema.Namespace && TryParseCode(value.Name, out code))
                    {
                        Code = code;
                    }
                    else
                    {
                        _name = value;
                        _code = null;
                        _schemaType = null;
                    }
                }
            }
        }

        public XmlTypeCode? Code
        {
            get { return _code; }
            set
            {
                if (_code != value)
                {
                    _name = null;
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
                    _name = null;
                    _code = null;
                    _schemaType = value;
                }
            }
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

        public static implicit operator SDataSchemaTypeReference(XmlQualifiedName name)
        {
            return new SDataSchemaTypeReference {QualifiedName = name};
        }

        public static implicit operator SDataSchemaTypeReference(XmlTypeCode code)
        {
            return new SDataSchemaTypeReference {Code = code};
        }

        public static implicit operator SDataSchemaTypeReference(SDataSchemaType type)
        {
            return new SDataSchemaTypeReference {SchemaType = type};
        }
    }
}