using System.Collections.Generic;
using System.Collections.Immutable;
using Super.Model.Selection;

namespace Super.Model.Collections
{
	public static class Set<T>
	{
		public static ImmutableArray<T> Enumerate(IEnumerable<T> items) => ImmutableArraySelector<T>.Default.Get(items);
		public static IEnumerable<T> Hide(ImmutableArray<T> items) => EnumerableSelector<T>.Default.Get(items);
	}

	public sealed class ImmutableArraySelector<T> : Delegated<IEnumerable<T>, ImmutableArray<T>>
	{
		public static ImmutableArraySelector<T> Default { get; } = new ImmutableArraySelector<T>();

		ImmutableArraySelector() : base(x => x.ToImmutableArray()) {}
	}

	public sealed class EnumerableSelector<T> : ISelect<ImmutableArray<T>, IEnumerable<T>>
	{
		public static EnumerableSelector<T> Default { get; } = new EnumerableSelector<T>();

		EnumerableSelector() {}

		public IEnumerable<T> Get(ImmutableArray<T> parameter)
		{
			var enumerator = parameter.GetEnumerator();
			while (enumerator.MoveNext())
			{
				yield return enumerator.Current;
			}
		}
	}
}