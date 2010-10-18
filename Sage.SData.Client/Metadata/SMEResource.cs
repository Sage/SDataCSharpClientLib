// Copyright (c) Sage (UK) Limited 2008. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use
// this code. Please contact [email@sage.com] if you do not have such a licence.
// Sage will take appropriate legal action against those who make unauthorised use of this
// code.
using System;
using System.Text;
using System.Xml;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines an SData resource and its associated properties.
    /// </summary>
    public class SMEResource
    {
        #region Constants

        internal const string CanGetName = "canGet";
        internal const string CanPostName = "canPost";
        internal const string CanPutName = "canPut";
        internal const string CanDeleteName = "canDelete";
        internal const string CanSearchName = "canSearch";
        internal const string PathName = "path";
        internal const string PluralNameName = "pluralName";
        internal const string LabelName = "label";
        internal const string CanPagePreviousName = "canPagePrevious";
        internal const string CanPageNextName = "canPageNext";
        internal const string CanPageIndexName = "canPageIndex";
        internal const string SupportsETagName = "supportsETag";
        internal const string HasUuidName = "hasUuid";
        internal const string HasTemplateName = "hasTemplate";
        internal const string BatchingModeName = "batchingMode";
        internal const string RoleName = "role";
        internal const string IsSyncSourceName = "isSyncSource";
        internal const string IsSyncTargetName = "isSyncTarget";

        #endregion

        #region Fields

        private bool _bCanGet;
        private bool _bCanPost;
        private bool _bCanPut;
        private bool _bCanDelete;
        private bool _bCanSearch;
        private string _strPath;
        private string _strPluralName;
        private string _strLabel;
        private bool _bCanPagePrevious;
        private bool _bCanPageNext;
        private bool _bCanPageIndex;
        private bool _bSupportsETag;
        private bool _bHasUuid;
        private bool _bHasTemplate;
        private SyncModesType _eBatchingMode;
        private RoleType _eRole;
        private bool _bIsSyncSource;
        private bool _bIsSyncTarget;

        #endregion

        public SMEResource()
        {
        }

        internal SMEResource(SMEResource copy)
        {
            _bCanGet = copy.CanGet;
            _bCanPost = copy.CanPost;
            _bCanPut = copy.CanPut;
            _bCanDelete = copy.CanDelete;
            _bCanSearch = copy.CanSearch;
            _strPath = copy.Path;
            _strPluralName = copy.PluralName;
            _strLabel = copy.Label;
            _bCanPagePrevious = copy.CanPagePrevious;
            _bCanPageNext = copy.CanPageNext;
            _bCanPageIndex = copy.CanPageIndex;
            _bSupportsETag = copy.SupportsETag;
            _bHasUuid = copy.HasUuid;
            _bHasTemplate = copy.HasTemplate;
            _eBatchingMode = copy.BatchingMode;
            _eRole = copy.Role;
            _bIsSyncSource = copy.IsSyncSource;
            _bIsSyncTarget = copy.IsSyncTarget;
        }

        /// <summary>
        /// Indicates if the relationship property supports GET operations.
        /// </summary>
        public bool CanGet
        {
            get { return _bCanGet; }
            set { _bCanGet = value; }
        }

        /// <summary>
        /// Indicates if the relationship property supports POST operations.
        /// </summary>
        public bool CanPost
        {
            get { return _bCanPost; }
            set { _bCanPost = value; }
        }

        /// <summary>
        /// Indicates if the relationship property supports PUT operations.
        /// </summary>
        public bool CanPut
        {
            get { return _bCanPut; }
            set { _bCanPut = value; }
        }

        /// <summary>
        /// Indicates if the relationship property supports DELETE operations.
        /// </summary>
        public bool CanDelete
        {
            get { return _bCanDelete; }
            set { _bCanDelete = value; }
        }

        /// <summary>
        /// Indicates if the resource kind supports full-text search 
        /// through the search query parameter
        /// </summary>
        public bool CanSearch
        {
            get { return _bCanSearch; }
            set { _bCanSearch = value; }
        }

        /// <summary>
        /// Relative URL to query resources
        /// </summary>
        public string Path
        {
            get { return _strPath; }
            set { _strPath = value; }
        }

        /// <summary>
        /// Name of the resource in plural form.
        /// </summary>
        public string PluralName
        {
            get { return _strPluralName; }
            set { _strPluralName = value; }
        }

        /// <summary>
        /// Localized friendly name for the resource
        /// </summary>
        public string Label
        {
            get { return _strLabel; }
            set { _strLabel = value; }
        }

        /// <summary>
        /// Indicates if the specified paging mode is supported by the relationship.
        /// </summary>
        public bool CanPagePrevious
        {
            get { return _bCanPagePrevious; }
            set { _bCanPagePrevious = value; }
        }

        /// <summary>
        /// Indicates if the specified paging mode is supported by the relationship.
        /// </summary>
        public bool CanPageNext
        {
            get { return _bCanPageNext; }
            set { _bCanPageNext = value; }
        }

        /// <summary>
        /// Indicates if the specified paging mode is supported by the relationship.
        /// </summary>
        public bool CanPageIndex
        {
            get { return _bCanPageIndex; }
            set { _bCanPageIndex = value; }
        }

        /// <summary>
        /// Indicates if the resource supports the ETag mechanism to control 
        /// concurrent updates.
        /// </summary>
        public bool SupportsETag
        {
            get { return _bSupportsETag; }
            set { _bSupportsETag = value; }
        }

        /// <summary>
        /// Indicates if the resource supports UUIDs to globally identify instances.
        /// </summary>
        public bool HasUuid
        {
            get { return _bHasUuid; }
            set { _bHasUuid = value; }
        }

        /// <summary>
        /// Indicates if the resource provides a template.
        /// </summary>
        public bool HasTemplate
        {
            get { return _bHasTemplate; }
            set { _bHasTemplate = value; }
        }

        /// <summary>
        /// Indicates if the resource kind supports batching, 
        /// and if so, describes the invocation modes supported.
        /// </summary>
        public SyncModesType BatchingMode
        {
            get { return _eBatchingMode; }
            set { _eBatchingMode = value; }
        }

        /// <summary>
        /// Indicates the type of resource.
        /// </summary>
        public RoleType Role
        {
            get { return _eRole; }
            set { _eRole = value; }
        }

        /// <summary>
        /// Indicates that the resource is a sync source.
        /// </summary>
        public bool IsSyncSource
        {
            get { return _bIsSyncSource; }
            set { _bIsSyncSource = value; }
        }

        /// <summary>
        /// Indicates that the resource is a sync target.
        /// </summary>
        public bool IsSyncTarget
        {
            get { return _bIsSyncTarget; }
            set { _bIsSyncTarget = value; }
        }

        /// <summary>
        /// Loads an unhandled attribute.
        /// </summary>
        /// <param name="attribute">The unhandled attribute.</param>
        public void LoadUnhandledAttribute(XmlAttribute attribute)
        {
            OnLoadUnhandledAttribute(attribute);
        }

        protected virtual void OnLoadUnhandledAttribute(XmlAttribute attribute)
        {
            if (attribute.NamespaceURI == Framework.Common.SME.Namespace)
            {
                switch (attribute.LocalName)
                {
                    case CanGetName:
                        bool.TryParse(attribute.Value, out _bCanGet);
                        break;

                    case CanPostName:
                        bool.TryParse(attribute.Value, out _bCanPost);
                        break;

                    case CanPutName:
                        bool.TryParse(attribute.Value, out _bCanPut);
                        break;

                    case CanDeleteName:
                        bool.TryParse(attribute.Value, out _bCanDelete);
                        break;

                    case CanSearchName:
                        bool.TryParse(attribute.Value, out _bCanSearch);
                        break;

                    case PathName:
                        _strPath = attribute.Value;
                        break;

                    case PluralNameName:
                        _strPluralName = attribute.Value;
                        break;

                    case LabelName:
                        _strLabel = attribute.Value;
                        break;

                    case CanPagePreviousName:
                        bool.TryParse(attribute.Value, out _bCanPagePrevious);
                        break;

                    case CanPageNextName:
                        bool.TryParse(attribute.Value, out _bCanPageNext);
                        break;

                    case CanPageIndexName:
                        bool.TryParse(attribute.Value, out _bCanPageIndex);
                        break;

                    case SupportsETagName:
                        bool.TryParse(attribute.Value, out _bSupportsETag);
                        break;

                    case HasUuidName:
                        bool.TryParse(attribute.Value, out _bHasUuid);
                        break;

                    case HasTemplateName:
                        bool.TryParse(attribute.Value, out _bHasTemplate);
                        break;

                    case BatchingModeName:
                        _eBatchingMode = GetBatchingMode(attribute.Value);
                        break;

                    case RoleName:
                        _eRole = GetRole(attribute.Value);
                        break;

                    case IsSyncSourceName:
                        bool.TryParse(attribute.Value, out _bIsSyncSource);
                        break;

                    case IsSyncTargetName:
                        bool.TryParse(attribute.Value, out _bIsSyncTarget);
                        break;
                }
            }
        }

        private static SyncModesType GetBatchingMode(string attributeString)
        {
            foreach (SyncModesType type in Enum.GetValues(typeof (SyncModesType)))
            {
                var typeString = type.ToString();

                if (String.Equals(attributeString, typeString, StringComparison.InvariantCultureIgnoreCase))
                    return type;
            }

            return SyncModesType.None;
        }

        private static RoleType GetRole(string attributeString)
        {
            foreach (RoleType type in Enum.GetValues(typeof (RoleType)))
            {
                var typeString = type.ToString();

                if (String.Equals(attributeString, typeString, StringComparison.InvariantCultureIgnoreCase))
                    return type;
            }

            return RoleType.ResourceKind;
        }

        internal static string GetSchemaAttributes(SMEResource resource)
        {
            if (resource == null)
                return String.Empty;

            var builder = new StringBuilder();

            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(CanGetName), resource.CanGet ? "true" : "false");
            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(CanPostName), resource.CanPost ? "true" : "false");
            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(CanPutName), resource.CanPut ? "true" : "false");
            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(CanDeleteName), resource.CanDelete ? "true" : "false");
            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(CanSearchName), resource.CanSearch ? "true" : "false");

            if (!String.IsNullOrEmpty(resource.Path))
                builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(PathName), resource.Path);

            if (!String.IsNullOrEmpty(resource.PluralName))
                builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(PluralNameName), resource.PluralName);

            if (!String.IsNullOrEmpty(resource.Label))
                builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(LabelName), TypeInfoHelper.Escape(resource.Label));

            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(CanPagePreviousName), resource.CanPagePrevious ? "true" : "false");
            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(CanPageNextName), resource.CanPageNext ? "true" : "false");
            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(CanPageIndexName), resource.CanPageIndex ? "true" : "false");
            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(SupportsETagName), resource.SupportsETag ? "true" : "false");
            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(HasUuidName), resource.HasUuid ? "true" : "false");
            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(HasTemplateName), resource.HasTemplate ? "true" : "false");
            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(BatchingModeName), resource.BatchingMode);
            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(RoleName), FormatRole(resource.Role));
            builder.AppendFormat("{0}=\"{1}\" ", SDataResource.FormatSME(IsSyncSourceName), resource.IsSyncSource ? "true" : "false");
            builder.AppendFormat("{0}=\"{1}\"", SDataResource.FormatSME(IsSyncTargetName), resource.IsSyncTarget ? "true" : "false");

            return builder.ToString();
        }

        private static string FormatRole(RoleType role)
        {
            var str = role.ToString();
            return char.ToLower(str[0]) + str.Substring(1);
        }
    }

    /// <summary>
    /// Defines the types of batching which the resouce kind supports.
    /// </summary>
    public enum SyncModesType
    {
        /// <summary>
        /// Batching is not supported.
        /// </summary>
        None,

        /// <summary>
        /// Batching is supported in synchronous mode only.
        /// </summary>
        Sync,

        /// <summary>
        /// Batching is supported in asynchronous mode only.
        /// </summary>
        Async,

        /// <summary>
        /// Batching is supported in either synchronous or asynchronous modes
        /// </summary>
        SyncOrAsync
    }

    /// <summary>
    /// Defines the role for a resource.
    /// </summary>
    public enum RoleType
    {
        /// <summary>
        /// The associated type is a resource.
        /// </summary>
        ResourceKind,

        /// <summary>
        /// The associated type is a service.
        /// </summary>
        ServiceOperation,

        /// <summary>
        /// The associated type is a named query.
        /// </summary>
        Query
    }
}