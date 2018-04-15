using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Reflection.Assemblies;
using System.Reflection;

namespace Super.Runtime.Environment
{
	public sealed class PrimaryAssemblyDetails : FixedDeferredSingleton<Assembly, AssemblyDetails>
	{
		public static PrimaryAssemblyDetails Default { get; } = new PrimaryAssemblyDetails();

		PrimaryAssemblyDetails() : base(AssemblyDetailsSelector.Default, PrimaryAssembly.Default) {}
	}

	sealed class PrimaryAssembly : Source<Assembly>
	{
		public static PrimaryAssembly Default { get; } = new PrimaryAssembly();

		PrimaryAssembly() : base(Assemblies.Default.Only(x => x.Has<PrimaryAssemblyAttribute>())) {}
	}
}