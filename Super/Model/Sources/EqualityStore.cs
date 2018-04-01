using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Super.Model.Sources.Tables;

namespace Super.Model.Sources
{
	public class EqualityStore<TParameter, TResult> : DecoratedSource<TParameter, TResult>
	{
		protected EqualityStore(Func<TParameter, TResult> source) : this(source, EqualityComparer<TParameter>.Default) {}

		protected EqualityStore(Func<TParameter, TResult> source, IEqualityComparer<TParameter> comparer)
			: this(source, new ConcurrentDictionary<TParameter, TResult>(comparer)) {}

		protected EqualityStore(Func<TParameter, TResult> source, IDictionary<TParameter, TResult> store)
			: base(new StandardTables<TParameter, TResult>(source).Get(store)) {}
	}
}