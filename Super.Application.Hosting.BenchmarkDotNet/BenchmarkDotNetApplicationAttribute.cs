using Super.Runtime.Environment;

namespace Super.Application.Hosting.BenchmarkDotNet
{
	public sealed class BenchmarkDotNetApplicationAttribute : HostingAttribute
	{
		public BenchmarkDotNetApplicationAttribute() : base(typeof(BenchmarkDotNetApplicationAttribute).Assembly) {}
	}
}