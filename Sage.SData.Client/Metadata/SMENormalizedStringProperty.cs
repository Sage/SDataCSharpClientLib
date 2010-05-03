namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a normalized <see cref="string"/> property.
    /// </summary>
    public class SMENormalizedStringProperty : SMEStringProperty
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SMENormalizedStringProperty"/> class
        /// with the specified maximum length and label,
        /// </summary>
        /// <param name="maxLength">Maximum length of the data.</param>
        /// <param name="label">The label for the property.</param>
        public SMENormalizedStringProperty(int maxLength, string label) :
            base(maxLength, label)
        {
            DataType = XSDataTypes.NormalizedString;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMENormalizedStringProperty"/> class
        /// with the specified maximum length.
        /// </summary>
        /// <param name="maxLength">Maximum length of the data.</param>
        public SMENormalizedStringProperty(int maxLength) :
            base(maxLength)
        {
            DataType = XSDataTypes.NormalizedString;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMENormalizedStringProperty"/> class.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMENormalizedStringProperty(string label) :
            base(label)
        {
            DataType = XSDataTypes.NormalizedString;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMENormalizedStringProperty"/> class.
        /// </summary>
        public SMENormalizedStringProperty()
        {
            DataType = XSDataTypes.NormalizedString;
        }
    }
}