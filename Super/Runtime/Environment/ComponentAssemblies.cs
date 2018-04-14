using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Reflection.Assemblies;
using System.Reflection;

namespace Super.Runtime.Environment
{
	sealed class ComponentAssemblies : Items<Assembly>
	{
		public static ComponentAssemblies Default { get; } = new ComponentAssemblies();

		ComponentAssemblies() : base(PrimaryAssembly.Default
		                                            .Select(PrimaryAssemblyMessage.Default.Guard<Assembly>())
		                                            .Select(AssemblyNameSelector.Default)
		                                            .Select(ComponentAssemblyNames.Default)
		                                            .Select(Load.Default.Select())
		                                            .Select(x => x.Assigned()).Get()) {}
	}
}