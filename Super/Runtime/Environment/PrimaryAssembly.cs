using Super.Compose;
using Super.Model.Results;
using Super.Reflection.Assemblies;
using System.Reflection;

namespace Super.Runtime.Environment
{
	public sealed class PrimaryAssemblyDetails : FixedSelectedSingleton<Assembly, AssemblyDetails>
	{
		public static PrimaryAssemblyDetails Default { get; } = new PrimaryAssemblyDetails();

		PrimaryAssemblyDetails() : base(AssemblyDetailsSelector.Default, PrimaryAssembly.Default) {}
	}

	sealed class PrimaryAssembly : Instance<Assembly>
	{
		public static PrimaryAssembly Default { get; } = new PrimaryAssembly();

		PrimaryAssembly() : base(Start.A.Selection.Of<Assembly>()
		                              .As.Sequence.Immutable.By.Self.Query()
		                              .Only(x => x.Has<HostingAttribute>())
		                              .Select(PrimaryAssemblyMessage.Default.AsGuard())
		                              .In(Reflection.Assemblies.Assemblies.Default.Get)
		                              .Get()) {}
	}
}