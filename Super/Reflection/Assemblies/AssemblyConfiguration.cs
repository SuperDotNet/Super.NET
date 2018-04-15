using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class AssemblyConfiguration : Attribute<AssemblyConfigurationAttribute, string>
	{
		public static IAttribute<string> Default { get; } = new AssemblyConfiguration();

		AssemblyConfiguration() : base(x => x.Configuration) {}
	}
}