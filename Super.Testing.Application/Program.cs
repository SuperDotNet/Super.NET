using Super.Application.Hosting.BenchmarkDotNet;

namespace Super.Testing.Application
{
	public class Program
	{
		static void Main()
		{
			Run.Default.Get();
		}
	}

	sealed class Run : Run<PagerBenchmarks>
	{
		public static Run Default { get; } = new Run();

		Run() {}
	}
}