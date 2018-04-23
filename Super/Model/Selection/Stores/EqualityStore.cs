using System;
using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public class EqualityStore<TParameter, TResult> : DecoratedSelect<TParameter, TResult>
	{
		protected EqualityStore(ISelect<TParameter, TResult> source, IDictionary<TParameter, TResult> store)
			: this(source.Get, store) {}

		protected EqualityStore(Func<TParameter, TResult> source, IDictionary<TParameter, TResult> store)
			: base(new StandardTables<TParameter, TResult>(source).Get(store)) {}
	}
}