// Copyright (c) Sage (UK) Limited 2008. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use
// this code. Please contact [email@sage.com] if you do not have such a licence.
// Sage will take appropriate legal action against those who make unauthorised use of this
// code.
using System;
using System.Collections.Generic;
using System.Xml;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a property which is a relationship to another type in the object
    /// model, and allows properties of the relationship to be specified.
    /// </summary>
    public class SMERelationshipProperty : SMEObjectProperty
    {
        #region Constants

        internal const string IncludeByDefaultName = "includeByDefault";
        internal const string RelationshipName = "relationship";
        internal const string CanGetName = "canGet";
        internal const string CanPostName = "canPost";
        internal const string CanPutName = "canPut";
        internal const string CanDeleteName = "canDelete";
        internal const string CanPagePreviousName = "canPagePrevious";
        internal const string CanPageNextName = "canPageNext";
        internal const string CanPageIndexName = "canPageIndex";
        internal const string IsCollectionName = "isCollection";

        private static readonly Dictionary<string, bool> _oRelationshipAttributes;

        static SMERelationshipProperty()
        {
            _oRelationshipAttributes = new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);
            _oRelationshipAttributes[IncludeByDefaultName] = true;
            _oRelationshipAttributes[RelationshipName] = true;
            _oRelationshipAttributes[CanGetName] = true;
            _oRelationshipAttributes[CanPostName] = true;
            _oRelationshipAttributes[CanPutName] = true;
            _oRelationshipAttributes[CanDeleteName] = true;
            _oRelationshipAttributes[CanPagePreviousName] = true;
            _oRelationshipAttributes[CanPageNextName] = true;
            _oRelationshipAttributes[CanPageIndexName] = true;
            _oRelationshipAttributes[IsCollectionName] = true;
        }

        internal static bool IsRelationshipAttribute(string name)
        {
            return _oRelationshipAttributes.ContainsKey(name);
        }

        #endregion

        #region Fields

        private RelationshipType _eRelationship;
        private bool _bIncludeByDefault;
        private bool _bCanGet = true;
        private bool _bCanPost;
        private bool _bCanPut;
        private bool _bCanDelete;
        private bool _bCanPagePrevious;
        private bool _bCanPageNext;
        private bool _bCanPageIndex;
        private bool _bIsCollection;
        private int _iMinOccurs;
        private int _iMaxOccurs;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEGuidProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        /// <param name="relationship">Indicates the relationship of the other type
        /// to this type.</param>
        /// <param name="includeByDefault">Is the object included by default in the 
        /// feed?</param>
        public SMERelationshipProperty(string label, RelationshipType relationship, bool includeByDefault)
            :
                base(label)
        {
            _eRelationship = relationship;
            _bIncludeByDefault = includeByDefault;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEGuidProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        /// <param name="relationship">Indicates the relationship of the other type
        /// to this type.</param>
        /// <param name="includeByDefault">Is the object included by default in the 
        /// feed?</param>
        public SMERelationshipProperty(string label, RelationshipType relationship)
            :
                this(label, relationship, false)
        {
        }

        public SMERelationshipProperty(SMERelationshipProperty copy)
        {
            _eRelationship = copy.Relationship;
            _bIncludeByDefault = copy.IncludeByDefault;
            _bCanGet = copy.CanGet;
            _bCanPost = copy.CanPost;
            _bCanPut = copy.CanPut;
            _bCanDelete = copy.CanDelete;
            _bCanPagePrevious = copy.CanPagePrevious;
            _bCanPageNext = copy.CanPageNext;
            _bCanPageIndex = copy.CanPageIndex;
            _bIsCollection = copy.IsCollection;
        }

        public SMERelationshipProperty()
        {
        }

        public RelationshipType Relationship
        {
            get { return _eRelationship; }
            set { _eRelationship = value; }
        }

        public bool IncludeByDefault
        {
            get { return _bIncludeByDefault; }
            set { _bIncludeByDefault = value; }
        }

        public bool CanGet
        {
            get { return _bCanGet; }
            set { _bCanGet = value; }
        }

        public bool CanPost
        {
            get { return _bCanPost; }
            set { _bCanPost = value; }
        }

        public bool CanPut
        {
            get { return _bCanPut; }
            set { _bCanPut = value; }
        }

        public bool CanDelete
        {
            get { return _bCanDelete; }
            set { _bCanDelete = value; }
        }

        public bool CanPagePrevious
        {
            get { return _bCanPagePrevious; }
            set { _bCanPagePrevious = value; }
        }

        public bool CanPageNext
        {
            get { return _bCanPageNext; }
            set { _bCanPageNext = value; }
        }

        public bool CanPageIndex
        {
            get { return _bCanPageIndex; }
            set { _bCanPageIndex = value; }
        }

        public bool IsCollection
        {
            get { return _bIsCollection; }
            set { _bIsCollection = value; }
        }


        public int MinOccurs
        {
            get { return _iMinOccurs; }
            set { _iMinOccurs = value; }
        }

        public int MaxOccurs
        {
            get { return _iMaxOccurs; }
            set { _iMaxOccurs = value; }
        }

        protected override void OnLoadUnhandledAttribute(XmlAttribute attribute)
        {
            if (attribute.NamespaceURI == Framework.Common.SME.Namespace)
            {
                switch (attribute.LocalName)
                {
                    case IncludeByDefaultName:
                        bool.TryParse(attribute.Value, out _bIncludeByDefault);
                        break;

                    case RelationshipName:
                        _eRelationship = GetRelationshipType(attribute.Value);
                        break;

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

                    case CanPagePreviousName:
                        bool.TryParse(attribute.Value, out _bCanPagePrevious);
                        break;

                    case CanPageNextName:
                        bool.TryParse(attribute.Value, out _bCanPageNext);
                        break;

                    case CanPageIndexName:
                        bool.TryParse(attribute.Value, out _bCanPageIndex);
                        break;

                    case IsCollectionName:
                        bool.TryParse(attribute.Value, out _bIsCollection);
                        break;
                }
            }
            else if (attribute.NamespaceURI == Framework.Common.XS.Namespace)
            {
                switch (attribute.LocalName)
                {
                    case SDataResource.XmlConstants.MinOccurs:
                        int.TryParse(attribute.Value, out _iMinOccurs);
                        break;

                    case SDataResource.XmlConstants.MaxOccurs:
                        if (attribute.Value.ToLower() == SDataResource.XmlConstants.Unbounded)
                            _iMaxOccurs = -1;
                        else
                            int.TryParse(attribute.Value, out _iMaxOccurs);
                        break;
                }
            }

            base.OnLoadUnhandledAttribute(attribute);
        }

        private RelationshipType GetRelationshipType(string attributeValue)
        {
            foreach (RelationshipType type in Enum.GetValues(typeof (RelationshipType)))
            {
                var enumString = type.ToString();

                if (String.Equals(enumString, attributeValue, StringComparison.InvariantCultureIgnoreCase))
                    return type;
            }

            return RelationshipType.Association;
        }

        protected override int GetDefaultAverageLength()
        {
            return 1;
        }
    }

    /// <summary>
    /// Lists the type of relationships which can exist between types in the
    /// object model.
    /// </summary>
    public enum RelationshipType
    {
        Association = 0x01,
        Parent = 0x02,
        Child = 0x03,
        Reference = 0x04
    }
}