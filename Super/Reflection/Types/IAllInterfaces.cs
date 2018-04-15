using System.Collections.Immutable;
using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Types
{
	public interface IAllInterfaces : ISelect<TypeInfo, ImmutableArray<TypeInfo>> {}
}