using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Selection.Stores
{
	public class ReferenceValueTable<TParameter, TResult> : DecoratedTable<TParameter, TResult>
		where TParameter : class
		where TResult : class
	{
		public ReferenceValueTable() : this(_ => default) {}

		public ReferenceValueTable(Func<TParameter, TResult> parameter)
			: base(ReferenceValueTables<TParameter, TResult>.Defaults
			                                                .Get(parameter)
			                                                .Get(new ConditionalWeakTable<TParameter, TResult>())) {}
	}
}