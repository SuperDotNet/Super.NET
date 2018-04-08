using Super.Model.Selection;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Model.Collections
{
	sealed class AnyOrNullSelector<T> : ISelect<ImmutableArray<T>, ImmutableArray<T>?>
	{
		public static AnyOrNullSelector<T> Default { get; } = new AnyOrNullSelector<T>();

		AnyOrNullSelector() {}

		public ImmutableArray<T>? Get(ImmutableArray<T> parameter) => parameter.Any() ? parameter : (ImmutableArray<T>?)null;
	}
}