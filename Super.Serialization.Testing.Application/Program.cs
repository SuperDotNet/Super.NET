using Super.Application.Hosting.BenchmarkDotNet;
using System.Linq;

namespace Super.Serialization.Testing.Application
{
	public class Program
	{
		static void Main(params string[] arguments)
		{
			Run.Default.In(arguments.Any() ? Quick.Default : Deployed.Default)
			   .Get();
		}
	}
}