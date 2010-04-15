using System;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a <see cref="Uri"/> property.
    /// </summary>
    public class SMEUriProperty : SMEProperty
    {
        #region Constants

        /// <summary>
        /// Default average length for a <see cref="Uri"/>.
        /// </summary>
        /// <value>The default average length for a <see cref="Uri"/>, which is <b>32</b>.</value>
        public const int DefaultAverageLength = 32;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEUriProperty"/> class.
        /// </summary>
        public SMEUriProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEUriProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEUriProperty(string label) :
            base(label)
        {
            DataType = XSDataTypes.AnyURI;
        }

        /// <summary>
        /// Returns the default average length (in characters) to use for this property.
        /// </summary>
        /// <returns>The default average length (in characters) to use for this property.</returns>
        protected override int GetDefaultAverageLength()
        {
            return DefaultAverageLength;
        }

        /// <summary>
        /// Validates the specified value against the constraints of the property.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        protected override void OnValidate(object value)
        {
            if (value == null || value is Uri)
            {
                base.OnValidate(value, typeof (Uri));
            }
            else
            {
                try
                {
                    new Uri(value.ToString());
                }
                catch
                {
                    ThrowValidationFailed("format");
                }
            }
        }
    }
}