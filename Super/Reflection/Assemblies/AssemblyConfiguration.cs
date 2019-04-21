using System.Reflection;

namespace Super.Reflection.Assemblies
{
	sealed class AssemblyConfiguration : Declared<AssemblyConfigurationAttribute, string>
	{
		public static AssemblyConfiguration Default { get; } = new AssemblyConfiguration();

		AssemblyConfiguration() : base(x => x.Configuration) {}
	}
}