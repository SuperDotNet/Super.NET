using System.Collections.Generic;
using System.Reflection;
using Super.Model.Sources;

namespace Super.Runtime.Environment
{
	sealed class ComponentAssemblyNames : ISource<AssemblyName, IEnumerable<AssemblyName>>
	{
		public static ComponentAssemblyNames Default { get; } = new ComponentAssemblyNames();

		ComponentAssemblyNames() {}

		public IEnumerable<AssemblyName> Get(AssemblyName parameter)
		{
			yield return EnvironmentAssemblyName.Default.Get(parameter);
			yield return PlatformAssemblyName.Default.Get(parameter);
		}
	}
}