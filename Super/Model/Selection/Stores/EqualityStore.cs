using System;
using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public class EqualityStore<TIn, TOut> : DecoratedSelect<TIn, TOut>
	{
		protected EqualityStore(ISelect<TIn, TOut> source, IDictionary<TIn, TOut> store)
			: this(source.Get, store) {}

		protected EqualityStore(Func<TIn, TOut> source, IDictionary<TIn, TOut> store)
			: base(new StandardTables<TIn, TOut>(source).Get(store)) {}
	}
}