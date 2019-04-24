using Super.Application.Hosting.BenchmarkDotNet;
using System.Linq;

namespace Super.Testing.Application
{
	public class Program
	{
		static void Main(params string[] arguments)
		{
			Run.Default
			   .In(arguments.Any() ? Quick.Default : Deployed.Default)
			   .Get();
		}
	}

	sealed class Run : Run<IterationBenchmarks>
	{
		public static Run Default { get; } = new Run();

		Run() {}
	}
}