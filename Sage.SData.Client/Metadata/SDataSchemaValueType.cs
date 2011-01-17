using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public abstract class SDataSchemaValueType : SDataSchemaType
    {
        private SDataSchemaTypeReference _baseType;

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
            var restriction = simpleType.Content as XmlSchemaSimpleTypeRestriction;

            if (restriction == null)
            {
                throw new NotSupportedException();
            }

            BaseType = new SDataSchemaTypeReference(restriction.BaseTypeName);
            base.Read(obj);
        }

        protected internal override void Write(XmlSchemaObject obj)
        {
            var simpleType = (XmlSchemaSimpleType) obj;
            var restriction = (XmlSchemaSimpleTypeRestriction) simpleType.Content;
            restriction.BaseTypeName = BaseType.QualifiedName;
            base.Write(obj);
        }
    }
}