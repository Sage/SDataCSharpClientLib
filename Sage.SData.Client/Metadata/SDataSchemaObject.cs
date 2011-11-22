using System.Collections.Generic;
using System.Linq;

namespace Sage.SData.Client.Metadata
{
    public abstract class SDataSchemaObject
    {
        protected const string SmeNamespaceUri = "http://schemas.sage.com/sdata/sme/2007";

        public SDataSchemaObject Parent { get; internal set; }

        public virtual IEnumerable<SDataSchemaObject> Children
        {
            get { return Enumerable.Empty<SDataSchemaObject>(); }
        }

        public IEnumerable<SDataSchemaObject> Ancestors()
        {
            var current = Parent;

            while (current != null)
            {
                yield return current;
                current = current.Parent;
            }
        }

        public IEnumerable<SDataSchemaObject> Descendents()
        {
            return Descendents(Children);
        }

        private static IEnumerable<SDataSchemaObject> Descendents(IEnumerable<SDataSchemaObject> items)
        {
            return items.Concat(items.SelectMany(item => Descendents(item.Children)));
        }
    }
}