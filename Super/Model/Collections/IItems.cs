using System.Collections.Generic;
using System.Collections.Immutable;
using Super.Model.Instances;

namespace Super.Model.Collections
{
	public interface IItems<T> : IEnumerable<T>, IInstance<ImmutableArray<T>> {}
}