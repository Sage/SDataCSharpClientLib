using System;
using IQToolkit;
using IQToolkit.Data.Common;

namespace Sage.SData.Client.Linq
{
    public class SDataTypeSystem : QueryTypeSystem
    {
        #region QueryTypeSystem Members

        public override QueryType GetColumnType(Type type)
        {
            var isNotNull = type.IsValueType && !TypeHelper.IsNullableType(type);
            type = TypeHelper.GetNonNullableType(type);

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return new SDataQueryType(isNotNull, 0, 0, 0);

                case TypeCode.Char:
                    return new SDataQueryType(isNotNull, 1, 0, 0);

                case TypeCode.SByte:
                case TypeCode.Byte:
                    return new SDataQueryType(isNotNull, 0, 0, 0);

                case TypeCode.Int16:
                case TypeCode.UInt16:
                    return new SDataQueryType(isNotNull, 0, 0, 0);

                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return new SDataQueryType(isNotNull, 0, 0, 0);

                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return new SDataQueryType(isNotNull, 0, 0, 0);

                case TypeCode.Single:
                case TypeCode.Double:
                    return new SDataQueryType(isNotNull, 0, 0, 0);

                case TypeCode.Decimal:
                    return new SDataQueryType(isNotNull, 0, 29, 4);

                case TypeCode.DateTime:
                    return new SDataQueryType(isNotNull, 0, 0, 0);

                case TypeCode.String:
                    return new SDataQueryType(isNotNull, int.MaxValue, 0, 0);
            }

            if (type == typeof (byte[]))
            {
                return new SDataQueryType(isNotNull, int.MaxValue, 0, 0);
            }

            if (type == typeof (Guid))
            {
                return new SDataQueryType(isNotNull, 0, 0, 0);
            }

            if (type == typeof (DateTimeOffset))
            {
                return new SDataQueryType(isNotNull, 0, 0, 0);
            }

            if (type == typeof (TimeSpan))
            {
                return new SDataQueryType(isNotNull, 0, 0, 0);
            }

            return null;
        }

        public override string GetVariableDeclaration(QueryType type, bool suppressSize)
        {
            throw new NotImplementedException();
        }

        public override QueryType Parse(string typeDeclaration)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}