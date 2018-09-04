using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Super.Model.Selection;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	sealed class QuickConfiguration : ISelect<Job, IConfig>
	{
		public static QuickConfiguration Default { get; } = new QuickConfiguration();

		QuickConfiguration() {}

		public IConfig Get(Job parameter)
		{
			var result = ManualConfig.Create(DefaultConfig.Instance);
			result.Add(parameter);
			result.Add(MemoryDiagnoser.Default);
			result.Set(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest));
			return result;
		}
	}
}