using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a <see cref="string"/> property.
    /// </summary>
    public class SMEStringProperty : SMEProperty
    {
        #region Fields

        private int _iMinLength;
        private int _iMaxLength;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEStringProperty"/> class
        /// with the specified maximum length and label,
        /// </summary>
        /// <param name="maxLength">Maximum length of the data.</param>
        /// <param name="label">The label for the property.</param>
        public SMEStringProperty(int maxLength, string label) :
            base(label)
        {
            _iMaxLength = maxLength;
            DataType = XSDataTypes.String;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEStringProperty"/> class
        /// with the specified maximum length.
        /// </summary>
        /// <param name="maxLength">Maximum length of the data.</param>
        public SMEStringProperty(int maxLength) :
            this(maxLength, String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEStringProperty"/> class.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEStringProperty(string label) :
            this(0, label)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEStringProperty"/> class.
        /// </summary>
        public SMEStringProperty() :
            this(String.Empty)
        {
        }

        #region Properties

        /// <summary>
        /// Gets or sets the minimum length of data.
        /// </summary>
        /// <value>An integer containing the minimum length of data.</value>
        public int MinimumLength
        {
            get { return _iMinLength; }
            set { _iMinLength = value; }
        }

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

        #region Overrides

        /// <summary>
        /// Returns the default average length (in characters) to use for this property.
        /// </summary>
        /// <returns>The default average length (in characters) to use for this property.</returns>
        protected override int GetDefaultAverageLength()
        {
            if (MaximumLength <= 0)
                return 0;

            return MinimumLength + ((MaximumLength - MinimumLength)/2);
        }

        /// <summary>
        /// Loads a facet for this property.
        /// </summary>
        /// <param name="facet">The facet to load for this property.</param>
        protected override void OnLoadFacet(XmlSchemaFacet facet)
        {
            base.OnLoadFacet(facet);

            var minLengthFacet = facet as XmlSchemaMinLengthFacet;

            if (minLengthFacet != null)
            {
                int.TryParse(minLengthFacet.Value, out _iMinLength);
            }
            else
            {
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
                    case SDataResource.XmlConstants.MinLength:
                        int.TryParse(attribute.Value, out _iMinLength);
                        break;

                    case SDataResource.XmlConstants.Length:
                    case SDataResource.XmlConstants.MaxLength:
                        int.TryParse(attribute.Value, out _iMaxLength);
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
            get { return base.HasFacets || MinimumLength > 0; }
        }

        /// <summary>
        /// Adds the schema facets for this property.
        /// </summary>
        /// <param name="builder">The <see cref="StringBuilder"/> to add the facets to.</param>
        protected override void OnGetSchemaFacets(StringBuilder builder)
        {
            base.OnGetSchemaFacets(builder);

            if (MinimumLength > 0)
            {
                // <xs:minLength value="x" />
                builder.AppendFormat("<{0} {1}=\"{2}\"/>",
                                     TypeInfoHelper.FormatXS(SDataResource.XmlConstants.MinLength),
                                     SDataResource.XmlConstants.Value,
                                     MinimumLength
                    );
            }
        }

        protected override IDictionary<string, string> OnGetSchemaAttributes()
        {
            var attributes = base.OnGetSchemaAttributes();

            if (MaximumLength > 0)
            {
                attributes.Add(
                    SDataResource.FormatSME(SDataResource.XmlConstants.MaxLength),
                    MaximumLength.ToString()
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
            base.OnValidate(value, typeof (string));

            if (value != null)
            {
                var typedValue = value.ToString();
                var length = typedValue.Length;

                if (typedValue.Length < MinimumLength)
                    ThrowValidationFailed(SDataResource.XmlConstants.MinLength);

                if (MaximumLength > 0 && typedValue.Length > MaximumLength)
                    ThrowValidationFailed(SDataResource.XmlConstants.MaxLength);
            }
        }

        #endregion
    }
}