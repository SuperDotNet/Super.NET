using Super.Model.Results;
using Super.Model.Selection;
using System;

namespace Super.Runtime
{
	public static class Pairs
	{
		public static Pair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value)
			=> new Pair<TKey, TValue>(key, value);

		public static ISelect<T, TIn, TOut> Select<T, TIn, TOut>(params Pair<T, Func<TIn, TOut>>[] pairs)
			=> pairs.ToSelect();
	}

	public interface IPair<TKey, TValue> : IResult<Pair<TKey, TValue>> {}

	public class Pairing<TKey, TValue> : Instance<Pair<TKey, TValue>>, IPair<TKey, TValue>
	{
		public Pairing(TKey key, TValue value) : base(Pairs.Create(key, value)) {}
	}

	public readonly struct Pair<TKey, TValue>
	{
		public Pair(TKey key, TValue value)
		{
			Key   = key;
			Value = value;
		}

		public TKey Key { get; }

		public TValue Value { get; }
	}
}