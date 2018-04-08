using System.Collections.Generic;
using System.Collections.Immutable;
using Super.Model.Sources;

namespace Super.Model.Collections
{
	public interface IItems<T> : IEnumerable<T>, ISource<ImmutableArray<T>> {}
}