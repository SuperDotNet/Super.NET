using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class AssemblyDescription : Declared<AssemblyDescriptionAttribute, string>
	{
		public static AssemblyDescription Default { get; } = new AssemblyDescription();

		AssemblyDescription() : base(x => x.Description) {}
	}
}