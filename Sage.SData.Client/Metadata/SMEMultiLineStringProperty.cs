using System;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a <see cref="string"/> property.
    /// </summary>
    public class SMEMultiLineStringProperty : SMEStringProperty
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SMEStringPropertyAttribute"/> class
        /// with the specified maximum length and label,
        /// </summary>
        /// <param name="maxLength">Maximum length of the data.</param>
        /// <param name="label">The label for the property.</param>
        public SMEMultiLineStringProperty(int maxLength, string label) :
            base(maxLength, label)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEStringPropertyAttribute"/> class
        /// with the specified maximum length.
        /// </summary>
        /// <param name="maxLength">Maximum length of the data.</param>
        public SMEMultiLineStringProperty(int maxLength) :
            base(maxLength, String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEStringPropertyAttribute"/> class.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEMultiLineStringProperty(string label) :
            base(0, label)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEStringPropertyAttribute"/> class.
        /// </summary>
        public SMEMultiLineStringProperty() :
            base(String.Empty)
        {
        }
    }
}