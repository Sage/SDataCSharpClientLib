using System;

namespace Sage.SData.Client.Extensions
{
    /// <summary>
    /// SDaata Diagnosis Codes
    /// </summary>
    [Flags]
    public enum SDataDiagnosisCode
    {
        /// <summary>
        /// Application specific diagnosis, detail is in the applicationCode element.
        /// </summary>
        ApplicationDiagnosis = -1,
        /// <summary>
        /// Invalid URL syntax.
        /// </summary>
        BadUrlSyntax = 1,
        /// <summary>
        /// Invalid Query Parameter.
        /// </summary>
        BadQueryParameter = 2,
        /// <summary>
        /// Application does not exist.
        /// </summary>
        ApplicationNotFound = 4,
        /// <summary>
        /// Application exists but is not available.
        /// </summary>
        ApplicationUnavailable = 8,
        /// <summary>
        /// Dataset does not exist.
        /// </summary>
        DatasetNotFound = 16,
        /// <summary>
        /// Dataset exists but is not available.
        /// </summary>
        DatasetUnavailable = 32,
        /// <summary>
        /// Contract does not exist.
        /// </summary>
        ContractNotFound = 64,
        /// <summary>
        /// Resource kind does not exist.
        /// </summary>
        ResourceKindNotFound = 128,
        /// <summary>
        /// Invalid syntax for a where condition.
        /// </summary>
        BadWhereSyntax = 256,
    }
}