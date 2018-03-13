using System.Reflection;
using Super.Model.Sources.Tables;

namespace Super.Model.Collections
{
	sealed class TypedSortOrder : DecoratedTable<TypeInfo, int>, ITypedSortOrder
	{
		public TypedSortOrder() : base(ReferenceTables<TypeInfo, int>.Default.Get(info => 1)) {}
	}
}