using BenchmarkDotNet.Configs;
using Super.Model.Sources;
using Super.Runtime.Environment;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	public sealed class Configuration : Conditional<IConfig>
	{
		public static Configuration Default { get; } = new Configuration();

		Configuration() : base(IsDeployed.Default.IsSatisfiedBy, Deployed.Default, Quick.Default) {}
	}
}