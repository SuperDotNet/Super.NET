using Super.Compose;
using Super.Model.Results;
using System.Reflection;

namespace Super.Runtime.Environment
{
	public sealed class PrimaryAssembly : Instance<Assembly>
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