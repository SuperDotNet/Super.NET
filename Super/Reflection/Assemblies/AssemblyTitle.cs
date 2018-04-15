using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class AssemblyTitle : Attribute<AssemblyTitleAttribute, string>
	{
		public static IAttribute<string> Default { get; } = new AssemblyTitle();

		AssemblyTitle() : base(x => x.Title) {}
	}
}