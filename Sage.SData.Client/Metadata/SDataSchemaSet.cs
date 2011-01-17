using System;
using System.Collections.Generic;
using System.Linq;
using Sage.SData.Client.Common;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaSet
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
            Compile();
        }

        public ICollection<SDataSchema> Schemas
        {
            get { return _schemas.AsReadOnly(); }
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
                    throw new NotSupportedException();
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
                    throw new NotSupportedException();
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
    }
}