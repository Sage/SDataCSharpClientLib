/****************************************************************************
Modification History:
*****************************************************************************
Date		Author		Description
*****************************************************************************
12/06/2007	brian.kuhn	Created ISyndicationResource Interface
****************************************************************************/
using System;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.XPath;

namespace Sage.SData.Client.Common
{
    /// <summary>
    /// Allows an object to implement a syndication resource by representing a set of properties, methods, indexers and events common to web content syndication resources.
    /// </summary>
    /// <seealso cref="Sage.SData.Client.Common.SyndicationResourceMetadata"/>
    public interface ISyndicationResource
    {
        //============================================================
        //	PUBLIC PROPERTIES
        //============================================================
        #region Format
        /// <summary>
        /// Gets the <see cref="SyndicationContentFormat"/> that the resource implements.
        /// </summary>
        /// <value>The <see cref="SyndicationContentFormat"/> enumeration value that indicates the type of syndication format that the resource implements.</value>
        SyndicationContentFormat Format
        {
            get;
        }
        #endregion

        #region Version
        /// <summary>
        /// Gets the <see cref="Version"/> of the <see cref="SyndicationContentFormat"/> that the resource conforms to.
        /// </summary>
        /// <value>The <see cref="Version"/> of the <see cref="SyndicationContentFormat"/> that the resource conforms to.</value>
        Version Version
        {
            get;
        }
        #endregion

        //============================================================
        //	PUBLIC EVENTS
        //============================================================
        #region Loaded
        /// <summary>
        /// Occurs when the syndication resource state has been changed by a load operation.
        /// </summary>
        /// <seealso cref="ISyndicationResource.Load(IXPathNavigable)"/>
        /// <seealso cref="ISyndicationResource.Load(XmlReader)"/>
        event EventHandler<SyndicationResourceLoadedEventArgs> Loaded;
        #endregion

        //============================================================
        //	PUBLIC METHODS
        //============================================================
        #region CreateNavigator()
        /// <summary>
        /// Initializes a read-only <see cref="XPathNavigator"/> object for navigating through nodes in this <see cref="ISyndicationResource"/>.
        /// </summary>
        /// <returns>A read-only <see cref="XPathNavigator"/> object.</returns>
        /// <remarks>
        ///     The <see cref="XPathNavigator"/> is positioned on the root element of the <see cref="ISyndicationResource"/>. 
        ///     If there is no root element, the <see cref="XPathNavigator"/> is positioned on the first element in the XML representation of the <see cref="ISyndicationResource"/>.
        /// </remarks>
        XPathNavigator CreateNavigator();
        #endregion

        #region Load(IXPathNavigable source)
        /// <summary>
        /// Loads the syndication resource from the specified <see cref="IXPathNavigable"/>.
        /// </summary>
        /// <param name="source">The <b>IXPathNavigable</b> used to load the syndication resource.</param>
        /// <remarks>
        ///     <para>Place your custom code in the <b>Load</b> abstract method to load the syndication resource from the specified <see cref="IXPathNavigable"/>.</para>
        ///     <para>
        ///         <b>Notes to Implementers:</b>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                     When implementing this method, the <paramref name="source"/> should be passed to the <see cref="ISyndicationResource.Load(IXPathNavigable, SyndicationResourceLoadSettings)"/> method 
        ///                     with the <item>settings</item> parameter as <b>null</b>.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="ISyndicationResource.Loaded"/> event <b>must</b> be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the resource remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the resource remains empty.</exception>
        void Load(IXPathNavigable source);
        #endregion

        #region Load(IXPathNavigable source, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Loads the syndication resource from the specified <see cref="IXPathNavigable"/> and <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <param name="source">The <b>IXPathNavigable</b> used to load the syndication resource.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="ISyndicationResource"/> instance. This value can be <b>null</b>.</param>
        /// <remarks>
        ///     <para>Place your custom code in the <b>Load</b> abstract method to load the syndication resource from the specified <see cref="IXPathNavigable"/>.</para>
        ///     <para><b>Notes to Implementers:</b> After the load operation has successfully completed, the <see cref="ISyndicationResource.Loaded"/> event <b>must</b> be raised.</para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the resource remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the resource remains empty.</exception>
        void Load(IXPathNavigable source, SyndicationResourceLoadSettings settings);
        #endregion

        #region Load(Stream stream)
        /// <summary>
        /// Loads the syndication resource from the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <b>Stream</b> used to load the syndication resource.</param>
        /// <remarks>
        ///     <para>Place your custom code in the <b>Load</b> abstract method to load the syndication resource from the specified <see cref="Stream"/>.</para>
        ///     <para>
        ///         <b>Notes to Implementers:</b>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                     When implementing this method, the <paramref name="stream"/> should be passed to the <see cref="ISyndicationResource.Load(Stream, SyndicationResourceLoadSettings)"/> method 
        ///                     with the <item>settings</item> parameter as <b>null</b>.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="ISyndicationResource.Loaded"/> event <b>must</b> be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="stream"/> data does not conform to the expected syndication content format. In this case, the resource remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the resource remains empty.</exception>
        void Load(Stream stream);
        #endregion

        #region Load(Stream stream, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Loads the syndication resource from the specified <see cref="Stream"/> and <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <param name="stream">The <b>Stream</b> used to load the syndication resource.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="ISyndicationResource"/> instance. This value can be <b>null</b>.</param>
        /// <remarks>
        ///     <para>Place your custom code in the <b>Load</b> abstract method to load the syndication resource from the specified <see cref="Stream"/>.</para>
        ///     <para>
        ///         <b>Notes to Implementers:</b>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                     When implementing this method, the <paramref name="stream"/> should be used to create a <see cref="XPathNavigator"/> 
        ///                     that is then passed to the <see cref="ISyndicationResource.Load(IXPathNavigable)"/> method.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="ISyndicationResource.Loaded"/> event <b>must</b> be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="stream"/> data does not conform to the expected syndication content format. In this case, the resource remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the resource remains empty.</exception>
        void Load(Stream stream, SyndicationResourceLoadSettings settings);
        #endregion

        #region Load(XmlReader reader)
        /// <summary>
        /// Loads the syndication resource from the specified <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to load the syndication resource.</param>
        /// <remarks>
        ///     <para>Place your custom code in the <b>Load</b> abstract method to load the syndication resource from the specified <see cref="XmlReader"/>.</para>
        ///     <para>
        ///         <b>Notes to Implementers:</b>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                     When implementing this method, the <paramref name="reader"/> should be passed to the <see cref="ISyndicationResource.Load(XmlReader, SyndicationResourceLoadSettings)"/> method 
        ///                     with the <item>settings</item> parameter as <b>null</b>.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="ISyndicationResource.Loaded"/> event <b>must</b> be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="reader"/> data does not conform to the expected syndication content format. In this case, the resource remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the resource remains empty.</exception>
        void Load(XmlReader reader);
        #endregion

        #region Load(XmlReader reader, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Loads the syndication resource from the specified <see cref="XmlReader"/> and <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <param name="reader">The <b>XmlReader</b> used to load the syndication resource.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="ISyndicationResource"/> instance. This value can be <b>null</b>.</param>
        /// <remarks>
        ///     <para>Place your custom code in the <b>Load</b> abstract method to load the syndication resource from the specified <see cref="XmlReader"/>.</para>
        ///     <para>
        ///         <b>Notes to Implementers:</b>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                     When implementing this method, the <paramref name="reader"/> should be used to create a <see cref="XPathNavigator"/> 
        ///                     that is then passed to the <see cref="ISyndicationResource.Load(IXPathNavigable)"/> method.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="ISyndicationResource.Loaded"/> event <b>must</b> be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="reader"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="reader"/> data does not conform to the expected syndication content format. In this case, the resource remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the resource remains empty.</exception>
        void Load(XmlReader reader, SyndicationResourceLoadSettings settings);
        #endregion

        #region Load(Uri source, ICredentials credentials, IWebProxy proxy)
        /// <summary>
        /// Loads the syndication resource from the supplied <see cref="Uri"/> using the specified <see cref="ICredentials">credentials</see> and <see cref="IWebProxy">proxy</see>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that points to the location of the web resource used to load the syndication resource.</param>
        /// <param name="credentials">
        ///     A <see cref="ICredentials"/> that provides the proper set of credentials to the <paramref name="source"/> resource when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="proxy">
        ///     A <see cref="IWebProxy"/> that provides proxy access to the <paramref name="source"/> resource when required. This value can be <b>null</b>.
        /// </param>
        /// <remarks>
        ///     <para>
        ///         Place your custom code in the <b>Load</b> abstract method to load the syndication resource from the supplied <see cref="Uri"/> 
        ///         using the specified <see cref="ICredentials">credentials</see> and <see cref="IWebProxy">proxy</see>.
        ///     </para>
        ///     <para>
        ///         <b>Notes to Implementers:</b>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                     When implementing this method, the <paramref name="source"/>, <paramref name="credentials"/>, and <paramref name="proxy"/> parameters should be passed 
        ///                     to the <see cref="ISyndicationResource.Load(Uri, ICredentials, IWebProxy, SyndicationResourceLoadSettings)"/> method with the <item>settings</item> parameter as <b>null</b>.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                      If <paramref name="credentials"/> is <b>null</b>, request should be made using the default application credentials.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     If <paramref name="proxy"/> is <b>null</b>, request should be made using the <see cref="WebRequest"/> default proxy settings.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="ISyndicationResource.Loaded"/> event <b>must</b> be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the resource remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the resource remains empty.</exception>
        void Load(Uri source, ICredentials credentials, IWebProxy proxy);
        #endregion

        #region Load(Uri source, WebRequestOptions options)
        /// <summary>
        /// Loads the syndication resource from the supplied <see cref="Uri"/> using the specified <see cref="ICredentials">credentials</see> and <see cref="IWebProxy">proxy</see>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that points to the location of the web resource used to load the syndication resource.</param>
        /// <param name="options">A <see cref="WebRequestOptions"/> that holds options that should be applied to web requests.</param>
        /// <remarks>
        ///     <para>
        ///         Place your custom code in the <b>Load</b> abstract method to load the syndication resource from the supplied <see cref="Uri"/> 
        ///         using the specified <see cref="ICredentials">credentials</see> and <see cref="IWebProxy">proxy</see>.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the resource remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the resource remains empty.</exception>
        void Load(Uri source, WebRequestOptions options);
        #endregion

        #region Load(Uri source, ICredentials credentials, IWebProxy proxy, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Loads the syndication resource from the supplied <see cref="Uri"/> using the specified <see cref="ICredentials">credentials</see>, <see cref="IWebProxy">proxy</see> and <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that points to the location of the web resource used to load the syndication resource.</param>
        /// <param name="credentials">
        ///     A <see cref="ICredentials"/> that provides the proper set of credentials to the <paramref name="source"/> resource when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="proxy">
        ///     A <see cref="IWebProxy"/> that provides proxy access to the <paramref name="source"/> resource when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="ISyndicationResource"/> instance. This value can be <b>null</b>.</param>
        /// <remarks>
        ///     <para>
        ///         Place your custom code in the <b>Load</b> abstract method to load the syndication resource from the supplied <see cref="Uri"/> 
        ///         using the specified <see cref="ICredentials">credentials</see> and <see cref="IWebProxy">proxy</see>.
        ///     </para>
        ///     <para>
        ///         <b>Notes to Implementers:</b>
        ///         <list type="bullet">
        ///             <item>
        ///                 <description>
        ///                      If <paramref name="credentials"/> is <b>null</b>, request should be made using the default application credentials.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     If <paramref name="proxy"/> is <b>null</b>, request should be made using the <see cref="WebRequest"/> default proxy settings.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     Implementers should consider using the <see cref="SyndicationEncodingUtility.CreateSafeNavigator(Uri, ICredentials, IWebProxy, System.Text.Encoding)"/> utility method 
        ///                     to retrieve the syndication resource information in a safe manner.
        ///                 </description>
        ///             </item>
        ///             <item>
        ///                 <description>
        ///                     After the load operation has successfully completed, the <see cref="ISyndicationResource.Loaded"/> event <b>must</b> be raised.
        ///                 </description>
        ///             </item>
        ///         </list>
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the resource remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the resource remains empty.</exception>
        void Load(Uri source, ICredentials credentials, IWebProxy proxy, SyndicationResourceLoadSettings settings);
        #endregion

        #region Load(Uri source, WebRequestOptions options, SyndicationResourceLoadSettings settings)
        /// <summary>
        /// Loads the syndication resource from the supplied <see cref="Uri"/> using the specified <see cref="ICredentials">credentials</see>, <see cref="IWebProxy">proxy</see> and <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that points to the location of the web resource used to load the syndication resource.</param>
        /// <param name="options">A <see cref="WebRequestOptions"/> that holds options that should be applied to web requests.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="ISyndicationResource"/> instance. This value can be <b>null</b>.</param>
        /// <remarks>
        ///     <para>
        ///         Place your custom code in the <b>Load</b> abstract method to load the syndication resource from the supplied <see cref="Uri"/> 
        ///         using the specified <see cref="ICredentials">credentials</see> and <see cref="IWebProxy">proxy</see>.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the resource remains empty.</exception>
        /// <exception cref="XmlException">There is a load or parse error in the XML. In this case, the resource remains empty.</exception>
        void Load(Uri source, WebRequestOptions options, SyndicationResourceLoadSettings settings);
        #endregion

        #region LoadAsync(Uri source, Object userToken)
        /// <summary>
        /// Loads the syndication resource asynchronously using the specified <see cref="Uri"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        /// <remarks>
        ///     <para>Place your custom code in the <b>LoadAsync</b> abstract method to load the syndication resource asynchronously from the specified <see cref="Uri"/>.</para>
        ///     <para>The <see cref="ISyndicationResource"/> should be loaded using the default <see cref="SyndicationResourceLoadSettings"/>.</para>
        ///     <para>
        ///         To receive notification when the operation has completed or the operation has been canceled, add an event handler to the <see cref="Loaded"/> event. 
        ///         You can cancel a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> operation by calling the <see cref="LoadAsyncCancel()"/> method.
        ///     </para>
        ///     <para>
        ///         After calling <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/>, you must wait for the load operation to complete before 
        ///         attempting to load the syndication resource using the <see cref="LoadAsync(Uri, Object)"/> method.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the feed remains empty.</exception>
        /// <exception cref="InvalidOperationException">This <see cref="ISyndicationResource"/> has a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> call in progress.</exception>
        void LoadAsync(Uri source, Object userToken);
        #endregion

        #region LoadAsync(Uri source, SyndicationResourceLoadSettings settings, Object userToken)
        /// <summary>
        /// Loads the syndication resource asynchronously using the specified <see cref="Uri"/> and <see cref="SyndicationResourceLoadSettings"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="ISyndicationResource"/> instance. This value can be <b>null</b>.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        /// <remarks>
        ///     <para>Place your custom code in the <b>LoadAsync</b> abstract method to load the syndication resource asynchronously from the specified <see cref="Uri"/> and <see cref="SyndicationResourceLoadSettings"/>.</para>
        ///     <para>
        ///         To receive notification when the operation has completed or the operation has been canceled, add an event handler to the <see cref="Loaded"/> event. 
        ///         You can cancel a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> operation by calling the <see cref="LoadAsyncCancel()"/> method.
        ///     </para>
        ///     <para>
        ///         After calling <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/>, you must wait for the load operation to complete before 
        ///         attempting to load the syndication resource using the <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, Object)"/> method.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the feed remains empty.</exception>
        /// <exception cref="InvalidOperationException">This <see cref="ISyndicationResource"/> has a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> call in progress.</exception>
        void LoadAsync(Uri source, SyndicationResourceLoadSettings settings, Object userToken);
        #endregion

        #region LoadAsync(Uri source, SyndicationResourceLoadSettings settings, ICredentials credentials, IWebProxy proxy, Object userToken)
        /// <summary>
        /// Loads the syndication resource asynchronously using the specified <see cref="Uri"/>, <see cref="SyndicationResourceLoadSettings"/>, <see cref="ICredentials"/>, and <see cref="IWebProxy"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="ISyndicationResource"/> instance. This value can be <b>null</b>.</param>
        /// <param name="credentials">
        ///     A <see cref="ICredentials"/> that provides the proper set of credentials to the <paramref name="source"/> when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="proxy">
        ///     A <see cref="IWebProxy"/> that provides proxy access to the <paramref name="source"/> when required. This value can be <b>null</b>.
        /// </param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        /// <remarks>
        ///     <para>Place your custom code in the <b>LoadAsync</b> abstract method to load the syndication resource asynchronously from the specified <see cref="Uri"/>, <see cref="SyndicationResourceLoadSettings"/>, <see cref="ICredentials"/> and <see cref="IWebProxy"/>.</para>
        ///     <para>
        ///         To receive notification when the operation has completed or the operation has been canceled, add an event handler to the <see cref="Loaded"/> event. 
        ///         You can cancel a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> operation by calling the <see cref="LoadAsyncCancel()"/> method.
        ///     </para>
        ///     <para>
        ///         After calling <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/>, 
        ///         you must wait for the load operation to complete before attempting to load the syndication resource using the <see cref="LoadAsync(Uri, Object)"/> method.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the feed remains empty.</exception>
        /// <exception cref="InvalidOperationException">This <see cref="ISyndicationResource"/> has a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> call in progress.</exception>
        void LoadAsync(Uri source, SyndicationResourceLoadSettings settings, ICredentials credentials, IWebProxy proxy, Object userToken);
        #endregion

        #region LoadAsync(Uri source, SyndicationResourceLoadSettings settings, WebRequestOptions options, Object userToken)
        /// <summary>
        /// Loads the syndication resource asynchronously using the specified <see cref="Uri"/>, <see cref="SyndicationResourceLoadSettings"/>, <see cref="ICredentials"/>, and <see cref="IWebProxy"/>.
        /// </summary>
        /// <param name="source">A <see cref="Uri"/> that represents the URL of the syndication resource XML data.</param>
        /// <param name="settings">The <see cref="SyndicationResourceLoadSettings"/> object used to configure the <see cref="ISyndicationResource"/> instance. This value can be <b>null</b>.</param>
        /// <param name="options">A <see cref="WebRequestOptions"/> that holds options that should be applied to web requests.</param>
        /// <param name="userToken">A user-defined object that is passed to the method invoked when the asynchronous operation completes.</param>
        /// <remarks>
        ///     <para>Place your custom code in the <b>LoadAsync</b> abstract method to load the syndication resource asynchronously from the specified <see cref="Uri"/>, <see cref="SyndicationResourceLoadSettings"/>, <see cref="ICredentials"/> and <see cref="IWebProxy"/>.</para>
        ///     <para>
        ///         To receive notification when the operation has completed or the operation has been canceled, add an event handler to the <see cref="Loaded"/> event. 
        ///         You can cancel a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> operation by calling the <see cref="LoadAsyncCancel()"/> method.
        ///     </para>
        ///     <para>
        ///         After calling <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/>, 
        ///         you must wait for the load operation to complete before attempting to load the syndication resource using the <see cref="LoadAsync(Uri, Object)"/> method.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="source"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="FormatException">The <paramref name="source"/> data does not conform to the expected syndication content format. In this case, the feed remains empty.</exception>
        /// <exception cref="InvalidOperationException">This <see cref="ISyndicationResource"/> has a <see cref="LoadAsync(Uri, SyndicationResourceLoadSettings, ICredentials, IWebProxy, Object)"/> call in progress.</exception>
        void LoadAsync(Uri source, SyndicationResourceLoadSettings settings, WebRequestOptions options, Object userToken);
        #endregion

        #region LoadAsyncCancel()
        /// <summary>
        /// Cancels an asynchronous operation to load the syndication resource.
        /// </summary>
        /// <remarks>
        ///     Use the LoadAsyncCancel method to cancel a pending <see cref="LoadAsync(Uri, Object)"/> operation. 
        ///     If there is a load operation in progress, this method releases resources used to execute the load operation. 
        ///     If there is no load operation pending, this method does nothing.
        /// </remarks>
        void LoadAsyncCancel();
        #endregion

        #region Save(Stream stream)
        /// <summary>
        /// Saves the syndication resource to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <b>Stream</b> to which you want to save the syndication resource.</param>
        /// <remarks>
        ///     <para>
        ///         Place your custom code in the <b>Save</b> virtual method to save the syndication resource to the specified <see cref="Stream"/>.
        ///     </para>
        ///     <para>
        ///         <b>Notes to Implementers:</b> When implementing this method, the <paramref name="stream"/> should be passed 
        ///         to the <see cref="ISyndicationResource.Save(Stream, SyndicationResourceSaveSettings)"/> method with the <item>settings</item> parameter as <b>null</b>.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication resource.</exception>
        void Save(Stream stream);
        #endregion

        #region Save(Stream stream, SyndicationResourceSaveSettings settings)
        /// <summary>
        /// Saves the syndication resource to the specified <see cref="Stream"/>.
        /// </summary>
        /// <param name="stream">The <b>Stream</b> to which you want to save the syndication resource.</param>
        /// <param name="settings">The <see cref="SyndicationResourceSaveSettings"/> object used to configure the persistance of the <see cref="ISyndicationResource"/> instance. This value can be <b>null</b>.</param>
        /// <remarks>
        ///     <para>
        ///         Place your custom code in the <b>Save</b> virtual method to save the syndication resource to the specified <see cref="Stream"/>.
        ///     </para>
        ///     <para>
        ///         <b>Notes to Implementers:</b> When implementing this method, the <paramref name="stream"/> should be used to create a <see cref="XmlWriter"/> 
        ///         that is then passed to the <see cref="ISyndicationResource.Save(XmlWriter)"/> method.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="stream"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication resource.</exception>
        void Save(Stream stream, SyndicationResourceSaveSettings settings);
        #endregion

        #region Save(XmlWriter writer)
        /// <summary>
        /// Saves the syndication resource to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to save the syndication resource.</param>
        /// <remarks>
        ///     <para>
        ///         Place your custom code in the <b>Save</b> virtual method to save the syndication resource to the specified <see cref="XmlWriter"/>.
        ///     </para>
        ///     <para>
        ///         <b>Notes to Implementers:</b> When implementing this method, a default instance the <see cref="SyndicationResourceSaveSettings"/> should be created 
        ///         and then passed to the <see cref="ISyndicationResource.Save(XmlWriter, SyndicationResourceSaveSettings)"/> method along with the supplied <paramref name="writer"/>.
        ///     </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication resource.</exception>
        void Save(XmlWriter writer);
        #endregion

        #region Save(XmlWriter writer, SyndicationResourceSaveSettings settings)
        /// <summary>
        /// Saves the syndication resource to the specified <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The <b>XmlWriter</b> to which you want to save the syndication resource.</param>
        /// <param name="settings">The <see cref="SyndicationResourceSaveSettings"/> object used to configure the persistance of the <see cref="ISyndicationResource"/> instance.</param>
        /// <remarks>
        ///     Place your custom code in the <b>Save</b> virtual method to save the syndication resource to the specified <see cref="XmlWriter"/> using the <see cref="SyndicationResourceSaveSettings"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">The <paramref name="writer"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="settings"/> is a null reference (Nothing in Visual Basic).</exception>
        /// <exception cref="XmlException">The operation would not result in well formed XML for the syndication resource.</exception>
        void Save(XmlWriter writer, SyndicationResourceSaveSettings settings);
        #endregion
    }
}
