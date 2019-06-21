using Super.Application.Hosting.BenchmarkDotNet;
using Super.Serialization.Testing.Application.Writing.Instructions;

namespace Super.Serialization.Testing.Application
{
	public class Program
	{
		static void Main(params string[] arguments)
		{
			Configuration.Default.Get(arguments)
			             .To(Run.A<DefaultStringInstructionTests.Benchmarks>);
		}
	}
}