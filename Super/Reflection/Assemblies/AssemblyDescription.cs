using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class AssemblyDescription : Attribute<AssemblyDescriptionAttribute, string>
	{
		public static IAttribute<string> Default { get; } = new AssemblyDescription();

		AssemblyDescription() : base(x => x.Description) {}
	}
}