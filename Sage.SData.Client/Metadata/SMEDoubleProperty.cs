using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a double property.
    /// </summary>
    public class SMEDoubleProperty : SMEProperty
    {
        #region Constants

        /// <summary>
        /// Default average length for a <see cref="double"/>.
        /// </summary>
        /// <value>The default average length for a <see cref="double"/>, which is <b>14</b>.</value>
        public const int DefaultAverageLength = 14;

        public const double DefaultMinimum = 0;
        public const double DefaultMaximum = 0;
        public const int DefaultDecimalPlaces = 4;
        public const int DefaultLength = 11;

        #endregion

        #region Fields

        private int _iDecimalPlaces;
        private int _iLength;
        private double _dMin;
        private double _dMax;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEDoubleProperty"/> class
        /// with the specified length, decimal places and lab el.
        /// </summary>
        /// <param name="length">Maximum length of the data.</param>
        /// <param name="decimalPlaces">Maximum number of decimal places.</param>
        /// <param name="label">The label for the property.</param>
        public SMEDoubleProperty(int length, int decimalPlaces, string label) :
            base(label)
        {
            _iLength = length;
            _iDecimalPlaces = decimalPlaces;
            DataType = XSDataTypes.Double;
            _dMin = DefaultMinimum;
            _dMax = DefaultMaximum;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEDoubleProperty"/> class
        /// with the specified length and decimal places.
        /// </summary>
        /// <param name="length">Maximum length of the data.</param>
        /// <param name="decimalPlaces">Maximum number of decimal places.</param>
        public SMEDoubleProperty(int length, int decimalPlaces) :
            this(length, decimalPlaces, String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEDoubleProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEDoubleProperty(string label) :
            this(DefaultLength, DefaultDecimalPlaces, label)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEDoubleProperty"/> class.
        /// </summary>
        public SMEDoubleProperty() :
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
        /// A <see cref="double"/> containing the minimum value;
        public double Minimum
        {
            get { return _dMin; }
            set { _dMin = value; }
        }

        /// <summary>
        /// Gets or sets the maximum value;
        /// </summary>
        /// A <see cref="double"/> containing the maximum value;
        public double Maximum
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
                double.TryParse(minFacet.Value, out _dMin);
            }
            else
            {
                var maxFacet = facet as XmlSchemaMaxInclusiveFacet;

                if (maxFacet != null)
                {
                    double.TryParse(maxFacet.Value, out _dMax);
                }
            }
        }

        /// <summary>
        /// Returns a value indicating if the property schema has facets.
        /// </summary>
        /// <value><b>true</b> if the property schema has facets; otherwise, <b>false</b>.</value>
        protected override bool HasFacets
        {
            get { return base.HasFacets || Minimum != DefaultMinimum || Maximum != DefaultMaximum; }
        }

        /// <summary>
        /// Adds the schema facets for this property.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to add the facets to.</param>
        protected override void OnGetSchemaFacets(StringBuilder builder)
        {
            base.OnGetSchemaFacets(builder);

            if (Minimum != DefaultMinimum)
            {
                // <xs:minInclusive value="x" />
                builder.AppendFormat("<{0} {1}=\"{2}\"/>",
                                     TypeInfoHelper.FormatXS(SDataResource.XmlConstants.MinInclusive),
                                     SDataResource.XmlConstants.Value,
                                     Minimum
                    );
            }

            if (Maximum != DefaultMaximum)
            {
                // <xs:maxInclusive value="x" />
                builder.AppendFormat("<{0} {1}=\"{2}\"/>",
                                     TypeInfoHelper.FormatXS(SDataResource.XmlConstants.MaxInclusive),
                                     SDataResource.XmlConstants.Value,
                                     Maximum
                    );
            }
        }

        protected override IDictionary<string, string> OnGetSchemaAttributes()
        {
            var attributes = base.OnGetSchemaAttributes();

            if (DecimalPlaces != DefaultDecimalPlaces)
            {
                attributes.Add(
                    SDataResource.FormatSME(SDataResource.XmlConstants.FractionDigits),
                    DecimalPlaces.ToString()
                    );
            }

            if (Length != DefaultLength)
            {
                attributes.Add(
                    SDataResource.FormatSME(SDataResource.XmlConstants.TotalDigits),
                    Length.ToString()
                    );
            }

            return attributes;
        }

        /// <summary>
        /// Validates the specified value against the constraints of the property.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        protected override void OnValidate(object value)
        {
            base.OnValidate(value, typeof (double));

            if (value != null)
            {
                var typedValue = (double) value;

                if (typedValue < Minimum)
                    ThrowValidationFailed(SDataResource.XmlConstants.MinInclusive);

                if (Maximum != DefaultMaximum && typedValue > Maximum)
                    ThrowValidationFailed(SDataResource.XmlConstants.MaxInclusive);
            }
        }

        #endregion
    }
}