using Super.Application.Hosting.BenchmarkDotNet;

namespace Super.Testing.Application
{
	public class Program
	{
		static void Main()
		{
			Run.Default.Get();
			/*var benchmarks = new IndexerBenchmarks{ Count = 10_000u };
			var iterate = benchmarks.Iteration();
			Console.WriteLine(iterate.Length);*/

/*			var benchmarks = new IterationBenchmarks { Count = 10_000u };
			var array = benchmarks.VerifyBuffer();
			Debugger.Break();*/
		}
	}

	sealed class Run : Run<IterationBenchmarks>
	{
		public static Run Default { get; } = new Run();

		Run() {}
	}
}