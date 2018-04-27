using Super.Model.Collections;
using Super.Reflection.Assemblies;
using System.Linq;
using System.Reflection;

namespace Super.Runtime.Environment
{
	sealed class ComponentAssemblies : Items<Assembly>
	{
		public static ComponentAssemblies Default { get; } = new ComponentAssemblies();

		ComponentAssemblies() : base(PrimaryAssembly.Default
		                                            .Out()
		                                            .Select(AssemblyNameSelector.Default)
		                                            .Select(ComponentAssemblyNames.Default)
		                                            .Select(Load.Default.Select())
		                                            .Select(x => x.Append(HostingAssembly.Default, PrimaryAssembly.Default))
		                                            .Select(x => x.Assigned().Distinct())
		                                            .Out()
		                                            .Get()) {}
	}
}