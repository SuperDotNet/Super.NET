using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Super.Model.Sources;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	public class Run<T> : DelegatedSelection<IConfig, Summary> // TODO: Convert to application.
	{
		protected Run() : this(BenchmarkRunner.Run<T>, Configuration.Default.Get) {}

		public Run(Func<IConfig, Summary> source, Func<IConfig> parameter) : base(source, parameter) {}
	}
}