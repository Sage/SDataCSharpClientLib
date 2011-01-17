using System;
using System.Collections.Generic;
using IQToolkit.Data.Common;
using Sage.SData.Client.Extensions;

namespace Sage.SData.Client.Linq
{
    public class SDataFieldReader : FieldReader
    {
        private readonly IList<string> _properties;
        private readonly SDataPayload _payload;

        public SDataFieldReader(IList<string> properties, SDataPayload payload)
        {
            _properties = properties;
            _payload = payload;
            Init();
        }

        protected override int FieldCount
        {
            get { return _properties.Count; }
        }

        protected override byte GetByte(int ordinal)
        {
            return GetValue<byte>(ordinal);
        }

        protected override char GetChar(int ordinal)
        {
            return GetValue<char>(ordinal);
        }

        protected override DateTime GetDateTime(int ordinal)
        {
            return GetValue<DateTime>(ordinal);
        }

        protected override decimal GetDecimal(int ordinal)
        {
            return GetValue<decimal>(ordinal);
        }

        protected override double GetDouble(int ordinal)
        {
            return GetValue<double>(ordinal);
        }

        protected override Guid GetGuid(int ordinal)
        {
            return GetValue<Guid>(ordinal);
        }

        protected override short GetInt16(int ordinal)
        {
            return GetValue<short>(ordinal);
        }

        protected override int GetInt32(int ordinal)
        {
            return GetValue<int>(ordinal);
        }

        protected override long GetInt64(int ordinal)
        {
            return GetValue<long>(ordinal);
        }

        protected override float GetSingle(int ordinal)
        {
            return GetValue<float>(ordinal);
        }

        protected override string GetString(int ordinal)
        {
            return GetValue<string>(ordinal);
        }

        protected override Type GetFieldType(int ordinal)
        {
            var value = GetValue<object>(ordinal);
            return value != null ? value.GetType() : typeof (object);
        }

        protected override bool IsDBNull(int ordinal)
        {
            var value = GetValue<object>(ordinal);
            return value == null;
        }

        protected override T GetValue<T>(int ordinal)
        {
            var name = _properties[ordinal];
            object value;

            if (!_payload.Values.TryGetValue(name, out value) && name == "Id")
            {
                value = _payload.Key;
            }

            return (T) Convert.ChangeType(value, typeof (T));
        }
    }
}