using System;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a <see cref="byte"/> property.
    /// </summary>
    public class SMEByteProperty : SMEStringProperty
    {
        #region Constants

        /// <summary>
        /// Default average length for a <see cref="byte"/>.
        /// </summary>
        /// <value>The default average length for a <see cref="byte"/>, which is <b>2</b>.</value>
        public const int DefaultAverageLength = 3;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEByteProperty"/> class.
        /// </summary>
        public SMEByteProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEByteProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEByteProperty(string label) :
            base(label)
        {
            DataType = XSDataTypes.Byte;
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

        #endregion
    }
}