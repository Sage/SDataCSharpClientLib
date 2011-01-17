using System.Reflection;
using System.Threading;
using IQToolkit.Data.Common;
using IQToolkit.Data.Mapping;

namespace Sage.SData.Client.Linq
{
    public class SDataImplicitMapping : ImplicitMapping
    {
        private static SDataImplicitMapping _default;

        public static SDataImplicitMapping Default
        {
            get
            {
                if (_default == null)
                {
                    Interlocked.CompareExchange(ref _default, new SDataImplicitMapping(), null);
                }
                return _default;
            }
        }

        public override bool IsPrimaryKey(MappingEntity entity, MemberInfo member)
        {
            return member.Name == "Id";
        }
    }
}