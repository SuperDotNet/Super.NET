using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Reflection;
using System.Reflection;

namespace Super.Runtime.Environment
{
	sealed class ComponentAssemblies : Items<Assembly>
	{
		public static ComponentAssemblies Default { get; } = new ComponentAssemblies();

		ComponentAssemblies() : base(PrimaryAssembly.Default
		                                            /*.Allow()
		                                            .Guard(PrimaryAssemblyMessage.Default)*/
		                                            .Select(AssemblyNameSelector.Default)
		                                            .Select(ComponentAssemblyNames.Default)
		                                            .Select(Load.Default.Select())
		                                            .Select(x => x.Assigned()).Get()) {}
	}
}