using Super.Model.Selection;
using Super.Model.Sources;
using System;
using System.Collections.Generic;

namespace Super.Runtime
{
	public static class Pairs
	{
		public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
			 => new KeyValuePair<TKey, TValue>(key, value);

		/*public static ISpecification<TParameter, TResult> Select<TParameter, TResult>(
			params ISource<KeyValuePair<TParameter, TResult>>[] pairs) => pairs.Select(x => x.Get()).ToStore();*/

		public static ISelect<TParameter, TIn, TOut> Select<TParameter, TIn, TOut>(
			params KeyValuePair<TParameter, Func<TIn, TOut>>[] pairs) => pairs.ToSelect();
	}

	public interface IPair<TKey, TValue> : ISource<KeyValuePair<TKey, TValue>> {}

	public class Pair<TKey, TValue> : Source<KeyValuePair<TKey, TValue>>, IPair<TKey, TValue>
	{
		protected Pair(TKey key, TValue value) : base(Pairs.Create(key, value)) {}
	}
}