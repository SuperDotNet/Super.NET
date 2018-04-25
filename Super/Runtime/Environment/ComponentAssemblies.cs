using Super.Model.Collections;
using Super.Model.Extents;
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
		                                            .Out(AssemblyNameSelector.Default)
		                                            .Out(ComponentAssemblyNames.Default)
		                                            .Out(Load.Default.Select())
		                                            .Out(x => x.Append(HostingAssembly.Default, PrimaryAssembly.Default))
		                                            .Out(x => x.Assigned().Distinct())
		                                            .Return()
		                                            .Get()) {}
	}
}