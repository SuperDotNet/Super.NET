using BenchmarkDotNet.Jobs;
using Super.Model.Selection.Alterations;
using System;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	class ConfigureJob : IAlteration<Job>
	{
		readonly ReadOnlyMemory<global::BenchmarkDotNet.Jobs.EnvironmentVariable> _variables;

		public ConfigureJob(params global::BenchmarkDotNet.Jobs.EnvironmentVariable[] variables)
			=> _variables = variables;

		public Job Get(Job parameter)
		{
			var variables = _variables;
			var result = parameter.With(variables.ToArray());
			return result;
		}
	}
}