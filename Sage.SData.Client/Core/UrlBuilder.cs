using System;
using System.Collections.Generic;
using System.Linq;

namespace Sage.SData.Client.Core
{
    public class UrlBuilder
    {
        private readonly UriBuilder _builder;
        private List<string> _pathSegments;
        private Dictionary<string, string> _queryParameters;

        public UrlBuilder()
        {
            _builder = new UriBuilder();
        }

        public UrlBuilder(string uri)
        {
            _builder = new UriBuilder(uri);
        }

        public UrlBuilder(Uri uri)
        {
            _builder = new UriBuilder(uri);
        }

        public UrlBuilder(string scheme, string host)
        {
            _builder = new UriBuilder(scheme, host);
        }

        public UrlBuilder(string scheme, string host, int port)
        {
            _builder = new UriBuilder(scheme, host, port);
        }

        public string Scheme
        {
            get { return _builder.Scheme; }
            set { _builder.Scheme = value; }
        }

        public string Host
        {
            get { return _builder.Host; }
            set { _builder.Host = value; }
        }

        public int Port
        {
            get { return _builder.Port; }
            set { _builder.Port = value; }
        }

        public string UserName
        {
            get { return _builder.UserName; }
            set { _builder.UserName = value; }
        }

        public string Password
        {
            get { return _builder.Password; }
            set { _builder.Password = value; }
        }

        public string Path
        {
            get
            {
                BuildPath();
                return _builder.Path;
            }
            set
            {
                _pathSegments = null;
                _builder.Path = value;
            }
        }

        public IList<string> PathSegments
        {
            get
            {
                ParsePath();
                return _pathSegments;
            }
        }

        private void BuildPath()
        {
            if (_pathSegments != null)
            {
                _builder.Path = string.Join("/", _pathSegments.Select(segment => Uri.EscapeUriString(segment).Replace("+", "%20")).ToArray());
            }
        }

        private void ParsePath()
        {
            if (_pathSegments == null)
            {
                var path = _builder.Path;

                if (path.StartsWith("/"))
                {
                    path = path.Substring(1);
                }

                _pathSegments = new List<string>(
                    !string.IsNullOrEmpty(path)
                        ? path.Split('/').Select(part => Uri.UnescapeDataString(part))
                        : new string[0]);
            }
        }

        public string Query
        {
            get
            {
                BuildQuery();
                return _builder.Query;
            }
            set
            {
                _queryParameters = null;
                _builder.Query = value;
            }
        }

        public IDictionary<string, string> QueryParameters
        {
            get
            {
                ParseQuery();
                return _queryParameters;
            }
        }

        private void BuildQuery()
        {
            if (_queryParameters != null)
            {
                _builder.Query = string.Join("&", _queryParameters
                                                      .Select(param => BuildQueryParameter(param))
                                                      .ToArray());
            }
        }

        private static string BuildQueryParameter(KeyValuePair<string, string> param)
        {
            var key = Uri.EscapeUriString(param.Key).Replace("+", "%20");
            return param.Value != null
                       ? string.Format("{0}={1}", key, Uri.EscapeUriString(param.Value).Replace("+", "%20"))
                       : key;
        }

        private void ParseQuery()
        {
            if (_queryParameters == null)
            {
                var query = _builder.Query;

                if (query.StartsWith("?"))
                {
                    query = query.Substring(1);
                }

                _queryParameters = !string.IsNullOrEmpty(query)
                                       ? query.Split(new[] {'&'})
                                             .Select(arg => ParseQueryParameter(arg))
                                             .ToDictionary(pair => pair.Key, pair => pair.Value)
                                       : new Dictionary<string, string>();
            }
        }

        private static KeyValuePair<string, string> ParseQueryParameter(string param)
        {
            var pos = param.IndexOf('=');
            string key, value;

            if (pos >= 0)
            {
                key = param.Substring(0, pos);
                value = param.Substring(pos + 1);
            }
            else
            {
                key = param;
                value = string.Empty;
            }

            return new KeyValuePair<string, string>(Uri.UnescapeDataString(key), Uri.UnescapeDataString(value));
        }

        public string Fragment
        {
            get { return _builder.Fragment; }
            set { _builder.Fragment = value; }
        }

        public Uri Uri
        {
            get
            {
                BuildPath();
                BuildQuery();
                return _builder.Uri;
            }
        }

        public override bool Equals(object rparam)
        {
            BuildPath();
            BuildQuery();
            return _builder.Equals(rparam);
        }

        public override int GetHashCode()
        {
            BuildPath();
            BuildQuery();
            return _builder.GetHashCode();
        }

        public override string ToString()
        {
            return Uri.ToString();
        }
    }
}