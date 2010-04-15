using System;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a <see cref="ulong"/> property.
    /// </summary>
    public class SMEUnsignedLongProperty : SMEProperty
    {
        #region Constants

        /// <summary>
        /// Default average length for a <see cref="ulong"/>.
        /// </summary>
        /// <value>The default average length for a <see cref="ulong"/>, which is <b>12</b>.</value>
        public const int DefaultAverageLength = 12;

        public const uint DefaultMinimum = 0;
        public const uint DefaultMaximum = 0;
        public const int DefaultLength = 0;

        #endregion

        #region Fields

        private int _iLength;
        private ulong _ulMin;
        private ulong _ulMax;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEUnsignedLongProperty"/> class
        /// with the specified length and label.
        /// </summary>
        /// <param name="length">Maximum length of the data.</param>
        /// <param name="label">The label for the property.</param>
        public SMEUnsignedLongProperty(int length, string label) :
            base(label)
        {
            _iLength = length;
            DataType = XSDataTypes.UnsignedLong;
            _ulMin = DefaultMinimum;
            _ulMax = DefaultMaximum;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEUnsignedLongProperty"/> class
        /// with the specified length.
        /// </summary>
        /// <param name="length">Maximum length of the data.</param>
        public SMEUnsignedLongProperty(int length) :
            this(length, String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEUnsignedLongProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEUnsignedLongProperty(string label) :
            this(DefaultLength, label)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEUnsignedLongProperty"/> class.
        /// </summary>
        public SMEUnsignedLongProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Gets or sets the maximum length of data.
        /// </summary>
        /// <value>An integer containing the maximum length of data.</value>
        public int Length
        {
            get { return _iLength; }
            set { _iLength = value; }
        }

        /// <summary>
        /// Gets or sets the minimum value;
        /// </summary>
        /// A <see cref="ulong"/> containing the minimum value;
        public ulong Minimum
        {
            get { return _ulMin; }
            set { _ulMin = value; }
        }

        /// <summary>
        /// Gets or sets the maximum value;
        /// </summary>
        /// A <see cref="ulong"/> containing the maximum value;
        public ulong Maximum
        {
            get { return _ulMax; }
            set { _ulMax = value; }
        }

        #region Overrides

        /// <summary>
        /// Returns the default average length (in characters) to use for this property.
        /// </summary>
        /// <returns>The default average length (in characters) to use for this property.</returns>
        protected override int GetDefaultAverageLength()
        {
            return _iLength <= 0 ? DefaultAverageLength : _iLength;
        }

        /// <summary>
        /// Loads a facet for this property.
        /// </summary>
        /// <param name="facet">The facet to load for this property.</param>
        protected override void OnLoadFacet(XmlSchemaFacet facet)
        {
            base.OnLoadFacet(facet);

            var minFacet = facet as XmlSchemaMinInclusiveFacet;

            if (minFacet != null)
            {
                ulong.TryParse(minFacet.Value, out _ulMin);
            }
            else
            {
                var maxFacet = facet as XmlSchemaMaxInclusiveFacet;

                if (maxFacet != null)
                {
                    ulong.TryParse(maxFacet.Value, out _ulMax);
                }
                else
                {
                    var totalDigitsFacet = facet as XmlSchemaTotalDigitsFacet;

                    if (totalDigitsFacet != null)
                        int.TryParse(totalDigitsFacet.Value, out _iLength);
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
                    case SDataResource.XmlConstants.TotalDigits:
                    case SDataResource.XmlConstants.Length:
                        int.TryParse(attribute.Value, out _iLength);
                        break;
                }
            }
        }

        /// <summary>
        /// Validates the specified value against the constraints of the property.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        protected override void OnValidate(object value)
        {
            base.OnValidate(value, typeof (ulong));

            if (value != null)
            {
                var typedValue = (ulong) value;

                if (typedValue < Minimum)
                    ThrowValidationFailed(SDataResource.XmlConstants.MinInclusive);

                if (Maximum != DefaultMaximum && typedValue > Maximum)
                    ThrowValidationFailed(SDataResource.XmlConstants.MaxInclusive);
            }
        }

        #endregion
    }
}