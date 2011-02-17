using System.Collections.Generic;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public abstract class SDataSchemaValueType : SDataSchemaType
    {
        private SDataSchemaTypeReference _baseType;

        protected SDataSchemaValueType()
        {
        }

        protected SDataSchemaValueType(string baseName, string defaultSuffix)
            : base(baseName, defaultSuffix)
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

        public override IEnumerable<SDataSchemaObject> Children
        {
            get { return BaseType != null ? new[] {BaseType} : base.Children; }
        }

        protected internal override void Read(XmlSchemaObject obj)
        {
            var simpleType = (XmlSchemaSimpleType) obj;
            var restriction = (XmlSchemaSimpleTypeRestriction) simpleType.Content;
            BaseType = restriction.BaseTypeName;
            base.Read(obj);
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
            var simpleType = (XmlSchemaSimpleType) obj;
            var restriction = (XmlSchemaSimpleTypeRestriction) simpleType.Content;

            if (BaseType != null)
            {
                restriction.BaseTypeName = BaseType.QualifiedName;
            }

            base.Write(obj);
        }
    }
}