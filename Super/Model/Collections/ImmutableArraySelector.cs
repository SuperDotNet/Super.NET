using Super.Model.Selection;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Model.Collections
{
	public sealed class ImmutableArraySelector<T> : Select<IEnumerable<T>, ImmutableArray<T>>
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