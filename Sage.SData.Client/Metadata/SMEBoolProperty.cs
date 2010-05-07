using System;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a <see cref="bool"/> property.
    /// </summary>
    public class SMEBoolProperty : SMEProperty
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SMEBoolProperty"/> class.
        /// </summary>
        public SMEBoolProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEBoolProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEBoolProperty(string label) :
            base(label)
        {
            DataType = XSDataTypes.Boolean;
        }

        #region Overrides

        /// <summary>
        /// Returns the default average length (in characters) to use for this property.
        /// </summary>
        /// <returns>The default average length (in characters) to use for this property.</returns>
        protected override int GetDefaultAverageLength()
        {
            return 5;
        }

        /// <summary>
        /// Validates the specified value against the constraints of the property.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        protected override void OnValidate(object value)
        {
            base.OnValidate(value, typeof (bool));
        }

        #endregion
    }
}