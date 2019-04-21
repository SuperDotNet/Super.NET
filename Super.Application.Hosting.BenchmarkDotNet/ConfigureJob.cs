using BenchmarkDotNet.Jobs;
using Super.Model.Selection.Alterations;
using Super.Model.Sequences;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	class ConfigureJob : IAlteration<Job>
	{
		readonly Array<global::BenchmarkDotNet.Jobs.EnvironmentVariable> _variables;

		public ConfigureJob(params global::BenchmarkDotNet.Jobs.EnvironmentVariable[] variables)
			=> _variables = variables;

		public Job Get(Job parameter) => _variables.Copy().To(parameter.With);
	}
}