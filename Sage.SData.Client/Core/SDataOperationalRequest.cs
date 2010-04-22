namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Base classes for non Resource based URLs
    /// </summary>
    public abstract class SDataOperationalRequest : SDataApplicationRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
        protected SDataOperationalRequest(ISDataService service)
            : base(service)
        {
        }
    }
}