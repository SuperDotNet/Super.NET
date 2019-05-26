using Super.Application.Hosting.BenchmarkDotNet;
using Super.Testing.Application.Model.Sequences.Query.Construction;

namespace Super.Testing.Application
{
	sealed class Run : Run<ExitTests.Benchmarks>
	{
		public static Run Default { get; } = new Run();

		Run() {}
	}
}