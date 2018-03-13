using System.Collections.Generic;
using System.Collections.Immutable;
using Super.Model.Sources;

namespace Super.Model.Collections
{
	public sealed class ImmutableArrayCoercer<T> : ISource<IEnumerable<T>, ImmutableArray<T>>
	{
		public static ImmutableArrayCoercer<T> Default { get; } = new ImmutableArrayCoercer<T>();

		ImmutableArrayCoercer() {}

		public ImmutableArray<T> Get(IEnumerable<T> parameter) => parameter.ToImmutableArray();
	}
}