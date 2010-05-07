using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a <see cref="Short"/> property.
    /// </summary>
    public class SMEUnsignedShortProperty : SMEProperty
    {
        #region Constants

        /// <summary>
        /// Default average length for a <see cref="short"/>.
        /// </summary>
        /// <value>The default average length for a <see cref="short"/>, which is <b>5</b>.</value>
        public const int DefaultAverageLength = 5;

        public const ushort DefaultMinimum = 0;
        public const ushort DefaultMaximum = 0;
        public const int DefaultLength = 0;

        #endregion

        #region Fields

        private int _iLength;
        private ushort _usMin;
        private ushort _usMax;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEUnsignedShortProperty"/> class
        /// with the specified length and label.
        /// </summary>
        /// <param name="length">Maximum length of the data.</param>
        /// <param name="label">The label for the property.</param>
        public SMEUnsignedShortProperty(int length, string label) :
            base(label)
        {
            _iLength = length;
            DataType = XSDataTypes.UnsignedShort;
            _usMin = DefaultMinimum;
            _usMax = DefaultMaximum;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEUnsignedShortProperty"/> class
        /// with the specified length.
        /// </summary>
        /// <param name="length">Maximum length of the data.</param>
        public SMEUnsignedShortProperty(int length) :
            this(length, String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEUnsignedShortProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEUnsignedShortProperty(string label) :
            this(DefaultLength, label)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEUnsignedShortProperty"/> class.
        /// </summary>
        public SMEUnsignedShortProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Gets or sets the maximum length of data.
        /// </summary>
        /// <value>An Short containing the maximum length of data.</value>
        public int Length
        {
            get { return _iLength; }
            set { _iLength = value; }
        }

        /// <summary>
        /// Gets or sets the minimum value;
        /// </summary>
        /// A <see cref="ushort"/> containing the minimum value;
        public ushort Minimum
        {
            get { return _usMin; }
            set { _usMin = value; }
        }

        /// <summary>
        /// Gets or sets the maximum value;
        /// </summary>
        /// A <see cref="ushort"/> containing the maximum value;
        public ushort Maximum
        {
            get { return _usMax; }
            set { _usMax = value; }
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
                ushort.TryParse(minFacet.Value, out _usMin);
            }
            else
            {
                var maxFacet = facet as XmlSchemaMaxInclusiveFacet;

                if (maxFacet != null)
                {
                    ushort.TryParse(maxFacet.Value, out _usMax);
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
            base.OnValidate(value, typeof (ushort));

            if (value != null)
            {
                var typedValue = (ushort) value;

                if (typedValue < Minimum)
                    ThrowValidationFailed(SDataResource.XmlConstants.MinInclusive);

                if (Maximum != DefaultMaximum && typedValue > Maximum)
                    ThrowValidationFailed(SDataResource.XmlConstants.MaxInclusive);
            }
        }

        #endregion
    }
}