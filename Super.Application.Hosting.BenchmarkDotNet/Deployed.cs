using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using Super.Model.Sources;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	sealed class Deployed : FixedDeferredSingleton<Job, IConfig>
	{
		public static Deployed Default { get; } = new Deployed();

		Deployed() : base(QuickConfiguration.Default, Job.MediumRun.WithLaunchCount(1).WithWarmupCount(5).WithIterationCount(5)) {}
	}
}