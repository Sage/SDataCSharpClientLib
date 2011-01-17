using System;
using System.Collections.Generic;
using System.Linq;
using IQToolkit.Data;
using IQToolkit.Data.Common;
using Sage.SData.Client.Atom;
using Sage.SData.Client.Core;
using Sage.SData.Client.Extensions;
using Sage.SData.Client.Framework;

namespace Sage.SData.Client.Linq
{
    public class SDataQueryProvider : EntityProvider
    {
        private readonly ISDataService _service;

        public SDataQueryProvider(ISDataService service)
            : base(SDataQueryLanguage.Default, SDataImplicitMapping.Default, QueryPolicy.Default)
        {
            _service = service;
        }

        #region EntityProvider Members

        protected override QueryExecutor CreateExecutor()
        {
            return new Executor(this);
        }

        public override void DoConnected(Action action)
        {
            throw new NotImplementedException();
        }

        public override void DoTransacted(Action action)
        {
            throw new NotImplementedException();
        }

        public override int ExecuteCommand(string commandText)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Nested type: Executor

        private class Executor : QueryExecutor
        {
            private readonly SDataQueryProvider _provider;

            public Executor(SDataQueryProvider provider)
            {
                _provider = provider;
            }

            #region QueryExecutor Members

            public override int RowsAffected
            {
                get { throw new NotImplementedException(); }
            }

            public override object Convert(object value, Type type)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<T> Execute<T>(QueryCommand command, Func<FieldReader, T> fnProjector, MappingEntity entity, object[] paramValues)
            {
                var pos = command.CommandText.IndexOf(' ');

                if (pos < 0)
                {
                    throw new InvalidOperationException();
                }

                var verb = command.CommandText.Substring(0, pos);
                var relative = command.CommandText.Substring(pos + 1);
                var slashEnd = _provider._service.Url.EndsWith("/");
                var slashStart = relative.StartsWith("/");

                if (slashEnd == slashStart)
                {
                    relative = slashStart ? relative.Substring(1) : "/" + relative;
                }

                var method = (HttpMethod) Enum.Parse(typeof (HttpMethod), verb, true);

                if (method != HttpMethod.Get)
                {
                    throw new InvalidOperationException();
                }

                var url = _provider._service.Url + relative;
                var properties = new SDataUri(url).Select.Split(',');
                var result = _provider._service.Read(url);

                if (result is AtomFeed)
                {
                    return Project((AtomFeed) result, properties, fnProjector).ToList();
                }

                if (result is AtomEntry)
                {
                    return new[] {Project((AtomEntry) result, properties, fnProjector)};
                }

                throw new InvalidOperationException();
            }

            public override IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize, bool stream)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<T> ExecuteBatch<T>(QueryCommand query, IEnumerable<object[]> paramSets, Func<FieldReader, T> fnProjector, MappingEntity entity, int batchSize, bool stream)
            {
                throw new NotImplementedException();
            }

            public override int ExecuteCommand(QueryCommand query, object[] paramValues)
            {
                var pos = query.CommandText.IndexOf(' ');

                if (pos < 0)
                {
                    throw new InvalidOperationException();
                }

                var verb = query.CommandText.Substring(0, pos);
                var relative = query.CommandText.Substring(pos + 1);
                var slashEnd = _provider._service.Url.EndsWith("/");
                var slashStart = relative.StartsWith("/");

                if (slashEnd == slashStart)
                {
                    relative = slashStart ? relative.Substring(1) : "/" + relative;
                }

                var method = (HttpMethod) Enum.Parse(typeof (HttpMethod), verb, true);
                var url = _provider._service.Url + relative;
                var payload = new SDataPayload {ResourceName = "Account", Namespace = "http://schemas.sage.com/dynamic/2007"}; //TODO

                for (var i = 0; i < query.Parameters.Count; i++)
                {
                    var param = query.Parameters[i];

                    if (param.Name == "Id") continue;

                    var value = paramValues[i];
                    payload.Values[param.Name] = value;
                }

                var entry = new AtomEntry();
                entry.SetSDataPayload(payload);

                switch (method)
                {
                    case HttpMethod.Post:
                        _provider._service.CreateEntry(url, entry);
                        break;
                    case HttpMethod.Put:
                        _provider._service.UpdateEntry(url, entry);
                        break;
                    case HttpMethod.Delete:
                        _provider._service.DeleteEntry(url, entry);
                        break;
                    default:
                        throw new InvalidOperationException();
                }

                return 1;
            }

            public override IEnumerable<T> ExecuteDeferred<T>(QueryCommand query, Func<FieldReader, T> fnProjector, MappingEntity entity, object[] paramValues)
            {
                throw new NotImplementedException();
            }

            #endregion

            private static IEnumerable<T> Project<T>(AtomFeed feed, IList<string> properties, Func<FieldReader, T> fnProjector)
            {
                return feed.Entries.Select(entry => Project(entry, properties, fnProjector));
            }

            private static T Project<T>(AtomEntry entry, IList<string> properties, Func<FieldReader, T> fnProjector)
            {
                var freader = new SDataFieldReader(properties, entry.GetSDataPayload());
                return fnProjector(freader);
            }
        }

        #endregion
    }
}