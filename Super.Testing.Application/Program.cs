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
		}
	}

	sealed class Run : Run<IndexerBenchmarks>
	{
		public static Run Default { get; } = new Run();

		Run() {}
	}
}