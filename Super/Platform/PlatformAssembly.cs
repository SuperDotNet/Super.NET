using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Instances;

namespace Super.Platform
{
	sealed class PlatformAssembly : IInstance<Assembly>
	{
		public static PlatformAssembly Default { get; } = new PlatformAssembly();

		PlatformAssembly() : this(PlatformAssemblyName.Default.Get()) {}

		readonly AssemblyName _name;

		public PlatformAssembly(AssemblyName name) => _name = name;

		public Assembly Get() => Assembly.Load(_name);
	}
}