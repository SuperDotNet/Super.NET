using Super.Model.Collections;
using Super.Reflection.Assemblies;
using System.Linq;
using System.Reflection;

namespace Super.Runtime.Environment
{
	sealed class Assemblies : DecoratedSequence<Assembly>
	{
		public static Assemblies Default { get; } = new Assemblies();

		Assemblies() : base(PrimaryAssembly.Default
		                                   .Select(AssemblyNameSelector.Default)
		                                   .Select(ComponentAssemblyNames.Default)
		                                   .Select(Load.Default.Select())
		                                   .Select(x => x.Append(HostingAssembly.Default, PrimaryAssembly.Default)
		                                                 .Where(y => y != null)
		                                                 .Distinct())) {}
	}
}