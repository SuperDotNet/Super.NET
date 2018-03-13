using System.Collections.Immutable;
using System.Linq;
using Super.Model.Sources;

namespace Super.Model.Collections
{
	sealed class AnyOrNullCoercer<T> : ISource<ImmutableArray<T>, ImmutableArray<T>?>
	{
		public static AnyOrNullCoercer<T> Default { get; } = new AnyOrNullCoercer<T>();

		AnyOrNullCoercer() {}

		public ImmutableArray<T>? Get(ImmutableArray<T> parameter) => parameter.Any() ? parameter : (ImmutableArray<T>?)null;
	}
}