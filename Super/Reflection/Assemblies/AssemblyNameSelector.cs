using System.Reflection;
using Super.Model.Selection;

namespace Super.Reflection.Assemblies
{
	public sealed class AssemblyNameSelector : Select<Assembly, AssemblyName>
	{
		public static AssemblyNameSelector Default { get; } = new AssemblyNameSelector();

		AssemblyNameSelector() : base(x => x.GetName()) {}
	}
}