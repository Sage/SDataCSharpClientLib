// Copyright (c) Sage (UK) Limited 2010. All rights reserved.
// This code may not be copied or used, except as set out in a written licence agreement
// between the user and Sage (UK) Limited, which specifically permits the user to use this
// code. Please contact Sage (UK) if you do not have such a licence. Sage will take
// appropriate legal action against those who make unauthorised use of this code.

using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;

namespace Sage.SData.Client.Framework
{
    /// <summary>
    /// Helper class for building a <see cref="Uri"/>.
    /// </summary>
    [Serializable]
    public class UriFormatter : ISerializable
    {
        #region Constants

        /// <summary>
        /// Returns the <see cref="string"/> used as the name of the uri property during serialization.
        /// </summary>
        public const string UriName = "uri";

        /// <summary>
        /// Returns the <see cref="string"/> used as the suffix for the scheme part of a <see cref="Uri"/>.
        /// </summary>
        /// <value>A <see cref="string"/> used as the suffix for the scheme part of a <see cref="Uri"/>.</value>
        public const string SchemeSuffix = ":";

        /// <summary>
        /// Returns the <see cref="string"/> used as the prefix for the port part of a <see cref="Uri"/>.
        /// </summary>
        /// <value>A <see cref="string"/> used as the prefix for the port part of a <see cref="Uri"/>.</value>
        public const string PortPrefix = ":";

        /// <summary>
        /// Returns the <see cref="string"/> used as the prefix for the query part of a <see cref="Uri"/>.
        /// </summary>
        /// <value>A <see cref="string"/> used as the prefix for the query part of a <see cref="Uri"/>.</value>
        public const string QueryPrefix = "?";

        /// <summary>
        /// Returns the <see cref="string"/> used as the argument separator for the query part of a <see cref="Uri"/>.
        /// </summary>
        /// <value>A <see cref="string"/> used as the argument separator for the query part of a <see cref="Uri"/>.</value>
        public const string QueryArgPrefix = "&";

        /// <summary>
        /// Returns the <see cref="string"/> used as the query argument and value separator.
        /// </summary>
        /// <value>A <see cref="string"/> used as the query argument and value separator.</value>
        public const string QueryArgValuePrefix = "=";

        /// <summary>
        /// Returns the <see cref="string"/> to use for separating the path parts of a <see cref="Uri"/>.
        /// </summary>
        /// <value>A <see cref="string"/> to use for separating the path parts of a <see cref="Uri"/>.</value>
        public const string PathSegmentPrefix = "/";

        /// <summary>
        /// Returns the <see cref="string"/> used as the fragment prefix.
        /// </summary>
        public const string FragmentPrefix = "#";

        /// <summary>
        /// Returns the Http scheme.
        /// </summary>
        /// <value>A <see cref="string"/> containing the Http scheme.</value>
        public const string Http = "http";

        /// <summary>
        /// Returns the Https scheme.
        /// </summary>
        /// <value>A <see cref="string"/> containing the Https scheme.</value>
        public const string Https = "https";

        /// <summary>
        /// Defines that a port has not been specified.
        /// </summary>
        /// <value>A value defining that a port has not been specified.</value>
        public const int UnspecifiedPort = -1;

        #endregion

        #region Fields

        /// <summary>
        /// Gets the identifier/IPAddress to use for the Local Host.
        /// </summary>
        public static readonly string LocalHost = Dns.GetHostName();

        private Uri _uri;
        private bool _requiresParseUri;
        private bool _requiresRebuildUri;

        private string _scheme;
        private int _port;
        private string _host;
        private string _pathPrefix;
        private string _server;

        private string _directPath;
        private bool _requiresParsePath;
        private bool _requiresRebuildPath;
        private List<UriPathSegment> _pathSegments;

        private IDictionary<string, string> _queryArgs;

        private string _fragment;

        #endregion

        /// <summary>
        /// Initialises a new instance of the <see cref="UriFormatter"/> class.
        /// </summary>
        public UriFormatter(SerializationInfo info, StreamingContext context)
            : this((Uri) null)
        {
            string uri = null;

            if (info.MemberCount > 0)
            {
                try
                {
                    uri = info.GetString(UriName);
                }
                catch
                {
                    // Ignore any exceptions, as the member will not be present
                }
            }

            if (uri != null)
                Uri = new Uri(uri);
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="UriFormatter"/> class.
        /// </summary>
        public UriFormatter()
            : this((Uri) null)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="UriFormatter"/> class with
        /// the specified <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> to assign.</param>
        public UriFormatter(string uri)
            : this(string.IsNullOrEmpty(uri) ? null : new Uri(uri))
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="UriFormatter"/> class with
        /// the specified <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> to assign.</param>
        public UriFormatter(Uri uri)
        {
            Uri = uri;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="UriFormatter"/> class with
        /// the specified <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> to assign.</param>
        public UriFormatter(UriFormatter uri)
        {
            _uri = uri._uri;
            _requiresParseUri = uri._requiresParseUri;
            _requiresRebuildUri = uri._requiresRebuildUri;
            _scheme = uri._scheme;
            _port = uri._port;
            _host = uri._host;
            _pathPrefix = uri._pathPrefix;
            _server = uri._server;

            PathInternal = uri.PathInternal;
            _directPath = uri._directPath;
            _requiresParsePath = uri._requiresParsePath;
            _requiresRebuildPath = uri._requiresRebuildPath;

            if (uri._pathSegments != null)
            {
                _pathSegments = new List<UriPathSegment>(uri._pathSegments.Count);

                foreach (var segment in uri._pathSegments)
                {
                    var clone = new UriPathSegment(segment) {Formatter = this};
                    _pathSegments.Add(clone);
                }
            }

            if (uri._queryArgs != null)
                _queryArgs = new Dictionary<string, string>(uri._queryArgs, StringComparer.InvariantCultureIgnoreCase);

            if (!string.IsNullOrEmpty(uri._fragment))
            {
                _fragment = uri._fragment.Substring(1);
            }
        }

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="Uri"/>.
        /// </summary>
        /// <value>The <see cref="Uri"/>.</value>
        public Uri Uri
        {
            get
            {
                CheckRebuildUri();
                return _uri;
            }
            set
            {
                _uri = value;
                _requiresParseUri = true;
                _requiresRebuildUri = false;
            }
        }

        /// <summary>
        /// Gets or sets the scheme for the <see cref="Uri"/>.
        /// </summary>
        /// <value>The scheme for the <see cref="Uri"/>.</value>
        public string Scheme
        {
            get
            {
                CheckParseUri();
                return _scheme;
            }
            set
            {
                CheckParseUri();
                _scheme = value;
                _requiresRebuildUri = true;
            }
        }

        /// <summary>
        /// Gets or sets the port for the <see cref="Uri"/>.
        /// </summary>
        /// <value>The port for the <see cref="Uri"/> if one exists; otherwise, <b>-1</b>.</value>
        public int Port
        {
            get
            {
                CheckParseUri();
                return _port;
            }
            set
            {
                CheckParseUri();
                _port = value;
                _requiresRebuildUri = true;
            }
        }

        /// <summary>
        /// Gets or sets the host for the <see cref="Uri"/>.
        /// </summary>
        /// <value>The host for the <see cref="Uri"/>.</value>
        public string Host
        {
            get
            {
                CheckParseUri();
                return _host;
            }
            set
            {
                CheckParseUri();
                _host = value;
                _requiresRebuildUri = true;
            }
        }

        /// <summary>
        /// Gets or sets the path prefix for the <see cref="Uri"/>.
        /// </summary>
        /// <remarks>Reprents the virtual root when hosting in IIS; otherwise empty.</remarks>
        public string PathPrefix
        {
            get
            {
                CheckParseUri();
                return _pathPrefix;
            }
            set
            {
                CheckParseUri();
                _pathPrefix = value;
                RequiresRebuildUri = true;
            }
        }

        /// <summary>
        /// Gets or sets the server for the <see cref="Uri"/>.
        /// </summary>
        /// <value>The server for the <see cref="Uri"/>.</value>
        public string Server
        {
            get
            {
                CheckParseUri();
                return _server;
            }
            set
            {
                CheckParseUri();
                _server = value;
                RequiresRebuildUri = true;
            }
        }

        /// <summary>
        /// Gets or sets the path of the <see cref="Uri"/>.
        /// </summary>
        /// <value>The path for the <see cref="Uri"/>.</value>
        public string Path
        {
            get
            {
                CheckParsePath();

                CheckRebuildPath();

                return PathInternal;
            }
            set
            {
                CheckParsePath();

                if (value != null && value.StartsWith(PathSegmentPrefix))
                    PathInternal = value.Substring(PathSegmentPrefix.Length);
                else
                    PathInternal = value;

                RequiresParsePath = true;
                _directPath = null;
            }
        }

        internal string PathInternal { get; set; }

        /// <summary>
        /// Returns the path of the <see cref="Uri"/> with all predicates removed.
        /// </summary>
        /// <value>The path for the <see cref="Uri"/> with all predicates removed</value>
        public string DirectPath
        {
            get
            {
                if (RequiresRebuildPath || _directPath == null)
                {
                    CheckRebuildPath();

                    var path = new StringBuilder();

                    foreach (var segment in DirectPathSegments)
                    {
                        if (segment == null)
                            continue;

                        if (path.Length > 0)
                            path.Append(PathSegmentPrefix);

                        path.Append(segment.Text);
                    }

                    _directPath = path.ToString();
                }

                return _directPath;
            }
        }

        /// <summary>
        /// Gets or sets the query for the <see cref="Uri"/>
        /// </summary>
        /// <value>The query for the query.</value>
        public string Query
        {
            get
            {
                CheckParsePath();

                return BuildQuery(QueryArgs);
            }
            set
            {
                if (value == null)
                    value = string.Empty;

                CheckParsePath();

                if (string.IsNullOrEmpty(value))
                    _queryArgs = null;
                else if (value.StartsWith(QueryPrefix))
                    _queryArgs = UriQueryParser.Parse(value.Substring(QueryPrefix.Length));
                else
                    _queryArgs = UriQueryParser.Parse(value);

                RequiresRebuildUri = true;
            }
        }

        /// <summary>
        /// Returns the query arguments for the <see cref="Uri"/>.
        /// </summary>
        /// <value>A <see cref="IDictionary{TKey, TValue}"/> containing the query arguments for the <see cref="Uri"/></value>
        public IDictionary<string, string> QueryArgs
        {
            get { return _queryArgs ?? (_queryArgs = new Dictionary<string, string>()); }
        }

        /// <summary>
        /// Gets or sets the path and query of the <see cref="Uri"/>.
        /// </summary>
        /// <value>The path and query for the <see cref="Uri"/>.</value>
        public string PathQuery
        {
            get
            {
                var path = Path;
                var query = Query;

                if (string.IsNullOrEmpty(query))
                    return path;
                return path + QueryPrefix + query;
            }
            set
            {
                var query = value.IndexOf(QueryPrefix);

                if (query < 0)
                {
                    Path = Path;
                    Query = string.Empty;
                }
                else
                {
                    Path = value.Substring(0, query);
                    Query = value.Substring(query + 1);
                }
            }
        }

        internal List<UriPathSegment> InternalPathSegments
        {
            get { return _pathSegments ?? (_pathSegments = new List<UriPathSegment>()); }
        }

        /// <summary>
        /// Returns the components that make up the path.
        /// </summary>
        /// <value>Array of components that make up the path.</value>
        public UriPathSegment[] PathSegments
        {
            get
            {
                CheckParsePath();

                return InternalPathSegments.ToArray();
            }
            set
            {
                CheckParsePath();

                PathSegmentsInternal = value;
                RequiresRebuildPath = true;
            }
        }

        /// <summary>
        /// Returns the components that make up the direct path.
        /// </summary>
        /// <value>Array of components that make up the direct path.</value>
        protected virtual UriPathSegment[] DirectPathSegments
        {
            get { return PathSegments; }
        }

        internal UriPathSegment[] PathSegmentsInternal
        {
            get { return _pathSegments.ToArray(); }
            set
            {
                var segments = InternalPathSegments;

                segments.Clear();

                if (value != null)
                {
                    segments.AddRange(value);
                    LinkSegment(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a flag indicating if the <see cref="Uri"/> uses SSL.
        /// </summary>
        /// <value><b>true</b> if the <see cref="Uri"/> uses SSL; otherwise, <b>false</b>.</value>
        public bool UseSsl
        {
            get
            {
                CheckParseUri();
                return string.Compare(_scheme, Https, StringComparison.InvariantCultureIgnoreCase) == 0;
            }
            set
            {
                CheckParseUri();

                _scheme = value ? Https : Http;
                RequiresRebuildUri = true;
            }
        }

        /// <summary>
        /// Returns a flag indicating if the <see cref="Uri"/> is empty.
        /// </summary>
        /// <value><b>true</b> if the <see cref="Uri"/> is empty; otherwiser, <b>false</b>.</value>
        public bool IsEmpty
        {
            get { return _uri == null; }
        }

        /// <summary>
        /// Gets or sets a query argument.
        /// </summary>
        /// <param name="name">The name of the argument.</param>
        /// <param name="value">The value of the argument.</param>
        public string this[string name]
        {
            get
            {
                CheckParseUri();

                string value;

                if (QueryArgs.TryGetValue(name, out value))
                    return value;
                return null;
            }
            set
            {
                CheckParseUri();

                if (value == null)
                {
                    if (_queryArgs != null && _queryArgs.ContainsKey(name))
                        _queryArgs.Remove(name);
                }
                else
                {
                    QueryArgs[name] = value;
                }

                RequiresRebuildUri = true;
            }
        }

        public string Fragment
        {
            get
            {
                CheckParseUri();
                return _fragment;
            }
            set
            {
                CheckParseUri();
                _fragment = value;
                _requiresRebuildUri = true;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="Path"/> needs to be rebuilt.
        /// </summary>
        /// <value><b>true</b> if the <see cref="Path"/> needs to be rebuilt; otherwise, <b>false</b>.</value>
        internal bool RequiresRebuildPath
        {
            get { return _requiresRebuildPath; }
            set
            {
                _requiresRebuildPath = value;
                if (value) RequiresRebuildUri = true;
                OnRequiresRebuildPathChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="Path"/> needs to be parsed.
        /// </summary>
        /// <value><b>true</b> if the <see cref="Path"/> needs to be parsed; otherwise, <b>false</b>.</value>
        internal bool RequiresParsePath
        {
            get { return _requiresParsePath; }
            set
            {
                _requiresParsePath = value;
                OnRequiresParsePathChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="Uri"/> needs to be rebuilt.
        /// </summary>
        /// <value><b>true</b> if the <see cref="Uri"/> needs to be rebuilt; otherwise, <b>false</b>.</value>
        internal bool RequiresRebuildUri
        {
            get { return _requiresRebuildUri; }
            set
            {
                _requiresRebuildUri = value;
                OnRequiresRebuildUriChanged();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating if the <see cref="Uri"/> needs to be parsed.
        /// </summary>
        /// <value><b>true</b> if the <see cref="Uri"/> needs to be parsed; otherwise, <b>false</b>.</value>
        internal bool RequiresParseUri
        {
            get { return _requiresParseUri; }
            set
            {
                _requiresParseUri = value;
                OnRequiresParseUriChanged();
            }
        }

        /// <summary>
        /// Called when the value of the <see cref="RequiresParsePath"/> value changes.
        /// </summary>
        internal virtual void OnRequiresParsePathChanged()
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="RequiresRebuildPath"/> value changes.
        /// </summary>
        internal virtual void OnRequiresRebuildPathChanged()
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="RequiresParseUri"/> value changes.
        /// </summary>
        internal virtual void OnRequiresParseUriChanged()
        {
        }

        /// <summary>
        /// Called when the value of the <see cref="RequiresRebuildUri"/> value changes.
        /// </summary>
        internal virtual void OnRequiresRebuildUriChanged()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resets the <see cref="Uri"/> back to an empty <see cref="Uri"/>.
        /// </summary>
        public UriFormatter Empty()
        {
            Uri = null;
            return this;
        }

        /// <summary>
        /// Adds a query argument to a URI.
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> to add the parameter to.</param>
        /// <param name="name">The name of the query argument.</param>
        /// <param name="value">The value of the query argument.</param>
        /// <returns>The specified <see cref="Uri"/> with the additional query argument.</returns>
        public static string AppendQueryArgument(string uri, string name, string value)
        {
            var formatted = name + QueryArgValuePrefix + value;

            if (uri.Contains(QueryPrefix))
                return uri + QueryArgPrefix + formatted;
            return uri + QueryPrefix + formatted;
        }

        /// <summary>
        /// Adds the specified path segments to the <see cref="Uri"/>.
        /// </summary>
        /// <param name="segments">The path segments segments to add to the <see cref="Uri"/>.</param>
        public UriFormatter AppendPath(params string[] segments)
        {
            return AppendPath(UriPathSegment.FromStrings(segments));
        }

        /// <summary>
        /// Adds the specified paths to the <see cref="Uri"/>.
        /// </summary>
        /// <param name="segment">The path segment to add to the <see cref="Uri"/>.</param>
        public UriFormatter AppendPath(UriPathSegment segment)
        {
            CheckParsePath();

            InternalPathSegments.Add(segment);

            LinkSegment(segment);

            RequiresRebuildPath = true;

            return this;
        }

        /// <summary>
        /// Adds the specified paths to the <see cref="Uri"/>.
        /// </summary>
        /// <param name="segments">The path segments to add to the <see cref="Uri"/>.</param>
        public UriFormatter AppendPath(IEnumerable<UriPathSegment> segments)
        {
            CheckParsePath();
            InternalPathSegments.AddRange(segments);

            LinkSegment(segments);

            RequiresRebuildPath = true;

            return this;
        }

        /// <summary>
        /// Sets the path for the <see cref="Uri"/>.
        /// </summary>
        /// <param name="segments">The path segments for the <see cref="Uri"/>.</param>
        public UriFormatter SetPath(params string[] segments)
        {
            return SetPath(UriPathSegment.FromStrings(segments));
        }

        /// <summary>
        /// Sets the path for the <see cref="Uri"/>.
        /// </summary>
        /// <param name="segments">The path segments for the <see cref="Uri"/>.</param>
        public UriFormatter SetPath(IEnumerable<UriPathSegment> segments)
        {
            CheckParseUri();

            var pathSegments = InternalPathSegments;

            pathSegments.Clear();
            pathSegments.AddRange(segments);

            LinkSegment(segments);

            RequiresParsePath = false;
            RequiresRebuildPath = true;

            return this;
        }

        /// <summary>
        /// Removes first path segment of the <see cref="Uri"/>.
        /// </summary>
        public void TrimStart()
        {
            if (PathSegments.Length > 0)
            {
                _pathSegments.RemoveAt(0);
                RequiresRebuildPath = true;
            }
        }

        /// <summary>
        /// Removes last path segment of the <see cref="Uri"/>.
        /// </summary>
        public void TrimEnd()
        {
            if (PathSegments.Length > 0)
            {
                _pathSegments.RemoveAt(_pathSegments.Count - 1);
                RequiresRebuildPath = true;
            }
        }

        /// <summary>
        /// Removes a range of path segments from the <see cref="Uri"/>.
        /// </summary>
        ///<param name="iPathSegmentIndex">The zero-based starting index of the range of path segments to remove</param>
        ///<param name="iPathSegmentCount">The number of path segments to remove from the end of the <see cref="Uri"/>.</param>
        public void TrimRange(int iPathSegmentIndex, int iPathSegmentCount)
        {
            if (PathSegments.Length >= (iPathSegmentIndex + iPathSegmentCount))
            {
                _pathSegments.RemoveRange(iPathSegmentIndex, iPathSegmentCount);
                RequiresRebuildPath = true;
            }
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Returns a <see cref="String"/> representing the <see cref="Uri"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Uri != null ? Uri.ToString() : base.ToString();
        }

        /// <summary>
        /// Returns the haskcode for the <see cref="UriPathSegment"/>.
        /// </summary>
        /// <returns>The hashcode for the <see cref="UriPathSegment"/>.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Compares the specified <see cref="object"/> with this <see cref="UriPathSegment"/>.
        /// </summary>
        /// <param name="obj">The <see cref="Object"/> to compare.</param>
        /// <returns><b>true</b> if <paramref name="obj"/> match this <see cref="UriPathSegment"/>; otherwise, <b>false</b>.</returns>
        public override bool Equals(object obj)
        {
            return obj.ToString() == ToString();
        }

        #endregion

        #region Local Methods

        private void LinkSegment(IEnumerable<UriPathSegment> segments)
        {
            foreach (var segment in segments)
            {
                if (segment != null)
                    segment.Formatter = this;
            }
        }

        private void LinkSegment(UriPathSegment segment)
        {
            if (segment != null)
                segment.Formatter = this;
        }

        /// <summary>
        /// Checks if the <see cref="Uri"/> needs rebuilding.
        /// </summary>
        protected void CheckRebuildUri()
        {
            if (!RequiresRebuildPath && !RequiresRebuildUri)
                return;

            RequiresRebuildUri = false;

            OnBuildUri();
        }

        /// <summary>
        /// Called when the <see cref="Uri"/> needs to be rebuilt.
        /// </summary>
        protected virtual void OnBuildUri()
        {
            // http
            var uri = new StringBuilder(string.IsNullOrEmpty(_scheme) ? Http : _scheme);

            // http:
            uri.Append(SchemeSuffix);

            // http://
            uri.Append(PathSegmentPrefix);
            uri.Append(PathSegmentPrefix);

            // http://host
            uri.Append(string.IsNullOrEmpty(_host) ? LocalHost : _host);

            // http://<host><:port>
            if (_port != UnspecifiedPort)
            {
                uri.Append(PortPrefix);
                uri.Append(_port.ToString());
            }

            // http://host<:port>/serverPrefix
            if (!string.IsNullOrEmpty(_pathPrefix))
            {
                if (!_pathPrefix.StartsWith(PathSegmentPrefix))
                    uri.Append(PathSegmentPrefix);

                uri.Append(_pathPrefix);
            }

            // http://host<:port>/serverPrefix/server
            if (!string.IsNullOrEmpty(_server))
            {
                if (!_server.StartsWith(PathSegmentPrefix))
                    uri.Append(PathSegmentPrefix);

                uri.Append(_server);
            }

            // http://<host><:port>/<path>
            CheckRebuildPath();

            if (!string.IsNullOrEmpty(PathInternal))
            {
                if (!PathInternal.StartsWith(PathSegmentPrefix))
                    uri.Append(PathSegmentPrefix);

                uri.Append(PathInternal);
            }

            // http://<host><:port>/<path><?query>
            var query = Query;

            if (!string.IsNullOrEmpty(query))
            {
                uri.Append(QueryPrefix);
                uri.Append(query);
            }

            if (!string.IsNullOrEmpty(_fragment))
            {
                uri.Append(FragmentPrefix);
                uri.Append(_fragment);
            }

            _uri = new Uri(uri.ToString());
        }

        /// <summary>
        /// Checks if the <see cref="Uri"/> needs parsing.
        /// </summary>
        protected void CheckParseUri()
        {
            if (!RequiresParseUri)
                return;

            RequiresParseUri = false;

            OnParseUri();
        }

        /// <summary>
        /// Called when the <see cref="Uri"/> needs to be parsed.
        /// </summary>
        protected virtual void OnParseUri()
        {
            RequiresParsePath = true;
            _directPath = null;

            if (_uri == null)
            {
                _port = UnspecifiedPort;
                _scheme = Http;
                _host = string.Empty;
                _pathPrefix = string.Empty;
                _server = string.Empty;
                PathInternal = string.Empty;
                _queryArgs = null;
                _fragment = string.Empty;
            }
            else
            {
                _scheme = _uri.Scheme;
                _port = _uri.Port;
                _host = _uri.Host;

                var path = Uri.UnescapeDataString(_uri.AbsolutePath);

                if (path.StartsWith(PathSegmentPrefix))
                    path = path.Substring(PathSegmentPrefix.Length);

                var endServer = path.IndexOf(PathSegmentPrefix[0]);

                if (endServer < 0)
                {
                    _server = path;
                    PathInternal = string.Empty;
                }
                else
                {
                    _server = path.Substring(0, endServer);
                    PathInternal = path.Substring(endServer + 1);
                }

                if (PathInternal != null && PathInternal.StartsWith(PathSegmentPrefix))
                    PathInternal = PathInternal.Substring(PathSegmentPrefix.Length);

                if (_uri.Query != null && _uri.Query.StartsWith(QueryPrefix))
                    _queryArgs = UriQueryParser.Parse(_uri.Query.Substring(QueryPrefix.Length));
                else
                    _queryArgs = UriQueryParser.Parse(_uri.Query);

                if (!string.IsNullOrEmpty(_uri.Fragment))
                {
                    _fragment = _uri.Fragment.Substring(1);
                }
            }
        }

        /// <summary>
        /// Builds a query <see cref="String"/> from the specified <see cref="IDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="args">The arguments to build the query string from.</param>
        /// <returns>A <see cref="String"/> containing the query.</returns>
        public static string BuildQuery(IEnumerable<KeyValuePair<string, string>> args)
        {
            var query = new StringBuilder();

            foreach (var pair in args)
            {
                if (query.Length > 0)
                    query.Append(QueryArgPrefix);

                query.Append(pair.Key);
                query.Append(QueryArgValuePrefix);
                query.Append(pair.Value);
            }

            return query.ToString();
        }

        /// <summary>
        /// Returns the segment at the specified index.
        /// </summary>
        /// <param name="index">The index of the segment.</param>
        /// <returns>The segment at the specified index.</returns>
        /// <remarks>If the <paramref name="index"/> is past the end of the current array the length is increased.</remarks>
        public UriPathSegment GetPathSegment(int index)
        {
            var segments = PathSegments;

            if (segments.Length < index + 1)
            {
                var copySegments = new UriPathSegment[index + 1];
                Array.Copy(segments, copySegments, segments.Length);
                segments = copySegments;
            }

            if (segments[index] == null)
            {
                segments[index] = new UriPathSegment();
                LinkSegment(segments[index]);
                PathSegments = segments;
            }

            return segments[index];
        }

        /// <summary>
        /// Checks if the path part of the <see cref="Uri"/> needs rebuilding.
        /// </summary>
        protected void CheckRebuildPath()
        {
            if (!RequiresRebuildPath)
                return;

            RequiresRebuildPath = false;

            OnBuildPath();
        }

        /// <summary>
        /// Called when the <see cref="Path"/> needs to be rebuilt.
        /// </summary>
        protected virtual void OnBuildPath()
        {
            var path = new StringBuilder();

            foreach (var segment in _pathSegments)
            {
                if (segment != null)
                    UriPathSegment.AppendPath(path, segment.Segment);
            }

            PathInternal = path.ToString();
        }

        /// <summary>
        /// Checks if the path part of the <see cref="Uri"/> needs parsing.
        /// </summary>
        protected void CheckParsePath()
        {
            CheckParseUri();

            if (!RequiresParsePath)
                return;

            RequiresParsePath = false;

            OnParsePath();
        }

        /// <summary>
        /// Called when the <see cref="Path"/> needs to be parsed.
        /// </summary>
        protected virtual void OnParsePath()
        {
            if (!string.IsNullOrEmpty(PathInternal))
            {
                var segments = InternalPathSegments;
                segments.Clear();

                foreach (var segment in UriPathSegment.FromStrings(UriPathSegment.GetPathSegments(PathInternal)))
                {
                    if (segment != null)
                    {
                        LinkSegment(segment);
                        segments.Add(segment);
                    }
                }
            }
        }

        #endregion

        #region ISerializable Members

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var uri = Uri;

            if (uri != null)
                info.AddValue(UriName, uri.ToString());
        }

        #endregion
    }
}