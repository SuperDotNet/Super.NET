using Super.Model.Selection.Stores;
using System;

namespace Super.Model.Collections
{
	sealed class TypedSortOrder : DecoratedTable<Type, int>, ITypedSortOrder
	{
		public TypedSortOrder() : base(ReferenceTables<Type, int>.Default.Get(info => 1)) {}
	}
}