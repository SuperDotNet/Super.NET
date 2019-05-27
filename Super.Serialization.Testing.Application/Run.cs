using Super.Application.Hosting.BenchmarkDotNet;

namespace Super.Serialization.Testing.Application
{
	sealed class Run : Run<Benchmarks>
	{
		public static Run Default { get; } = new Run();

		Run() {}
	}

	public class Benchmarks
	{
			
	}

}