using System.Reflection;
using Super.Model.Sources;

namespace Super.Runtime.Environment
{
	public sealed class ExecutionConfiguration : Source<string>
	{
		public static ExecutionConfiguration Default { get; } = new ExecutionConfiguration();

		ExecutionConfiguration() : base(Assembly.GetExecutingAssembly()
		                                        .GetCustomAttribute<AssemblyConfigurationAttribute>()
		                                        .Configuration) {}
	}
}