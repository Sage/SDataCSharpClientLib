using System;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines an <see cref="Array"/> property.
    /// </summary>
    public abstract class SMEArrayProperty : SMEProperty
    {
        #region Fields

        private int _iMaxLength;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEArrayProperty"/> class.
        /// </summary>
        protected SMEArrayProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEArrayProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        protected SMEArrayProperty(string label) :
            base(label)
        {
        }

        #region Properties

        /// <summary>
        /// Gets or sets the maximum length of data.
        /// </summary>
        /// <value>An integer containing the maximum length of data.</value>
        public int MaximumLength
        {
            get { return _iMaxLength; }
            set { _iMaxLength = value; }
        }

        #endregion

        /// <summary>
        /// Loads a facet for this property.
        /// </summary>
        /// <param name="facet">The facet to load for this property.</param>
        protected override void OnLoadFacet(XmlSchemaFacet facet)
        {
            base.OnLoadFacet(facet);

            var maxLengthFacet = facet as XmlSchemaMaxLengthFacet;

            if (maxLengthFacet != null)
            {
                int.TryParse(maxLengthFacet.Value, out _iMaxLength);
            }
            else
            {
                var lengthFacet = facet as XmlSchemaLengthFacet;

                if (lengthFacet != null)
                {
                    int.TryParse(lengthFacet.Value, out _iMaxLength);
                }
            }
        }

        /// <summary>
        /// Loads an unhandled attribute.
        /// </summary>
        /// <param name="attribute">The unhandled attribute.</param>
        protected override void OnLoadUnhandledAttribute(XmlAttribute attribute)
        {
            base.OnLoadUnhandledAttribute(attribute);

            if (attribute.NamespaceURI == Framework.Common.SME.Namespace)
            {
                switch (attribute.LocalName)
                {
                    case SDataResource.XmlConstants.Length:
                    case SDataResource.XmlConstants.MaxLength:
                        int.TryParse(attribute.Value, out _iMaxLength);
                        break;
                }
            }
        }

        /// <summary>
        /// Returns the default average length (in characters) to use for this property.
        /// </summary>
        /// <returns>The default average length (in characters) to use for this property.</returns>
        protected override int GetDefaultAverageLength()
        {
            return Math.Min(128, MaximumLength/2);
        }
    }
}