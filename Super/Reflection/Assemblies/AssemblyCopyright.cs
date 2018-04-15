using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class AssemblyCopyright : Attribute<AssemblyCopyrightAttribute, string>
	{
		public static IAttribute<string> Default { get; } = new AssemblyCopyright();

		AssemblyCopyright() : base(x => x.Copyright) {}
	}
}