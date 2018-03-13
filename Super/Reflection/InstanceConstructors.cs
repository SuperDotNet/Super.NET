using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	sealed class InstanceConstructors : DelegatedSource<TypeInfo, IEnumerable<ConstructorInfo>>
	{
		public static InstanceConstructors Default { get; } = new InstanceConstructors();

		InstanceConstructors() : base(info => info.DeclaredConstructors.Where(c => c.IsPublic && !c.IsStatic)) {}
	}
}