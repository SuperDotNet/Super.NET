using Super.Reflection;
using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Selection.Stores
{
	sealed class StructureValueTable<TIn, TOut> : DecoratedTable<TIn, TOut>
		where TIn : class
		where TOut : struct
	{
		public StructureValueTable(Func<TIn, TOut> parameter)
			: base(parameter.To(I<StructureValueTables<TIn, TOut>>.Default)
			                .Get(new ConditionalWeakTable<TIn, Tuple<TOut>>())) {}
	}
}