using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Super.ExtensionMethods;

namespace Super.Model.Selection.Stores
{
	public class EqualityStore<TParameter, TResult> : Decorated<TParameter, TResult>
	{
		protected EqualityStore(ISelect<TParameter, TResult> @select) : this(@select.ToDelegate()) {}

		protected EqualityStore(Func<TParameter, TResult> source) : this(source, EqualityComparer<TParameter>.Default) {}

		protected EqualityStore(Func<TParameter, TResult> source, IEqualityComparer<TParameter> comparer)
			: this(source, new ConcurrentDictionary<TParameter, TResult>(comparer)) {}

		protected EqualityStore(Func<TParameter, TResult> source, IDictionary<TParameter, TResult> store)
			: base(new StandardTables<TParameter, TResult>(source).Get(store)) {}
	}
}