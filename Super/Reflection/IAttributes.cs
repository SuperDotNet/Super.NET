using System.Collections.Immutable;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection
{
	public interface IAttributes<T> : ISelect<ICustomAttributeProvider, ImmutableArray<T>> {}
}