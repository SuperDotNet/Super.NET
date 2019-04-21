using Super.Compose;
using Super.Model.Sequences;
using Super.Reflection.Assemblies;
using System.Reflection;

namespace Super.Runtime.Environment
{
	sealed class Assemblies : DecoratedArray<Assembly>
	{
		public static Assemblies Default { get; } = new Assemblies();

		Assemblies() : base(A.This(PrimaryAssembly.Default)
		                     .Select(AssemblyNameSelector.Default)
		                     .Select(ComponentAssemblyNames.Default)
		                     .Query()
		                     .Select(Load.Default)
		                     .Append(HostingAssembly.Default.And(PrimaryAssembly.Default))
		                     .WhereBy(y => y != null)
		                     .Distinct()
		                     .Get()
		                     .ToResult()) {}
	}
}