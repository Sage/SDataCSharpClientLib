using System.Collections.Generic;
using System.Xml;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaRelationshipProperty : SDataSchemaProperty
    {
        private bool? _isCollection;

        /// <summary>
        /// Type of relationship.
        /// </summary>
        public RelationshipType? Relationship { get; set; }

        /// <summary>
        /// Does the property refer to a collection of resources or a single resource?
        /// </summary>
        public bool IsCollection
        {
            get { return _isCollection ?? false; }
            set { _isCollection = value; }
        }

        /// <summary>
        /// Does the relationship property support GET (read and query) operations?
        /// </summary>
        public bool CanGet { get; set; }

        /// <summary>
        /// Does the relationship property support POST operations?
        /// </summary>
        public bool CanPost { get; set; }

        /// <summary>
        /// Does the relationship property support PUT operations?
        /// </summary>
        public bool CanPut { get; set; }

        /// <summary>
        /// Does the relationship property support DELETE operations?
        /// </summary>
        public bool CanDelete { get; set; }

        /// <summary>
        /// Does the relationship property support paging backwards?
        /// </summary>
        public bool CanPagePrevious { get; set; }

        /// <summary>
        /// Does the relationship property support paging forwards?
        /// </summary>
        public bool CanPageNext { get; set; }

        /// <summary>
        /// Does the relationship property support seeking?
        /// </summary>
        public bool CanPageIndex { get; set; }

        protected override bool ReadSmeAttribute(XmlAttribute attribute)
        {
            switch (attribute.LocalName)
            {
                case "relationship":
                    Relationship = EnumEx.Parse<RelationshipType>(attribute.Value, true);
                    return true;
                case "isCollection":
                    _isCollection = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "canGet":
                    CanGet = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "canPost":
                    CanPost = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "canPut":
                    CanPut = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "canDelete":
                    CanDelete = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "canPagePrevious":
                    CanPagePrevious = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "canPageNext":
                    CanPageNext = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "canPageIndex":
                    CanPageIndex = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                default:
                    return base.ReadSmeAttribute(attribute);
            }
        }

        protected override void WriteSmeAttributes(ICollection<XmlAttribute> attributes)
        {
            WriteSmeAttribute("relationship", Relationship, attributes);
            WriteSmeAttribute("isCollection", _isCollection, attributes);
            WriteSmeAttribute("canGet", CanGet, attributes);
            WriteSmeAttribute("canPost", CanPost, attributes);
            WriteSmeAttribute("canPut", CanPut, attributes);
            WriteSmeAttribute("canDelete", CanDelete, attributes);
            WriteSmeAttribute("canPagePrevious", CanPagePrevious, attributes);
            WriteSmeAttribute("canPageNext", CanPageNext, attributes);
            WriteSmeAttribute("canPageIndex", CanPageIndex, attributes);
            base.WriteSmeAttributes(attributes);
        }

        protected override XmlQualifiedName GetTypeName()
        {
            if (IsCollection)
            {
                var complexType = Type.SchemaType as SDataSchemaComplexType;

                if (complexType != null && complexType.ListQualifiedName != null)
                {
                    return complexType.ListQualifiedName;
                }
            }

            return base.GetTypeName();
        }
    }
}