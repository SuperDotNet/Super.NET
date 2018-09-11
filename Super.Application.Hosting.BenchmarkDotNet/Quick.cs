using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess;
using Super.Model.Sources;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	sealed class Quick : FixedDeferredSingleton<Job, IConfig>
	{
		public static Quick Default { get; } = new Quick();

		Quick() : base(QuickConfiguration.Default, Job.ShortRun.WithIterationCount(5).With(InProcessToolchain.Instance)) {}
	}
}