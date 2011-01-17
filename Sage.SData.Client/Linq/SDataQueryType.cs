using IQToolkit.Data.Common;

namespace Sage.SData.Client.Linq
{
    public class SDataQueryType : QueryType
    {
        private readonly int _length;
        private readonly bool _notNull;
        private readonly short _precision;
        private readonly short _scale;

        public SDataQueryType(bool notNull, int length, short precision, short scale)
        {
            _notNull = notNull;
            _length = length;
            _precision = precision;
            _scale = scale;
        }

        public override int Length
        {
            get { return _length; }
        }

        public override bool NotNull
        {
            get { return _notNull; }
        }

        public override short Precision
        {
            get { return _precision; }
        }

        public override short Scale
        {
            get { return _scale; }
        }
    }
}