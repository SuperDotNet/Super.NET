using BenchmarkDotNet.Jobs;
using Super.Model.Collections;
using Super.Model.Selection.Alterations;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	class ConfigureJob : IAlteration<Job>
	{
		readonly Array<global::BenchmarkDotNet.Jobs.EnvironmentVariable> _variables;

		public ConfigureJob(params global::BenchmarkDotNet.Jobs.EnvironmentVariable[] variables)
			: this(new Array<global::BenchmarkDotNet.Jobs.EnvironmentVariable>(variables)) {}

		public ConfigureJob(Array<global::BenchmarkDotNet.Jobs.EnvironmentVariable> variables)
			=> _variables = variables;

		public Job Get(Job parameter) => parameter.With(_variables.Get());
	}
}