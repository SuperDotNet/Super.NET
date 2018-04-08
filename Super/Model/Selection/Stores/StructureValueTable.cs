using System;
using System.Runtime.CompilerServices;
using Super.Reflection;
using Super.Runtime.Activation;

namespace Super.Model.Selection.Stores
{
	sealed class StructureValueTable<TParameter, TResult> : DecoratedTable<TParameter, TResult>
		where TParameter : class
		where TResult : struct
	{
		public StructureValueTable(Func<TParameter, TResult> parameter)
			: base(parameter.To(I<StructureValueTables<TParameter, TResult>>.Default)
			                .Get(new ConditionalWeakTable<TParameter, Tuple<TResult>>())) {}
	}
}