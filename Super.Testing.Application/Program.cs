using Super.Application.Hosting.BenchmarkDotNet;
using Super.Testing.Application.Model.Sequences;
using System.Linq;

namespace Super.Testing.Application
{
	public class Program
	{
		static void Main(params string[] arguments)
		{
			Run.Default
			   .Out(arguments.Any() ? Quick.Default : Deployed.Default)
			   .Get();
		}
	}

	sealed class Run : Run<WhereTests.Benchmarks>
	{
		public static Run Default { get; } = new Run();

		Run() {}
	}
}