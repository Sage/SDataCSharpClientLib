using System;
using System.Text;
using System.Xml.Schema;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a <see cref="DateTime"/> property.
    /// </summary>
    public class SMEDateTimeProperty : SMEProperty
    {
        #region Constants

        /// <summary>
        /// Default average length for a <see cref="DateTime"/>.
        /// </summary>
        /// <value>The default average length for a <see cref="DateTime"/>, which is <b>32</b>.</value>
        public const int DefaultAverageLength = 32;

        public static DateTime DefaultMinimum = DateTime.MinValue;
        public static DateTime DefaultMaximum = DateTime.MaxValue;

        #endregion

        #region Fields

        private DateTime _oMin;
        private DateTime _oMax;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEDateTimeProperty"/> class.
        /// </summary>
        public SMEDateTimeProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEDateTimeProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEDateTimeProperty(string label) :
            base(label)
        {
            _oMin = DefaultMinimum;
            _oMax = DefaultMaximum;
            DataType = XSDataTypes.DateTime;
        }

        /// <summary>
        /// Gets or sets the minimum value;
        /// </summary>
        /// A <see cref="DateTime"/> containing the minimum value;
        public DateTime Minimum
        {
            get { return _oMin; }
            set { _oMin = value; }
        }

        /// <summary>
        /// Gets or sets the maximum value;
        /// </summary>
        /// A <see cref="DateTime"/> containing the maximum value;
        public DateTime Maximum
        {
            get { return _oMax; }
            set { _oMax = value; }
        }

        #region Overrides

        /// <summary>
        /// Returns the default average length (in characters) to use for this property.
        /// </summary>
        /// <returns>The default average length (in characters) to use for this property.</returns>
        protected override int GetDefaultAverageLength()
        {
            return DefaultAverageLength;
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
                W3CDateTime dateTime;

                if (!W3CDateTime.TryParse(minFacet.Value, out dateTime))
                    _oMin = DateTime.MinValue;
                else
                    _oMin = dateTime.DateTime;
            }
            else
            {
                var maxFacet = facet as XmlSchemaMaxInclusiveFacet;

                if (maxFacet != null)
                {
                    W3CDateTime dateTime;

                    if (!W3CDateTime.TryParse(maxFacet.Value, out dateTime))
                        _oMax = DateTime.MaxValue;
                    else
                        _oMax = dateTime.DateTime;
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
                                     new W3CDateTime(Minimum)
                    );
            }

            if (Maximum != DefaultMaximum)
            {
                // <xs:maxInclusive value="x" />
                builder.AppendFormat("<{0} {1}=\"{2}\"/>",
                                     TypeInfoHelper.FormatXS(SDataResource.XmlConstants.MaxInclusive),
                                     SDataResource.XmlConstants.Value,
                                     new W3CDateTime(Maximum)
                    );
            }
        }

        /// <summary>
        /// Validates the specified value against the constraints of the property.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        protected override void OnValidate(object value)
        {
            base.OnValidate(value, typeof (DateTime));

            if (value != null)
            {
                var typedValue = (DateTime) value;

                if (typedValue < Minimum)
                    ThrowValidationFailed(SDataResource.XmlConstants.MinInclusive);

                if (typedValue > Maximum)
                    ThrowValidationFailed(SDataResource.XmlConstants.MaxInclusive);
            }
        }

        #endregion
    }
}