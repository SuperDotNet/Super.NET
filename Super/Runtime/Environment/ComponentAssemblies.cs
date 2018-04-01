using System.Linq;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Reflection;

namespace Super.Runtime.Environment
{
	sealed class ComponentAssemblies : Items<Assembly>
	{
		public static ComponentAssemblies Default { get; } = new ComponentAssemblies();

		ComponentAssemblies() : base(PrimaryAssembly.Default.Adapt()
		                                            .Guard(PrimaryAssemblyMessage.Default)
		                                            .Out(AssemblyNameCoercer.Default)
		                                            .Out(ComponentAssemblyNames.Default)
		                                            .Out(Load.Default)
		                                            .Get()
		                                            .ToArray()
		                                            .Assigned()) {}
	}
}