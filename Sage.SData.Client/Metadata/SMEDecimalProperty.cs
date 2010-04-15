using System;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a Decimalal property.
    /// </summary>
    public class SMEDecimalProperty : SMEProperty
    {
        #region Constants

        /// <summary>
        /// Default average length for a <see cref="float"/>.
        /// </summary>
        /// <value>The default average length for a <see cref="float"/>, which is <b>14</b>.</value>
        public const int DefaultAverageLength = 14;

        public const decimal DefaultMinimum = 0;
        public const decimal DefaultMaximum = 0;
        public const int DefaultDecimalPlaces = 4;
        public const int DefaultLength = 11;

        #endregion

        #region Fields

        private int _iDecimalPlaces;
        private int _iLength;
        private decimal _dMin;
        private decimal _dMax;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEDecimalProperty"/> class
        /// with the specified length, decimal places and lab el.
        /// </summary>
        /// <param name="length">Maximum length of the data.</param>
        /// <param name="decimalPlaces">Maximum number of decimal places.</param>
        /// <param name="label">The label for the property.</param>
        public SMEDecimalProperty(int length, int decimalPlaces, string label) :
            base(label)
        {
            _iLength = length;
            _iDecimalPlaces = decimalPlaces;
            DataType = XSDataTypes.Decimal;
            _dMin = DefaultMinimum;
            _dMax = DefaultMaximum;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEDecimalProperty"/> class
        /// with the specified length and decimal places.
        /// </summary>
        /// <param name="length">Maximum length of the data.</param>
        /// <param name="decimalPlaces">Maximum number of decimal places.</param>
        public SMEDecimalProperty(int length, int decimalPlaces) :
            this(length, decimalPlaces, String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEDecimalProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEDecimalProperty(string label) :
            this(DefaultLength, DefaultDecimalPlaces, label)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEDecimalProperty"/> class.
        /// </summary>
        public SMEDecimalProperty() :
            this(null)
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
        /// Gets or sets the maximum number of decimal places.
        /// </summary>
        /// <value>An integer containing the maximum number of decimal places.</value>
        public int DecimalPlaces
        {
            get { return _iDecimalPlaces; }
            set { _iDecimalPlaces = value; }
        }

        /// <summary>
        /// Gets or sets the minimum value;
        /// </summary>
        /// A <see cref="decimal"/> containing the minimum value;
        public decimal Minimum
        {
            get { return _dMin; }
            set { _dMin = value; }
        }

        /// <summary>
        /// Gets or sets the maximum value;
        /// </summary>
        /// A <see cref="decimal"/> containing the maximum value;
        public decimal Maximum
        {
            get { return _dMax; }
            set { _dMax = value; }
        }

        #region Overrides

        /// <summary>
        /// Returns the default average length (in characters) to use for this property.
        /// </summary>
        /// <returns>The default average length (in characters) to use for this property.</returns>
        protected override int GetDefaultAverageLength()
        {
            if (_iLength <= 0)
                return DefaultAverageLength;

            return _iLength + _iDecimalPlaces + 1;
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
                decimal.TryParse(minFacet.Value, out _dMin);
            }
            else
            {
                var maxFacet = facet as XmlSchemaMaxInclusiveFacet;

                if (maxFacet != null)
                {
                    decimal.TryParse(maxFacet.Value, out _dMax);
                }
                else
                {
                    var totalDigitsFacet = facet as XmlSchemaTotalDigitsFacet;

                    if (totalDigitsFacet != null)
                    {
                        int.TryParse(totalDigitsFacet.Value, out _iLength);
                    }
                    else
                    {
                        var factionFacet = facet as XmlSchemaFractionDigitsFacet;

                        if (factionFacet != null)
                        {
                            int.TryParse(factionFacet.Value, out _iDecimalPlaces);
                        }
                    }
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

                    case SDataResource.XmlConstants.FractionDigits:
                        int.TryParse(attribute.Value, out _iDecimalPlaces);
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
            base.OnValidate(value, typeof (decimal));

            if (value != null)
            {
                var typedValue = (decimal) value;

                if (typedValue < Minimum)
                    ThrowValidationFailed(SDataResource.XmlConstants.MinInclusive);

                if (Maximum != DefaultMaximum && typedValue > Maximum)
                    ThrowValidationFailed(SDataResource.XmlConstants.MaxInclusive);
            }
        }

        #endregion
    }
}