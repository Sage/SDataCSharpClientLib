using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a float property.
    /// </summary>
    public class SMEFloatProperty : SMEProperty
    {
        #region Constants

        /// <summary>
        /// Default average length for a <see cref="float"/>.
        /// </summary>
        /// <value>The default average length for a <see cref="float"/>, which is <b>14</b>.</value>
        public const int DefaultAverageLength = 14;

        public const float DefaultMinimum = 0;
        public const float DefaultMaximum = 0;
        public const int DefaultDecimalPlaces = 4;
        public const int DefaultLength = 11;

        #endregion

        #region Fields

        private int _iDecimalPlaces;
        private int _iLength;
        private float _fMin;
        private float _fMax;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEFloatProperty"/> class
        /// with the specified length, decimal places and label.
        /// </summary>
        /// <param name="length">Maximum length of the data.</param>
        /// <param name="decimalPlaces">Maximum number of decimal places.</param>
        /// <param name="label">The label for the property.</param>
        public SMEFloatProperty(int length, int decimalPlaces, string label) :
            base(label)
        {
            _iLength = length;
            _iDecimalPlaces = decimalPlaces;
            DataType = XSDataTypes.Float;
            _fMin = DefaultMinimum;
            _fMax = DefaultMaximum;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEFloatProperty"/> class
        /// with the specified length and decimal places.
        /// </summary>
        /// <param name="length">Maximum length of the data.</param>
        /// <param name="decimalPlaces">Maximum number of decimal places.</param>
        public SMEFloatProperty(int length, int decimalPlaces) :
            this(length, decimalPlaces, String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEFloatProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEFloatProperty(string label) :
            this(DefaultLength, DefaultDecimalPlaces, label)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEFloatProperty"/> class.
        /// </summary>
        public SMEFloatProperty() :
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
        /// A <see cref="float"/> containing the minimum value;
        public float Minimum
        {
            get { return _fMin; }
            set { _fMin = value; }
        }

        /// <summary>
        /// Gets or sets the maximum value;
        /// </summary>
        /// A <see cref="float"/> containing the maximum value;
        public float Maximum
        {
            get { return _fMax; }
            set { _fMax = value; }
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
                float.TryParse(minFacet.Value, out _fMin);
            }
            else
            {
                var maxFacet = facet as XmlSchemaMaxInclusiveFacet;

                if (maxFacet != null)
                {
                    float.TryParse(maxFacet.Value, out _fMax);
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
            base.OnValidate(value, typeof (float));

            if (value != null)
            {
                var typedValue = (float) value;

                if (typedValue < Minimum)
                    ThrowValidationFailed(SDataResource.XmlConstants.MinInclusive);

                if (Maximum != DefaultMaximum && typedValue > Maximum)
                    ThrowValidationFailed(SDataResource.XmlConstants.MaxInclusive);
            }
        }

        #endregion
    }
}