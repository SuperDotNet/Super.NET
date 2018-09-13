using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess;
using Super.Model.Sources;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	sealed class Quick : FixedDeferredSingleton<Job, IConfig>
	{
		public static Quick Default { get; } = new Quick();

		Quick() : base(DeployedConfiguration.Default, Job.ShortRun.With(InProcessToolchain.DontLogOutput)) {}
	}
}