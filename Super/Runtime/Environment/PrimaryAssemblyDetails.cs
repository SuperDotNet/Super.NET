using System.Reflection;
using Super.Model.Results;
using Super.Reflection.Assemblies;

namespace Super.Runtime.Environment
{
	public sealed class PrimaryAssemblyDetails : FixedSelectedSingleton<Assembly, AssemblyDetails>
	{
		public static PrimaryAssemblyDetails Default { get; } = new PrimaryAssemblyDetails();

		PrimaryAssemblyDetails() : base(AssemblyDetailsSelector.Default, PrimaryAssembly.Default) {}
	}
}