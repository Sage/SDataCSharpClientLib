using System;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a token <see cref="string"/> property.
    /// </summary>
    public class SMETokenProperty : SMEStringProperty
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SMETokenProperty"/> class
        /// with the specified maximum length and label,
        /// </summary>
        /// <param name="maxLength">Maximum length of the data.</param>
        /// <param name="label">The label for the property.</param>
        public SMETokenProperty(int maxLength, string label) :
            base(maxLength, label)
        {
            DataType = XSDataTypes.Token;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMETokenProperty"/> class
        /// with the specified maximum length.
        /// </summary>
        /// <param name="maxLength">Maximum length of the data.</param>
        public SMETokenProperty(int maxLength) :
            this(maxLength, String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMETokenProperty"/> class.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMETokenProperty(string label) :
            this(0, label)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMETokenProperty"/> class.
        /// </summary>
        public SMETokenProperty() :
            this(String.Empty)
        {
        }
    }
}