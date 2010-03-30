namespace Sage.SData.Client.Core
{
    /// <summary>
    /// Interface which defines the settings common to all
    /// SData Requests
    /// </summary>
    public interface ISDataRequestSettings
    {
        /// <summary>
        /// Accessor method for protocol
        /// </summary>
        /// <remarks>HTTP is the default but can be HTTPS</remarks>
        string Protocol { get; set; }

        /// <remarks>IP address is also allowed (192.168.1.1).
        /// Can be followed by port number. For example www.example.com:5493. 
        /// 5493 is the recommended port number for SData services that are not exposed on the Internet.
        /// </remarks>
        string ServerName { get; set; }

        /// <summary>
        /// Accessor method for port
        /// </summary>
        int? Port { get; set; }

        /// <summary>
        /// Accessor method for virtual directory
        /// </summary>
        /// <remarks>
        /// Must be sdata, unless the technical framework imposes something different.
        ///</remarks>
        string VirtualDirectory { get; set; }
    }
}