namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// Defines the severity of an error.
    /// </summary>
    public enum SDataDiagnosisSeverity
    {
        /// <summary>
        /// Informational message, does not require any special attention.
        /// </summary>
        Info,
        /// <summary>
        /// Warning message: does not prevent operation from succeeding but may require attention.
        /// </summary>
        Warning,
        /// <summary>
        /// Transient error, operation failed but may succeed later in the same condition.
        /// </summary>
        Transient,
        /// <summary>
        /// Error, operation failed, request should be modified before resubmitting.
        /// </summary>
        Error,
        /// <summary>
        /// Severe error, operation should not be reattempted (and other operations are likely to fail too).
        /// </summary>
        Fatal,
    }
}