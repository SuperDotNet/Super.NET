using System;
using System.Runtime.CompilerServices;
using Super.ExtensionMethods;
using Super.Reflection;

namespace Super.Model.Sources.Tables
{
	sealed class StructureValueTable<TParameter, TResult> : DecoratedTable<TParameter, TResult>
		where TParameter : class
		where TResult : struct
	{
		public StructureValueTable(Func<TParameter, TResult> parameter)
			: base(I<StructureValueTables<TParameter, TResult>>
			       .Default
			       .Get(parameter)
			       .Get(new ConditionalWeakTable<TParameter, Tuple<TResult>>())) {}
	}
}