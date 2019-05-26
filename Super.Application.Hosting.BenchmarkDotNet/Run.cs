using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Super.Model.Selection;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	public class Run<T> : Select<IConfig, Summary>
	{
		protected Run() : this(BenchmarkRunner.Run<T>) {}

		public Run(Func<IConfig, Summary> select) : base(select) {}
	}
}