using System.Reflection;
using Super.Model.Selection.Stores;

namespace Super.Model.Collections
{
	sealed class TypedSortOrder : DecoratedTable<TypeInfo, int>, ITypedSortOrder
	{
		public TypedSortOrder() : base(ReferenceTables<TypeInfo, int>.Default.Get(info => 1)) {}
	}
}