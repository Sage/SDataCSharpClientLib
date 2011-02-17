using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchemaResourceType : SDataSchemaTopLevelType
    {
        public SDataSchemaResourceType()
        {
        }

        public SDataSchemaResourceType(string elementName)
            : base(elementName)
        {
            ListName = elementName + "--list";
            ListItemName = elementName;
            CanGet = true;
            AnyAttribute = new XmlSchemaAnyAttribute();
            ListAnyAttribute = new XmlSchemaAnyAttribute();
        }

        /// <summary>
        /// Name of the resource kind in plural form. For example, accounts.
        /// </summary>
        public string PluralName { get; set; }

        /// <summary>
        /// Does the resource kind support full-text search through the search query parameter?
        /// </summary>
        public bool CanSearch { get; set; }

        /// <summary>
        /// Does the resource kind use UUIDs to identify resources?
        /// When this attribute is set, feeds payload MUST carry sdata:uuid attributes.
        /// </summary>
        public bool HasUuid { get; set; }

        /// <summary>
        /// Does the resource kind support the ETag mechanism to control concurrent updates?
        /// </summary>
        public bool SupportsETag { get; set; }

        /// <summary>
        /// Does the resource kind support batching? If so, which invocation modes does it support?
        /// </summary>
        public BatchingMode BatchingMode { get; set; }

        /// <summary>
        /// Can the resource kind be used as a source in a synchronization operation?
        /// </summary>
        public bool IsSyncSource { get; set; }

        /// <summary>
        /// Can the resource kind be used as a target in a synchronization operation?
        /// </summary>
        public bool IsSyncTarget { get; set; }

        /// <summary>
        /// Priority to resolve conflicts in synchronization.
        /// </summary>
        public int? SyncConflictPriority { get; set; }

        /// <summary>
        /// Order in which the resource kind should be processed in a synchronization pass.
        /// </summary>
        public int? SyncOrder { get; set; }

        /// <summary>
        /// Does the resource kind support GET (read and query) operations?
        /// </summary>
        public bool CanGet { get; set; }

        /// <summary>
        /// Does the resource kind support POST operations?
        /// </summary>
        public bool CanPost { get; set; }

        /// <summary>
        /// Does the resource kind support PUT operations?
        /// </summary>
        public bool CanPut { get; set; }

        /// <summary>
        /// Does the resource kind support DELETE operations?
        /// </summary>
        public bool CanDelete { get; set; }

        /// <summary>
        /// Does the resource kind support paging backwards?
        /// </summary>
        public bool CanPagePrevious { get; set; }

        /// <summary>
        /// Does the resource kind support paging forwards?
        /// </summary>
        public bool CanPageNext { get; set; }

        /// <summary>
        /// Does the resource kind support seeking?
        /// </summary>
        public bool CanPageIndex { get; set; }

        protected override bool ReadSmeAttribute(XmlAttribute attribute)
        {
            switch (attribute.LocalName)
            {
                case "pluralName":
                    PluralName = attribute.Value;
                    return true;
                case "canSearch":
                    CanSearch = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "hasUuid":
                    HasUuid = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "supportsETag":
                    SupportsETag = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "batchingMode":
                    BatchingMode = EnumEx.Parse<BatchingMode>(attribute.Value, true);
                    return true;
                case "isSyncSource":
                    IsSyncSource = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "isSyncTarget":
                    IsSyncTarget = XmlConvert.ToBoolean(attribute.Value);
                    return true;
                case "syncConflictPriority":
                    SyncConflictPriority = XmlConvert.ToInt32(attribute.Value);
                    return true;
                case "syncOrder":
                    SyncOrder = XmlConvert.ToInt32(attribute.Value);
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
            WriteSmeAttribute("role", "resourceKind", attributes);
            WriteSmeAttribute("pluralName", PluralName, attributes);
            WriteSmeAttribute("canSearch", CanSearch, attributes);
            WriteSmeAttribute("hasUuid", HasUuid, attributes);
            WriteSmeAttribute("supportsETag", SupportsETag, attributes);
            WriteSmeAttribute("batchingMode", BatchingMode, attributes);
            WriteSmeAttribute("isSyncSource", IsSyncSource, attributes);
            WriteSmeAttribute("isSyncTarget", IsSyncTarget, attributes);
            WriteSmeAttribute("syncConflictPriority", SyncConflictPriority, attributes);
            WriteSmeAttribute("syncOrder", SyncOrder, attributes);
            WriteSmeAttribute("canGet", CanGet, attributes);
            WriteSmeAttribute("canPost", CanPost, attributes);
            WriteSmeAttribute("canPut", CanPut, attributes);
            WriteSmeAttribute("canDelete", CanDelete, attributes);
            WriteSmeAttribute("canPagePrevious", CanPagePrevious, attributes);
            WriteSmeAttribute("canPageNext", CanPageNext, attributes);
            WriteSmeAttribute("canPageIndex", CanPageIndex, attributes);
            base.WriteSmeAttributes(attributes);
        }
    }
}