using Super.Application.Hosting.BenchmarkDotNet;
using Super.Testing.Application.Model.Sequences.Query.Construction;
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

	sealed class Run : Run<ExitTests.Benchmarks>
	{
		public static Run Default { get; } = new Run();

		Run() {}
	}
}