using System;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a Time property.
    /// </summary>
    public class SMETimeProperty : SMEDateTimeProperty
    {
        #region Constants

        /// <summary>
        /// Default average length for a time.
        /// </summary>
        /// <value>The default average length for a time, which is <b>32</b>.</value>
        public new const int DefaultAverageLength = 16;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMETimeProperty"/> class.
        /// </summary>
        public SMETimeProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMETimeProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMETimeProperty(string label) :
            base(label)
        {
            DataType = XSDataTypes.Time;
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