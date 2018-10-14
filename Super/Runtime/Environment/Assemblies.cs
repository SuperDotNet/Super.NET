using Super.Model.Sequences;
using Super.Reflection.Assemblies;
using System.Linq;
using System.Reflection;

namespace Super.Runtime.Environment
{
	sealed class Assemblies : DecoratedArray<Assembly>
	{
		public static Assemblies Default { get; } = new Assemblies();

		Assemblies() : base(PrimaryAssembly.Default
		                                   .Select(AssemblyNameSelector.Default)
		                                   .Select(ComponentAssemblyNames.Default)
		                                   .Select(x => x.Select(Load.Default.Get))
		                                   .Select(x => x.Append(HostingAssembly.Default, PrimaryAssembly.Default)
		                                                 .Where(y => y != null)
		                                                 .Distinct())
		                                   .Result()) {}
	}
}