using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	sealed class DeployedConfiguration : ISelect<Job, IConfig>
	{
		public static DeployedConfiguration Default { get; } = new DeployedConfiguration();

		DeployedConfiguration() : this(AlignJitLoops.Default) {}

		readonly IAlteration<Job> _configure;

		public DeployedConfiguration(IAlteration<Job> configure) => _configure = configure;

		public IConfig Get(Job parameter)
		{
			var result = ManualConfig.Create(DefaultConfig.Instance);
			result.Add(parameter);
			result.Add(parameter.To(_configure));
			result.Set(new DefaultOrderProvider(SummaryOrderPolicy.FastestToSlowest));
			result.Add(MemoryDiagnoser.Default);
			return result;
		}
	}
}