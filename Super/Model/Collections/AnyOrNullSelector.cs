using Super.Model.Selection;
using System.Collections.Immutable;

namespace Super.Model.Collections
{
	sealed class AnyOrNullSelector<T> : ISelect<ImmutableArray<T>, ImmutableArray<T>?>
	{
		public static AnyOrNullSelector<T> Default { get; } = new AnyOrNullSelector<T>();

		AnyOrNullSelector() {}

		public ImmutableArray<T>? Get(ImmutableArray<T> parameter)
		{
			var array = parameter;
			return array.Length > 0 ? array : (ImmutableArray<T>?)null;
		}
	}
}