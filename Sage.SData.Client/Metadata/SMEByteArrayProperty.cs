using System;

namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines an <see cref="Array"/> property.
    /// </summary>
    public class SMEByteArrayProperty : SMEArrayProperty
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SMEArrayProperty"/> class.
        /// </summary>
        public SMEByteArrayProperty() :
            this(String.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMEArrayProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMEByteArrayProperty(string label) :
            base(label)
        {
            DataType = XSDataTypes.HexBinary;
        }

        /// <summary>
        /// Validates the specified value against the constraints of the property.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        protected override void OnValidate(object value)
        {
            base.OnValidate(value, typeof (byte[]));

            if (value != null && MaximumLength != 0)
            {
                var typedValue = (byte[]) value;

                if (typedValue.Length > MaximumLength)
                    ThrowValidationFailed(SDataResource.XmlConstants.Length);
            }
        }
    }
}