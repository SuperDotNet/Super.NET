using Super.Model.Selection;
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

		PrimaryAssembly() : base(AssemblyGuard.Default.Get(Assemblies.Default.Only(x => x.Has<HostingAttribute>()))) {}
	}

	sealed class AssemblyGuard : DecoratedSelect<Assembly, Assembly>
	{
		public static AssemblyGuard Default { get; } = new AssemblyGuard();

		AssemblyGuard() : base(In<Assembly>.Start()
		                                   .Guard(PrimaryAssemblyMessage.Default)) {}
	}
}