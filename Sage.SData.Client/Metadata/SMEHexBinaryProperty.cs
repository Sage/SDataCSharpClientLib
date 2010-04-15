using System;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines an Base 64 Binary property.
    /// </summary>
    public class SMEHexBinaryProperty : SMEByteArrayProperty
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SMEHexBinaryProperty"/> class.
        /// </summary>
        public SMEHexBinaryProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEHexBinaryProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEHexBinaryProperty(string label) :
            base(label)
        {
            DataType = XSDataTypes.HexBinary;
        }
    }
}