using Super.Application.Hosting.BenchmarkDotNet;
using Super.Testing.Application.Model.Sequences.Query;
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
			/*new ProjectionTests.Benchmarks().Subject();*/
		}
	}

	sealed class Run : Run<ProjectionManyTests.Benchmarks>
	{
		public static Run Default { get; } = new Run();

		Run() {}
	}
}