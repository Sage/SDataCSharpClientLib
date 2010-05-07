using System;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines an Base 64 Binary property.
    /// </summary>
    public class SMEBase64BinaryProperty : SMEByteArrayProperty
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SMEBase64BinaryProperty"/> class.
        /// </summary>
        public SMEBase64BinaryProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEBase64BinaryProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEBase64BinaryProperty(string label) :
            base(label)
        {
            DataType = XSDataTypes.Base64Binary;
        }
    }
}