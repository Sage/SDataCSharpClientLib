using System;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a Date property.
    /// </summary>
    public class SMEDateProperty : SMEDateTimeProperty
    {
        #region Constants

        /// <summary>
        /// Default average length for a time.
        /// </summary>
        /// <value>The default average length for a time, which is <b>16</b>.</value>
        public new const int DefaultAverageLength = 16;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEDateProperty"/> class.
        /// </summary>
        public SMEDateProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEDateProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEDateProperty(string label) :
            base(label)
        {
            DataType = XSDataTypes.Date;
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