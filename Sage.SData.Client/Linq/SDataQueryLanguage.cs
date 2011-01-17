using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using IQToolkit.Data.Common;

namespace Sage.SData.Client.Linq
{
    public class SDataQueryLanguage : QueryLanguage
    {
        private static SDataQueryLanguage _default;

        public static SDataQueryLanguage Default
        {
            get
            {
                if (_default == null)
                {
                    Interlocked.CompareExchange(ref _default, new SDataQueryLanguage(), null);
                }
                return _default;
            }
        }

        private readonly QueryTypeSystem _typeSystem = new SDataTypeSystem();

        #region QueryLanguage Members

        public override QueryTypeSystem TypeSystem
        {
            get { return _typeSystem; }
        }

        public override QueryLinguist CreateLinguist(QueryTranslator translator)
        {
            return new SDataQueryLinguist(this, translator);
        }

        public override Expression GetGeneratedIdExpression(MemberInfo member)
        {
            throw new NotImplementedException();
        }

        #endregion

        private class SDataQueryLinguist : QueryLinguist
        {
            public SDataQueryLinguist(QueryLanguage language, QueryTranslator translator)
                : base(language, translator)
            {
            }

            public override string Format(Expression expression)
            {
                return SDataUriFormatter.Format(expression);
            }
        }
    }
}