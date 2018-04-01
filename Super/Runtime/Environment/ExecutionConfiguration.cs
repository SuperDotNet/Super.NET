using Super.Model.Instances;
using System.Reflection;

namespace Super.Runtime.Environment
{
	public sealed class ExecutionConfiguration : Instance<string>
	{
		public static ExecutionConfiguration Default { get; } = new ExecutionConfiguration();

		ExecutionConfiguration() : base(Assembly.GetExecutingAssembly()
		                                        .GetCustomAttribute<AssemblyConfigurationAttribute>()
		                                        .Configuration) {}
	}
}