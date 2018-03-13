using System.Collections.Immutable;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	public interface IAllInterfaces : ISource<TypeInfo, ImmutableArray<TypeInfo>> {}
}