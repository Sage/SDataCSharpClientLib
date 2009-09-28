using System;
using System.Net;
using System.Xml.XPath;

namespace Sage.SData.Client.Common
{
    /// <summary>
    /// Provides data for the <see cref="ISyndicationResource.Loaded"/> event.
    /// </summary>
    /// <remarks>
    ///     A <see cref="ISyndicationResource.Loaded"/> event occurs whenever the <see cref="ISyndicationResource.Load(System.Xml.XmlReader)"/> 
    ///     or <see cref="ISyndicationResource.Load(System.Xml.XPath.IXPathNavigable)"/> methods are called.
    /// </remarks>
    /// <seealso cref="ISyndicationResource"/>
    /// <seealso cref="ISyndicationResource.Load(System.Xml.XPath.IXPathNavigable)"/>
    /// <seealso cref="ISyndicationResource.Load(System.Xml.XmlReader)"/>
    [Serializable()]
    public class SyndicationResourceLoadedEventArgs : EventArgs, IComparable
    {
        //============================================================
        //	PUBLIC/PRIVATE/PROTECTED MEMBERS
        //============================================================
        #region PRIVATE/PROTECTED/PUBLIC MEMBERS
        /// <summary>
        /// Private member to hold instance of event with no event data.
        /// </summary>
        private static readonly SyndicationResourceLoadedEventArgs emptyEventArguments  = new SyndicationResourceLoadedEventArgs();
        /// <summary>
        /// Private member to hold read-only XPathNavigator object for navigating the XML data used to load the syndication resource.
        /// </summary>
        [NonSerialized()]
        private XPathNavigator eventNavigator;
        /// <summary>
        /// Private member to hold the URI that the syndication resource information was retrieved from.
        /// </summary>
        private Uri eventSource;
        /// <summary>
        /// Private member to hold the network credentials used for authenticating the request to a web resource.
        /// </summary>
        private ICredentials eventCredentials;
        /// <summary>
        /// Private member to hold the network proxy to used to access a web resource.
        /// </summary>
        private IWebProxy eventProxy;
        /// <summary>
        /// Private member to hold the cookie container used to share state information between requests.
        /// </summary>
        private CookieContainer eventCookies;
        /// <summary>
        /// Private member to hold an object containing state information that was passed to the asynchronous load operation.
        /// </summary>
        private Object eventUserToken;
        #endregion
        
        //============================================================
		//	CONSTRUCTORS
        //============================================================
        #region SyndicationResourceLoadedEventArgs()
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationResourceLoadedEventArgs"/> class.
        /// </summary>
        public SyndicationResourceLoadedEventArgs()
		{
			//------------------------------------------------------------
			//	
			//------------------------------------------------------------
		}
		#endregion

        #region SyndicationResourceLoadedEventArgs(IXPathNavigable navigator)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationResourceLoadedEventArgs"/> class using the supplied <see cref="IXPathNavigable"/>.
        /// </summary>
        /// <param name="data">A <see cref="IXPathNavigable"/> object that represents the XML data that was used to load the syndication resource.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="data"/> is a null reference (Nothing in Visual Basic).</exception>
        public SyndicationResourceLoadedEventArgs(IXPathNavigable data) : this()
        {
            //------------------------------------------------------------
            //	Validate parameter
            //------------------------------------------------------------
            Guard.ArgumentNotNull(data, "data");

            eventNavigator  = data.CreateNavigator();
        }
        #endregion

        #region SyndicationResourceLoadedEventArgs(IXPathNavigable data, Uri source, ICredentials credentials, IWebProxy proxy)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationResourceLoadedEventArgs"/> class using the supplied <see cref="IXPathNavigable"/>, <see cref="ICredentials">credentials</see> and <see cref="IWebProxy">proxy</see>.
        /// </summary>
        /// <param name="data">A <see cref="IXPathNavigable"/> object that represents the XML data that was used to load the syndication resource.</param>
        /// <param name="source">
        ///     The <see cref="Uri"/> of the Internet resource that the syndication resource was loaded from. Can be <b>null</b> if syndication resource was not loaded using an Internet resource.
        /// </param>
        /// <param name="credentials">
        ///    The <see cref="ICredentials"/> that were used to authenticate the request to an Internet resource. Can be <b>null</b> if syndication resource was not loaded using an Internet resource.
        /// </param>
        /// <param name="proxy">
        ///     The <see cref="IWebProxy"/> used to access the Internet resource. Can be <b>null</b> if syndication resource was not loaded using an Internet resource.
        /// </param>
        /// <exception cref="ArgumentNullException">The <paramref name="data"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        public SyndicationResourceLoadedEventArgs(IXPathNavigable data, Uri source, ICredentials credentials, IWebProxy proxy) : this(data)
        {
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");

            eventSource         = source;
            eventCredentials    = credentials;
            eventProxy          = proxy;
        }
        #endregion

        #region SyndicationResourceLoadedEventArgs(IXPathNavigable data, Uri source, WebRequestContext context)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationResourceLoadedEventArgs"/> class using the supplied <see cref="IXPathNavigable"/>, <see cref="ICredentials">credentials</see> and <see cref="IWebProxy">proxy</see>.
        /// </summary>
        /// <param name="data">A <see cref="IXPathNavigable"/> object that represents the XML data that was used to load the syndication resource.</param>
        /// <param name="source">
        ///     The <see cref="Uri"/> of the Internet resource that the syndication resource was loaded from. Can be <b>null</b> if syndication resource was not loaded using an Internet resource.
        /// </param>
        /// <param name="context">A <see cref="WebRequestContext"/> that holds shared contextual information about the web request.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="data"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        public SyndicationResourceLoadedEventArgs(IXPathNavigable data, Uri source, WebRequestContext context) : this(data)
        {
            //------------------------------------------------------------
            //	Validate parameters
            //------------------------------------------------------------
            Guard.ArgumentNotNull(source, "source");

            eventSource         = source;
            eventCredentials    = context.Credentials;
            eventProxy          = context.Proxy;
            eventCookies        = context.Cookies;
        }
        #endregion

        #region SyndicationResourceLoadedEventArgs(IXPathNavigable data, Uri source, ICredentials credentials, IWebProxy proxy, Object state)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationResourceLoadedEventArgs"/> class using the supplied <see cref="IXPathNavigable"/>, <see cref="ICredentials">credentials</see>, <see cref="IWebProxy">proxy</see> and user token.
        /// </summary>
        /// <param name="data">A <see cref="IXPathNavigable"/> object that represents the XML data that was used to load the syndication resource.</param>
        /// <param name="source">
        ///     The <see cref="Uri"/> of the Internet resource that the syndication resource was loaded from. Can be <b>null</b> if syndication resource was not loaded using an Internet resource.
        /// </param>
        /// <param name="credentials">
        ///    The <see cref="ICredentials"/> that were used to authenticate the request to an Internet resource. Can be <b>null</b> if syndication resource was not loaded using an Internet resource.
        /// </param>
        /// <param name="proxy">
        ///     The <see cref="IWebProxy"/> used to access the Internet resource. Can be <b>null</b> if syndication resource was not loaded using an Internet resource.
        /// </param>
        /// <param name="state">The user-defined object that was passed to the asynchronous operation.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="data"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        public SyndicationResourceLoadedEventArgs(IXPathNavigable data, Uri source, ICredentials credentials, IWebProxy proxy, Object state) : this(data, source, credentials, proxy)
        {
            eventUserToken  = state;
        }
        #endregion

        #region SyndicationResourceLoadedEventArgs(IXPathNavigable data, Uri source, WebRequestContext context, Object state)
        /// <summary>
        /// Initializes a new instance of the <see cref="SyndicationResourceLoadedEventArgs"/> class using the supplied <see cref="IXPathNavigable"/>, <see cref="ICredentials">credentials</see>, <see cref="IWebProxy">proxy</see> and user token.
        /// </summary>
        /// <param name="data">A <see cref="IXPathNavigable"/> object that represents the XML data that was used to load the syndication resource.</param>
        /// <param name="source">
        ///     The <see cref="Uri"/> of the Internet resource that the syndication resource was loaded from. Can be <b>null</b> if syndication resource was not loaded using an Internet resource.
        /// </param>
        /// <param name="context">A <see cref="WebRequestContext"/> that holds shared contextual information about the web request.</param>
        /// <param name="state">The user-defined object that was passed to the asynchronous operation.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="data"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        public SyndicationResourceLoadedEventArgs(IXPathNavigable data, Uri source, WebRequestContext context, Object state) : this(data, source, context)
        {
            eventUserToken  = state;
        }
        #endregion

        //============================================================
        //	STATIC PROPERTIES
        //============================================================
        #region Empty
        /// <summary>
        /// Represents an syndication resource loaded event with no event data.
        /// </summary>
        /// <value>An uninitialized instance of the <see cref="SyndicationResourceLoadedEventArgs"/> class.</value>
        /// <remarks>The value of Empty is a read-only instance of <see cref="SyndicationResourceLoadedEventArgs"/> equivalent to the result of calling the <see cref="SyndicationResourceLoadedEventArgs()"/> constructor.</remarks>
        public static new SyndicationResourceLoadedEventArgs Empty
        {
            get
            {
                return emptyEventArguments;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Credentials
        /// <summary>
        /// Gets the network credentials used for authenticating the request to the Internet resource that the syndication resource was loaded from.
        /// </summary>
        /// <value>
        ///     The <see cref="ICredentials"/> that were used to authenticate the request to an Internet resource. 
        ///     If the <see cref="ISyndicationResource"/> was not loaded by an Internet resource or no credentials were provided, returns <b>null</b>.
        /// </value>
        /// <seealso cref="ISyndicationResource.Load(Uri, ICredentials, IWebProxy)"/>
        public ICredentials Credentials
        {
            get
            {
                return eventCredentials;
            }
        }
        #endregion

        #region Data
        /// <summary>
        /// Gets a read-only <see cref="XPathNavigator"/> object for navigating the XML data that was used to load the syndication resource.
        /// </summary>
        /// <value>
        ///     A read-only <see cref="XPathNavigator"/> object for navigating the XML data that was used to load the syndication resource.
        /// </value>
        public XPathNavigator Data
        {
            get
            {
                return eventNavigator;
            }
        }
        #endregion

        #region Proxy
        /// <summary>
        /// Gets the network proxy used to access the Internet resource that the syndication resource was loaded from.
        /// </summary>
        /// <value>
        ///     The <see cref="IWebProxy"/> used to access the Internet resource. 
        ///     If the <see cref="ISyndicationResource"/> was not loaded by an Internet resource or no proxy was specified, returns <b>null</b>.
        /// </value>
        /// <seealso cref="ISyndicationResource.Load(Uri, ICredentials, IWebProxy)"/>
        public IWebProxy Proxy
        {
            get
            {
                return eventProxy;
            }
        }
        #endregion

        #region Cookies
        /// <summary>
        /// Gets or sets the cookie container used by the client to share state between requests.
        /// </summary>
        /// <value>
        ///     A <see cref="CookieContainer"/> object that represents the cookie container used by the client to share state between requests.
        ///     The default is a null reference (Nothing in Visual Basic), which indicates no cookies will be shared between requests.
        /// </value>
        public CookieContainer Cookies
        {
            get
            {
                return eventCookies;
            }
        }
        #endregion

        #region Source
        /// <summary>
        /// Gets the <see cref="Uri"/> of the Internet resource that the syndication resource was loaded from.
        /// </summary>
        /// <value>
        ///     The <see cref="Uri"/> of the Internet resource that the syndication resource was loaded from. 
        ///     If the <see cref="ISyndicationResource"/> was not loaded by an Internet resource, returns <b>null</b>.
        /// </value>
        /// <seealso cref="ISyndicationResource.Load(Uri, ICredentials, IWebProxy)"/>
        public Uri Source
        {
            get
            {
                return eventSource;
            }
        }
        #endregion

        #region State
        /// <summary>
        /// Gets an <see cref="Object"/> containing state information that was passed to the asynchronous load operation.
        /// </summary>
        /// <value>
        ///     A <see cref="Object"/> containing state information that was passed to the asynchronous load operation. 
        ///     If the <see cref="ISyndicationResource"/> was not loaded by an Internet resource or no user token provided, returns <b>null</b>.
        /// </value>
        /// <seealso cref="ISyndicationResource.LoadAsync(Uri, Object)"/>
        public Object State
        {
            get
            {
                return eventUserToken;
            }
        }
        #endregion

        //============================================================
        //	PUBLIC OVERRIDES
        //============================================================
        #region ToString()
        /// <summary>
        /// Returns a <see cref="String"/> that represents the current <see cref="SyndicationResourceLoadedEventArgs"/>.
        /// </summary>
        /// <returns>A <see cref="String"/> that represents the current <see cref="SyndicationResourceLoadedEventArgs"/>.</returns>
        /// <remarks>
        ///     This method returns a human-readable string for the current instance. Hash code values are displayed for applicable properties.
        /// </remarks>
        public override string ToString()
        {
            //------------------------------------------------------------
            //	Build the string representation
            //------------------------------------------------------------
            string source       = this.Source != null ? this.Source.ToString() : String.Empty;
            string data         = this.Data != null ? this.Data.GetHashCode().ToString(System.Globalization.NumberFormatInfo.InvariantInfo) : String.Empty;
            string credentials  = this.Credentials != null ? this.Credentials.GetHashCode().ToString(System.Globalization.NumberFormatInfo.InvariantInfo) : String.Empty;
            string proxy        = this.Proxy != null ? this.Proxy.GetHashCode().ToString(System.Globalization.NumberFormatInfo.InvariantInfo) : String.Empty;
            string cookies      = this.Cookies != null ? this.Cookies.GetHashCode().ToString(System.Globalization.NumberFormatInfo.InvariantInfo) : String.Empty;
            string state        = this.State != null ? this.State.GetHashCode().ToString(System.Globalization.NumberFormatInfo.InvariantInfo) : String.Empty;

            return String.Format(null, "[SyndicationResourceLoadedEventArgs(Source = \"{0}\", Data = \"{1}\", Credentials = \"{2}\", Proxy = \"{3}\", Cookies = \"{4}\", State = \"{5}\")]", source, data, credentials, proxy, cookies, state);
        }
        #endregion

        //============================================================
        //	ICOMPARABLE IMPLEMENTATION
        //============================================================
        #region CompareTo(object obj)
        /// <summary>
        /// Compares the current instance with another object of the same type.
        /// </summary>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <returns>A 32-bit signed integer that indicates the relative order of the objects being compared.</returns>
        /// <exception cref="ArgumentException">The <paramref name="obj"/> is not the expected <see cref="Type"/>.</exception>
        public int CompareTo(object obj)
        {
            //------------------------------------------------------------
            //	If target is a null reference, instance is greater
            //------------------------------------------------------------
            if (obj == null)
            {
                return 1;
            }

            //------------------------------------------------------------
            //	Determine comparison result using property state of objects
            //------------------------------------------------------------
            SyndicationResourceLoadedEventArgs value  = obj as SyndicationResourceLoadedEventArgs;

            if (value != null)
            {
                int result  = 0;
                result      = result | String.Compare(this.Data.OuterXml, value.Data.OuterXml, StringComparison.OrdinalIgnoreCase);
                result      = result | Uri.Compare(this.Source, value.Source, UriComponents.AbsoluteUri, UriFormat.SafeUnescaped, StringComparison.OrdinalIgnoreCase);

                return result;
            }
            else
            {
                throw new ArgumentException(String.Format(null, "obj is not of type {0}, type was found to be '{1}'.", this.GetType().FullName, obj.GetType().FullName), "obj");
            }
        }
        #endregion

        #region Equals(Object obj)
        /// <summary>
        /// Determines whether the specified <see cref="Object"/> is equal to the current instance.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare with the current instance.</param>
        /// <returns><b>true</b> if the specified <see cref="Object"/> is equal to the current instance; otherwise, <b>false</b>.</returns>
        public override bool Equals(Object obj)
        {
            //------------------------------------------------------------
            //	Determine equality via type then by comparision
            //------------------------------------------------------------
            if (!(obj is SyndicationResourceLoadedEventArgs))
            {
                return false;
            }

            return (this.CompareTo(obj) == 0);
        }
        #endregion

        #region GetHashCode()
        /// <summary>
        /// Returns a hash code for the current instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode()
        {
            //------------------------------------------------------------
            //	Generate has code using unique value of ToString() method
            //------------------------------------------------------------
            char[] charArray    = this.ToString().ToCharArray();

            return charArray.GetHashCode();
        }
        #endregion

        #region == operator
        /// <summary>
        /// Determines if operands are equal.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>true</b> if the values of its operands are equal, otherwise; <b>false</b>.</returns>
        public static bool operator ==(SyndicationResourceLoadedEventArgs first, SyndicationResourceLoadedEventArgs second)
        {
            //------------------------------------------------------------
            //	Handle null reference comparison
            //------------------------------------------------------------
            if (object.Equals(first, null) && object.Equals(second, null))
            {
                return true;
            }
            else if (object.Equals(first, null) && !object.Equals(second, null))
            {
                return false;
            }

            return first.Equals(second);
        }
        #endregion

        #region != operator
        /// <summary>
        /// Determines if operands are not equal.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>false</b> if its operands are equal, otherwise; <b>true</b>.</returns>
        public static bool operator !=(SyndicationResourceLoadedEventArgs first, SyndicationResourceLoadedEventArgs second)
        {
            return !(first == second);
        }
        #endregion

        #region < operator
        /// <summary>
        /// Determines if first operand is less than second operand.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>true</b> if the first operand is less than the second, otherwise; <b>false</b>.</returns>
        public static bool operator <(SyndicationResourceLoadedEventArgs first, SyndicationResourceLoadedEventArgs second)
        {
            //------------------------------------------------------------
            //	Handle null reference comparison
            //------------------------------------------------------------
            if (object.Equals(first, null) && object.Equals(second, null))
            {
                return false;
            }
            else if (object.Equals(first, null) && !object.Equals(second, null))
            {
                return true;
            }

            return (first.CompareTo(second) < 0);
        }
        #endregion

        #region > operator
        /// <summary>
        /// Determines if first operand is greater than second operand.
        /// </summary>
        /// <param name="first">Operand to be compared.</param>
        /// <param name="second">Operand to compare to.</param>
        /// <returns><b>true</b> if the first operand is greater than the second, otherwise; <b>false</b>.</returns>
        public static bool operator >(SyndicationResourceLoadedEventArgs first, SyndicationResourceLoadedEventArgs second)
        {
            //------------------------------------------------------------
            //	Handle null reference comparison
            //------------------------------------------------------------
            if (object.Equals(first, null) && object.Equals(second, null))
            {
                return false;
            }
            else if (object.Equals(first, null) && !object.Equals(second, null))
            {
                return false;
            }

            return (first.CompareTo(second) > 0);
        }
        #endregion
    }
}