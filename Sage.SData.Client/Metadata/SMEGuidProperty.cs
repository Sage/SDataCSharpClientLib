using System;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a <see cref="Guid"/> property.
    /// </summary>
    public class SMEGuidProperty : SMEProperty
    {
        #region Constants

        /// <summary>
        /// Default average length for a <see cref="Guid"/>.
        /// </summary>
        /// <value>The default average length for a <see cref="Guid"/>, which is <b>32</b>.</value>
        public const int DefaultAverageLength = 32;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEGuidProperty"/> class.
        /// </summary>
        public SMEGuidProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEGuidProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEGuidProperty(string label) :
            base(label)
        {
            DataType = XSDataTypes.String;
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
            if (value == null || value is Guid)
            {
                base.OnValidate(value, typeof (Guid));
            }
            else
            {
                var strGuid = value.ToString();

                if (!strGuid.Contains("-"))
                    ThrowValidationFailed("format");

                try
                {
                    new Guid(strGuid);
                }
                catch
                {
                    ThrowValidationFailed("format");
                }
            }
        }
    }
}