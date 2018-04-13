using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Model.Sources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Runtime
{
	public static class Pairs
	{
		public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) =>
			new KeyValuePair<TKey, TValue>(key, value);

		public static ISelect<TParameter, TIn, TOut> From<TParameter, TIn, TOut>(
			params ISource<KeyValuePair<TParameter, Func<TIn, TOut>>>[] pairs)
			=> From(pairs.Select(x => x.Get()).ToArray());

		public static ISelect<TParameter, TIn, TOut> From<TParameter, TIn, TOut>(
			params KeyValuePair<TParameter, Func<TIn, TOut>>[] pairs)
			=> @pairs.ToSelect();
	}

	public interface IPair<TKey, TValue> : ISource<KeyValuePair<TKey, TValue>> {}

	class Pair<TKey, TValue> : Source<KeyValuePair<TKey, TValue>>, IPair<TKey, TValue>
	{
		protected Pair(TKey key, TValue value) : base(Pairs.Create(key, value)) {}
	}
}