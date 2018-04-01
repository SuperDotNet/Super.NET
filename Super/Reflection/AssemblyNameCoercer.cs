using System.Reflection;
using Super.Model.Sources;

namespace Super.Reflection
{
	public sealed class AssemblyNameCoercer : ISource<Assembly, AssemblyName>
	{
		public static AssemblyNameCoercer Default { get; } = new AssemblyNameCoercer();

		AssemblyNameCoercer() {}

		public AssemblyName Get(Assembly parameter) => parameter.GetName();
	}
}