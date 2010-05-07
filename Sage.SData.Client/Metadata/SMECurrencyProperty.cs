namespace Sage.SData.Client.Metadata
{
    /// <summary>
    /// Defines a numerical property.
    /// </summary>
    public class SMECurrencyProperty : SMEDecimalProperty
    {
        #region Constants

        public new const int DefaultDecimalPlaces = 2;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="SMECurrencyProperty"/> class
        /// with the specified length, decimal places and lab el.
        /// </summary>
        /// <param name="length">Maximum length of the data.</param>
        /// <param name="decimalPlaces">Maximum number of decimal places.</param>
        /// <param name="label">The label for the property.</param>
        public SMECurrencyProperty(int length, int decimalPlaces, string label) :
            base(length, decimalPlaces, label)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMECurrencyProperty"/> class
        /// with the specified length, decimal places and lab el.
        /// </summary>
        /// <param name="length">Maximum length of the data.</param>
        /// <param name="decimalPlaces">Maximum number of decimal places.</param>
        public SMECurrencyProperty(int length, int decimalPlaces) :
            base(length, decimalPlaces)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMECurrencyProperty"/> class
        /// with the specified label.
        /// </summary>
        /// <param name="label">The label for the property.</param>
        public SMECurrencyProperty(string label) :
            this(DefaultLength, DefaultDecimalPlaces, label)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SMECurrencyProperty"/> class.
        /// </summary>
        public SMECurrencyProperty() :
            this(null)
        {
        }
    }
}