namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a <see cref="object"/> property.
    /// </summary>
    public class SMEObjectProperty : SMEProperty
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SMEObjectProperty"/> class.
        /// </summary>
        public SMEObjectProperty()
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEGuidProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEObjectProperty(string label) :
            base(label)
        {
        }

        /// <summary>
        /// Returns the default average length (in characters) to use for this property.
        /// </summary>
        /// <returns>The default average length (in characters) to use for this property.</returns>
        protected override int GetDefaultAverageLength()
        {
            return 0;
        }

        /// <summary>
        /// Validates the specified value against the constraints of the property.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        protected override void OnValidate(object value)
        {
        }
    }
}