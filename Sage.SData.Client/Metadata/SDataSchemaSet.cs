using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaSet : SDataSchemaObject
    {
        private readonly List<SDataSchema> _schemas;

        public SDataSchemaSet(params SDataSchema[] schemas)
            : this((IEnumerable<SDataSchema>) schemas)
        {
        }

        public SDataSchemaSet(IEnumerable<SDataSchema> schemas)
        {
            Guard.ArgumentNotNull(schemas, "schemas");
            _schemas = new List<SDataSchema>(schemas);

            foreach (var schema in schemas)
            {
                schema.Parent = this;
            }

            Compile();
        }

        public override IEnumerable<SDataSchemaObject> Children
        {
            get { return Schemas.Cast<SDataSchemaObject>(); }
        }

        public ICollection<SDataSchema> Schemas
        {
            get { return _schemas.AsReadOnly(); }
        }

        public KeyedEnumerable<XmlQualifiedName, SDataSchemaType> Types
        {
            get { return new KeyedEnumerable<XmlQualifiedName, SDataSchemaType>(_schemas.SelectMany(schema => schema.Types), type => type.QualifiedName); }
        }

        public KeyedEnumerable<XmlQualifiedName, SDataSchemaSimpleType> SimpleTypes
        {
            get { return new KeyedEnumerable<XmlQualifiedName, SDataSchemaSimpleType>(Types.OfType<SDataSchemaSimpleType>(), type => type.QualifiedName); }
        }

        public KeyedEnumerable<XmlQualifiedName, SDataSchemaEnumType> EnumTypes
        {
            get { return new KeyedEnumerable<XmlQualifiedName, SDataSchemaEnumType>(Types.OfType<SDataSchemaEnumType>(), type => type.QualifiedName); }
        }

        public KeyedEnumerable<XmlQualifiedName, SDataSchemaComplexType> ComplexTypes
        {
            get { return new KeyedEnumerable<XmlQualifiedName, SDataSchemaComplexType>(Types.OfType<SDataSchemaComplexType>(), type => type.QualifiedName); }
        }

        public KeyedEnumerable<XmlQualifiedName, SDataSchemaResourceType> ResourceTypes
        {
            get { return new KeyedEnumerable<XmlQualifiedName, SDataSchemaResourceType>(Types.OfType<SDataSchemaResourceType>(), GetTopLevelTypeKey); }
        }

        public KeyedEnumerable<XmlQualifiedName, SDataSchemaServiceOperationType> ServiceOperationTypes
        {
            get { return new KeyedEnumerable<XmlQualifiedName, SDataSchemaServiceOperationType>(Types.OfType<SDataSchemaServiceOperationType>(), GetTopLevelTypeKey); }
        }

        public KeyedEnumerable<XmlQualifiedName, SDataSchemaNamedQueryType> NamedQueryTypes
        {
            get { return new KeyedEnumerable<XmlQualifiedName, SDataSchemaNamedQueryType>(Types.OfType<SDataSchemaNamedQueryType>(), GetTopLevelTypeKey); }
        }

        public SDataSchemaSet Add(params SDataSchema[] schemas)
        {
            return Add((IEnumerable<SDataSchema>) schemas);
        }

        public SDataSchemaSet Add(IEnumerable<SDataSchema> schemas)
        {
            Guard.ArgumentNotNull(schemas, "schemas");

            foreach (var schema in schemas)
            {
                if (schema == null)
                {
                    throw new ArgumentException("Collection contains null entries", "schemas");
                }

                _schemas.Add(schema);
            }

            Compile();
            return this;
        }

        public SDataSchemaSet Remove(params SDataSchema[] schemas)
        {
            return Remove((IEnumerable<SDataSchema>) schemas);
        }

        public SDataSchemaSet Remove(IEnumerable<SDataSchema> schemas)
        {
            Guard.ArgumentNotNull(schemas, "schemas");

            foreach (var schema in schemas)
            {
                if (schema == null)
                {
                    throw new ArgumentException("Collection contains null entries", "schemas");
                }

                _schemas.Remove(schema);
            }

            Compile();
            return this;
        }

        private void Compile()
        {
            var listTypes = _schemas.SelectMany(
                schema => schema.ComplexTypes
                              .Where(type => type.ListName != null)
                              .Select(type => new {name = type.ListQualifiedName, type = (SDataSchemaType) type}));
            var types = _schemas.SelectMany(
                schema => schema.Types.Select(type => new {name = type.QualifiedName, type}))
                .Concat(listTypes)
                .ToDictionary(type => type.name, type => type.type);

            foreach (var schema in _schemas)
            {
                schema.Compile(types);
            }
        }

        private static XmlQualifiedName GetTopLevelTypeKey(SDataSchemaTopLevelType type)
        {
            return new XmlQualifiedName(type.ElementName, type.Schema != null ? type.Schema.TargetNamespace : null);
        }
    }
}