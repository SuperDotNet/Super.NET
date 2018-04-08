using System.Collections.Immutable;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection
{
	public interface IAllInterfaces : ISelect<TypeInfo, ImmutableArray<TypeInfo>> {}
}