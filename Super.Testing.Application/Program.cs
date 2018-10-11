using Super.Application.Hosting.BenchmarkDotNet;
using Super.Testing.Application.Runtime.Environment;
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

	sealed class Run : Run<DefaultComponentTests.Benchmarks>
	{
		public static Run Default { get; } = new Run();

		Run() {}
	}
}