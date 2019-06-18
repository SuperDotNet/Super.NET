using Super.Application.Hosting.BenchmarkDotNet;

namespace Super.Serialization.Testing.Application
{
	public class Program
	{
		static void Main(params string[] arguments)
		{
			Configuration.Default.Get(arguments)
			             .To(Run.A<ShortInstructionTests.Benchmarks>);
		}
	}
}