using Super.Model.Sources;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Model.Collections
{
	public sealed class ImmutableArraySelector<T> : DelegatedSource<IEnumerable<T>, ImmutableArray<T>>
	{
		public static ImmutableArraySelector<T> Default { get; } = new ImmutableArraySelector<T>();

		ImmutableArraySelector() : base(x => x.ToImmutableArray()) {}
	}

	public sealed class EnumerableSelector<T> : ISource<ImmutableArray<T>, IEnumerable<T>>
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