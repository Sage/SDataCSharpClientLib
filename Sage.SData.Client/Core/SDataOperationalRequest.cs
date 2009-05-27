using System;
using System.Collections.Generic;
using System.Text;

namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Base classes for non Resource based URLs
    /// </summary>
    abstract public class SDataOperationalRequest : SDataApplicationRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="service">ISDataService for this request</param>
         public SDataOperationalRequest(ISDataService service)
            : base(service)
        {
 
        }
    }
}
